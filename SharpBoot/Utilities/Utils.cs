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
        public static readonly Random CurrentRandom = new Random();

        public static bool Is64 => Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE").IndexOf("64") > 0;

        public static void CallAdminProcess(params string[] args)
        {
            var d = FileIO.GetTemporaryDirectory();
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

            Utils.WaitWhile(() => !File.Exists(p.StartInfo.FileName));

            try
            {
                p.Start();
            }
            catch (Win32Exception e) when (e.NativeErrorCode == WinError.ERROR_CANCELLED)
            {
                throw new OperationCanceledException(Strings.OpCancelled, e);
            }

            p.WaitForExit();

            FileIO.SafeDel(d);
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

        public static string RandomString(int Size)
        {
            var input = "abcdefghijklmnopqrstuvwxyz0123456789";
            var chars = Enumerable.Range(0, Size)
                .Select(x => input[CurrentRandom.Next(0, input.Length)]);
            return new string(chars.ToArray());
        }

        public static string[] AddRecommended(this string[] arr, int recIndex)
        {
            var copy = (string[]) arr.Clone();
            var rec = copy[recIndex] + " " + Strings.Recommended;
            copy[recIndex] = copy[0];
            copy[0] = rec;
            return copy;
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

        public static bool WaitWhile(Func<bool> predicate, int max=3000)
        {
            var start = DateTime.Now;

            while (predicate())
            {
                if (max != -1 && (DateTime.Now - start).TotalMilliseconds >= max)
                    return false;
            }

            return true;
        }

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

        public static IEnumerable<T> ConcatOne<T>(this IEnumerable<T> e, T elem)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(e));

            foreach (var x in e)
                yield return x;

            yield return elem;
        }
    }

    public class OrdinalStringComparer : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            return string.Compare(x, y, StringComparison.Ordinal);
        }
    }
}