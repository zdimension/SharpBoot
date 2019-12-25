using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using SharpBoot.Models;

namespace SharpBoot.Utilities
{
    class Localization
    {
        public static List<CultureInfo> GetAvailableCultures(Type t)
        {
            var result = new List<CultureInfo>();
            var rm = new ResourceManager(t);

            var cultures = CultureInfo.GetCultures(CultureTypes.AllCultures);
            foreach (var culture in cultures)
                try
                {
                    if (culture.Equals(CultureInfo.InvariantCulture)) continue;

                    var rs = rm.GetResourceSet(culture, true, false);
                    if (rs != null)
                        result.Add(culture);
                }
                catch (CultureNotFoundException)
                {
                    //NOP
                }

            return result;
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

        public static List<CultureInfo> GetDisplayCultures()
        {
            var result = GetAvailableCultures();

            var systemLng = GetSystemCulture();

            if (!IsSystemCultureSupported())
                result.Add(systemLng);

            result = result.Distinct().ToList();
            result.Sort((x, y) => string.Compare(x.NativeName, y.NativeName, StringComparison.Ordinal));

            return result;
        }

        public static List<CultureInfo> GetAvailableCultures()
        {
            var result = GetAvailableCultures(typeof(Strings));

            result.AddRange(GetAvailableCultures(typeof(ISOCat)));

            return result;
        }
    }
}
