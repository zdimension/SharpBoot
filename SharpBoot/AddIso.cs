using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
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
            cbxISOS.DataSource = ISOInfo.ISOs.Select(x => new {x.Name, x.Category, x.LatestVersion.Hash});
            fempty = true;
            cbxISOS.SelectedItem = null;
            cbxVersion.SelectedItem = null;
        }

        private void rbnFile_CheckedChanged(object sender, EventArgs e)
        {
            txtFile.Enabled = rbnFile.Checked;
            btnBrowse.Enabled = rbnFile.Checked;
            cbxISOS.Visible = cbxVersion.Visible = rbnDown.Checked;

            if (rbnDown.Checked)
            {
                btnOK.DialogResult = DialogResult.None;
                btnOK.Text = Strings.Download;
            }
            else
            {
                btnOK.DialogResult = DialogResult.OK;
                btnOK.Text = "OK";
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

        private ISOInfo selinfo => ISOInfo.ISOs.FirstOrDefault(x =>
        {
            dynamic tmp = cbxISOS.SelectedValue;
            return x.LatestVersion.Hash == tmp.Hash;
        });

        private ISOV selinfoversion => selinfo.Versions.FirstOrDefault(x =>
        {
            dynamic tmp = cbxVersion.SelectedValue;
            return x.Name.Equals(tmp.Name.ToString());
        });

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

            var dn = selinfoversion.DownloadLink;

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
            IsoV = selinfoversion;
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
                md5stuff();
            }
        }


        private Thread th;

        private void md5stuff()
        {
            th = new Thread(() =>
            {
                string t = "";

                var resk = ISOInfo.GetFromFile(ISOPath);

                if (resk == null)
                {
                    t = Strings.CouldntDetect;
                }
                else
                {
                    t = Strings.Detected + " " + resk.Name;
                    IsoV = resk;
                }

                Invoke((MethodInvoker) (() => lblDetected.Text = t));
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
    }

    public enum ISOChooseType
    {
        Local = 1,
        Download = 2
    }

    public class ISOChooseResult
    {
        public ISOChooseType ISOType { get; set; }
        public string ISOPath { get; set; }

        public string ISOMd5 { get; set; }
    }
}