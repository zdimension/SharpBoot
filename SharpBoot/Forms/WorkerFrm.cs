using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using SharpBoot.Models;
using SharpBoot.Properties;
using SharpBoot.Utilities;

namespace SharpBoot.Forms
{
    [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
    public abstract partial class WorkerFrm : Form
    {
        public bool abort;

        private bool closeonclick;

        protected WorkerFrm()
        {
            InitializeComponent();
            lblStatus.Text = Strings.Init;
            btnAnnul.Text = Strings.Cancel;
        }

        public event EventHandler WorkFinished;

        protected virtual void OnFinished(EventArgs e)
        {
            ChangeProgress(100, 100, "");
            WorkFinished?.Invoke(this, e);
            this.InvokeIfRequired(Close);
        }

        public void ChangeProgressBar(int val, int max)
        {
            pbx.InvokeIfRequired(() =>
            {
                pbx.Maximum = max;
                pbx.Value = val;
            });
        }

        public void ChangeStatus(string stat)
        {
            lblStatus.InvokeIfRequired(() => lblStatus.Text = stat);
        }

        public void ChangeProgress(int val, int max, string stat)
        {
            ChangeProgressBar(val, max);

            ChangeStatus(stat);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            if (bwkWorker.IsBusy)
                bwkWorker.CancelAsync();
        }

        private void WorkerFrm_Load(object sender, EventArgs e)
        {
            Show();

            bwkWorker.RunWorkerAsync();
        }

        private void btnAnnul_Click(object sender, EventArgs e)
        {
            if (closeonclick)
            {
                DialogResult = DialogResult.Cancel;
                Close();
            }
            else
            {
                bwkWorker.CancelAsync();
                SetCancel();
            }
        }

        public abstract void DoWork();

        private void bwkISO_DoWork(object sender, DoWorkEventArgs e)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(Settings.Default.Lang);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(Settings.Default.Lang);

            DoWork();

            if (abort)
            {
                closeonclick = true;
                SetCancel();
            }

            Invoke((MethodInvoker)(() => OnFinished(EventArgs.Empty)));
        }

        private void SetCancel()
        {
            ChangeProgress(0, 100, Strings.OpCancelled);

            btnAnnul.InvokeIfRequired(() =>
            {
                btnAnnul.Text = Strings.Close;
                btnAnnul.DialogResult = DialogResult.Cancel;
            });
        }

        private void bwkISO_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                abort = true;
                SetCancel();
            }

            if (e.Error != null)
            {
                if (e.Error is FileNotFoundException exception)
                    MessageBox.Show("File not found: " + exception.FileName);
                else throw new Exception("Error: " + e.Error.Message + "\n", e.Error);
            }
        }
    }
}