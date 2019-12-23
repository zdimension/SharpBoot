using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using SharpBoot.Properties;

namespace SharpBoot
{
    public partial class AddIso : Form
    {
        private bool changing;

        private WebClient client;

        public string DownFile = "";

        private bool fempty;

        public ISOV IsoV;


        private Stopwatch sw;


        private Thread th;

        public AddIso()
        {
            InitializeComponent();
            ISOPath = "";
            Closing += AddIso_Closing;
            btnOK.Text = Strings.OK;
            btnAnnul.Text = Strings.Cancel;
            btnBrowse.Text = Strings.Browse;
            ofpIso.Title = label1.Text;
            sfdIso.Filter = Strings.ISOImg + " (*.iso)|*.iso";
            var iter = 0;
            while (ISOInfo.ISOs.Count == 0 && iter < 20)
            {
                Thread.Sleep(50);
                iter++;
            }

            if (ISOInfo.ISOs.Count == 0)
                MessageBox.Show(
                    "If you see this message (honestly, you shouldn't ever see it), then something horribly wrong that shouldn't happen has happened.");

            while (cbxDetIso.Items.Count < 2)
            {
                var isos =
                    ISOInfo.ISOs.Where(x => !x.NoDL)
                        .Select(x => new {Val = x, x.Name, Category = x.CategoryTxt, x.LatestVersion.Hash});

                cbxISOS.DataSource = isos;
                var iso2 = isos.ToList();
                iso2.Insert(0,
                    new
                    {
                        Val = new ISOInfo("", new Dictionary<CultureInfo, string>(), ISOCat.Empty),
                        Name = Strings.Other,
                        Category = "",
                        Hash = ""
                    });
                iso2.AddRange(
                    ISOInfo.ISOs.Where(x => x.NoDL)
                        .Select(x => new {Val = x, x.Name, Category = x.CategoryTxt, Hash = ""})
                        .ToList());
                cbxDetIso.DataSource = iso2;
                cbxDetIso.DisplayMember = "Name";
                fempty = true;
                cbxDetIso.SelectedItem = null;
                cbxISOS.SelectedItem = null;
                cbxVersion.SelectedItem = null;
            }

            rtbIsoDesc.Text = "";
        }

        public string ISOPath { get; set; }

        private ISOInfo selinfo => cbxISOS.SelectedItem == null ? null : ((dynamic) cbxISOS.SelectedItem).Val;

        private ISOInfo selinfo2 => cbxDetIso.SelectedItem == null ? null : ((dynamic) cbxDetIso.SelectedItem).Val;

        public bool IsDownload => rbnDown.Checked;

        private void AddIso_Closing(object sender, CancelEventArgs e)
        {
            th?.Abort();
        }

        private void AddIso_Load(object sender, EventArgs e)
        {
        }

        private void rbnFile_CheckedChanged(object sender, EventArgs e)
        {
            txtFile.Enabled = btnBrowse.Enabled = cbxDetIso.Enabled = rbnFile.Checked;
            cbxISOS.Visible = cbxVersion.Visible = rtbIsoDesc.Visible = rbnDown.Checked;

            if (rbnDown.Checked)
            {
                btnOK.DialogResult = DialogResult.None;
                btnOK.Text = Strings.Download;
                btnOK.TextAlign = ContentAlignment.MiddleRight;
                btnOK.Image = Resources.download;
            }
            else
            {
                btnOK.DialogResult = DialogResult.OK;
                btnOK.Text = Strings.OK;
                btnOK.TextAlign = ContentAlignment.MiddleCenter;
                btnOK.Image = null;
            }


            if (rbnFile.Checked)
                ISOPath = txtFile.Text;

            if (fempty) fempty = false;

            CheckNotEmpty();
        }

        private void txtFile_TextChanged(object sender, EventArgs e)
        {
            btnOK.Enabled = !string.IsNullOrWhiteSpace(txtFile.Text);
            ISOPath = txtFile.Text;
        }

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
                cbxVersion.Enabled = cbxVersion.Visible = rtbIsoDesc.Visible = true;
                var ds = selinfo.Versions.Select(x => new {x.Name, Value = x}).ToList();
                cbxVersion.DataSource = ds;
                var latest = selinfo.LatestVersion;
                cbxVersion.SelectedIndex = 0;
                for (var i = 0; i < ds.Count; i++)
                    if (ds[i].Value == latest)
                    {
                        cbxVersion.SelectedIndex = i;
                        break;
                    }

                rtbIsoDesc.Text = selinfo.Description;
            }
            else
            {
                cbxVersion.Enabled = cbxVersion.Visible = rtbIsoDesc.Visible = false;
            }
        }

        private void setprg(int v)
        {
            pbxPrg.Value = v;
            lblPercent.Text = v + " %";
        }

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

        public void CheckNotEmpty()
        {
            if (rbnFile.Checked)
                btnOK.Enabled = !string.IsNullOrWhiteSpace(txtFile.Text);
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

        private void md5stuff()
        {
            th = new Thread(() =>
            {
                Invoke((MethodInvoker) (() => pbxLoading.Visible = true));

                var sw = new Stopwatch();
                sw.Start();
                var resk = ISOInfo.GetFromFile(ISOPath, false);
                sw.Stop();

                Invoke((MethodInvoker) (() =>
                {
                    if (resk == null)
                    {
                        cbxDetIso.SelectedIndex = 0;
                    }
                    else
                    {
                        IsoV = resk;
                        for (var index = 0; index < cbxDetIso.Items.Count; index++)
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

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (IsDownload)
            {
                IsoV = selinfoversion();
                if (IsoV != null && IsoV.DownloadLink != "") sfdIso.FileName = Path.GetFileName(IsoV.DownloadLink);

                if (sfdIso.ShowDialog(this) == DialogResult.OK)
                {
                    btnOK.Enabled = false;
                    ControlBox = false;

                    DownFile = sfdIso.FileName;
                    DownloadStuff();
                }
            }
        }

        private void ClientOnDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            if (sw.Elapsed.TotalSeconds >= 1)
                lblSpeed.Text = Program.GetSizeString(e.BytesReceived / (long) sw.Elapsed.TotalSeconds) + "/s";
            setprg(e.ProgressPercentage);

            lblProg.Text = Program.GetSizeString(e.BytesReceived) + " / " +
                           Program.GetSizeString(e.TotalBytesToReceive);
        }

        private void btnAnnul_Click(object sender, EventArgs e)
        {
            client?.CancelAsync();
        }


        private void cbxDetIso_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (changing) return;
            if (cbxDetIso.SelectedIndex != -1 && cbxDetIso.SelectedItem != null)
                IsoV = cbxDetIso.SelectedIndex == 0
                    ? new ISOV("other", "")
                    : selinfo2 == null
                        ? null
                        : selinfo2.LatestVersion ??
                          new ISOV("nover", selinfo2.Name, "", selinfo2.Filename, true) {Parent = selinfo2};
        }
    }
}