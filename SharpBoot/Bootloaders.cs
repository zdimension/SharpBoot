using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Management;
using System.Threading;
using SharpBoot.Properties;

namespace SharpBoot
{
    public interface IBootloader
    {
        string GetCode(BootMenu menu);

        string GetCode(BootMenuItem item);

        void SetImage(Image img, Size sz);

        string BinFile { get; set; }

        byte[] Archive { get; set; }

        string FolderName { get; set; }
        string Name { get; set; }

        string FileExt { get; set; }

        string WorkingDir { get; set; }

        string CmdArgs { get; set; }

        Size Resolution { get; set; }

        bool SupportAccent { get; set; }
    }

    public class Syslinux : IBootloader
    {
        public string CmdArgs { get; set; } = " -iso-level 4 ";
        public string WorkingDir { get; set; } = "";

        public string FolderName { get; set; } = "syslinux";
        public string Name { get; set; } = "Syslinux";

        public string FileExt { get; set; } = ".cfg";

        public Size Resolution { get; set; } = new Size(640, 480);
        public bool SupportAccent { get; set; } = true;

        public string GetCode(BootMenu menu)
        {
            var code = "";

            code += "INCLUDE /boot/syslinux/theme.cfg\n";
            code += "MENU title " + menu.Title.RemoveAccent() + "\n";


            if (menu.MainMenu)
                code += "TIMEOUT 100\n";
            else
                code += "### MENU START\n" +
                        "LABEL mainmenu\n" +
                        "MENU LABEL " + Strings.MainMenu.RemoveAccent() + "\n" +
                        "KERNEL /boot/syslinux/vesamenu.c32\n" +
                        "APPEND /boot/syslinux/syslinux.cfg\n" +
                        "### MENU END\n";

            menu.Items.ForEach(x => code += GetCode(x));

            return code;
        }

        public string GetCode(BootMenuItem item)
        {
            var code = "";


            switch (item.Type)
            {
                case MenuItemType.BootHDD:
                    code += "LABEL localboot\n";
                    code += "localboot 0x80\n";
                    break;
                case MenuItemType.Category:
                    code += "LABEL -\n";
                    code += "KERNEL /boot/syslinux/vesamenu.c32\n";
                    code += "APPEND /boot/syslinux/" + item.IsoName + ".cfg\n";
                    break;
                case MenuItemType.ISO:
                    code += "LABEL -\n";
                    code += "LINUX /boot/syslinux/grub.exe\n";
                    code +=
                        string.Format(
                            "APPEND --config-file=\"ls /images/{0} || find --set-root /images/{0};map /images/{0} (0xff);map --hook;root (0xff);chainloader (0xff);boot\"\n",
                            item.IsoName);
                    break;
                case MenuItemType.IMG:
                    code += "LABEL -\n";
                    code += "LINUX /boot/syslinux/grub.exe\n";
                    code +=
                        string.Format(
                            "APPEND --config-file=\"ls /images/{0} || find --set-root /images/{0};map /images/{0} (fd0);map --hook;chainloader (fd0)+1;rootnoverify (fd0);boot\"\n",
                            item.IsoName);
                    break;
            }

            code += "MENU LABEL " + item.Name.RemoveAccent() + "\n";

            if (item.Start)
            {
                code += "MENU START\n";
                code += "MENU DEFAULT\n";
            }

            code += "TEXT HELP\n";
            code += splitwidth(item.Description.RemoveAccent(), 78) + "\n";
            code += "ENDTEXT\n";

            return code;
        }

        public void SetImage(Image image, Size sz)
        {
            var width = sz.Width;
            var height = sz.Height;

            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height, PixelFormat.Format16bppRgb555);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            destImage.Save(Path.Combine(WorkingDir, "sharpboot.jpg"), ImageFormat.Jpeg);
        }

        public string BinFile { get; set; } = "boot/syslinux/isolinux.bin";
        public byte[] Archive { get; set; } = Resources.syslinux1;

        private static string splitwidth(string s, int w)
        {
            var words = s.Split(' ');
            var lines = new List<string>();
            var currentLine = "";

            foreach (var currentWord in words)
            {
                if ((currentLine.Length > w) ||
                    ((currentLine.Length + currentWord.Length) > w))
                {
                    lines.Add(currentLine);
                    currentLine = "";
                }

                if (currentLine.Length > 0)
                    currentLine += " " + currentWord;
                else
                    currentLine += currentWord;
            }

            if (currentLine.Length > 0)
                lines.Add(currentLine);


            return string.Join("\n", lines);
        }
    }

    public class Grub4DOS : IBootloader
    {
        public string CmdArgs { get; set; } = " ";
        public string WorkingDir { get; set; } = "";

        public string Name { get; set; } = "Grub4DOS";
        public string FileExt { get; set; } = ".lst";
        public string FolderName { get; set; } = "grub4dos";

        public Size Resolution { get; set; } = new Size(640, 480);
        public bool SupportAccent { get; set; } = false;

        public string GetCode(BootMenu menu)
        {
            var code = "";

            //code += "color magenta/white white/magenta black/white black/white\n";

            if (noback) code += $"graphicsmode -1 {Resolution.Width} {Resolution.Height} 32\n";
            else code += "splashimage /boot/grub4dos/sharpboot.xpm.lzma\n";

            if (menu.MainMenu)
            {
                code += "timeout 100\n\n";
            }
            else code += "title " + Strings.MainMenu.RemoveAccent().Trim() + "\nconfigfile /menu.lst\n\n";


            menu.Items.ForEach(x => code += GetCode(x));

            return code;
        }

        public string GetCode(BootMenuItem item)
        {
            var code = "";

            code += "title " + item.Name.RemoveAccent() + "\\n";

            code += item.Description.RemoveAccent().Trim().Replace("\r\n", "\n").Replace("\n", "\\n") + "\n";

            switch (item.Type)
            {
                case MenuItemType.BootHDD:
                    code += "map (hd0) (hd1)\n";
                    code += "map (hd1) (hd0)\n";
                    code += "map --hook\n";
                    code += "chainloader (hd0,0)\n";
                    break;
                case MenuItemType.Category:
                    code += "configfile /boot/grub4dos/" + item.IsoName + ".lst\n";
                    break;
                case MenuItemType.IMG:
                case MenuItemType.ISO:
                    code +=
                        string.Format(
                            "ls /images/{0} || find --set-root /images/{0}\nmap --heads=0 --sectors-per-track=0 /images/{0} (0xff) || map --heads=0 --sectors-per-track=0 --mem /images/{0} (0xff)\nmap --hook\nchainloader (0xff)\n",
                            item.IsoName);
                    break;
            }

            code += "\n";

            return code;
        }

        private bool noback;

        public void SetImage(Image image, Size sz)
        {
            if (image == null)
            {
                noback = true;
                return;
            }

            var width = sz.Width;
            var height = sz.Height;

            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height, PixelFormat.Format16bppRgb555);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            var imgf = Path.Combine(WorkingDir, "sharpboot.bmp");

            destImage.Save(imgf, ImageFormat.Bmp);

            var convertdir = Path.Combine(WorkingDir, "imagemagick");
            Directory.CreateDirectory(convertdir);
            File.WriteAllBytes(Path.Combine(convertdir, "convert.7z"), Resources.imagemagick);

            var ext = new SevenZipExtractor();
            ext.Extract(Path.Combine(convertdir, "convert.7z"), convertdir);

            var p = new Process
            {
                StartInfo =
                {
                    UseShellExecute = false,
                    FileName = Path.Combine(convertdir, "convert.exe"),
                    CreateNoWindow = true,
                    WorkingDirectory = convertdir
                }
            };
            p.StartInfo.Arguments += " ../sharpboot.bmp ../sharpboot.xpm.lzma";
            Thread.Sleep(300);
            p.Start();
            p.WaitForExit();
            Thread.Sleep(1000);
            File.Delete(imgf);
            ext.Close();
            while (Directory.Exists(convertdir))
            {
                try
                {
                    Directory.Delete(convertdir, true);
                }
                catch
                {

                }
            }
        }

        public string BinFile { get; set; } = "grldr";
        public byte[] Archive { get; set; } = Resources.grub4dos;
    }

    public class SyslinuxTheme
    {
        public int Margin { get; set; } = 4;
        public Size Resolution { get; set; } = new Size(640, 480);

        public bool noback = false;

        public enum ShadowType
        {
            /// <summary>
            ///     No shadowing
            /// </summary>
            none,

            /// <summary>
            ///     Standard shadowing -- foreground pixels are raised
            /// </summary>
            std,

            /// <summary>
            ///     Both background and foreground raised
            /// </summary>
            all,

            /// <summary>
            ///     Background pixels are raised
            /// </summary>
            rev
        }

        public string GetCode()
        {
            return "prompt 0\n" +
                   "UI /boot/syslinux/vesamenu.c32\n" +
                   "menu resolution " + Resolution.Width + " " + Resolution.Height + "\n" +
                   "menu margin " + Margin + "\n" +
                   (noback ? "" : "menu background /boot/syslinux/sharpboot.jpg\n") +
                   string.Concat(Entries.Select(x => x.GetCode())) +
                   "menu helpmsgrow 19\n" +
                   "menu helpmsendrow -1\n" + 
                   (Program.UseCyrillicFont ? "FONT /boot/syslinux/cyrillic_cp866.psf\n" : "");
        }

        public List<ThemeEntry> Entries = new List<ThemeEntry>
        {
            new ThemeEntry("border", "#ffffffff", "#ee000000", ShadowType.std),
            new ThemeEntry("title", "#ffffffff", "#ee000000", ShadowType.std),
            new ThemeEntry("sel", "#ffffffff", "#85000000", ShadowType.std),
            new ThemeEntry("unsel", "#ffffffff", "#ee000000", ShadowType.std),
            new ThemeEntry("pwdheader", "#ff000000", "#99ffffff", ShadowType.rev),
            new ThemeEntry("pwdborder", "#ff000000", "#99ffffff", ShadowType.rev),
            new ThemeEntry("pwdentry", "#ff000000", "#99ffffff", ShadowType.rev),
            new ThemeEntry("hotkey", "#ff00ff00", "#ee000000", ShadowType.std),
            new ThemeEntry("hotsel", "#ffffffff", "#85000000", ShadowType.std)
        };

        public class ThemeEntry
        {
            public ThemeEntry(string name, Color foreground, Color background, ShadowType shadowType = ShadowType.none)
            {
                Name = name;
                Foreground = foreground;
                Background = background;
                ShadowType = shadowType;
            }

            public ThemeEntry(string name, string foreground, string background, ShadowType shadowType = ShadowType.none)
                : this(name, ColorTranslator.FromHtml(foreground), ColorTranslator.FromHtml(background), shadowType)
            {
            }

            public string Name { get; set; }
            public Color Foreground { get; set; }
            public Color Background { get; set; }

            public ShadowType ShadowType { get; set; }

            public string GetCode()
            {
                return $"menu color {Name} 0 {Foreground.ToHexArgb()} {Background.ToHexArgb()} {ShadowType}\n";
            }
        }
    }

    public enum Bootloaders
    {
        Syslinux = 0,
        Grub4Dos = 1
    }
    public class BootloaderInst
    {
        public static void Install(string l, int bl)
        {
            Install(l, (Bootloaders)bl);
        }

        public static void Install(string l, string bl)
        {
            if (bl == "grub4dos") Install(l, Bootloaders.Grub4Dos);
            if (bl == "syslinux") Install(l, Bootloaders.Syslinux);
        }

        public static void Install(string l, Bootloaders bl)
        {
            var exename = bl == Bootloaders.Grub4Dos ? "grubinst.exe" : "syslinux.exe";

            var d = Program.GetTemporaryDirectory();
            var exepath = Path.Combine(d, exename);
            File.WriteAllBytes(exepath, bl == Bootloaders.Grub4Dos ? Resources.grubinst : Resources.syslinux);

            var p = new Process
            {
                StartInfo =
                {
                    CreateNoWindow = true,
                    UseShellExecute = true,
                    FileName = exepath,
                    Verb = "runas"
                }
            };
            var driveletter = l.ToLower().Substring(0, 2);
            if (bl == Bootloaders.Grub4Dos)
            {
                var deviceId = string.Empty;
                var queryResults = new ManagementObjectSearcher(
                    $"ASSOCIATORS OF {{Win32_LogicalDisk.DeviceID='{driveletter}'}} WHERE AssocClass = Win32_LogicalDiskToPartition");
                var partitions = queryResults.Get();
                foreach (var partition in partitions)
                {
                    queryResults = new ManagementObjectSearcher(
                        $"ASSOCIATORS OF {{Win32_DiskPartition.DeviceID='{partition["DeviceID"]}'}} WHERE AssocClass = Win32_DiskDriveToDiskPartition");
                    var drives = queryResults.Get();
                    foreach (var drive in drives)
                        deviceId = drive["DeviceID"].ToString();
                }
                p.StartInfo.Arguments = " --skip-mbr-test (hd" + string.Concat(deviceId.Where(char.IsDigit)) + ")";
            }
            else
            {
                p.StartInfo.Arguments = " -m -a " + driveletter;
            }
            p.Start();
            p.WaitForExit();
            if (bl == Bootloaders.Grub4Dos)
            {
                File.WriteAllBytes(Path.Combine(driveletter, "grldr"), Resources.grldr);
            }

            Program.SafeDel(d);
        }
    }

    public class driveitem
    {
        public string Disp { get; set; }
        public DriveInfo Value { get; set; }
    }
}