using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace SharpBoot
{
    

    public partial class USBFrm : Form
    {
       

        public USBFrm()
        {
            InitializeComponent();
            loadkeys();
        }

        public USBFrm(string header, string cbxt, string button, bool dialog = true, params object[] cbxitems) : this()
        {
            lblHeader.Text = header;
            lblCbx.Text = cbxt;
            comboBox.DataSource = cbxitems;
            btnOK.Text = "  " + button;
            if (dialog) btnOK.DialogResult = DialogResult.OK;
        }

        public bool ProgressVisible
        {
            get { return pbxPrg.Visible = true; }
            set { pbxPrg.Visible = lblPercent.Visible = value; }
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


        private void cbxUSB_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnOK.Enabled = cbxUSB.SelectedIndex != -1;
        }

        // ReSharper disable once ConvertToAutoPropertyWhenPossible
        public ComboBox TheComboBox => comboBox;

        public void SetProgress (int v)
        {
            pbxPrg.Value = v;
            lblPercent.Text = v + " %";
        }

        public DriveInfo SelectedUSB => ((driveitem) cbxUSB.SelectedItem).Value;


       

        public event EventHandler BtnClicked = (sender, args) => { };

        private void btnOK_Click(object sender, EventArgs e)
        {
            BtnClicked(sender, e);
        }
    }
}