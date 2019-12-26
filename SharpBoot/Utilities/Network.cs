using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SharpBoot.Utilities
{
    public static class Network
    {
        public static string DownloadWithoutCache(string url, bool redl = true)
        {
            url = MakeURLRandom(url);
            var res = "";
            Stream remote = null;
            WebResponse resp = null;
            try
            {
                var req = WebRequest.Create(url);
                req.CachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);
                try
                {
                    resp = req.GetResponse();
                }
                catch (WebException)
                {
                    if (Utils.IsMono && Utils.IsLinux)
                    {
                        var p = new Process {StartInfo = new ProcessStartInfo("mozroots", "--import --sync")};
                        p.Start();
                        p.WaitForExit(10000);
                        resp = req.GetResponse();
                    }
                }

                if (resp != null)
                {
                    remote = resp.GetResponseStream();

                    var mem = new MemoryStream();
                    var buf = new byte[1024];
                    var br = 0;
                    do
                    {
                        br = remote.Read(buf, 0, 1024);
                        mem.Write(buf, 0, br);
                    } while (br > 0);

                    mem.Position = 0;
                    res = new StreamReader(mem).ReadToEnd();
                }
                else
                {
                    MessageBox.Show("resp is null");
                }
            }
            catch
            {
                // ignored
            }
            finally
            {
                resp?.Close();
                remote?.Close();
            }

            return res;
        }

        public static string MakeURLRandom(string url)
        {
            if (url.Contains("?")) url += "&";
            else url += "?";
            url += Utils.RandomString(5);
            url += "=";
            url += Utils.RandomString(5);
            return url;
        }
    }
}
