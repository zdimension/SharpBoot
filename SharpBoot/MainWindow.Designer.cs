using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using SharpBoot.Properties;
using wyDay.Controls;

namespace SharpBoot
{
    partial class MainWindow
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager resources = new ComponentResourceManager(typeof(MainWindow));
            this.pnlBottom = new Panel();
            this.btnUSB = new Button();
            this.btnInstBoot = new Button();
            this.gbxTest = new GroupBox();
            this.label1 = new Label();
            this.btnGen = new Button();
            this.txImInfo = new TextBox();
            this.panel1 = new Panel();
            this.groupBox3 = new GroupBox();
            this.cbxRes = new ComboBox();
            this.groupBox2 = new GroupBox();
            this.cbxBootloader = new ComboBox();
            this.btnAbout = new Button();
            this.gbxLng = new GroupBox();
            this.cbxLng = new ComboBox();
            this.cmsChecksum = new ContextMenuStrip(this.components);
            this.mD5ToolStripMenuItem = new ToolStripMenuItem();
            this.btnSha1 = new ToolStripMenuItem();
            this.btnSha256 = new ToolStripMenuItem();
            this.btnSha384 = new ToolStripMenuItem();
            this.btnSha512 = new ToolStripMenuItem();
            this.gbxBckd = new GroupBox();
            this.cbxBackType = new ComboBox();
            this.txtBackFile = new TextBox();
            this.btnBackBrowse = new Button();
            this.gbxTitle = new GroupBox();
            this.txtTitle = new TextBox();
            this.btnRemISO = new Button();
            this.groupBox1 = new GroupBox();
            this.tbxSize = new TextBox();
            this.lvIsos = new DataGridView();
            this.clmnName = new DataGridViewTextBoxColumn();
            this.clmnSize = new DataGridViewTextBoxColumn();
            this.clmnCate = new DataGridViewTextBoxColumn();
            this.clmnDescr = new DataGridViewTextBoxColumn();
            this.clmnFilePath = new DataGridViewTextBoxColumn();
            this.menuStrip = new MenuStrip();
            this.sharpBootToolStripMenuItem = new ToolStripMenuItem();
            this.openToolStripMenuItem = new ToolStripMenuItem();
            this.saveToolStripMenuItem = new ToolStripMenuItem();
            this.toolStripSeparator2 = new ToolStripSeparator();
            this.addISOToolStripMenuItem = new ToolStripMenuItem();
            this.automaticallyAddISOInfoToolStripMenuItem = new ToolStripMenuItem();
            this.toolStripSeparator1 = new ToolStripSeparator();
            this.exitToolStripMenuItem = new ToolStripMenuItem();
            this.openFileDialog = new OpenFileDialog();
            this.saveFileDialog = new SaveFileDialog();
            this.btnChecksum = new SplitButton();
            this.pnlBottom.SuspendLayout();
            this.gbxTest.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.gbxLng.SuspendLayout();
            this.cmsChecksum.SuspendLayout();
            this.gbxBckd.SuspendLayout();
            this.gbxTitle.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((ISupportInitialize)(this.lvIsos)).BeginInit();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            resources.ApplyResources(this.pnlBottom, "pnlBottom");
            this.pnlBottom.Controls.Add(this.btnUSB);
            this.pnlBottom.Controls.Add(this.btnInstBoot);
            this.pnlBottom.Controls.Add(this.gbxTest);
            this.pnlBottom.Controls.Add(this.btnGen);
            this.pnlBottom.Controls.Add(this.txImInfo);
            this.pnlBottom.Name = "pnlBottom";
            // 
            // btnUSB
            // 
            resources.ApplyResources(this.btnUSB, "btnUSB");
            this.btnUSB.Image = Resources.drive_disk;
            this.btnUSB.Name = "btnUSB";
            this.btnUSB.UseVisualStyleBackColor = true;
            this.btnUSB.Click += new EventHandler(this.btnUSB_Click);
            // 
            // btnInstBoot
            // 
            resources.ApplyResources(this.btnInstBoot, "btnInstBoot");
            this.btnInstBoot.Image = Resources.compile;
            this.btnInstBoot.Name = "btnInstBoot";
            this.btnInstBoot.UseVisualStyleBackColor = true;
            this.btnInstBoot.Click += new EventHandler(this.btnInstBoot_Click);
            // 
            // gbxTest
            // 
            resources.ApplyResources(this.gbxTest, "gbxTest");
            this.gbxTest.AllowDrop = true;
            this.gbxTest.Controls.Add(this.label1);
            this.gbxTest.Name = "gbxTest";
            this.gbxTest.TabStop = false;
            this.gbxTest.DragDrop += new DragEventHandler(this.gbxTest_DragDrop);
            this.gbxTest.DragEnter += new DragEventHandler(this.gbxTest_DragEnter);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.AllowDrop = true;
            this.label1.Name = "label1";
            this.label1.DragDrop += new DragEventHandler(this.gbxTest_DragDrop);
            this.label1.DragEnter += new DragEventHandler(this.gbxTest_DragEnter);
            // 
            // btnGen
            // 
            resources.ApplyResources(this.btnGen, "btnGen");
            this.btnGen.Image = Resources.cd;
            this.btnGen.Name = "btnGen";
            this.btnGen.UseVisualStyleBackColor = true;
            this.btnGen.Click += new EventHandler(this.btnGen_Click);
            // 
            // txImInfo
            // 
            resources.ApplyResources(this.txImInfo, "txImInfo");
            this.txImInfo.BackColor = SystemColors.Window;
            this.txImInfo.Name = "txImInfo";
            this.txImInfo.ReadOnly = true;
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Controls.Add(this.groupBox3);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.btnAbout);
            this.panel1.Controls.Add(this.gbxLng);
            this.panel1.Controls.Add(this.btnChecksum);
            this.panel1.Controls.Add(this.gbxBckd);
            this.panel1.Controls.Add(this.gbxTitle);
            this.panel1.Controls.Add(this.btnRemISO);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Name = "panel1";
            // 
            // groupBox3
            // 
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Controls.Add(this.cbxRes);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // cbxRes
            // 
            resources.ApplyResources(this.cbxRes, "cbxRes");
            this.cbxRes.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbxRes.FormattingEnabled = true;
            this.cbxRes.Items.AddRange(new object[] {
            resources.GetString("cbxRes.Items"),
            resources.GetString("cbxRes.Items1"),
            resources.GetString("cbxRes.Items2")});
            this.cbxRes.Name = "cbxRes";
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Controls.Add(this.cbxBootloader);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // cbxBootloader
            // 
            resources.ApplyResources(this.cbxBootloader, "cbxBootloader");
            this.cbxBootloader.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbxBootloader.FormattingEnabled = true;
            this.cbxBootloader.Items.AddRange(new object[] {
            resources.GetString("cbxBootloader.Items"),
            resources.GetString("cbxBootloader.Items1")});
            this.cbxBootloader.Name = "cbxBootloader";
            this.cbxBootloader.SelectedIndexChanged += new EventHandler(this.cbxBootloader_SelectedIndexChanged);
            // 
            // btnAbout
            // 
            resources.ApplyResources(this.btnAbout, "btnAbout");
            this.btnAbout.Image = Resources.question;
            this.btnAbout.Name = "btnAbout";
            this.btnAbout.UseVisualStyleBackColor = true;
            this.btnAbout.Click += new EventHandler(this.button1_Click);
            // 
            // gbxLng
            // 
            resources.ApplyResources(this.gbxLng, "gbxLng");
            this.gbxLng.Controls.Add(this.cbxLng);
            this.gbxLng.Name = "gbxLng";
            this.gbxLng.TabStop = false;
            // 
            // cbxLng
            // 
            resources.ApplyResources(this.cbxLng, "cbxLng");
            this.cbxLng.DisplayMember = "Name";
            this.cbxLng.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbxLng.FormattingEnabled = true;
            this.cbxLng.Name = "cbxLng";
            this.cbxLng.ValueMember = "Value";
            this.cbxLng.SelectedIndexChanged += new EventHandler(this.cbxLng_SelectedIndexChanged);
            // 
            // cmsChecksum
            // 
            resources.ApplyResources(this.cmsChecksum, "cmsChecksum");
            this.cmsChecksum.Items.AddRange(new ToolStripItem[] {
            this.mD5ToolStripMenuItem,
            this.btnSha1,
            this.btnSha256,
            this.btnSha384,
            this.btnSha512});
            this.cmsChecksum.Name = "cmsChecksum";
            // 
            // mD5ToolStripMenuItem
            // 
            resources.ApplyResources(this.mD5ToolStripMenuItem, "mD5ToolStripMenuItem");
            this.mD5ToolStripMenuItem.Name = "mD5ToolStripMenuItem";
            this.mD5ToolStripMenuItem.Click += new EventHandler(this.mD5ToolStripMenuItem_Click);
            // 
            // btnSha1
            // 
            resources.ApplyResources(this.btnSha1, "btnSha1");
            this.btnSha1.Name = "btnSha1";
            this.btnSha1.Click += new EventHandler(this.btnSha1_Click);
            // 
            // btnSha256
            // 
            resources.ApplyResources(this.btnSha256, "btnSha256");
            this.btnSha256.Name = "btnSha256";
            this.btnSha256.Click += new EventHandler(this.btnSha256_Click);
            // 
            // btnSha384
            // 
            resources.ApplyResources(this.btnSha384, "btnSha384");
            this.btnSha384.Name = "btnSha384";
            this.btnSha384.Click += new EventHandler(this.btnSha384_Click);
            // 
            // btnSha512
            // 
            resources.ApplyResources(this.btnSha512, "btnSha512");
            this.btnSha512.Name = "btnSha512";
            this.btnSha512.Click += new EventHandler(this.btnSha512_Click);
            // 
            // gbxBckd
            // 
            resources.ApplyResources(this.gbxBckd, "gbxBckd");
            this.gbxBckd.Controls.Add(this.cbxBackType);
            this.gbxBckd.Controls.Add(this.txtBackFile);
            this.gbxBckd.Controls.Add(this.btnBackBrowse);
            this.gbxBckd.Name = "gbxBckd";
            this.gbxBckd.TabStop = false;
            // 
            // cbxBackType
            // 
            resources.ApplyResources(this.cbxBackType, "cbxBackType");
            this.cbxBackType.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbxBackType.FormattingEnabled = true;
            this.cbxBackType.Items.AddRange(new object[] {
            resources.GetString("cbxBackType.Items"),
            resources.GetString("cbxBackType.Items1"),
            resources.GetString("cbxBackType.Items2")});
            this.cbxBackType.Name = "cbxBackType";
            this.cbxBackType.SelectedIndexChanged += new EventHandler(this.cbxBackType_SelectedIndexChanged);
            // 
            // txtBackFile
            // 
            resources.ApplyResources(this.txtBackFile, "txtBackFile");
            this.txtBackFile.BackColor = SystemColors.Window;
            this.txtBackFile.Name = "txtBackFile";
            this.txtBackFile.ReadOnly = true;
            // 
            // btnBackBrowse
            // 
            resources.ApplyResources(this.btnBackBrowse, "btnBackBrowse");
            this.btnBackBrowse.Name = "btnBackBrowse";
            this.btnBackBrowse.UseVisualStyleBackColor = true;
            this.btnBackBrowse.Click += new EventHandler(this.btnBackBrowse_Click);
            // 
            // gbxTitle
            // 
            resources.ApplyResources(this.gbxTitle, "gbxTitle");
            this.gbxTitle.Controls.Add(this.txtTitle);
            this.gbxTitle.Name = "gbxTitle";
            this.gbxTitle.TabStop = false;
            // 
            // txtTitle
            // 
            resources.ApplyResources(this.txtTitle, "txtTitle");
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.TextChanged += new EventHandler(this.txtTitle_TextChanged);
            // 
            // btnRemISO
            // 
            resources.ApplyResources(this.btnRemISO, "btnRemISO");
            this.btnRemISO.Image = Resources.cd_delete1;
            this.btnRemISO.Name = "btnRemISO";
            this.btnRemISO.UseVisualStyleBackColor = true;
            this.btnRemISO.Click += new EventHandler(this.btnRemISO_Click);
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.tbxSize);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // tbxSize
            // 
            resources.ApplyResources(this.tbxSize, "tbxSize");
            this.tbxSize.Name = "tbxSize";
            this.tbxSize.ReadOnly = true;
            // 
            // lvIsos
            // 
            resources.ApplyResources(this.lvIsos, "lvIsos");
            this.lvIsos.AllowDrop = true;
            this.lvIsos.AllowUserToAddRows = false;
            this.lvIsos.AllowUserToResizeRows = false;
            this.lvIsos.BackgroundColor = SystemColors.Window;
            this.lvIsos.BorderStyle = BorderStyle.None;
            this.lvIsos.CellBorderStyle = DataGridViewCellBorderStyle.SingleVertical;
            this.lvIsos.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            this.lvIsos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.lvIsos.Columns.AddRange(new DataGridViewColumn[] {
            this.clmnName,
            this.clmnSize,
            this.clmnCate,
            this.clmnDescr,
            this.clmnFilePath});
            this.lvIsos.GridColor = SystemColors.Window;
            this.lvIsos.MultiSelect = false;
            this.lvIsos.Name = "lvIsos";
            this.lvIsos.RowHeadersVisible = false;
            this.lvIsos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.lvIsos.CellValueChanged += new DataGridViewCellEventHandler(this.lvIsos_CellValueChanged);
            this.lvIsos.RowsAdded += new DataGridViewRowsAddedEventHandler(this.lvIsos_RowsAdded);
            this.lvIsos.RowsRemoved += new DataGridViewRowsRemovedEventHandler(this.lvIsos_RowsRemoved);
            this.lvIsos.SelectionChanged += new EventHandler(this.lvIsos_SelectionChanged);
            this.lvIsos.UserDeletingRow += new DataGridViewRowCancelEventHandler(this.lvIsos_UserDeletingRow);
            this.lvIsos.DragDrop += new DragEventHandler(this.lvIsos_DragDrop);
            this.lvIsos.DragEnter += new DragEventHandler(this.lvIsos_DragEnter);
            // 
            // clmnName
            // 
            resources.ApplyResources(this.clmnName, "clmnName");
            this.clmnName.Name = "clmnName";
            // 
            // clmnSize
            // 
            resources.ApplyResources(this.clmnSize, "clmnSize");
            this.clmnSize.Name = "clmnSize";
            this.clmnSize.ReadOnly = true;
            // 
            // clmnCate
            // 
            resources.ApplyResources(this.clmnCate, "clmnCate");
            this.clmnCate.Name = "clmnCate";
            // 
            // clmnDescr
            // 
            resources.ApplyResources(this.clmnDescr, "clmnDescr");
            this.clmnDescr.Name = "clmnDescr";
            // 
            // clmnFilePath
            // 
            resources.ApplyResources(this.clmnFilePath, "clmnFilePath");
            this.clmnFilePath.Name = "clmnFilePath";
            this.clmnFilePath.ReadOnly = true;
            // 
            // menuStrip
            // 
            resources.ApplyResources(this.menuStrip, "menuStrip");
            this.menuStrip.Items.AddRange(new ToolStripItem[] {
            this.sharpBootToolStripMenuItem});
            this.menuStrip.Name = "menuStrip";
            // 
            // sharpBootToolStripMenuItem
            // 
            resources.ApplyResources(this.sharpBootToolStripMenuItem, "sharpBootToolStripMenuItem");
            this.sharpBootToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.toolStripSeparator2,
            this.addISOToolStripMenuItem,
            this.automaticallyAddISOInfoToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.sharpBootToolStripMenuItem.Name = "sharpBootToolStripMenuItem";
            // 
            // openToolStripMenuItem
            // 
            resources.ApplyResources(this.openToolStripMenuItem, "openToolStripMenuItem");
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Click += new EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            resources.ApplyResources(this.saveToolStripMenuItem, "saveToolStripMenuItem");
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Click += new EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            // 
            // addISOToolStripMenuItem
            // 
            resources.ApplyResources(this.addISOToolStripMenuItem, "addISOToolStripMenuItem");
            this.addISOToolStripMenuItem.Image = Resources.cd_add;
            this.addISOToolStripMenuItem.Name = "addISOToolStripMenuItem";
            this.addISOToolStripMenuItem.Click += new EventHandler(this.addISOToolStripMenuItem_Click);
            // 
            // automaticallyAddISOInfoToolStripMenuItem
            // 
            resources.ApplyResources(this.automaticallyAddISOInfoToolStripMenuItem, "automaticallyAddISOInfoToolStripMenuItem");
            this.automaticallyAddISOInfoToolStripMenuItem.Checked = true;
            this.automaticallyAddISOInfoToolStripMenuItem.CheckState = CheckState.Checked;
            this.automaticallyAddISOInfoToolStripMenuItem.Name = "automaticallyAddISOInfoToolStripMenuItem";
            this.automaticallyAddISOInfoToolStripMenuItem.Click += new EventHandler(this.automaticallyAddISOInfoToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            // 
            // exitToolStripMenuItem
            // 
            resources.ApplyResources(this.exitToolStripMenuItem, "exitToolStripMenuItem");
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Click += new EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.DefaultExt = "sbt";
            resources.ApplyResources(this.openFileDialog, "openFileDialog");
            this.openFileDialog.SupportMultiDottedExtensions = true;
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.DefaultExt = "sbt";
            resources.ApplyResources(this.saveFileDialog, "saveFileDialog");
            // 
            // btnChecksum
            // 
            resources.ApplyResources(this.btnChecksum, "btnChecksum");
            this.btnChecksum.ContextMenuStrip = this.cmsChecksum;
            this.btnChecksum.Image = Resources.gear_in;
            this.btnChecksum.Name = "btnChecksum";
            this.btnChecksum.SplitMenuStrip = this.cmsChecksum;
            this.btnChecksum.UseVisualStyleBackColor = true;
            this.btnChecksum.Click += new EventHandler(this.btnChecksum_Click);
            // 
            // MainWindow
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = AutoScaleMode.Font;
            this.Controls.Add(this.lvIsos);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pnlBottom);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.Name = "MainWindow";
            this.Load += new EventHandler(this.MainWindow_Load);
            this.pnlBottom.ResumeLayout(false);
            this.pnlBottom.PerformLayout();
            this.gbxTest.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.gbxLng.ResumeLayout(false);
            this.cmsChecksum.ResumeLayout(false);
            this.gbxBckd.ResumeLayout(false);
            this.gbxBckd.PerformLayout();
            this.gbxTitle.ResumeLayout(false);
            this.gbxTitle.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((ISupportInitialize)(this.lvIsos)).EndInit();
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Panel pnlBottom;
        private Panel panel1;
        private GroupBox groupBox1;
        private TextBox tbxSize;
        private Button btnGen;
        private TextBox txImInfo;
        private Button btnRemISO;
        private GroupBox gbxTest;
        private Label label1;
        private DataGridView lvIsos;
        private GroupBox gbxTitle;
        private TextBox txtTitle;
        private GroupBox gbxBckd;
        private TextBox txtBackFile;
        private Button btnBackBrowse;
        private SplitButton btnChecksum;
        private GroupBox gbxLng;
        private ComboBox cbxLng;
        private Button btnAbout;
        private MenuStrip menuStrip;
        private ToolStripMenuItem sharpBootToolStripMenuItem;
        private ToolStripMenuItem addISOToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem automaticallyAddISOInfoToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator2;
        private OpenFileDialog openFileDialog;
        private SaveFileDialog saveFileDialog;
        private Button btnInstBoot;
        private ContextMenuStrip cmsChecksum;
        private ToolStripMenuItem btnSha1;
        private ToolStripMenuItem btnSha256;
        private ToolStripMenuItem btnSha512;
        private ToolStripMenuItem btnSha384;
        private GroupBox groupBox2;
        private ComboBox cbxBootloader;
        private GroupBox groupBox3;
        private ComboBox cbxRes;
        private ComboBox cbxBackType;
        private Button btnUSB;
        private ToolStripMenuItem mD5ToolStripMenuItem;
        private DataGridViewTextBoxColumn clmnName;
        private DataGridViewTextBoxColumn clmnSize;
        private DataGridViewTextBoxColumn clmnCate;
        private DataGridViewTextBoxColumn clmnDescr;
        private DataGridViewTextBoxColumn clmnFilePath;
    }
}

