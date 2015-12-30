namespace SharpBoot
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
            this.lblHeader.AutoSize = true;
            this.lblHeader.Font = new System.Drawing.Font("Segoe UI", 15.75F);
            this.lblHeader.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblHeader.Location = new System.Drawing.Point(10, 11);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(207, 30);
            this.lblHeader.TabIndex = 11;
            this.lblHeader.Text = "Header custom entry";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.Controls.Add(this.btnOK);
            this.panel1.Controls.Add(this.btnAnnul);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 317);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(444, 59);
            this.panel1.TabIndex = 12;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Enabled = false;
            this.btnOK.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnOK.Location = new System.Drawing.Point(269, 18);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnAnnul
            // 
            this.btnAnnul.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAnnul.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnAnnul.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnAnnul.Location = new System.Drawing.Point(350, 18);
            this.btnAnnul.Name = "btnAnnul";
            this.btnAnnul.Size = new System.Drawing.Size(75, 23);
            this.btnAnnul.TabIndex = 4;
            this.btnAnnul.Text = "Cancel";
            this.btnAnnul.UseVisualStyleBackColor = true;
            // 
            // cbxEntryType
            // 
            this.cbxEntryType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxEntryType.FormattingEnabled = true;
            this.cbxEntryType.Items.AddRange(new object[] {
            "Windows ntldr/setupldr.bin/bootmgr/grldr",
            "Grub4DOS grldr",
            "NT/2000/XP cmldr",
            "FreeDOS kernel.sys",
            "MS-DOS io.sys",
            "MS-DOS 7+ io.sys",
            "PC-DOS ibmbio.com",
            "DRMK dellbio.bin",
            "ReactOS freeldr"});
            this.cbxEntryType.Location = new System.Drawing.Point(11, 53);
            this.cbxEntryType.Name = "cbxEntryType";
            this.cbxEntryType.Size = new System.Drawing.Size(421, 23);
            this.cbxEntryType.TabIndex = 13;
            this.cbxEntryType.SelectedIndexChanged += new System.EventHandler(this.cbxEntryType_SelectedIndexChanged);
            // 
            // btnBrowse
            // 
            this.btnBrowse.Image = global::SharpBoot.Properties.Resources.folder;
            this.btnBrowse.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnBrowse.Location = new System.Drawing.Point(402, 105);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(31, 25);
            this.btnBrowse.TabIndex = 15;
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // tbxDest
            // 
            this.tbxDest.Location = new System.Drawing.Point(11, 106);
            this.tbxDest.Name = "tbxDest";
            this.tbxDest.Size = new System.Drawing.Size(385, 23);
            this.tbxDest.TabIndex = 14;
            this.tbxDest.TextChanged += new System.EventHandler(this.tbxDest_TextChanged);
            // 
            // CustomEntryFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(444, 376);
            this.ControlBox = false;
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.tbxDest);
            this.Controls.Add(this.cbxEntryType);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lblHeader);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CustomEntryFrm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = " ";
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