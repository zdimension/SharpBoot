using System.ComponentModel;
using System.Windows.Forms;

namespace SharpBoot.Forms
{
    partial class GenIsoFrm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GenIsoFrm));
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnAnnul = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.pbx = new System.Windows.Forms.ProgressBar();
            this.bwkISO = new System.ComponentModel.BackgroundWorker();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.Controls.Add(this.btnAnnul);
            this.panel1.Name = "panel1";
            // 
            // btnAnnul
            // 
            resources.ApplyResources(this.btnAnnul, "btnAnnul");
            this.btnAnnul.Name = "btnAnnul";
            this.btnAnnul.UseVisualStyleBackColor = true;
            this.btnAnnul.Click += new System.EventHandler(this.btnAnnul_Click);
            // 
            // lblStatus
            // 
            resources.ApplyResources(this.lblStatus, "lblStatus");
            this.lblStatus.Name = "lblStatus";
            // 
            // pbx
            // 
            resources.ApplyResources(this.pbx, "pbx");
            this.pbx.Name = "pbx";
            // 
            // bwkISO
            // 
            this.bwkISO.WorkerSupportsCancellation = true;
            this.bwkISO.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bwkISO_DoWork);
            this.bwkISO.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bwkISO_RunWorkerCompleted);
            // 
            // GenIsoFrm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ControlBox = false;
            this.Controls.Add(this.pbx);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "GenIsoFrm";
            this.Load += new System.EventHandler(this.GenIsoFrm_Load);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Panel panel1;
        private Button btnAnnul;
        private Label lblStatus;
        private ProgressBar pbx;
        private BackgroundWorker bwkISO;
    }
}