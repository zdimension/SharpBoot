namespace SharpBoot
{
    partial class InstallBoot
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InstallBoot));
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblPercent = new System.Windows.Forms.Label();
            this.pbxPrg = new System.Windows.Forms.ProgressBar();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnAnnul = new System.Windows.Forms.Button();
            this.cbxUSB = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.rbnSyslinux = new System.Windows.Forms.RadioButton();
            this.rbnG4D = new System.Windows.Forms.RadioButton();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.Controls.Add(this.lblPercent);
            this.panel1.Controls.Add(this.pbxPrg);
            this.panel1.Controls.Add(this.btnOK);
            this.panel1.Controls.Add(this.btnAnnul);
            this.panel1.Name = "panel1";
            // 
            // lblPercent
            // 
            resources.ApplyResources(this.lblPercent, "lblPercent");
            this.lblPercent.Name = "lblPercent";
            // 
            // pbxPrg
            // 
            resources.ApplyResources(this.pbxPrg, "pbxPrg");
            this.pbxPrg.Name = "pbxPrg";
            // 
            // btnOK
            // 
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.Image = global::SharpBoot.Properties.Resources.accept_button1;
            this.btnOK.Name = "btnOK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnAnnul
            // 
            resources.ApplyResources(this.btnAnnul, "btnAnnul");
            this.btnAnnul.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnAnnul.Name = "btnAnnul";
            this.btnAnnul.UseVisualStyleBackColor = true;
            // 
            // cbxUSB
            // 
            resources.ApplyResources(this.cbxUSB, "cbxUSB");
            this.cbxUSB.DisplayMember = "Disp";
            this.cbxUSB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxUSB.FormattingEnabled = true;
            this.cbxUSB.Name = "cbxUSB";
            this.cbxUSB.ValueMember = "Value";
            this.cbxUSB.SelectedIndexChanged += new System.EventHandler(this.cbxUSB_SelectedIndexChanged);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // rbnSyslinux
            // 
            resources.ApplyResources(this.rbnSyslinux, "rbnSyslinux");
            this.rbnSyslinux.Checked = true;
            this.rbnSyslinux.Name = "rbnSyslinux";
            this.rbnSyslinux.TabStop = true;
            this.rbnSyslinux.UseVisualStyleBackColor = true;
            this.rbnSyslinux.CheckedChanged += new System.EventHandler(this.rbnSyslinux_CheckedChanged);
            // 
            // rbnG4D
            // 
            resources.ApplyResources(this.rbnG4D, "rbnG4D");
            this.rbnG4D.Name = "rbnG4D";
            this.rbnG4D.UseVisualStyleBackColor = true;
            this.rbnG4D.CheckedChanged += new System.EventHandler(this.rbnSyslinux_CheckedChanged);
            // 
            // InstallBoot
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ControlBox = false;
            this.Controls.Add(this.rbnG4D);
            this.Controls.Add(this.rbnSyslinux);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbxUSB);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InstallBoot";
            this.ShowInTaskbar = false;
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnAnnul;
        private System.Windows.Forms.ComboBox cbxUSB;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RadioButton rbnSyslinux;
        private System.Windows.Forms.RadioButton rbnG4D;
        private System.Windows.Forms.ProgressBar pbxPrg;
        private System.Windows.Forms.Label lblPercent;
    }
}