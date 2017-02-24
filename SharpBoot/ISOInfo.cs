using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Linq;
using SharpBoot.Properties;

namespace SharpBoot
{
    public class ISOV
    {
        public string Hash { get; set; }

        public string Name { get; set; }

        public string DownloadLink { get; set; }
        public bool Latest { get; set; }
        public string Filename { get; set; }

        public ISOInfo Parent { get; set; }

        public ISOV(string h, string n, string d = "", string fn = "", bool l = false)
        {
            Hash = h;
            Name = n;
            DownloadLink = d == "" ? "www.google.com/search?q=" + n : d;
            Latest = l;
            Filename = fn;
        }
    }

    public class ISOInfo
    {
        public string Name { get; set; }

        public string Description
            =>
                Descriptions.ContainsKey(Thread.CurrentThread.CurrentUICulture)
                    ? Descriptions[Thread.CurrentThread.CurrentUICulture]
                    : (Descriptions.ContainsKey(new CultureInfo("en")) ? Descriptions[new CultureInfo("en")] : "");

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
            {
                Versions.Where(t => LatestVersion == t).ToList()[0].Latest = true;
            }
        }

        public static void UpdateISOs()
        {
            try
            {
                var wc = new WebClient {Encoding = Encoding.UTF8};
                /*var appsxml = wc.DownloadString("http://www.zdimension.tk/sharpboot/apps.xml");
                appsxml = wc.DownloadString("http://www.zdimension.tk/sharpboot/apps.xml");

                while (!appsxml.Contains("</apps>"))
                {
                    appsxml = wc.DownloadString("http://www.zdimension.tk/sharpboot/apps.xml");
                }*/

                var appsxml = Utils.DownloadWithoutCache("http://www.zdimension.tk/sharpboot/apps.php?s");

                Settings.Default.AppsXml = appsxml;

                Settings.Default.LastAppsUpdate = DateTime.Now;

                Settings.Default.Save();
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public static bool IsUpdateAvailable()
        {
            try
            {
                var wc = new WebClient {Encoding = Encoding.UTF8};
                // ReSharper disable once UnusedVariable
                var temporary = wc.DownloadString("http://www.zdimension.tk/sharpboot/apps.xml");
                // Strangely when a PHP page is updated you need to request it twice to see the update
                var lastappsdate = DateTime.Parse(wc.DownloadString("http://www.zdimension.tk/sharpboot/apps.xml"));
                return lastappsdate > Settings.Default.LastAppsUpdate;
            }
            catch
            {
                return false;
            }
        }

        public static event EventHandler UpdateFinished = delegate { IsUpdating = false; };

        public static bool IsUpdating = false;

        public static int AppDBVersion = -1;


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

                    var wc = new WebClient { Encoding = Encoding.UTF8 };
                    /*var appsxml = wc.DownloadString("http://www.zdimension.tk/sharpboot/apps.xml");
                    appsxml = wc.DownloadString("http://www.zdimension.tk/sharpboot/apps.xml");

                    while (!appsxml.Contains("</apps>"))
                    {
                        appsxml = wc.DownloadString("http://www.zdimension.tk/sharpboot/apps.xml");
                    }*/

                    /*var check = Utils.DownloadWithoutCache("http://www.zdimension.ml/sharpboot/apps.php?c");
                    if (check != "All your base are belong to us")
                    {
                        MessageBox.Show("The update server is down. Update aborted.\n" +
                                        "Go to the GitHub repository, a fix release will be released in a few hours.\n" +
                                        "If not, file an issue.");
                        UpdateFinished(null, EventArgs.Empty);
                        return;
                    }*/

                    var appsxml = Utils.DownloadWithoutCache("https://gitcdn.xyz/repo/zdimension/SharpBoot-AppDB/master/apps.xml");

                    if (appsxml.Substring(appsxml.Length - 37, 37) != "<!--All your base are belong to us-->")
                    {
                        appsxml = Resources.DefaultISOs;
                    }

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
                                    ? new ISOV[] {}
                                    : x.Element("versions").Elements("version").Select(y =>
                                        new ISOV(
                                            y.Element("hash").Value,
                                            y.Element("name").Value,
                                            y.Element("download").Value,
                                            y.Element("filenameRegex").Value,
                                            (y.Element("isLatest") != null && y.Element("isLatest").Value == "true")
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
                        
                    }
                    
                }
                catch(Exception e)
                {
                    MessageBox.Show(e.Message);
                }
                UpdateFinished(null, EventArgs.Empty);
            });
            th.Start();
        }

        public static List<ISOInfo> ISOs = new List<ISOInfo>();


        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public static ISOV GetFromFile(string filename, bool fast)
        {
            while (IsUpdating) Thread.Sleep(100);

            ISOV resk = null;

            var s = ISOs.FirstOrDefault(x => Regex.IsMatch(Path.GetFileName(filename), x.Filename)); // find by filename regex

            /*if (s != null)
            {
                resk = s.LatestVersion ?? new ISOV("nover", s.Name, "", s.Filename, true) {Parent=s};
            }*/
            if (s != null && s.LatestVersion == null)
                resk = new ISOV("nover", s.Name, "", s.Filename, true) {Parent = s};
            else
            {
                if (s != null && s.Versions.Count == 1)
                {
                    resk = s.LatestVersion;
                }
                else
                {
                    var sta = s == null ? ISOs.SelectMany(x => x.Versions) : s.Versions;
                    var st =
                        sta.FirstOrDefault(
                            x =>
                                !x.Filename.StartsWith("/") && 
                                string.Equals(Path.GetFileName(filename).Trim(), x.Filename.Trim(),
                                        StringComparison.CurrentCultureIgnoreCase)); // find by version filename regex or string

                    if (st == null)
                    {
                        st =
                        sta.FirstOrDefault(
                            x =>
                                x.Filename.StartsWith("/") &&
                                    Regex.IsMatch(Path.GetFileName(filename), x.Filename.Substring(1)));

                        if (st == null)
                        {
                            var md5 = fast ? "" : Utils.FileHash(filename, "md5");
                            st = (fast
                                ? null
                                : sta.FirstOrDefault(
                                    x =>
                                        x.Hash ==
                                        (x.Hash.Contains(':') ? Utils.FileHash(filename, x.Hash.Split(':')[0]) : md5)));
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
        /// Initialise une nouvelle instance de la classe <see cref="ISOCat"/>.
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
         
        public string GetName()
        {
            return GetName(Thread.CurrentThread.CurrentUICulture);
        }

        public string GetName(CultureInfo c)
        {
            return Names.ContainsKey(c) ? Names[c] : EnglishName;
        }

        public static ISOCat Empty => new ISOCat(-1, "", new Dictionary<CultureInfo, string>());

        public static Dictionary<int, ISOCat> Categories
        {
            get { return _categories; }
            set
            {
                _categories = value;
                if (!_categories.ContainsKey(-1))
                    _categories.Add(-1, Empty);
            }
        }
    }
}