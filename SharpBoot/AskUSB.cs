using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace SharpBoot
{
    public partial class AskUSB : AskPForm
    {
        public AskUSB()
        {
            InitializeComponent();
            cbxFS.SelectedIndex = 0;
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

        public override string FileName => getseldrive().Name.ToUpper().Substring(0, 3);

        public string FileSystem => cbxFS.SelectedItem.ToString().Split(' ')[0].ToUpper();

        public DriveInfo getseldrive()
        {
            return ((driveitem)cbxUSB.SelectedItem).Value;
        }

        private void cbxUSB_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnOK.Enabled = cbxUSB.SelectedIndex != -1;
        }
    }
}