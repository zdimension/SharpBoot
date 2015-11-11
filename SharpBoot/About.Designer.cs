using System.ComponentModel;
using System.Windows.Forms;

namespace SharpBoot
{
    partial class About
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(About));
            this.btnOK = new System.Windows.Forms.Button();
            this.lblAbout = new System.Windows.Forms.Label();
            this.lblUseSoftware = new System.Windows.Forms.Label();
            this.linkLabelEx6 = new SharpBoot.LinkLabelEx();
            this.linkLabelEx5 = new SharpBoot.LinkLabelEx();
            this.linkLabelEx4 = new SharpBoot.LinkLabelEx();
            this.linkLabelEx3 = new SharpBoot.LinkLabelEx();
            this.linkLabelEx1 = new SharpBoot.LinkLabelEx();
            this.lblQEMU = new SharpBoot.LinkLabelEx();
            this.linkLabelEx2 = new SharpBoot.LinkLabelEx();
            this.lbl7zip = new SharpBoot.LinkLabelEx();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Name = "btnOK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // lblAbout
            // 
            resources.ApplyResources(this.lblAbout, "lblAbout");
            this.lblAbout.Name = "lblAbout";
            // 
            // lblUseSoftware
            // 
            resources.ApplyResources(this.lblUseSoftware, "lblUseSoftware");
            this.lblUseSoftware.Name = "lblUseSoftware";
            // 
            // linkLabelEx6
            // 
            resources.ApplyResources(this.linkLabelEx6, "linkLabelEx6");
            this.linkLabelEx6.Name = "linkLabelEx6";
            this.linkLabelEx6.TabStop = true;
            this.linkLabelEx6.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelClicked);
            // 
            // linkLabelEx5
            // 
            resources.ApplyResources(this.linkLabelEx5, "linkLabelEx5");
            this.linkLabelEx5.Name = "linkLabelEx5";
            this.linkLabelEx5.TabStop = true;
            this.linkLabelEx5.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelClicked);
            // 
            // linkLabelEx4
            // 
            resources.ApplyResources(this.linkLabelEx4, "linkLabelEx4");
            this.linkLabelEx4.Name = "linkLabelEx4";
            this.linkLabelEx4.TabStop = true;
            this.linkLabelEx4.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelClicked);
            // 
            // linkLabelEx3
            // 
            resources.ApplyResources(this.linkLabelEx3, "linkLabelEx3");
            this.linkLabelEx3.Name = "linkLabelEx3";
            this.linkLabelEx3.TabStop = true;
            this.linkLabelEx3.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelClicked);
            // 
            // linkLabelEx1
            // 
            resources.ApplyResources(this.linkLabelEx1, "linkLabelEx1");
            this.linkLabelEx1.Name = "linkLabelEx1";
            this.linkLabelEx1.TabStop = true;
            this.linkLabelEx1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelClicked);
            // 
            // lblQEMU
            // 
            resources.ApplyResources(this.lblQEMU, "lblQEMU");
            this.lblQEMU.Name = "lblQEMU";
            this.lblQEMU.TabStop = true;
            this.lblQEMU.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelClicked);
            // 
            // linkLabelEx2
            // 
            resources.ApplyResources(this.linkLabelEx2, "linkLabelEx2");
            this.linkLabelEx2.Name = "linkLabelEx2";
            this.linkLabelEx2.TabStop = true;
            this.linkLabelEx2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelClicked);
            // 
            // lbl7zip
            // 
            resources.ApplyResources(this.lbl7zip, "lbl7zip");
            this.lbl7zip.Name = "lbl7zip";
            this.lbl7zip.TabStop = true;
            this.lbl7zip.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelClicked);
            // 
            // About
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.linkLabelEx6);
            this.Controls.Add(this.linkLabelEx5);
            this.Controls.Add(this.linkLabelEx4);
            this.Controls.Add(this.linkLabelEx3);
            this.Controls.Add(this.linkLabelEx1);
            this.Controls.Add(this.lblQEMU);
            this.Controls.Add(this.lblUseSoftware);
            this.Controls.Add(this.linkLabelEx2);
            this.Controls.Add(this.lbl7zip);
            this.Controls.Add(this.lblAbout);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "About";
            this.ShowInTaskbar = false;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button btnOK;
        private Label lblAbout;
        private LinkLabelEx lbl7zip;
        private LinkLabelEx linkLabelEx2;
        private Label lblUseSoftware;
        private LinkLabelEx lblQEMU;
        private LinkLabelEx linkLabelEx1;
        private LinkLabelEx linkLabelEx3;
        private LinkLabelEx linkLabelEx4;
        private LinkLabelEx linkLabelEx5;
        private LinkLabelEx linkLabelEx6;
    }
}