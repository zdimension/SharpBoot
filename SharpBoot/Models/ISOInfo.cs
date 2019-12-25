using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Linq;
using SharpBoot.Properties;
using SharpBoot.Utilities;

// ReSharper disable PossibleNullReferenceException

namespace SharpBoot.Models
{
    public class ISOV
    {
        public ISOV(string h, string n, string d = "", string fn = "", bool l = false)
        {
            Hash = h;
            Name = n;
            DownloadLink = d == "" ? "www.google.com/search?q=" + n : d;
            Latest = l;
            Filename = fn;
        }

        public string Hash { get; set; }

        public string Name { get; set; }

        public string DownloadLink { get; set; }
        public bool Latest { get; set; }
        public string Filename { get; set; }

        public ISOInfo Parent { get; set; }
    }

    public class ISOInfo
    {
        public static bool IsUpdating;

        public static int AppDBVersion = -1;

        public static List<ISOInfo> ISOs = new List<ISOInfo>();

        public ISOInfo(string name, Dictionary<CultureInfo, string> descs, ISOCat cat, string fn = "",
            params ISOV[] vers)
        {
            Name = name;
            Descriptions = descs;
            Category = cat;
            Filename = fn;
            Versions = vers.ToList();
            Versions.ForEach(x => x.Parent = this);
            if (!Versions.Any(x => x.Latest) && Versions.Count > 0)
                Versions.Where(t => LatestVersion == t).ToList()[0].Latest = true;
        }

        public string Name { get; set; }

        public string Description
            =>
                Descriptions.ContainsKey(Thread.CurrentThread.CurrentUICulture)
                    ? Descriptions[Thread.CurrentThread.CurrentUICulture]
                    : Descriptions.ContainsKey(new CultureInfo("en"))
                        ? Descriptions[new CultureInfo("en")]
                        : "";

        public string CategoryTxt => Category.GetName();

        public ISOCat Category { get; set; }

        //public string DownloadLink { get; set; }

        public string Filename { get; set; }

        public List<ISOV> Versions { get; set; }

        public bool NoDL { get; set; }

        public ISOV LatestVersion
        {
            get
            {
                if (Versions.Count == 0)
                    return null;
                var r = Versions.FirstOrDefault(x => x.Latest);
                return r ?? Versions.OrderByDescending(x => x.Name).First();
            }
        }

        public Dictionary<CultureInfo, string> Descriptions { get; set; }

        public static event EventHandler UpdateFinished = delegate { IsUpdating = false; };

        public static void RefreshISOs()
        {
            /*bool upd = IsUpdateAvailable();

            if (upd)*/
            IsUpdating = true;
            var th = new Thread(() =>
            {
                try
                {
                    Thread.CurrentThread.CurrentCulture = new CultureInfo(Settings.Default.Lang);
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo(Settings.Default.Lang);

                    var appsxml = Utils.DownloadWithoutCache("https://zdimension.fr/sharpboot/apps.xml");

                    if (appsxml?.Length > 40 && appsxml.Substring(appsxml.Length - 37, 37) !=
                        "<!--All your base are belong to us-->")
                        appsxml = Resources.DefaultISOs;

                    try
                    {
                        XDocument xd;
                        try
                        {
                            xd = XDocument.Parse(appsxml);
                        }
                        catch
                        {
                            appsxml = Resources.DefaultISOs;
                            xd = XDocument.Parse(appsxml);
                        }

                        var root = xd.Element("apps");

                        var ver = int.Parse(root.Attribute("version").Value);

                        var isocat = root.Element("categories").Elements("cat").Select(x =>
                            new ISOCat(int.Parse(x.Attribute("id").Value), x.Attribute("def").Value,
                                x.Elements("text")
                                    .Select(y => new {lang = new CultureInfo(y.Attribute("lang").Value), val = y.Value})
                                    .ToDictionary(z => z.lang, z => z.val))).ToDictionary(x => x.ID, x => x);

                        var isos = root.Elements("app").Select(x =>
                            new ISOInfo(
                                x.Element("name").Value,
                                x.Element("description")
                                    .Elements("desc")
                                    .Select(y => new {Lang = new CultureInfo(y.Attribute("lang").Value), Val = y.Value})
                                    .ToDictionary(k => k.Lang, k => k.Val),
                                isocat[int.Parse(x.Element("category").Value)],
                                x.Element("filenameRegex").Value,
                                x.Element("versions").IsEmpty
                                    ? new ISOV[] { }
                                    : x.Element("versions").Elements("version").Select(y =>
                                        new ISOV(
                                            y.Element("hash").Value,
                                            y.Element("name").Value,
                                            y.Element("download").Value,
                                            y.Element("filenameRegex").Value,
                                            y.Element("isLatest") != null && y.Element("isLatest").Value == "true"
                                        )).ToArray()
                            ) {NoDL = x.Element("noDl") != null && x.Element("noDl").Value == "true"}).ToList();

                        AppDBVersion = ver;
                        ISOCat.Categories = isocat;
                        ISOs = isos;
                        Settings.Default.AppsXml = appsxml;

                        Settings.Default.LastAppsUpdate = DateTime.Now;

                        Settings.Default.Save();
                    }
                    catch
                    {
                        // ignored
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("Error in RefreshISOs: \n" + e.Message + "\n\n" + e.StackTrace);
                }

                UpdateFinished(null, EventArgs.Empty);
            });
            th.Start();
        }


        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public static ISOV GetFromFile(string filename, bool fast)
        {
            while (IsUpdating) Thread.Sleep(100);

            ISOV resk;

            var s = ISOs.FirstOrDefault(x =>
                Regex.IsMatch(Path.GetFileName(filename), x.Filename)); // find by filename regex

            /*if (s != null)
            {
                resk = s.LatestVersion ?? new ISOV("nover", s.Name, "", s.Filename, true) {Parent=s};
            }*/
            if (s != null && s.LatestVersion == null)
            {
                resk = new ISOV("nover", s.Name, "", s.Filename, true) {Parent = s};
            }
            else
            {
                if (s != null && s.Versions.Count == 1)
                {
                    resk = s.LatestVersion;
                }
                else
                {
                    var sta = s == null ? ISOs.SelectMany(x => x.Versions).ToList() : s.Versions;
                    var st =
                        sta.FirstOrDefault(
                            x =>
                                !x.Filename.StartsWith("/") &&
                                string.Equals(Path.GetFileName(filename).Trim(), x.Filename.Trim(),
                                    StringComparison
                                        .CurrentCultureIgnoreCase)); // find by version filename regex or string

                    if (st == null)
                    {
                        st =
                            sta.FirstOrDefault(
                                x =>
                                    x.Filename.StartsWith("/") &&
                                    Regex.IsMatch(Path.GetFileName(filename), x.Filename.Substring(1)));

                        if (st == null && !fast)
                        {
                            var md5 = Hash.FileHash(filename, "md5");
                            st = sta.FirstOrDefault(
                                    x =>
                                        x.Hash ==
                                        (x.Hash.Contains(':') ? Hash.FileHash(filename, x.Hash.Split(':')[0]) : md5));
                        }
                    }

                    resk = st ?? (s == null ? null : new ISOV("nover", s.Name, "", s.Filename, true) {Parent = s});
                }
            }

            return resk;
        }
    }

    public class ISOCat
    {
        private static Dictionary<int, ISOCat> _categories = new Dictionary<int, ISOCat> {{-1, Empty}};

        /// <summary>
        ///     Initialise une nouvelle instance de la classe <see cref="ISOCat" />.
        /// </summary>
        public ISOCat(int id, string englishName, Dictionary<CultureInfo, string> names)
        {
            ID = id;
            EnglishName = englishName;
            Names = names;
        }

        public int ID { get; set; }

        public string EnglishName { get; set; }

        public Dictionary<CultureInfo, string> Names { get; set; }

        public static ISOCat Empty => new ISOCat(-1, "", new Dictionary<CultureInfo, string>());

        public static Dictionary<int, ISOCat> Categories
        {
            get => _categories;
            set
            {
                _categories = value;
                if (!_categories.ContainsKey(-1))
                    _categories.Add(-1, Empty);
            }
        }

        public string GetName()
        {
            return GetName(Thread.CurrentThread.CurrentUICulture);
        }

        public string GetName(CultureInfo c)
        {
            return Names.ContainsKey(c) ? Names[c] : EnglishName;
        }
    }
}