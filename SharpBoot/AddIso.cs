using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;

namespace SharpBoot
{
    public partial class AddIso : Form
    {
        public string ISOPath { get; set; }

        public AddIso()
        {
            InitializeComponent();
            ISOPath = "";
            Closing += AddIso_Closing;
        }

        private void AddIso_Closing(object sender, CancelEventArgs e)
        {
            th?.Abort();
        }

        private bool fempty;

        private void AddIso_Load(object sender, EventArgs e)
        {
            var isos = ISOInfo.ISOs.Where(x => !x.NoDL).Select(x => new { Val = x, x.Name, x.Category, x.LatestVersion.Hash });

            cbxISOS.DataSource = isos;
            var iso2 = isos.ToList();
            iso2.Insert(0, new { Val = new ISOInfo("", "", ""), Name = Strings.Other, Category = "", Hash = "" });
            iso2.AddRange(ISOInfo.ISOs.Where(x => x.NoDL).Select(x => new { Val = x, x.Name, x.Category, Hash = "" }));
            cbxDetIso.DataSource = iso2;
            cbxDetIso.DisplayMember = "Name";
            fempty = true;
            cbxDetIso.SelectedItem = null;
            cbxISOS.SelectedItem = null;
            cbxVersion.SelectedItem = null;
        }

        private void rbnFile_CheckedChanged(object sender, EventArgs e)
        {
            txtFile.Enabled = btnBrowse.Enabled = cbxDetIso.Enabled = rbnFile.Checked;
            cbxISOS.Visible = cbxVersion.Visible = rbnDown.Checked;

            if (rbnDown.Checked)
            {
                btnOK.DialogResult = DialogResult.None;
                btnOK.Text = Strings.Download;
            }
            else
            {
                btnOK.DialogResult = DialogResult.OK;
                btnOK.Text = Strings.OK;
            }


            if (rbnFile.Checked)
                ISOPath = txtFile.Text;

            if (fempty) fempty = false;

            CheckNotEmpty();
        }

        private void txtFile_TextChanged(object sender, EventArgs e)
        {
            btnOK.Enabled = !(string.IsNullOrWhiteSpace(txtFile.Text));
            ISOPath = txtFile.Text;
        }

        private ISOInfo selinfo => cbxISOS.SelectedItem == null ? null : ((dynamic) cbxISOS.SelectedItem).Val;

        private ISOInfo selinfo2 => cbxDetIso.SelectedItem == null ? null : ((dynamic)cbxDetIso.SelectedItem).Val;

        private ISOV selinfoversion()
        {
            var ret = selinfo.Versions.FirstOrDefault(x => x.Name.Equals(((dynamic) cbxVersion.SelectedValue).Name));
            if (ret != default(ISOV)) return ret;
            var si = selinfo2;
            return new ISOV("nover", si.Name, si.Description, si.Filename, true) {Parent = si};
        }

        private void cbxISOS_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnOK.Enabled = cbxISOS.SelectedIndex != -1;
            if (cbxISOS.SelectedIndex != -1 && selinfo != null)
            {
                cbxVersion.Enabled = cbxVersion.Visible = true;
                var ds = selinfo.Versions.Select(x => new {x.Name, Value = x}).ToList();
                cbxVersion.DataSource = ds;
                var latest = selinfo.LatestVersion;
                cbxVersion.SelectedIndex = 0;
                for (int i = 0; i < ds.Count; i++)
                {
                    if (ds[i].Value == latest)
                    {
                        cbxVersion.SelectedIndex = i;
                        break;
                    }
                }
            }
            else cbxVersion.Enabled = cbxVersion.Visible = false;
        }

        private void setprg(int v)
        {
            pbxPrg.Value = v;
            lblPercent.Text = v + " %";
        }

        private WebClient client;

        public void DownloadStuff()
        {
            pbxPrg.Visible = lblPercent.Visible = lblSpeed.Visible = lblProg.Visible = true;

            var dn = selinfoversion().DownloadLink;

            sw = new Stopwatch();

            using (client = new WebClient())
            {
                client.DownloadProgressChanged += ClientOnDownloadProgressChanged;
                client.DownloadFileCompleted += Client_DownloadFileCompleted;

                sw.Start();

                client.DownloadFileAsync(new Uri(dn), DownFile);
            }
        }

        private void Client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            sw.Stop();
            pbxPrg.Visible = lblPercent.Visible = lblSpeed.Visible = lblProg.Visible = false;
            if (!e.Cancelled) MessageBox.Show(Strings.DownComplete, "SharpBoot");
            pbxPrg.Value = 100;
            btnOK.Enabled = true;
            ControlBox = true;
            ISOPath = DownFile;
            IsoV = selinfoversion();
            DialogResult = DialogResult.OK;
            Close();
        }

        public bool IsDownload => rbnDown.Checked;

        public string DownFile = "";

        public void CheckNotEmpty()
        {
            if (rbnFile.Checked)
                btnOK.Enabled = !(string.IsNullOrWhiteSpace(txtFile.Text));
            else
                btnOK.Enabled = cbxISOS.SelectedIndex != -1;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (ofpIso.ShowDialog() == DialogResult.OK)
            {
                txtFile.Text = ofpIso.FileName;
                cbxDetIso.Visible = true;
                md5stuff();
            }
        }


        private Thread th;

        private bool changing = false;

        private void md5stuff()
        {
            th = new Thread(() =>
            {
                Invoke((MethodInvoker) (() => pbxLoading.Visible = true));

                var resk = ISOInfo.GetFromFile(ISOPath, false);


                Invoke((MethodInvoker) (() =>
                {
                    if (resk == null)
                    {
                        cbxDetIso.SelectedIndex = 0;
                    }
                    else
                    {
                        IsoV = resk;
                        for (int index = 0; index < cbxDetIso.Items.Count; index++)
                        {
                            dynamic it = cbxDetIso.Items[index];
                            if (it.Val == resk.Parent)
                            {
                                changing = true;
                                cbxDetIso.SelectedIndex = index;
                                
                                break;
                            }
                        }
                    }
                    pbxLoading.Visible = false;
                }));
                changing = false;
            })
            {
                CurrentCulture = CultureInfo.CurrentCulture,
                CurrentUICulture = CultureInfo.CurrentUICulture
            };
            th.Start();

        }

        public ISOV IsoV;

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (IsDownload)
            {
                if (sfdIso.ShowDialog(this) == DialogResult.OK)
                {
                    btnOK.Enabled = false;
                    ControlBox = false;

                    DownFile = sfdIso.FileName;
                    DownloadStuff();
                }
            }
        }


        private Stopwatch sw;

        private void ClientOnDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            lblSpeed.Text = Program.GetSizeString(e.BytesReceived / (long) sw.Elapsed.TotalSeconds) + "/s";
            setprg(e.ProgressPercentage);

            lblProg.Text = Program.GetSizeString(e.BytesReceived) + " / " + Program.GetSizeString(e.TotalBytesToReceive);
        }

        private void btnAnnul_Click(object sender, EventArgs e)
        {
            client?.CancelAsync();
        }

        

        private void cbxDetIso_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (changing) return;
            if(cbxDetIso.SelectedIndex != -1 && cbxDetIso.SelectedItem != null)
            {
                IsoV = cbxDetIso.SelectedIndex == 0 ? new ISOV("other", "") : (selinfo2 == null ? null : (selinfo2.LatestVersion ?? new ISOV("nover", selinfo2.Name, "", selinfo2.Filename, true) {Parent=selinfo2}));
            }
        }
    }
}