using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.Cache;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32.SafeHandles;
using SharpBoot.Forms;
using SharpBoot.Models;
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
            var d = GetTemporaryDirectory();
            var exepath = Path.Combine(d, "adminprocess.exe");
            File.WriteAllBytes(exepath, Resources.adminprocess);

            var p = new Process
            {
                StartInfo =
                {
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    UseShellExecute = true,
                    FileName = exepath,
                    Verb = "runas",
                    Arguments = string.Join(" ", args)
                }
            };
            p.Start();
            p.WaitForExit();

            SafeDel(d);
        }

        public static void AttemptTry(Action func, int n=5)
        {
            for (var j = 0; j < n; j++)
                try
                {
                    func();
                    return;
                }
                catch (OperationCanceledException)
                {
                    return;
                }
                catch
                {
                    if (j == n - 1)
                        throw;
                }
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
                    if (IsMono && IsLinux)
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
            return String.Format(s, args);
        }

        public enum Platform
        {
            Windows,
            Linux,
            Mac
        }

        public static List<CultureInfo> UseSystemSize => new List<CultureInfo>();
        public static bool IsMono => Type.GetType("Mono.Runtime") != null;
        public static bool IsLinux => RunningPlatform() == Platform.Linux;
        public static bool IsWin => RunningPlatform() == Platform.Windows;

        public static void HandleUnhandled(Exception ex, string title = "Unhandled exception")
        {
            if (ex is FileNotFoundException)
                MessageBox.Show(((FileNotFoundException) ex).FileName);
            MessageBox.Show(title + ": \n" + ex.Message + "\n\n" + ex.StackTrace, title);
        }

        public static string GetVersion()
        {
            var v = Assembly.GetEntryAssembly().GetName().Version;
            return v.Major + "." + v.Minor + (v.Build == 0 ? "" : "." + v.Build);
        }

        public static Encoding GetEnc()
        {
            switch (Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName)
            {
                case "ru":
                case "uk":
                    return Encoding.GetEncoding(866);
                default:
                    return Encoding.GetEncoding(437);
            }
        }

        public static void ClrTmp(bool first = false)
        {
            Directory.GetDirectories(Path.GetTempPath())
                .Where(x => Path.GetFileName(x).StartsWith("SharpBoot_") && (first || !QEMUISO.Paths.Contains(x)))
                .ToList()
                .ForEach(SafeDel);
        }

        public static void SafeDel(string d)
        {
            for (var i = 0; i < 3 && Directory.Exists(d); i++)
            {
                try
                {
                    Directory.Delete(d, true);
                }
                catch
                {
                    // ignored
                }
            }
        }

        public static void SetAppLng(CultureInfo c)
        {
            Settings.Default.Lang = c.Name;
            Settings.Default.Save();
            Thread.CurrentThread.CurrentCulture = new CultureInfo(Settings.Default.Lang);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(Settings.Default.Lang);
            Settings.Default.Save();
            ISOInfo.RefreshISOs();
        }

        public static CultureInfo GetCulture()
        {
            return new CultureInfo(Settings.Default.Lang);
        }

        public static string GetFileSizeString(string file)
        {
            var b = new FileInfo(file).Length;
            return GetSizeString(b);
        }

        [DllImport("Shlwapi.dll", CharSet = CharSet.Auto)]
        public static extern long StrFormatByteSize(long fileSize, StringBuilder buffer, int bufferSize);

        /// http://stackoverflow.com/q/10138040/2196124
        public static Platform RunningPlatform()
        {
            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.Unix:
                    // Well, there are chances MacOSX is reported as Unix instead of MacOSX.
                    // Instead of platform check, we'll do a feature checks (Mac specific root folders)
                    if (Directory.Exists("/Applications")
                        & Directory.Exists("/System")
                        & Directory.Exists("/Users")
                        & Directory.Exists("/Volumes"))
                        return Platform.Mac;
                    else
                        return Platform.Linux;

                case PlatformID.MacOSX:
                    return Platform.Mac;

                default:
                    return Platform.Windows;
            }
        }

        public static string GetSizeString(long file)
        {
            if (UseSystemSize.Contains(Thread.CurrentThread.CurrentUICulture))
            {
                var sb = new StringBuilder(20);
                StrFormatByteSize(file, sb, sb.Capacity);
                return sb.ToString();
            }

            var suf = Strings.SizeSuffixes.Split(',').Select(x => x + Strings.FileUnit).ToArray();
            if (file == 0)
                return "0 " + suf[0];
            var bytes = Math.Abs(file);
            var place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            var num = Math.Round(bytes / Math.Pow(1024, place), 1);
            return Math.Sign(file) * num + " " + suf[place];
        }

        public static string GetTemporaryDirectory()
        {
            var tempDirectory = Path.Combine(Path.GetTempPath(), "SharpBoot_" + Path.GetRandomFileName());
            Directory.CreateDirectory(tempDirectory);
            return tempDirectory;
        }

        public static void InvokeIfRequired(this ISynchronizeInvoke obj, MethodInvoker action)
        {
            if (obj.InvokeRequired)
            {
                obj.Invoke(action, new object[0]);
            }
            else
            {
                action();
            }
        }
    }
}