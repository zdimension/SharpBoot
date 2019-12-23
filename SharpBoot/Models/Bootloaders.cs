using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using SharpBoot.Utilities;

namespace SharpBoot.Models
{
    public static class Grub2
    {
        public static string GetCode(BootMenu menu, Size res = default)
        {
            var code = "";

            code += "insmod ntfs\n";
            code += "insmod exfat\n";
            code += "insmod fat\n";
            code += "loadfont /boot/grub/unicode.pf2\n";
            code += "insmod all_video\n";
            code += "set gfxmode=" + (res == default ? "auto" : $"{res.Width}x{res.Height}") + "\n";
            code += "insmod gfxterm\n";
            code += "terminal_output gfxterm\n";
            code += "insmod png\n";
            code += "background_image /boot/grub/sharpboot.png\n";
            code += "\n";

            menu.Items.ForEach(x => code += GetCode(x));

            return code;
        }

        public static string GetCode(BootMenuItem item)
        {
            if (item.CustomCode != "") return item.CustomCode;

            var code = "";

            code += "menuentry \"" + item.Name + "\" {\n";

            switch (item.Type)
            {
                case EntryType.BootHDD:
                    code += "insmod part_msdos\n";
                    code += "insmod chain\n";
                    code += "chainloader (hd0,0)\n";
                    break;
                case EntryType.Category:
                    code += "configfile /boot/grub/" + item.IsoName + ".cfg\n";
                    break;
                default:
                    code += $"search --file --set root --no-floppy \"/images/{item.IsoName}\"\n";
                    switch (item.Type)
                    {
                        case EntryType.ISO:
                            code += string.Format(
                                "set opt='ls /images/{0} || find --set-root /images/{0};map /images/{0} (0xff);map --hook;root (0xff);chainloader (0xff);boot'\n",
                                item.IsoName);
                            code += "linux /boot/grub/grub.exe --config-file=$opt\n";
                            break;
                        case EntryType.IMG:
                            code += $"linux16 /boot/grub/memdisk\ninitrd16 /images/{item.IsoName}\n";
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
                                "set opt='ls /images/{0} || find --set-root /images/{0};chainloader /images/{0};boot'\n",
                                item.IsoName);
                            code += "linux /boot/grub/grub.exe --config-file=$opt\n";
                            break;
                    }

                    break;
            }

            code += "}\n";

            return code;
        }

        public static void SetImage(Image image, Size sz, string wdir)
        {
            if (image == null) return;

            var width = sz.Width;
            var height = sz.Height;

            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height, PixelFormat.Format32bppRgb);

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

            destImage.Save(Path.Combine(wdir, "sharpboot.png"), ImageFormat.Png);
        }

        public static void Install(string l)
        {
            Utils.CallAdminProcess("grub2", l);
        }
    }

    public class driveitem
    {
        public string Disp { get; set; }
        public DriveInfo Value { get; set; }
    }
}