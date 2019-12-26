using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using SharpBoot.Models;
using SharpBoot.Utilities;

namespace SharpBoot.Forms
{
    public partial class USBFrm : Form
    {
        public USBFrm()
        {
            InitializeComponent();
            btnAnnul.Text = Strings.Cancel;
            LoadDrives();
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
            get => pbxPrg.Visible;
            set => pbxPrg.Visible = lblPercent.Visible = value;
        }

        // ReSharper disable once ConvertToAutoPropertyWhenPossible
        public ComboBox TheComboBox => comboBox;

        public DriveInfo SelectedUSB => ((dynamic) cbxUSB.SelectedItem).Value;


        public void LoadDrives()
        {
            cbxUSB.DataSource = DriveIO.GetValidDrives()
                .Select(drive => new
                {
                    Disp =
                        drive.VolumeLabel + " (" + drive.Name + ")         " + FileIO.GetSizeString(drive.TotalSize) +
                        " " + drive.DriveFormat,
                    Value = drive
                }).ToList();
        }


        private void cbxUSB_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnOK.Enabled = cbxUSB.SelectedIndex != -1;
        }

        public void SetProgress(int v)
        {
            pbxPrg.Value = v;
            lblPercent.Text = v + " %";
        }


        public event EventHandler BtnClicked;

        private void btnOK_Click(object sender, EventArgs e)
        {
            BtnClicked?.Invoke(sender, e);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadDrives();
        }
    }
}