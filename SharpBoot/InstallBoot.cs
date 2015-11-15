using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Windows.Forms;
using SharpBoot.Properties;

namespace SharpBoot
{
    

    public partial class InstallBoot : Form
    {
       

        public InstallBoot()
        {
            InitializeComponent();
            loadkeys();
        }

        public void loadkeys()
        {
            foreach (var drive in DriveInfo.GetDrives().Where(d => d.DriveType == DriveType.Removable && d.IsReady))
            {
                cbxUSB.Items.Add(new driveitem
                {
                    Disp =
                        drive.VolumeLabel + " (" + drive.Name + ")         " + Program.GetSizeString(drive.TotalSize) +
                        " " + drive.DriveFormat,
                    Value = drive
                });
            }
        }

        private string bl = "syslinux";

        private void rbnSyslinux_CheckedChanged(object sender, EventArgs e)
        {
            bl = rbnG4D.Checked ? "grub4dos" : "syslinux";
        }

        private void cbxUSB_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnOK.Enabled = cbxUSB.SelectedIndex != -1;
        }

        private void setprg(int v)
        {
            pbxPrg.Value = v;
            lblPercent.Text = v + " %";
        }

        private DriveInfo getseldrive()
        {
            return ((driveitem) cbxUSB.SelectedItem).Value;
        }

        private void installit()
        {
            pbxPrg.Visible = true;
            lblPercent.Visible = true;

            setprg(5);

            var di = getseldrive();

            BootloaderInst.Install(di.Name, bl);

            onfinish();
        }

        private void onfinish()
        {
            setprg(100);
            MessageBox.Show(
                string.Format(Strings.BootloaderInstalled, (bl == "grub4dos" ? "Grub4DOS" : "Syslinux"),
                    getseldrive().Name), "SharpBoot", 0, MessageBoxIcon.Information);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            installit();
        }
    }

    public class BootloaderInst
    {
        public static void Install(string l, string bl)
        {
            var exename = bl == "grub4dos" ? "grubinst.exe" : "syslinux.exe";

            var d = Program.GetTemporaryDirectory();
            var exepath = Path.Combine(d, exename);
            File.WriteAllBytes(exepath, bl == "grub4dos" ? Resources.grubinst : Resources.syslinux);

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
            if (bl == "grub4dos")
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
            if (bl == "grub4dos")
            {
                File.WriteAllBytes(Path.Combine(driveletter, "grldr"), Resources.grldr);
            }
        }
    }

    public class driveitem
    {
        public string Disp { get; set; }
        public DriveInfo Value { get; set; }
    }
}