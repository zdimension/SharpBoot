using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;

namespace SharpBoot.Utilities
{
    public static class FileIO
    {
        public static void ClrTmp(bool first = false)
        {
            Directory.GetDirectories(Path.GetTempPath())
                .Where(x => Path.GetFileName(x).StartsWith("SharpBoot_") && (first || !QEMU.Paths.Contains(x)))
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

        public static string GetFileSizeString(string file)
        {
            var b = new FileInfo(file).Length;
            return GetSizeString(b);
        }

        [DllImport("Shlwapi.dll", CharSet = CharSet.Auto)]
        public static extern long StrFormatByteSize(long fileSize, StringBuilder buffer, int bufferSize);

        public static string GetSizeString(long file, int decimals=0)
        {
            var suf = Strings.SizeSuffixes.Split(',').Select(x => x + Strings.FileUnit).ToArray();
            if (file == 0)
                return "0 " + suf[0];
            var bytes = Math.Abs(file);
            var place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            var num = bytes / Math.Pow(1024, place);
            return (Math.Sign(file) * num).ToString("F" + decimals) + " " + suf[place];
        }

        public static string GetSpeedString(long speed, int decimals=0)
        {
            return GetSizeString(speed, decimals) + "/" + Strings.SecondUnit;
        }

        public static string GetTemporaryDirectory()
        {
            var tempDirectory = Path.Combine(Path.GetTempPath(), "SharpBoot_" + Path.GetRandomFileName());
            Directory.CreateDirectory(tempDirectory);
            return tempDirectory;
        }
    }
}
