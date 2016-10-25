using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Management;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using SharpBoot.Properties;

namespace SharpBoot
{
    public abstract class IBootloader
    {
        public abstract string GetCode(BootMenu menu);
        public abstract string GetCode(BootMenuItem item);
        public abstract void SetImage(Image img, Size sz);
        public abstract string BinFile { get; set; }
        public abstract byte[] Archive { get; set; }
        public abstract string FolderName { get; set; }
        public abstract string DisplayName { get; set; }
        public abstract string FileExt { get; set; }
        public string WorkingDir { get; set; }
        public abstract string CmdArgs { get; set; }
        public Size Resolution { get; set; } = new Size(640, 480);
        public abstract bool SupportAccent { get; set; }
        public abstract long TotalSize { get; set; }
        public abstract void Install(string driveLetter);
    }

    public class Syslinux : IBootloader
    {
        public override string CmdArgs { get; set; } = " -iso-level 4 ";
        public override string FolderName { get; set; } = "syslinux";
        public override string DisplayName { get; set; } = "Syslinux";

        public override string FileExt { get; set; } = ".cfg";

        public override bool SupportAccent { get; set; } = true;

        public override string GetCode(BootMenu menu)
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

        public override string GetCode(BootMenuItem item)
        {
            if (item.CustomCode != "") return item.CustomCode;

            var code = "";

            code += "LABEL -\n";
            switch (item.Type)
            {
                case EntryType.BootHDD:
                    code += "localboot 0x80\n";
                    break;
                case EntryType.Category:
                    code += "KERNEL /boot/syslinux/vesamenu.c32\n";
                    code += "APPEND /boot/syslinux/" + item.IsoName + ".cfg\n";
                    break;
                case EntryType.ISO:
                    code += "LINUX /boot/syslinux/grub.exe\n";
                    code +=
                        string.Format(
                            "APPEND --config-file=\"ls /images/{0} || find --set-root /images/{0};map /images/{0} (0xff);map --hook;root (0xff);chainloader (0xff);boot\"\n",
                            item.IsoName);
                    break;
                case EntryType.IMG:
                    code += "LINUX /boot/syslinux/grub.exe\n";
                    code +=
                        string.Format(
                            "APPEND --config-file=\"ls /images/{0} || find --set-root /images/{0};map /images/{0} (fd0);map --hook;chainloader (fd0)+1;rootnoverify (fd0);boot\"\n",
                            item.IsoName);
                    break;
                case EntryType.NTLDR:
                    code += "COM32 /boot/syslinux/chain.c32\n";
                    code += "APPEND ntldr=/images/" + item.IsoName + "\n";
                    break;
                case EntryType.GRLDR:
                    code += "COM32 /boot/syslinux/chain.c32\n";
                    code += "APPEND grldr=/images/" + item.IsoName + "\n";
                    break;
                case EntryType.CMLDR:
                    code += "COM32 /boot/syslinux/chain.c32\n";
                    code += "APPEND cmldr=/images/" + item.IsoName + "\n";
                    break;
                case EntryType.FreeDOS:
                    code += "COM32 /boot/syslinux/chain.c32\n";
                    code += "APPEND freedos=/images/" + item.IsoName + "\n";
                    break;
                case EntryType.MS_DOS:
                    code += "COM32 /boot/syslinux/chain.c32\n";
                    code += "APPEND msdos=/images/" + item.IsoName + "\n";
                    break;
                case EntryType.MS_DOS_7:
                    code += "COM32 /boot/syslinux/chain.c32\n";
                    code += "APPEND msdos7=/images/" + item.IsoName + "\n";
                    break;
                case EntryType.PC_DOS:
                    code += "COM32 /boot/syslinux/chain.c32\n";
                    code += "APPEND pcdos=/images/" + item.IsoName + "\n";
                    break;
                case EntryType.DRMK:
                    code += "COM32 /boot/syslinux/chain.c32\n";
                    code += "APPEND drmk=/images/" + item.IsoName + "\n";
                    break;
                case EntryType.ReactOS:
                    code += "COM32 /boot/syslinux/chain.c32\n";
                    code += "APPEND reactos=/images/" + item.IsoName + "\n";
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

        public override void SetImage(Image image, Size sz)
        {
            if (image == null) return;

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

        public override string BinFile { get; set; } = "boot/syslinux/isolinux.bin";
        public override byte[] Archive { get; set; } = Resources.syslinux1;
        public override long TotalSize { get; set; } = 1064874;
        public override void Install(string l)
        {
            /*var d = Program.GetTemporaryDirectory();
            var exepath = Path.Combine(d, "syslinux.exe");
            File.WriteAllBytes(exepath, Resources.syslinux);

            var p = new Process
            {
                StartInfo =
                {
                    CreateNoWindow = true,
                    UseShellExecute = true,
                    FileName = exepath,
                    Verb = "runas",
                    Arguments = " -i -m -a " + l.ToLower().Substring(0, 2)
                }
            };
            p.Start();
            p.WaitForExit();

            Program.SafeDel(d);*/

            var syslinux_mbr = new byte[440]
            {
                0x33, 0xc0, 0xfa, 0x8e, 0xd8, 0x8e, 0xd0, 0xbc, 0x00, 0x7c, 0x89, 0xe6,
  0x06, 0x57, 0x8e, 0xc0, 0xfb, 0xfc, 0xbf, 0x00, 0x06, 0xb9, 0x00, 0x01,
  0xf3, 0xa5, 0xea, 0x1f, 0x06, 0x00, 0x00, 0x52, 0x52, 0xb4, 0x41, 0xbb,
  0xaa, 0x55, 0x31, 0xc9, 0x30, 0xf6, 0xf9, 0xcd, 0x13, 0x72, 0x13, 0x81,
  0xfb, 0x55, 0xaa, 0x75, 0x0d, 0xd1, 0xe9, 0x73, 0x09, 0x66, 0xc7, 0x06,
  0x8d, 0x06, 0xb4, 0x42, 0xeb, 0x15, 0x5a, 0xb4, 0x08, 0xcd, 0x13, 0x83,
  0xe1, 0x3f, 0x51, 0x0f, 0xb6, 0xc6, 0x40, 0xf7, 0xe1, 0x52, 0x50, 0x66,
  0x31, 0xc0, 0x66, 0x99, 0xe8, 0x66, 0x00, 0xe8, 0x35, 0x01, 0x4d, 0x69,
  0x73, 0x73, 0x69, 0x6e, 0x67, 0x20, 0x6f, 0x70, 0x65, 0x72, 0x61, 0x74,
  0x69, 0x6e, 0x67, 0x20, 0x73, 0x79, 0x73, 0x74, 0x65, 0x6d, 0x2e, 0x0d,
  0x0a, 0x66, 0x60, 0x66, 0x31, 0xd2, 0xbb, 0x00, 0x7c, 0x66, 0x52, 0x66,
  0x50, 0x06, 0x53, 0x6a, 0x01, 0x6a, 0x10, 0x89, 0xe6, 0x66, 0xf7, 0x36,
  0xf4, 0x7b, 0xc0, 0xe4, 0x06, 0x88, 0xe1, 0x88, 0xc5, 0x92, 0xf6, 0x36,
  0xf8, 0x7b, 0x88, 0xc6, 0x08, 0xe1, 0x41, 0xb8, 0x01, 0x02, 0x8a, 0x16,
  0xfa, 0x7b, 0xcd, 0x13, 0x8d, 0x64, 0x10, 0x66, 0x61, 0xc3, 0xe8, 0xc4,
  0xff, 0xbe, 0xbe, 0x7d, 0xbf, 0xbe, 0x07, 0xb9, 0x20, 0x00, 0xf3, 0xa5,
  0xc3, 0x66, 0x60, 0x89, 0xe5, 0xbb, 0xbe, 0x07, 0xb9, 0x04, 0x00, 0x31,
  0xc0, 0x53, 0x51, 0xf6, 0x07, 0x80, 0x74, 0x03, 0x40, 0x89, 0xde, 0x83,
  0xc3, 0x10, 0xe2, 0xf3, 0x48, 0x74, 0x5b, 0x79, 0x39, 0x59, 0x5b, 0x8a,
  0x47, 0x04, 0x3c, 0x0f, 0x74, 0x06, 0x24, 0x7f, 0x3c, 0x05, 0x75, 0x22,
  0x66, 0x8b, 0x47, 0x08, 0x66, 0x8b, 0x56, 0x14, 0x66, 0x01, 0xd0, 0x66,
  0x21, 0xd2, 0x75, 0x03, 0x66, 0x89, 0xc2, 0xe8, 0xac, 0xff, 0x72, 0x03,
  0xe8, 0xb6, 0xff, 0x66, 0x8b, 0x46, 0x1c, 0xe8, 0xa0, 0xff, 0x83, 0xc3,
  0x10, 0xe2, 0xcc, 0x66, 0x61, 0xc3, 0xe8, 0x76, 0x00, 0x4d, 0x75, 0x6c,
  0x74, 0x69, 0x70, 0x6c, 0x65, 0x20, 0x61, 0x63, 0x74, 0x69, 0x76, 0x65,
  0x20, 0x70, 0x61, 0x72, 0x74, 0x69, 0x74, 0x69, 0x6f, 0x6e, 0x73, 0x2e,
  0x0d, 0x0a, 0x66, 0x8b, 0x44, 0x08, 0x66, 0x03, 0x46, 0x1c, 0x66, 0x89,
  0x44, 0x08, 0xe8, 0x30, 0xff, 0x72, 0x27, 0x66, 0x81, 0x3e, 0x00, 0x7c,
  0x58, 0x46, 0x53, 0x42, 0x75, 0x09, 0x66, 0x83, 0xc0, 0x04, 0xe8, 0x1c,
  0xff, 0x72, 0x13, 0x81, 0x3e, 0xfe, 0x7d, 0x55, 0xaa, 0x0f, 0x85, 0xf2,
  0xfe, 0xbc, 0xfa, 0x7b, 0x5a, 0x5f, 0x07, 0xfa, 0xff, 0xe4, 0xe8, 0x1e,
  0x00, 0x4f, 0x70, 0x65, 0x72, 0x61, 0x74, 0x69, 0x6e, 0x67, 0x20, 0x73,
  0x79, 0x73, 0x74, 0x65, 0x6d, 0x20, 0x6c, 0x6f, 0x61, 0x64, 0x20, 0x65,
  0x72, 0x72, 0x6f, 0x72, 0x2e, 0x0d, 0x0a, 0x5e, 0xac, 0xb4, 0x0e, 0x8a,
  0x3e, 0x62, 0x04, 0xb3, 0x07, 0xcd, 0x10, 0x3c, 0x0a, 0x75, 0xf1, 0xcd,
  0x18, 0xf4, 0xeb, 0xfd, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
  0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
            };

            Utils.InstallMBR(l, syslinux_mbr);
        }

        private static string splitwidth(string s, int w)
        {
            return string.Join("\n", s.Wrap(w));
        }
    }

    public class Grub4DOS : IBootloader
    {
        public override string DisplayName { get; set; } = "Grub4DOS";
        public override string FileExt { get; set; } = ".lst";
        public override string CmdArgs { get; set; } = "";
        public override string FolderName { get; set; } = "grub4dos";
        public override bool SupportAccent { get; set; } = false;

        public override string GetCode(BootMenu menu)
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

        public override string GetCode(BootMenuItem item)
        {
            if (item.CustomCode != "") return item.CustomCode;

            var code = "";

            code += "title " + item.Name.RemoveAccent() + "\\n";

            code += item.Description.RemoveAccent().Trim().Replace("\r\n", "\n").Replace("\n", "\\n") + "\n";

            switch (item.Type)
            {
                case EntryType.BootHDD:
                    code += "map (hd0) (hd1)\n";
                    code += "map (hd1) (hd0)\n";
                    code += "map --hook\n";
                    code += "chainloader (hd0,0)\n";
                    break;
                case EntryType.Category:
                    code += "configfile /boot/grub4dos/" + item.IsoName + ".lst\n";
                    break;
                case EntryType.ISO:
                    code +=
                        string.Format(
                            "ls /images/{0} || find --set-root /images/{0}\nmap /images/{0} (0xff)\nmap --hook\nroot (0xff)\nchainloader (0xff)\n",
                            item.IsoName);
                    break;
                case EntryType.IMG:
                    code +=
                        string.Format(
                            "ls /images/{0} || find --set-root /images/{0}\nmap /images/{0} (fd0)\nmap --hook\nchainloader (fd0)+1\nrootnoverify (fd0)\n",
                            item.IsoName);
                    break;
                case EntryType.NTLDR:
                case EntryType.GRLDR:
                case EntryType.CMLDR:
                case EntryType.FreeDOS:
                case EntryType.MS_DOS:
                case EntryType.MS_DOS_7:
                case EntryType.PC_DOS:
                case EntryType.DRMK:
                case EntryType.ReactOS:
                    code += string.Format(
                        "ls /images/{0} || find --set-root /Images/{0}\nchainloader /images/{0}\n",
                        item.IsoName);
                    break;
            }

            code += "\n";

            return code;
        }

        private bool noback;

        public override void SetImage(Image image, Size sz)
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
            var begin = DateTime.Now;
            while(!File.Exists(p.StartInfo.FileName))
            {
                if ((DateTime.Now - begin).TotalSeconds > 15)
                {
                    break;
                }
            }
            begin = DateTime.Now;
            p.Start();
            while (!File.Exists(Path.Combine(WorkingDir, "sharpboot.xpm.lzma")))
            {
                if (p.HasExited)
                {
                    p.Start();
                    continue;
                }
                if ((DateTime.Now - begin).TotalSeconds > 15)
                {
                    break;
                }
            }
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

        public override string BinFile { get; set; } = "grldr";
        public override byte[] Archive { get; set; } = Resources.grub4dos;

        public override long TotalSize { get; set; } = 280911;

        public override void Install(string l)
        {
            /*var d = Program.GetTemporaryDirectory();
            var exepath = Path.Combine(d, "grubinst.exe");
            File.WriteAllBytes(exepath, Resources.grubinst);

            var p = new Process
            {
                StartInfo =
                {
                    CreateNoWindow = true,
                    UseShellExecute = true,
                    FileName = exepath,
                    Verb = "runas",
                    Arguments =
                        " --skip-mbr-test --no-backup-mbr -t=0 (hd" +
                        string.Concat(Utils.GetPhysicalPath(l.ToLower().Substring(0, 2)).Where(char.IsDigit)) + ")"
                }
            };
            p.Start();
            p.WaitForExit();

            Program.SafeDel(d);*/
            var grub_mbr = new byte[423]
            {
                0xEB, 0x5E, 0x90, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80, 0x00, 0x20, 0x39, 0xFF, 0xFF,
                0x31, 0xDB, 0x8E, 0xD3, 0xBC, 0x80, 0x05, 0xE8, 0x00, 0x00, 0x5B, 0x81,
                0xEB, 0x6A, 0x00, 0xC1, 0xEB, 0x04, 0x8C, 0xC8, 0x01, 0xC3, 0x53, 0x68,
                0x7B, 0x00, 0xCB, 0x68, 0x00, 0x20, 0x17, 0xBC, 0x00, 0x90, 0x80, 0xFA,
                0x00, 0x74, 0x02, 0xB2, 0x80, 0x16, 0x07, 0xB0, 0x10, 0x30, 0xF6, 0x31,
                0xDB, 0xE8, 0xC2, 0x00, 0x72, 0x29, 0x0E, 0x1F, 0x31, 0xF6, 0x31, 0xFF,
                0xB9, 0xDF, 0x00, 0xFC, 0xF3, 0xA5, 0xBB, 0xFC, 0x1F, 0x66, 0xB8, 0x47,
                0x52, 0x55, 0xAA, 0x66, 0x39, 0x07, 0x75, 0x03, 0xE8, 0x98, 0x19, 0xEA,
                0xB8, 0x00, 0x00, 0x20, 0x16, 0x1F, 0x66, 0x39, 0x07, 0x74, 0x1F, 0x80,
                0xF2, 0x80, 0x68, 0xC0, 0x07, 0x07, 0xB0, 0x10, 0x30, 0xF6, 0x31, 0xDB,
                0xE8, 0x87, 0x00, 0x72, 0x05, 0xEA, 0x00, 0x00, 0xC0, 0x07, 0xBE, 0x92,
                0x01, 0xE8, 0xAF, 0x00, 0xEB, 0xFE, 0xBE, 0xBE, 0x01, 0xE8, 0x04, 0x10,
                0xF6, 0x06, 0xA4, 0x01, 0x80, 0x0F, 0x84, 0x6C, 0x19, 0x83, 0xC6, 0x0C,
                0x81, 0xFE, 0xFE, 0x01, 0x72, 0xEB, 0x77, 0x2D, 0xF6, 0x06, 0xA7, 0x01,
                0x01, 0x0F, 0x85, 0x1E, 0x17, 0x80, 0x0E, 0xA7, 0x01, 0x01, 0xF6, 0x06,
                0xA7, 0x01, 0x02, 0x75, 0xD4, 0x68, 0x00, 0x0D, 0x07, 0xB0, 0x04, 0x31,
                0xD2, 0x31, 0xDB, 0xE8, 0x3C, 0x00, 0x73, 0xC2, 0xBE, 0xAD, 0x1C, 0xE8,
                0x69, 0x00, 0xE9, 0xFA, 0x16, 0xF6, 0x06, 0xA7, 0x01, 0x01, 0x0F, 0x85,
                0xF1, 0x16, 0x80, 0x0E, 0xA7, 0x01, 0x01, 0xE9, 0xD2, 0x16, 0x1E, 0x06,
                0x52, 0x56, 0x57, 0x55, 0xF9, 0xCD, 0x13, 0x5D, 0x5F, 0x5E, 0x5A, 0x07,
                0x1F, 0xC3, 0xFA, 0xB8, 0x00, 0x20, 0x8E, 0xD0, 0xBC, 0xDC, 0x8F, 0xFB,
                0x66, 0x61, 0x07, 0x1F, 0xEB, 0x97, 0xB4, 0x02, 0xBF, 0x03, 0x00, 0xB9,
                0x01, 0x00, 0x60, 0x50, 0x53, 0x51, 0xFE, 0xC8, 0x00, 0xC1, 0xD0, 0xE0,
                0x00, 0xC7, 0xB0, 0x01, 0xE8, 0xC7, 0xFF, 0x59, 0x5B, 0x58, 0x72, 0x04,
                0xFE, 0xC8, 0x75, 0xE7, 0x61, 0x73, 0x0B, 0x60, 0x31, 0xC0, 0xE8, 0xB5,
                0xFF, 0x61, 0x4F, 0x75, 0xD9, 0xF9, 0xC3, 0xB4, 0x0E, 0xCD, 0x10, 0x2E,
                0xAC, 0x3C, 0x00, 0x75, 0xF6, 0xC3, 0x0D, 0x0A, 0x4D, 0x69, 0x73, 0x73,
                0x69, 0x6E, 0x67, 0x20, 0x68, 0x65, 0x6C, 0x70, 0x65, 0x72, 0x2E, 0x00,
                0x00, 0x3F, 0xFF
            };

            Utils.InstallMBR(l, grub_mbr);
        }
    }

    public class Grub2 : IBootloader
    {
        public override string GetCode(BootMenu menu)
        {
            var code = "";

            code += $"set gfxmode={Resolution.Width}x{Resolution.Height}\n";
            code += "insmod video_bochs\ninsmod video_cirrus\ninsmod png\n";
            code += "background_image /win32-loader/sharpboot.png\n";

            menu.Items.ForEach(x => code += GetCode(x));

            return code;
        }

        public override string GetCode(BootMenuItem item)
        {
            if (item.CustomCode != "") return item.CustomCode;

            var code = "";

            code += "menuentry \"" + item.Name.RemoveAccent() + "\" {\n";

            if (item.Type == EntryType.BootHDD)
            {
                code += "insmod part_msdos\n";
                code += "insmod chain\n";
                code += "chainloader (hd0,0)\n";
            }
            else if(item.Type == EntryType.Category)
            {
  code += "configfile /win32-loader/" + item.IsoName + ".cfg\n";
            }
            else
            {
                code += $"search -f \"--set-root /images/{item.IsoName}\"\n";
                switch (item.Type)
                {
                    case EntryType.ISO:
                        code += $"drivemap \"/images/{item.IsoName}\" '(hd32)'\ndrivemap '--hook' ''\nset root='(hd32)'\nchainloader +1\n";
                        break;
                    case EntryType.IMG:
                        code += $"linux /memdisk\ninitrd /images/{item.IsoName}\n";
                        break;
                    case EntryType.NTLDR:
                        code += $"insmod part_msdos\ninsmod ntldr\ninsmod ntfs\nntldr /images/{item.IsoName}";
                        break;
                    case EntryType.GRLDR:
                    case EntryType.CMLDR:
                    case EntryType.FreeDOS:
                    case EntryType.MS_DOS:
                    case EntryType.MS_DOS_7:
                    case EntryType.PC_DOS:
                    case EntryType.DRMK:
                    case EntryType.ReactOS:
                        code += string.Format(
                            "ls /images/{0} || find --set-root /images/{0}\nchainloader /images/{0}\n",
                            item.IsoName);
                        break;
                }
            }

            code += "}\n";

            return code;
        }

        public override void SetImage(Image image, Size sz)
        {
            if (image == null) return;

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

            destImage.Save(Path.Combine(WorkingDir, "sharpboot.png"), ImageFormat.Png);
        }

        public override string BinFile { get; set; } = "g2ldr";
        public override byte[] Archive { get; set; } = Resources.grub2;
        public override string FolderName { get; set; } = "../win32-loader";
        public override string DisplayName { get; set; } = "Grub2";
        public override string FileExt { get; set; } = ".cfg";
        public override string CmdArgs { get; set; } = "";
        public override bool SupportAccent { get; set; } = true;
        public override long TotalSize { get; set; } = 180335;

        public override void Install(string l)
        {
           /* var d = Program.GetTemporaryDirectory();
            var exepath = Path.Combine(d, "grubinst.exe");
            File.WriteAllBytes(exepath, Resources.grubinst);

            var p = new Process
            {
                StartInfo =
                {
                    CreateNoWindow = true,
                    UseShellExecute = true,
                    FileName = exepath,
                    Verb = "runas",
                    Arguments =
                        " --skip-mbr-test --no-backup-mbr -t=0 --grub2 (hd" +
                        string.Concat(Utils.GetPhysicalPath(l.ToLower().Substring(0, 2)).Where(char.IsDigit)) + ")"
                }
            };
            p.Start();
            p.WaitForExit();

            Program.SafeDel(d);*/
            var grub2_mbr = new byte[432]
            {
                0xEB, 0x63, 0x90, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80, 0x01, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0xFF, 0xFA, 0x90, 0x90, 0xF6, 0xC2, 0x80, 0x74,
                0x05, 0xF6, 0xC2, 0x70, 0x74, 0x02, 0xB2, 0x80, 0xEA, 0x79, 0x7C, 0x00,
                0x00, 0x31, 0xC0, 0x8E, 0xD8, 0x8E, 0xD0, 0xBC, 0x00, 0x20, 0xFB, 0xA0,
                0x64, 0x7C, 0x3C, 0xFF, 0x74, 0x02, 0x88, 0xC2, 0x52, 0xBE, 0x80, 0x7D,
                0xE8, 0x17, 0x01, 0xBE, 0x05, 0x7C, 0xB4, 0x41, 0xBB, 0xAA, 0x55, 0xCD,
                0x13, 0x5A, 0x52, 0x72, 0x3D, 0x81, 0xFB, 0x55, 0xAA, 0x75, 0x37, 0x83,
                0xE1, 0x01, 0x74, 0x32, 0x31, 0xC0, 0x89, 0x44, 0x04, 0x40, 0x88, 0x44,
                0xFF, 0x89, 0x44, 0x02, 0xC7, 0x04, 0x10, 0x00, 0x66, 0x8B, 0x1E, 0x5C,
                0x7C, 0x66, 0x89, 0x5C, 0x08, 0x66, 0x8B, 0x1E, 0x60, 0x7C, 0x66, 0x89,
                0x5C, 0x0C, 0xC7, 0x44, 0x06, 0x00, 0x70, 0xB4, 0x42, 0xCD, 0x13, 0x72,
                0x05, 0xBB, 0x00, 0x70, 0xEB, 0x76, 0xB4, 0x08, 0xCD, 0x13, 0x73, 0x0D,
                0x5A, 0x84, 0xD2, 0x0F, 0x83, 0xD8, 0x00, 0xBE, 0x8B, 0x7D, 0xE9, 0x82,
                0x00, 0x66, 0x0F, 0xB6, 0xC6, 0x88, 0x64, 0xFF, 0x40, 0x66, 0x89, 0x44,
                0x04, 0x0F, 0xB6, 0xD1, 0xC1, 0xE2, 0x02, 0x88, 0xE8, 0x88, 0xF4, 0x40,
                0x89, 0x44, 0x08, 0x0F, 0xB6, 0xC2, 0xC0, 0xE8, 0x02, 0x66, 0x89, 0x04,
                0x66, 0xA1, 0x60, 0x7C, 0x66, 0x09, 0xC0, 0x75, 0x4E, 0x66, 0xA1, 0x5C,
                0x7C, 0x66, 0x31, 0xD2, 0x66, 0xF7, 0x34, 0x88, 0xD1, 0x31, 0xD2, 0x66,
                0xF7, 0x74, 0x04, 0x3B, 0x44, 0x08, 0x7D, 0x37, 0xFE, 0xC1, 0x88, 0xC5,
                0x30, 0xC0, 0xC1, 0xE8, 0x02, 0x08, 0xC1, 0x88, 0xD0, 0x5A, 0x88, 0xC6,
                0xBB, 0x00, 0x70, 0x8E, 0xC3, 0x31, 0xDB, 0xB8, 0x01, 0x02, 0xCD, 0x13,
                0x72, 0x1E, 0x8C, 0xC3, 0x60, 0x1E, 0xB9, 0x00, 0x01, 0x8E, 0xDB, 0x31,
                0xF6, 0xBF, 0x00, 0x80, 0x8E, 0xC6, 0xFC, 0xF3, 0xA5, 0x1F, 0x61, 0xFF,
                0x26, 0x5A, 0x7C, 0xBE, 0x86, 0x7D, 0xEB, 0x03, 0xBE, 0x95, 0x7D, 0xE8,
                0x34, 0x00, 0xBE, 0x9A, 0x7D, 0xE8, 0x2E, 0x00, 0xCD, 0x18, 0xEB, 0xFE,
                0x47, 0x52, 0x55, 0x42, 0x20, 0x00, 0x47, 0x65, 0x6F, 0x6D, 0x00, 0x48,
                0x61, 0x72, 0x64, 0x20, 0x44, 0x69, 0x73, 0x6B, 0x00, 0x52, 0x65, 0x61,
                0x64, 0x00, 0x20, 0x45, 0x72, 0x72, 0x6F, 0x72, 0x0D, 0x0A, 0x00, 0xBB,
                0x01, 0x00, 0xB4, 0x0E, 0xCD, 0x10, 0xAC, 0x3C, 0x00, 0x75, 0xF4, 0xC3
            };

            Utils.InstallMBR(l, grub2_mbr);
        }
    }

    public interface IBootloaderTheme
    {
        Size Resolution { get; set; }

        string GetCode();
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

        public List<SLTEntry> Entries = new List<SLTEntry>
        {
            new SLTEntry("screen", "#80ffffff", "#00000000", ShadowType.std),
            new SLTEntry("border", "#ffffffff", "#ee000000", ShadowType.std),
            new SLTEntry("title", "#ffffffff", "#ee000000", ShadowType.std),
            new SLTEntry("unsel", "#ffffffff", "#ee000000", ShadowType.std),
            new SLTEntry("hotkey", "#ff00ff00", "#ee000000", ShadowType.std),
            new SLTEntry("sel", "#ffffffff", "#85000000", ShadowType.std),
            new SLTEntry("hotsel", "#ffffffff", "#85000000", ShadowType.std),
            new SLTEntry("disabled", "#60cccccc", "#00000000", ShadowType.std),
            new SLTEntry("scrollbar", "#40000000", "#00000000", ShadowType.std),
            new SLTEntry("tabmsg", "#90ffff00", "#00000000", ShadowType.std),
            new SLTEntry("cmdmark", "#c000ffff", "#00000000", ShadowType.std),
            new SLTEntry("cmdline", "#c0ffffff", "#00000000", ShadowType.std),
            new SLTEntry("pwdborder", "#80ffffff", "#20ffffff", ShadowType.rev),
            new SLTEntry("pwdheader", "#80ff8080", "#20ffffff", ShadowType.rev),
            new SLTEntry("pwdentry", "#80ffffff", "#20ffffff", ShadowType.rev),
            new SLTEntry("timeout_msg", "#80ffffff", "#00000000", ShadowType.std),
            new SLTEntry("timeout", "#c0ffffff", "#00000000", ShadowType.std),
            new SLTEntry("help", "#c0ffffff", "#00000000", ShadowType.std)
        };

        // SLTEntry = SysLinux Theme Entry
        public class SLTEntry
        {
            public SLTEntry(string name, Color foreground, Color background, ShadowType shadowType = ShadowType.none)
            {
                Name = name;
                Foreground = foreground;
                Background = background;
                ShadowType = shadowType;
            }

            public SLTEntry(string name, string foreground, string background, ShadowType shadowType = ShadowType.none)
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
    public class Bootloaders
    {
        public static IBootloader Syslinux = new Syslinux();
        public static IBootloader Grub4DOS = new Grub4DOS();
        //public static IBootloader Grub2 = new Grub2();

        public static List<IBootloader> Bloaders
        {
            get
            {
                var t = typeof (Bootloaders);
                var fls =
                    t.GetFields(BindingFlags.Public | BindingFlags.Static)
                        .Where(x => x.FieldType == typeof (IBootloader));
                return fls.Select(x => (IBootloader) x.GetValue(null)).ToList();
            }
        } 
    }

    public class BlItem
    {
        public string DisplayName { get; set; }

        public int ID { get; set; }
    }

    public class BootloaderInst
    {
        public static void Install(string l, string bl)
        {
            Install(l, Bootloaders.Bloaders.FirstOrDefault(x => x.FolderName == bl));
        }

        public static void Install(string l, IBootloader bl)
        {
            var d = Program.GetTemporaryDirectory();
            File.WriteAllBytes(Path.Combine(d, "bloader.7z"), bl.Archive);
            var ext = new SevenZipExtractor();
            ext.Extract(Path.Combine(d, "bloader.7z"), l);
            ext.Close();
            bl.Install(l);
         

            return;

            /*if (bl == Bootloaders.Grub2)
            {
                var grub2_mbr = new byte[]
                {
                    0xEB, 0x63, 0x90, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                    0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                    0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                    0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                    0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                    0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                    0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                    0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80, 0x01, 0x00, 0x00, 0x00,
                    0x00, 0x00, 0x00, 0x00, 0xFF, 0xFA, 0x90, 0x90, 0xF6, 0xC2, 0x80, 0x74,
                    0x05, 0xF6, 0xC2, 0x70, 0x74, 0x02, 0xB2, 0x80, 0xEA, 0x79, 0x7C, 0x00,
                    0x00, 0x31, 0xC0, 0x8E, 0xD8, 0x8E, 0xD0, 0xBC, 0x00, 0x20, 0xFB, 0xA0,
                    0x64, 0x7C, 0x3C, 0xFF, 0x74, 0x02, 0x88, 0xC2, 0x52, 0xBE, 0x80, 0x7D,
                    0xE8, 0x17, 0x01, 0xBE, 0x05, 0x7C, 0xB4, 0x41, 0xBB, 0xAA, 0x55, 0xCD,
                    0x13, 0x5A, 0x52, 0x72, 0x3D, 0x81, 0xFB, 0x55, 0xAA, 0x75, 0x37, 0x83,
                    0xE1, 0x01, 0x74, 0x32, 0x31, 0xC0, 0x89, 0x44, 0x04, 0x40, 0x88, 0x44,
                    0xFF, 0x89, 0x44, 0x02, 0xC7, 0x04, 0x10, 0x00, 0x66, 0x8B, 0x1E, 0x5C,
                    0x7C, 0x66, 0x89, 0x5C, 0x08, 0x66, 0x8B, 0x1E, 0x60, 0x7C, 0x66, 0x89,
                    0x5C, 0x0C, 0xC7, 0x44, 0x06, 0x00, 0x70, 0xB4, 0x42, 0xCD, 0x13, 0x72,
                    0x05, 0xBB, 0x00, 0x70, 0xEB, 0x76, 0xB4, 0x08, 0xCD, 0x13, 0x73, 0x0D,
                    0x5A, 0x84, 0xD2, 0x0F, 0x83, 0xD8, 0x00, 0xBE, 0x8B, 0x7D, 0xE9, 0x82,
                    0x00, 0x66, 0x0F, 0xB6, 0xC6, 0x88, 0x64, 0xFF, 0x40, 0x66, 0x89, 0x44,
                    0x04, 0x0F, 0xB6, 0xD1, 0xC1, 0xE2, 0x02, 0x88, 0xE8, 0x88, 0xF4, 0x40,
                    0x89, 0x44, 0x08, 0x0F, 0xB6, 0xC2, 0xC0, 0xE8, 0x02, 0x66, 0x89, 0x04,
                    0x66, 0xA1, 0x60, 0x7C, 0x66, 0x09, 0xC0, 0x75, 0x4E, 0x66, 0xA1, 0x5C,
                    0x7C, 0x66, 0x31, 0xD2, 0x66, 0xF7, 0x34, 0x88, 0xD1, 0x31, 0xD2, 0x66,
                    0xF7, 0x74, 0x04, 0x3B, 0x44, 0x08, 0x7D, 0x37, 0xFE, 0xC1, 0x88, 0xC5,
                    0x30, 0xC0, 0xC1, 0xE8, 0x02, 0x08, 0xC1, 0x88, 0xD0, 0x5A, 0x88, 0xC6,
                    0xBB, 0x00, 0x70, 0x8E, 0xC3, 0x31, 0xDB, 0xB8, 0x01, 0x02, 0xCD, 0x13,
                    0x72, 0x1E, 0x8C, 0xC3, 0x60, 0x1E, 0xB9, 0x00, 0x01, 0x8E, 0xDB, 0x31,
                    0xF6, 0xBF, 0x00, 0x80, 0x8E, 0xC6, 0xFC, 0xF3, 0xA5, 0x1F, 0x61, 0xFF,
                    0x26, 0x5A, 0x7C, 0xBE, 0x86, 0x7D, 0xEB, 0x03, 0xBE, 0x95, 0x7D, 0xE8,
                    0x34, 0x00, 0xBE, 0x9A, 0x7D, 0xE8, 0x2E, 0x00, 0xCD, 0x18, 0xEB, 0xFE,
                    0x47, 0x52, 0x55, 0x42, 0x20, 0x00, 0x47, 0x65, 0x6F, 0x6D, 0x00, 0x48,
                    0x61, 0x72, 0x64, 0x20, 0x44, 0x69, 0x73, 0x6B, 0x00, 0x52, 0x65, 0x61,
                    0x64, 0x00, 0x20, 0x45, 0x72, 0x72, 0x6F, 0x72, 0x0D, 0x0A, 0x00, 0xBB,
                    0x01, 0x00, 0xB4, 0x0E, 0xCD, 0x10, 0xAC, 0x3C, 0x00, 0x75, 0xF4, 0xC3
                };

                var dp = @"\\.\" + l.Substring(0, 2);
                /*var dh = Utils.CreateFile(dp, 0xC0000000, 0x03, IntPtr.Zero, 0x03, DeviceIO.FILE_FLAG_WRITE_THROUGH | DeviceIO.FILE_FLAG_NO_BUFFERING, IntPtr.Zero);
                if (dh.IsInvalid)
                {
                    dh.Close();
                    dh.Dispose();
                    dh = null;
                    throw new Exception("Win32 Exception : 0x" +
                                        Convert.ToString(Marshal.GetHRForLastWin32Error(), 16).PadLeft(8, '0'));
                }
                var ds = new FileStream(dh, FileAccess.ReadWrite);

                var buf = new byte[512];
                ds.Read(buf, 0, 512);
                //Array.Copy(grub2_mbr, buf, grub2_mbr.Length);
                ds.Position = 0;

                //ds.Write(buf, 0, 512);
                

                for (var i = 0; i < grub2_mbr.Length; i++)
                {
                    ds.WriteByte(grub2_mbr[i]);
                }


                //ds.Close();
                //ds.Dispose();
                if (!dh.IsClosed) dh.Close();
                dh.Dispose();
                dh = null;* /


                using (var da = new DriveAccess(dp))
                {
                    foreach (byte t in grub2_mbr)
                    {
                        da.driveStream.WriteByte(t);
                    }
                }


                return;
            }

            var exename = bl == Bootloaders.Grub4DOS ? "grubinst.exe" : "syslinux.exe";

            var d = Program.GetTemporaryDirectory();
            var exepath = Path.Combine(d, exename);
            File.WriteAllBytes(exepath, bl == Bootloaders.Grub4DOS ? Resources.grubinst : Resources.syslinux);

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
            if (bl == Bootloaders.Grub4DOS)
            {
                var deviceId = Utils.GetPhysicalPath(driveletter);
                
                p.StartInfo.Arguments = " --skip-mbr-test --no-backup-mbr -t=0 (hd" + string.Concat(deviceId.Where(char.IsDigit)) + ")";
            }
            else
            {
                p.StartInfo.Arguments = " -m -a " + driveletter;
            }
            p.Start();
            p.WaitForExit();

            Program.SafeDel(d);*/
        }
    }

    public class driveitem
    {
        public string Disp { get; set; }
        public DriveInfo Value { get; set; }
    }
}