using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
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

        public static List<CultureInfo> GetAvailableCultures()
        {
            var result = Localization.GetAvailableCultures(typeof(Strings));

            result.AddRange(Localization.GetAvailableCultures(typeof(ISOCat)));

            var systemLng = GetSystemCulture();

            if (result.All(x => x.ThreeLetterISOLanguageName != systemLng.ThreeLetterISOLanguageName))
                result.Add(systemLng);

            result = result.Distinct().ToList();
            result.Sort((x, y) => String.Compare(x.NativeName, y.NativeName, StringComparison.Ordinal));

            return result;
        }
    }
}
