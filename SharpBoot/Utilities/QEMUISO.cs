using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using SharpBoot.Properties;

namespace SharpBoot.Utilities
{
    public class QEMUISO
    {
        public static List<string> Paths = new List<string>();

        public static void LaunchQemu(string iso, bool usb = false)
        {
            var f = Utils.GetTemporaryDirectory();
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
            if (usb)
            {
                var logicalDiskId = iso.Substring(0, 2);
                var deviceId = DriveIO.GetPhysicalPath(logicalDiskId);
                p.StartInfo.Arguments = " -L . -boot c  -drive file=" + deviceId +
                                        ",if=ide,index=0,media=disk -m 512 -localtime";
                p.StartInfo.UseShellExecute = true;
                p.StartInfo.Verb = "runas";
            }
            else
            {
                p.StartInfo.Arguments = "-m 512 -localtime -M pc " + (floppy ? "-fda" : "-cdrom") + " \"" + iso + "\"";
            }

            Thread.Sleep(300);
            p.Start();
            ext.Close();
            p.Exited += (sender, args) =>
            {
                Utils.SafeDel(f);
                Paths.Remove(f);
            };
        }
    }
}