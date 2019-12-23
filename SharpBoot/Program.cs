using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using SharpBoot.Forms;
using SharpBoot.Models;
using SharpBoot.Properties;
using SharpBoot.Utilities;

namespace SharpBoot
{
    public static class Program
    {
        public enum Platform
        {
            Windows,
            Linux,
            Mac
        }

        public static string editcode = "";
        public static string fpath = "";

        public static readonly string DirCharStr = Path.DirectorySeparatorChar.ToString();

        public static List<CultureInfo> UseSystemSize => new List<CultureInfo>();

        public static bool IsMono => Type.GetType("Mono.Runtime") != null;

        public static bool IsLinux => RunningPlatform() == Platform.Linux;

        public static bool IsWin => RunningPlatform() == Platform.Windows;


        /// <summary>
        ///     Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            ServicePointManager.ServerCertificateValidationCallback += delegate { return true; };

            ClrTmp(true);

            Utils.CurrentRandom = new Random();

            Settings.Default.PropertyChanged += Default_PropertyChanged;

            if (Settings.Default.AppsXml == "") Settings.Default.AppsXml = Resources.DefaultISOs;
            ISOInfo.RefreshISOs();


            Thread.CurrentThread.CurrentCulture = new CultureInfo(Settings.Default.Lang);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(Settings.Default.Lang);

            Application.ApplicationExit += Application_ApplicationExit;
            Application.ThreadException += Application_ThreadException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;

            W7RUtils.Install();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainWindow());

            ClrTmp();
        }

        private static void CurrentDomainOnUnhandledException(object sender,
            UnhandledExceptionEventArgs unhandledExceptionEventArgs)
        {
            HandleUnhandled((Exception) unhandledExceptionEventArgs.ExceptionObject);
        }

        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            HandleUnhandled(e.Exception, "Thread exception");
        }

        private static void HandleUnhandled(Exception ex, string title = "Unhandled exception")
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

        private static void Application_ApplicationExit(object sender, EventArgs e)
        {
            ClrTmp();
        }

        private static void Default_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Settings.Default.Save();
            if (e.PropertyName == "Lang")
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(Settings.Default.Lang);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(Settings.Default.Lang);
                ISOInfo.RefreshISOs();
            }
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
    }
}