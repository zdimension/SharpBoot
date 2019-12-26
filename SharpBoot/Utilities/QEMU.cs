using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using SharpBoot.Properties;

namespace SharpBoot.Utilities
{
    public class QEMU
    {
        public static List<string> Paths = new List<string>();

        public static void LaunchQemu(string iso, bool usb = false)
        {
            var f = FileIO.GetTemporaryDirectory();
            Paths.Add(f);

            var floppy = Path.GetExtension(iso).ToLower() == ".img";

            var ext = new SevenZipExtractor();

            File.WriteAllBytes(Path.Combine(f, "qemutmp.7z"), Resources.qemu);

            ext.Extract(Path.Combine(f, "qemutmp.7z"), f);


            var p = new Process
            {
                StartInfo =
                {
                    UseShellExecute = false
                },
                EnableRaisingEvents = true
            };
            p.StartInfo.FileName = Path.Combine(f, "qemu.exe");
            p.StartInfo.WorkingDirectory = f;
            p.StartInfo.Arguments = " -m 1536 -M pc -cpu max ";
            if (usb)
            {
                var logicalDiskId = iso.Substring(0, 2);
                var deviceId = DriveIO.GetPhysicalPath(logicalDiskId);
                p.StartInfo.Arguments += "-boot c -drive file=" + deviceId +
                                        ",if=ide,index=0,media=disk,format=raw ";
                p.StartInfo.UseShellExecute = true;
                p.StartInfo.Verb = "runas";
            }
            else
            {
                p.StartInfo.Arguments += (floppy ? "-boot a -fda" : "-boot d -cdrom") + " \"" + iso + "\"";
            }

            Utils.WaitWhile(() => !File.Exists(p.StartInfo.FileName));
            p.Start();
            ext.Close();
            p.Exited += (sender, args) =>
            {
                FileIO.SafeDel(f);
                Paths.Remove(f);
            };
        }
    }
}