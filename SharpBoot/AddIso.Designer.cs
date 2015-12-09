using System.ComponentModel;
using System.Windows.Forms;

namespace SharpBoot
{
    partial class AddIso
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddIso));
            this.btnOK = new System.Windows.Forms.Button();
            this.btnAnnul = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.rbnFile = new System.Windows.Forms.RadioButton();
            this.txtFile = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.rbnDown = new System.Windows.Forms.RadioButton();
            this.ofpIso = new System.Windows.Forms.OpenFileDialog();
            this.lblPercent = new System.Windows.Forms.Label();
            this.pbxPrg = new System.Windows.Forms.ProgressBar();
            this.sfdIso = new System.Windows.Forms.SaveFileDialog();
            this.cbxVersion = new System.Windows.Forms.ComboBox();
            this.lblSpeed = new System.Windows.Forms.Label();
            this.lblProg = new System.Windows.Forms.Label();
            this.pbxLoading = new System.Windows.Forms.PictureBox();
            this.rtbIsoDesc = new SharpBoot.DisabledRichTextBox();
            this.cbxDetIso = new GroupedComboBox();
            this.cbxISOS = new GroupedComboBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbxLoading)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
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
            this.btnAnnul.Click += new System.EventHandler(this.btnAnnul_Click);
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.Controls.Add(this.btnOK);
            this.panel1.Controls.Add(this.btnAnnul);
            this.panel1.Name = "panel1";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // rbnFile
            // 
            resources.ApplyResources(this.rbnFile, "rbnFile");
            this.rbnFile.Checked = true;
            this.rbnFile.Name = "rbnFile";
            this.rbnFile.TabStop = true;
            this.rbnFile.UseVisualStyleBackColor = true;
            this.rbnFile.CheckedChanged += new System.EventHandler(this.rbnFile_CheckedChanged);
            // 
            // txtFile
            // 
            resources.ApplyResources(this.txtFile, "txtFile");
            this.txtFile.BackColor = System.Drawing.Color.White;
            this.txtFile.Name = "txtFile";
            this.txtFile.ReadOnly = true;
            this.txtFile.TextChanged += new System.EventHandler(this.txtFile_TextChanged);
            // 
            // btnBrowse
            // 
            resources.ApplyResources(this.btnBrowse, "btnBrowse");
            this.btnBrowse.Image = global::SharpBoot.Properties.Resources.folder;
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // rbnDown
            // 
            resources.ApplyResources(this.rbnDown, "rbnDown");
            this.rbnDown.Name = "rbnDown";
            this.rbnDown.UseVisualStyleBackColor = true;
            this.rbnDown.CheckedChanged += new System.EventHandler(this.rbnFile_CheckedChanged);
            // 
            // ofpIso
            // 
            this.ofpIso.DefaultExt = "iso";
            resources.ApplyResources(this.ofpIso, "ofpIso");
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
            // sfdIso
            // 
            this.sfdIso.DefaultExt = "iso";
            resources.ApplyResources(this.sfdIso, "sfdIso");
            // 
            // cbxVersion
            // 
            resources.ApplyResources(this.cbxVersion, "cbxVersion");
            this.cbxVersion.DisplayMember = "Name";
            this.cbxVersion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxVersion.FormattingEnabled = true;
            this.cbxVersion.Name = "cbxVersion";
            // 
            // lblSpeed
            // 
            resources.ApplyResources(this.lblSpeed, "lblSpeed");
            this.lblSpeed.Name = "lblSpeed";
            // 
            // lblProg
            // 
            resources.ApplyResources(this.lblProg, "lblProg");
            this.lblProg.Name = "lblProg";
            // 
            // pbxLoading
            // 
            resources.ApplyResources(this.pbxLoading, "pbxLoading");
            this.pbxLoading.BackColor = System.Drawing.Color.Transparent;
            this.pbxLoading.Image = global::SharpBoot.Properties.Resources.ajax_loader;
            this.pbxLoading.Name = "pbxLoading";
            this.pbxLoading.TabStop = false;
            // 
            // rtbIsoDesc
            // 
            resources.ApplyResources(this.rtbIsoDesc, "rtbIsoDesc");
            this.rtbIsoDesc.BackColor = System.Drawing.Color.White;
            this.rtbIsoDesc.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbIsoDesc.Cursor = System.Windows.Forms.Cursors.Default;
            this.rtbIsoDesc.Name = "rtbIsoDesc";
            // 
            // cbxDetIso
            // 
            resources.ApplyResources(this.cbxDetIso, "cbxDetIso");
            this.cbxDetIso.DataSource = null;
            this.cbxDetIso.DisplayMember = "Name";
            this.cbxDetIso.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxDetIso.FormattingEnabled = true;
            this.cbxDetIso.GroupMember = "Category";
            this.cbxDetIso.Name = "cbxDetIso";
            this.cbxDetIso.ValueMember = "Val";
            this.cbxDetIso.SelectedIndexChanged += new System.EventHandler(this.cbxDetIso_SelectedIndexChanged);
            // 
            // cbxISOS
            // 
            resources.ApplyResources(this.cbxISOS, "cbxISOS");
            this.cbxISOS.DataSource = null;
            this.cbxISOS.DisplayMember = "Name";
            this.cbxISOS.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxISOS.FormattingEnabled = true;
            this.cbxISOS.GroupMember = "Category";
            this.cbxISOS.Name = "cbxISOS";
            this.cbxISOS.ValueMember = "Val";
            this.cbxISOS.SelectedIndexChanged += new System.EventHandler(this.cbxISOS_SelectedIndexChanged);
            // 
            // AddIso
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.rtbIsoDesc);
            this.Controls.Add(this.pbxLoading);
            this.Controls.Add(this.cbxDetIso);
            this.Controls.Add(this.lblProg);
            this.Controls.Add(this.lblSpeed);
            this.Controls.Add(this.cbxVersion);
            this.Controls.Add(this.lblPercent);
            this.Controls.Add(this.pbxPrg);
            this.Controls.Add(this.cbxISOS);
            this.Controls.Add(this.rbnDown);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.txtFile);
            this.Controls.Add(this.rbnFile);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddIso";
            this.ShowInTaskbar = false;
            this.Load += new System.EventHandler(this.AddIso_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbxLoading)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button btnOK;
        private Button btnAnnul;
        private Panel panel1;
        private Label label1;
        private RadioButton rbnFile;
        private TextBox txtFile;
        private Button btnBrowse;
        private RadioButton rbnDown;
        private GroupedComboBox cbxISOS;
        private OpenFileDialog ofpIso;
        private Label lblPercent;
        private ProgressBar pbxPrg;
        private SaveFileDialog sfdIso;
        private ComboBox cbxVersion;
        private Label lblSpeed;
        private Label lblProg;
        private GroupedComboBox cbxDetIso;
        private PictureBox pbxLoading;
        private DisabledRichTextBox rtbIsoDesc;
    }
}