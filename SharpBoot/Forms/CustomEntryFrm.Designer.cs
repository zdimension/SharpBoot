namespace SharpBoot.Forms
{
    partial class CustomEntryFrm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomEntryFrm));
            this.lblHeader = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnAnnul = new System.Windows.Forms.Button();
            this.cbxEntryType = new System.Windows.Forms.ComboBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.tbxDest = new System.Windows.Forms.TextBox();
            this.ofp = new System.Windows.Forms.OpenFileDialog();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblHeader
            // 
            resources.ApplyResources(this.lblHeader, "lblHeader");
            this.lblHeader.Name = "lblHeader";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.Controls.Add(this.btnOK);
            this.panel1.Controls.Add(this.btnAnnul);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // btnOK
            // 
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Name = "btnOK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnAnnul
            // 
            resources.ApplyResources(this.btnAnnul, "btnAnnul");
            this.btnAnnul.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnAnnul.Name = "btnAnnul";
            this.btnAnnul.UseVisualStyleBackColor = true;
            // 
            // cbxEntryType
            // 
            this.cbxEntryType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxEntryType.FormattingEnabled = true;
            this.cbxEntryType.Items.AddRange(new object[] {
            resources.GetString("cbxEntryType.Items"),
            resources.GetString("cbxEntryType.Items1"),
            resources.GetString("cbxEntryType.Items2"),
            resources.GetString("cbxEntryType.Items3"),
            resources.GetString("cbxEntryType.Items4"),
            resources.GetString("cbxEntryType.Items5"),
            resources.GetString("cbxEntryType.Items6"),
            resources.GetString("cbxEntryType.Items7"),
            resources.GetString("cbxEntryType.Items8")});
            resources.ApplyResources(this.cbxEntryType, "cbxEntryType");
            this.cbxEntryType.Name = "cbxEntryType";
            this.cbxEntryType.SelectedIndexChanged += new System.EventHandler(this.cbxEntryType_SelectedIndexChanged);
            // 
            // btnBrowse
            // 
            this.btnBrowse.Image = global::SharpBoot.Properties.Resources.folder;
            resources.ApplyResources(this.btnBrowse, "btnBrowse");
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // tbxDest
            // 
            resources.ApplyResources(this.tbxDest, "tbxDest");
            this.tbxDest.Name = "tbxDest";
            this.tbxDest.TextChanged += new System.EventHandler(this.tbxDest_TextChanged);
            // 
            // CustomEntryFrm
            // 
            this.AcceptButton = this.btnOK;
            resources.ApplyResources(this, "$this");
                        this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnAnnul;
            this.ControlBox = false;
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.tbxDest);
            this.Controls.Add(this.cbxEntryType);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lblHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CustomEntryFrm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnAnnul;
        private System.Windows.Forms.ComboBox cbxEntryType;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.TextBox tbxDest;
        private System.Windows.Forms.OpenFileDialog ofp;
    }
}