using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using SharpBoot.Properties;

namespace SharpBoot
{
    public static class Program
    {
        public static string SevenZipPath = "";

        public static IDictionary<TKey, TValue> ToDictionary<TKey, TValue>(
            this IEnumerable<KeyValuePair<TKey, TValue>> list)
        {
            return list.ToDictionary(x => x.Key, x => x.Value);
        }

        /// <summary>
        ///     Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Settings.Default.PropertyChanged += Default_PropertyChanged;


            ISOInfo.RefreshISOs();


            Thread.CurrentThread.CurrentCulture = new CultureInfo(Settings.Default.Lang);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(Settings.Default.Lang);

            Application.ApplicationExit += Application_ApplicationExit;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainWindow());
        }

        public static string GetVersion()
        {
            var v = Assembly.GetEntryAssembly().GetName().Version;
            return v.Major + "." + v.Minor;
        }

        public static void ClrTmp()
        {
            Directory.GetDirectories(Path.GetTempPath())
                .Where(x => x.StartsWith("SharpBoot_"))
                .ToList()
                .ForEach(x => new DirectoryInfo(x).Delete(true));
        }

        private static void Application_ApplicationExit(object sender, EventArgs e)
        {
            ClrTmp();
        }

        private static void Default_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Settings.Default.Save();
            Thread.CurrentThread.CurrentCulture = new CultureInfo(Settings.Default.Lang);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(Settings.Default.Lang);
            ISOInfo.RefreshISOs();
        }

        public static string ShortLang()
        {
            return GetCulture().Name;
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

        public static string ToHexArgb(this Color c)
        {
            return "#" + c.A.ToString("X2") + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
        }


        public static string GetSizeUnit()
        {
            return Strings.FileUnit;
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

        public static string GetSizeString(long file)
        {
            string[] suf =
            {
                GetSizeUnit(), "K" + GetSizeUnit(), "M" + GetSizeUnit(), "G" + GetSizeUnit(),
                "T" + GetSizeUnit(), "P" + GetSizeUnit(), "E" + GetSizeUnit()
            };
            if (file == 0)
                return "0 " + suf[0];
            var bytes = Math.Abs(file);
            var place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            var num = Math.Round(bytes / Math.Pow(1024, place), 1);
            return (Math.Sign(file) * num) + " " + suf[place];
        }

        public static string RemoveAccent(this string str)
        {
            var normalizedString = str.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in from c in normalizedString let unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c) where unicodeCategory != UnicodeCategory.NonSpacingMark select c)
            {
                stringBuilder.Append(c);
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        public static string GetTemporaryDirectory()
        {
            var tempDirectory = Path.Combine(Path.GetTempPath(), "SharpBoot_" + Path.GetRandomFileName());
            Directory.CreateDirectory(tempDirectory);
            return tempDirectory;
        }
    }
}