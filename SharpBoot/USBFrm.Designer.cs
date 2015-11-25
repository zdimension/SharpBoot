namespace SharpBoot
{
    partial class USBFrm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(USBFrm));
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblPercent = new System.Windows.Forms.Label();
            this.pbxPrg = new System.Windows.Forms.ProgressBar();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnAnnul = new System.Windows.Forms.Button();
            this.cbxUSB = new System.Windows.Forms.ComboBox();
            this.lblHeader = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblCbx = new System.Windows.Forms.Label();
            this.comboBox = new System.Windows.Forms.ComboBox();
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
            // lblHeader
            // 
            resources.ApplyResources(this.lblHeader, "lblHeader");
            this.lblHeader.Name = "lblHeader";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // lblCbx
            // 
            resources.ApplyResources(this.lblCbx, "lblCbx");
            this.lblCbx.Name = "lblCbx";
            // 
            // comboBox
            // 
            resources.ApplyResources(this.comboBox, "comboBox");
            this.comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox.FormattingEnabled = true;
            this.comboBox.Items.AddRange(new object[] {
            resources.GetString("comboBox.Items"),
            resources.GetString("comboBox.Items1")});
            this.comboBox.Name = "comboBox";
            // 
            // USBFrm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ControlBox = false;
            this.Controls.Add(this.comboBox);
            this.Controls.Add(this.lblCbx);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblHeader);
            this.Controls.Add(this.cbxUSB);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "USBFrm";
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
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblCbx;
        private System.Windows.Forms.ProgressBar pbxPrg;
        private System.Windows.Forms.Label lblPercent;
        private System.Windows.Forms.ComboBox comboBox;
    }
}