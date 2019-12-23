using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.Cache;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32.SafeHandles;
using SharpBoot.Forms;
using SharpBoot.Properties;

// ReSharper disable UnusedMember.Local
// ReSharper disable EventNeverSubscribedTo.Local

namespace SharpBoot.Utilities
{
    public static class Utils
    {
        public static Random CurrentRandom;

        public static bool Is64 => Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE").IndexOf("64") > 0;

        public static void CallAdminProcess(params string[] args)
        {
            var d = Program.GetTemporaryDirectory();
            var exepath = Path.Combine(d, "adminprocess.exe");
            File.WriteAllBytes(exepath, Resources.adminprocess);

            var p = new Process
            {
                StartInfo =
                {
                    CreateNoWindow = true,
                    UseShellExecute = true,
                    FileName = exepath,
                    Verb = "runas",
                    Arguments = string.Join(" ", args)
                }
            };
            p.Start();
            p.WaitForExit();

            Program.SafeDel(d);
        }

        public static byte[] ToByteArray(this Image img)
        {
            var ms = new MemoryStream();
            img.Save(ms, ImageFormat.Png);
            return ms.ToArray();
        }

        public static Image GetFlag(string twocode)
        {
            if (twocode == "en") return Resources.flag_usa;
            var dc = new List<string> {"de", "fr", "ro", "zh-Hans", "zh-Hant", "ru", "uk", "es", "cs", "it", "pt", "pl", "hu"};
            var index = dc.IndexOf(twocode);
            return index == -1 ? null : About.Flags[index];
        }

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
                    if (Program.IsMono && Program.IsLinux)
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

        public static string RandomString(int Size)
        {
            var input = "abcdefghijklmnopqrstuvwxyz0123456789";
            var chars = Enumerable.Range(0, Size)
                .Select(x => input[CurrentRandom.Next(0, input.Length)]);
            return new string(chars.ToArray());
        }

        public static string MakeURLRandom(string url)
        {
            if (url.Contains("?")) url += "&";
            else url += "?";
            url += RandomString(5);
            url += "=";
            url += RandomString(5);
            return url;
        }

        public static List<string> AddRecommended(this List<string> arr, int recIndex)
        {
            var a = arr.ToList();
            var f = a[recIndex] + " " + Strings.Recommended;
            a.RemoveAt(recIndex);
            //a.Sort();
            a.Insert(0, f);
            return a;
        }

        public static string[] AddRecommended(this string[] arr, int recIndex)
        {
            return AddRecommended(arr.ToList(), recIndex).ToArray();
        }

        public static string RemoveRecommended(this string s)
        {
            return s.Replace(" " + Strings.Recommended, "");
        }



        // thanks http://www.codeproject.com/Articles/115598/Formatting-a-Drive-using-C-and-WMI
        public static string FormatEx(this string s, params object[] args)
        {
            return string.Format(s, args);
        }
    }
}