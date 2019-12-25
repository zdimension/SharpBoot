using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SharpBoot.Utilities
{
    public static class Updater
    {
        public static bool Check()
        {
            try
            {
                using (var wb = new WebClient())
                {
                    wb.Headers["User-Agent"] =
                        "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2526.58 Safari/537.36";
                    var ct = wb.DownloadString("https://api.github.com/repos/zdimension/SharpBoot/releases/latest");

                    var lnid = ct.IndexOf("tag_name");
                    ct = ct.Substring(lnid + 13);
                    ct = ct.Substring(0, ct.IndexOf('"'));

                    var v = Version.Parse(ct);
                    //v = new Version(3, 7);
                    return v > Assembly.GetEntryAssembly().GetName().Version;
                }
            }
            catch
            {
                // ignored
            }

            return false;
        }
    }
}
