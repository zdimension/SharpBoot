using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
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

    public enum IsoCategory
    {
        None = -1,

        Backup = 0,
        Bios = 1,
        CPU = 2,
        Linux = 3,
        Partition = 4,
        Password = 5,
        RAM = 6,
        Recovery = 7,
        Testing = 8,
        Utility = 9
    }

    public class ISOInfo
    {
        public static string GetCatString(IsoCategory ct)
        {
            var inttocat = new Dictionary<int, string>
            {
                {-1, ""},
                {0, ISOCat.Backup},
                {1, ISOCat.Bios},
                {2, ISOCat.CPU},
                {3, ISOCat.Linux},
                {4, ISOCat.Partition},
                {5, ISOCat.Password},
                {6, ISOCat.RAM},
                {7, ISOCat.Recovery},
                {8, ISOCat.Testing},
                {9, ISOCat.Utility}
            };
            return inttocat[(int) ct];
        }

        public string Name { get; set; }

        public string Description
            =>
                Descriptions.ContainsKey(Thread.CurrentThread.CurrentUICulture)
                    ? Descriptions[Thread.CurrentThread.CurrentUICulture]
                    : (Descriptions.ContainsKey(new CultureInfo("en")) ? Descriptions[new CultureInfo("en")] : "");

        public string CategoryTxt => GetCatString(Category);

        public IsoCategory Category { get; set; }

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

        public ISOInfo(string name, Dictionary<CultureInfo, string> descs, IsoCategory cat, string fn = "", params ISOV[] vers)
        {
            Name = name;
            Descriptions = descs;
            Category = cat;
            Filename = fn;
            Versions = vers.ToList();
            Versions.ForEach(x => x.Parent = this);
            if(!Versions.Any(x => x.Latest) && Versions.Count > 0)
            {
                Versions.Where(t => LatestVersion == t).ToList()[0].Latest = true;
            }
        }

        public static void UpdateISOs()
        {
            try
            {
                var wc = new WebClient {Encoding = Encoding.UTF8};
                var appsxml = wc.DownloadString("http://www.zdimension.tk/sharpboot/apps.php?s");
                appsxml = wc.DownloadString("http://www.zdimension.tk/sharpboot/apps.php?s");

                if (appsxml.Contains("</apps>"))
                {

                    Settings.Default.AppsXml = appsxml;

                    Settings.Default.LastAppsUpdate = DateTime.Now;

                    Settings.Default.Save();
                }
            }
            catch
            {

            }
        }

        public static bool IsUpdateAvailable()
        {
            try
            {
                var wc = new WebClient {Encoding = Encoding.UTF8};
                var temporary = wc.DownloadString("http://www.zdimension.tk/sharpboot/apps.php");
                    // Strangely when a PHP page is updated you need to request it twice to see the update
                var lastappsdate = DateTime.Parse(wc.DownloadString("http://www.zdimension.tk/sharpboot/apps.php"));
                return lastappsdate > Settings.Default.LastAppsUpdate;
            }
            catch
            {
                return false;
            }
        }

        public static event EventHandler UpdateFinished = delegate {  };


        public static void RefreshISOs()
        {
            /*bool upd = IsUpdateAvailable();

            if (upd)*/

            var th = new Thread(() =>
            {
                try
                {
                    Thread.CurrentThread.CurrentCulture = new CultureInfo(Settings.Default.Lang);
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo(Settings.Default.Lang);

                    UpdateISOs();

                    var xd = XDocument.Parse(Settings.Default.AppsXml);

                    ISOs = xd.Element("apps").Elements("app").Select(x =>
                        new ISOInfo(
                            x.Element("name").Value,
                            x.Element("description")
                                .Elements("desc")
                                .Select(y => new {Lang = new CultureInfo(y.Attribute("lang").Value), Val = y.Value})
                                .ToDictionary(k => k.Lang, k => k.Val),
                            (IsoCategory) int.Parse(x.Element("category").Value),
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
                }
                catch
                {

                }
                UpdateFinished(null, EventArgs.Empty);
            });
            th.Start();
        }

        public static List<ISOInfo> ISOs = new List<ISOInfo>();


        public static ISOInfo FromFile(string f)
        {
            return null;
        }

        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public static ISOV GetFromFile(string filename, bool fast)
        {
            ISOV resk = null;

            var s = ISOs.FirstOrDefault(x => Regex.IsMatch(Path.GetFileName(filename), x.Filename));
            /*if (s != null)
            {
                resk = s.LatestVersion ?? new ISOV("nover", s.Name, "", s.Filename, true) {Parent=s};
            }*/
            if (s != null && s.LatestVersion == null) resk = new ISOV("nover", s.Name, "", s.Filename, true) {Parent = s};
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
                                x.Filename.StartsWith("/")
                                    ? Regex.IsMatch(Path.GetFileName(filename), x.Filename.Substring(1))
                                    : string.Equals(Path.GetFileName(filename).Trim(), x.Filename.Trim(),
                                        StringComparison.CurrentCultureIgnoreCase));
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

                    resk = st ?? (s == null ? null : new ISOV("nover", s.Name, "", s.Filename, true) {Parent = s});
                }
            }

            return resk;
        }
    }
}