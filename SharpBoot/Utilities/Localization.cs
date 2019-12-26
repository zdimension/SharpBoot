using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SharpBoot.Forms;
using SharpBoot.Models;
using SharpBoot.Properties;

namespace SharpBoot.Utilities
{
    class Localization
    {
        public static IEnumerable<CultureInfo> GetAvailableCultures(Type t)
        {
            var rm = new ResourceManager(t);

            return CultureInfo.GetCultures(CultureTypes.AllCultures)
                .Where(culture =>
                {
                    try
                    {
                        return !culture.Equals(CultureInfo.InvariantCulture) &&
                               rm.GetResourceSet(culture, true, false) != null;
                    }
                    catch (CultureNotFoundException)
                    {
                        return false;
                    }
                });
        }

        public static CultureInfo GetSystemCulture()
        {
            var systemLng = CultureInfo.InstalledUICulture;
            if (!systemLng.IsNeutralCulture)
                systemLng = systemLng.Parent;
            return systemLng;
        }

        public static bool IsSystemCultureSupported()
        {
            var systemLng = GetSystemCulture();

            return GetAvailableCultures().Any(x => x.ThreeLetterISOLanguageName == systemLng.ThreeLetterISOLanguageName);
        }

        public static IOrderedEnumerable<CultureInfo> GetDisplayCultures()
        {
            var result = GetAvailableCultures();

            if (!IsSystemCultureSupported())
                result = result.ConcatOne(GetSystemCulture());

            return result.Distinct().OrderBy(x => x.Name, new OrdinalStringComparer());
        }

        public static void UpdateThreadCulture()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(Settings.Default.Lang);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(Settings.Default.Lang);
        }

        public static IEnumerable<CultureInfo> GetAvailableCultures()
        {
            return GetAvailableCultures(typeof(Strings)).Concat(GetAvailableCultures(typeof(ISOCat)));
        }

        public static void SetAppLanguage(CultureInfo c)
        {
            Settings.Default.Lang = c.Name;
            Settings.Default.Save();
            UpdateThreadCulture();
            Settings.Default.Save();
            ISOInfo.RefreshISOs();
        }

        public static CultureInfo GetCulture()
        {
            return new CultureInfo(Settings.Default.Lang);
        }

        public static Image GetFlag(string twocode)
        {
            if (twocode == "en") return Resources.flag_usa;
            var dc = new List<string> {"de", "fr", "ro", "zh-Hans", "zh-Hant", "ru", "uk", "es", "cs", "it", "pt", "pl", "hu"};
            var index = dc.IndexOf(twocode);
            return index == -1 ? null : About.Flags[index];
        }
    }
}
