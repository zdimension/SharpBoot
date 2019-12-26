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
        public volatile bool abort;

        private bool closeonclick;

        protected WorkerFrm()
        {
            InitializeComponent();
            lblStatus.Text = Strings.Init;
            btnAnnul.Text = Strings.Cancel;
            lblAdditional.Text = lblSpeed.Text = "";
        }

        public event EventHandler WorkFinished;
        public event EventHandler WorkCancelled;

        public CancellationTokenSource CancellationTokenSource { get; } = new CancellationTokenSource();

        protected virtual void OnFinished(EventArgs e)
        {
            if (!abort)
            {
                ChangeProgress(100, 100, "");
                pbxLoading.InvokeIfRequired(() => { pbxLoading.Image = Resources.accept_button; });
            }

            ChangeAdditional("");

            WorkFinished?.Invoke(this, e);

            if (!abort)
                this.InvokeIfRequired(Close);
        }

        public void ChangeProgressBar(int val, int max)
        {
            if (abort) return;

            pbx.InvokeIfRequired(() =>
            {
                pbx.Maximum = max;
                pbx.Value = val;
            });
        }

        public void ChangeProgressBarEstimate(long val, long max, DateTime started, bool showSpeed=false)
        {
            Localization.UpdateThreadCulture();

            var speed = val / (DateTime.Now - started).TotalSeconds;
            var rem =  TimeSpan.FromSeconds(val == 0 ? 0 : (max - val) / speed);

            ChangeProgressBar((int)Math.Round(val * 10000.0 / max), 10000);

            if (showSpeed)
            {
                ChangeSpeed(FileIO.GetSpeedString((long)Math.Round(speed), 0));
            }

            ChangeAdditional(string.Format(Strings.RemainingTime, rem));
        }

        public void ChangeStatus(string stat)
        {
            lblStatus.InvokeIfRequired(() => lblStatus.Text = stat);
        }

        public void ChangeAdditional(string info)
        {
            lblAdditional.InvokeIfRequired(() => lblAdditional.Text = info);
        }

        public void ChangeSpeed(string speed)
        {
            lblSpeed.InvokeIfRequired(() => lblSpeed.Text = speed);
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
                CancelWork();
            }
        }

        public bool IsCancelled => abort || bwkWorker.CancellationPending;

        public void CancelWork()
        {
            SetCancel();
            closeonclick = true;
            abort = true;
            CancellationTokenSource.Cancel();
            bwkWorker.CancelAsync();
            WorkCancelled?.Invoke(this, EventArgs.Empty);
        }

        public abstract void DoWork();

        private void bwkWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Localization.UpdateThreadCulture();

            DoWork();

            this.InvokeIfRequired(() => OnFinished(EventArgs.Empty));
        }

        private void SetCancel()
        {
            ChangeProgress(0, 100, Strings.OpCancelled);

            btnAnnul.InvokeIfRequired(() =>
            {
                btnAnnul.Text = Strings.Close;
                btnAnnul.DialogResult = DialogResult.Cancel;
            });

            pbxLoading.InvokeIfRequired(() => { pbxLoading.Image = Resources.delete; });
        }

        private void bwkWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                if (e.Error is FileNotFoundException exception)
                    MessageBox.Show("File not found: " + exception.FileName);
                else throw new Exception("Error: " + e.Error.Message + "\n", e.Error);
            }
        }
    }
}