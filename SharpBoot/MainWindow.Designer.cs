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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.btnUSB = new System.Windows.Forms.Button();
            this.gbxTest = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnGen = new System.Windows.Forms.Button();
            this.txImInfo = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnCustomCode = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cbxRes = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cbxBootloader = new System.Windows.Forms.ComboBox();
            this.btnAbout = new System.Windows.Forms.Button();
            this.btnChecksum = new wyDay.Controls.SplitButton();
            this.cmsChecksum = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mD5ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnSha1 = new System.Windows.Forms.ToolStripMenuItem();
            this.btnSha256 = new System.Windows.Forms.ToolStripMenuItem();
            this.btnSha384 = new System.Windows.Forms.ToolStripMenuItem();
            this.btnSha512 = new System.Windows.Forms.ToolStripMenuItem();
            this.gbxBckd = new System.Windows.Forms.GroupBox();
            this.cbxBackType = new System.Windows.Forms.ComboBox();
            this.txtBackFile = new System.Windows.Forms.TextBox();
            this.btnBackBrowse = new System.Windows.Forms.Button();
            this.gbxTitle = new System.Windows.Forms.GroupBox();
            this.txtTitle = new System.Windows.Forms.TextBox();
            this.btnRemISO = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tbxSize = new System.Windows.Forms.TextBox();
            this.lvIsos = new System.Windows.Forms.DataGridView();
            this.clmnName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmnSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmnCate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmnDescr = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmnFilePath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmnCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.sharpBootToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.addISOToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.automaticallyAddISOInfoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mniUpdate = new System.Windows.Forms.ToolStripMenuItem();
            this.updateAvailableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.languageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.installBootloaderOnUSBKeyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.lblDragHere = new System.Windows.Forms.Label();
            this.pnlBottom.SuspendLayout();
            this.gbxTest.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.cmsChecksum.SuspendLayout();
            this.gbxBckd.SuspendLayout();
            this.gbxTitle.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lvIsos)).BeginInit();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            resources.ApplyResources(this.pnlBottom, "pnlBottom");
            this.pnlBottom.Controls.Add(this.button1);
            this.pnlBottom.Controls.Add(this.btnUSB);
            this.pnlBottom.Controls.Add(this.gbxTest);
            this.pnlBottom.Controls.Add(this.btnGen);
            this.pnlBottom.Controls.Add(this.txImInfo);
            this.pnlBottom.Name = "pnlBottom";
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.Image = global::SharpBoot.Properties.Resources.cd;
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // btnUSB
            // 
            resources.ApplyResources(this.btnUSB, "btnUSB");
            this.btnUSB.Image = global::SharpBoot.Properties.Resources.drive_disk;
            this.btnUSB.Name = "btnUSB";
            this.btnUSB.UseVisualStyleBackColor = true;
            this.btnUSB.Click += new System.EventHandler(this.btnUSB_Click);
            // 
            // gbxTest
            // 
            resources.ApplyResources(this.gbxTest, "gbxTest");
            this.gbxTest.AllowDrop = true;
            this.gbxTest.Controls.Add(this.label1);
            this.gbxTest.Name = "gbxTest";
            this.gbxTest.TabStop = false;
            this.gbxTest.DragDrop += new System.Windows.Forms.DragEventHandler(this.gbxTest_DragDrop);
            this.gbxTest.DragEnter += new System.Windows.Forms.DragEventHandler(this.gbxTest_DragEnter);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.AllowDrop = true;
            this.label1.Name = "label1";
            this.label1.DragDrop += new System.Windows.Forms.DragEventHandler(this.gbxTest_DragDrop);
            this.label1.DragEnter += new System.Windows.Forms.DragEventHandler(this.gbxTest_DragEnter);
            // 
            // btnGen
            // 
            resources.ApplyResources(this.btnGen, "btnGen");
            this.btnGen.Image = global::SharpBoot.Properties.Resources.cd;
            this.btnGen.Name = "btnGen";
            this.btnGen.UseVisualStyleBackColor = true;
            this.btnGen.Click += new System.EventHandler(this.btnGen_Click);
            // 
            // txImInfo
            // 
            resources.ApplyResources(this.txImInfo, "txImInfo");
            this.txImInfo.BackColor = System.Drawing.SystemColors.Window;
            this.txImInfo.Name = "txImInfo";
            this.txImInfo.ReadOnly = true;
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Controls.Add(this.btnCustomCode);
            this.panel1.Controls.Add(this.groupBox3);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.btnAbout);
            this.panel1.Controls.Add(this.btnChecksum);
            this.panel1.Controls.Add(this.gbxBckd);
            this.panel1.Controls.Add(this.gbxTitle);
            this.panel1.Controls.Add(this.btnRemISO);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Name = "panel1";
            // 
            // btnCustomCode
            // 
            resources.ApplyResources(this.btnCustomCode, "btnCustomCode");
            this.btnCustomCode.Image = global::SharpBoot.Properties.Resources.script_edit;
            this.btnCustomCode.Name = "btnCustomCode";
            this.btnCustomCode.UseVisualStyleBackColor = true;
            this.btnCustomCode.Click += new System.EventHandler(this.btnCustomCode_Click);
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
            this.cbxRes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
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
            this.cbxBootloader.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxBootloader.FormattingEnabled = true;
            this.cbxBootloader.Items.AddRange(new object[] {
            resources.GetString("cbxBootloader.Items"),
            resources.GetString("cbxBootloader.Items1")});
            this.cbxBootloader.Name = "cbxBootloader";
            this.cbxBootloader.SelectedIndexChanged += new System.EventHandler(this.cbxBootloader_SelectedIndexChanged);
            // 
            // btnAbout
            // 
            resources.ApplyResources(this.btnAbout, "btnAbout");
            this.btnAbout.Image = global::SharpBoot.Properties.Resources.question;
            this.btnAbout.Name = "btnAbout";
            this.btnAbout.UseVisualStyleBackColor = true;
            this.btnAbout.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnChecksum
            // 
            resources.ApplyResources(this.btnChecksum, "btnChecksum");
            this.btnChecksum.ContextMenuStrip = this.cmsChecksum;
            this.btnChecksum.Image = global::SharpBoot.Properties.Resources.gear_in;
            this.btnChecksum.Name = "btnChecksum";
            this.btnChecksum.SplitMenuStrip = this.cmsChecksum;
            this.btnChecksum.UseVisualStyleBackColor = true;
            this.btnChecksum.Click += new System.EventHandler(this.btnChecksum_Click);
            // 
            // cmsChecksum
            // 
            resources.ApplyResources(this.cmsChecksum, "cmsChecksum");
            this.cmsChecksum.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
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
            this.mD5ToolStripMenuItem.Click += new System.EventHandler(this.mD5ToolStripMenuItem_Click);
            // 
            // btnSha1
            // 
            resources.ApplyResources(this.btnSha1, "btnSha1");
            this.btnSha1.Name = "btnSha1";
            this.btnSha1.Click += new System.EventHandler(this.btnSha1_Click);
            // 
            // btnSha256
            // 
            resources.ApplyResources(this.btnSha256, "btnSha256");
            this.btnSha256.Name = "btnSha256";
            this.btnSha256.Click += new System.EventHandler(this.btnSha256_Click);
            // 
            // btnSha384
            // 
            resources.ApplyResources(this.btnSha384, "btnSha384");
            this.btnSha384.Name = "btnSha384";
            this.btnSha384.Click += new System.EventHandler(this.btnSha384_Click);
            // 
            // btnSha512
            // 
            resources.ApplyResources(this.btnSha512, "btnSha512");
            this.btnSha512.Name = "btnSha512";
            this.btnSha512.Click += new System.EventHandler(this.btnSha512_Click);
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
            this.cbxBackType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxBackType.FormattingEnabled = true;
            this.cbxBackType.Items.AddRange(new object[] {
            resources.GetString("cbxBackType.Items"),
            resources.GetString("cbxBackType.Items1"),
            resources.GetString("cbxBackType.Items2")});
            this.cbxBackType.Name = "cbxBackType";
            this.cbxBackType.SelectedIndexChanged += new System.EventHandler(this.cbxBackType_SelectedIndexChanged);
            // 
            // txtBackFile
            // 
            resources.ApplyResources(this.txtBackFile, "txtBackFile");
            this.txtBackFile.BackColor = System.Drawing.SystemColors.Window;
            this.txtBackFile.Name = "txtBackFile";
            this.txtBackFile.ReadOnly = true;
            // 
            // btnBackBrowse
            // 
            resources.ApplyResources(this.btnBackBrowse, "btnBackBrowse");
            this.btnBackBrowse.Name = "btnBackBrowse";
            this.btnBackBrowse.UseVisualStyleBackColor = true;
            this.btnBackBrowse.Click += new System.EventHandler(this.btnBackBrowse_Click);
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
            this.txtTitle.TextChanged += new System.EventHandler(this.txtTitle_TextChanged);
            // 
            // btnRemISO
            // 
            resources.ApplyResources(this.btnRemISO, "btnRemISO");
            this.btnRemISO.Image = global::SharpBoot.Properties.Resources.cd_delete1;
            this.btnRemISO.Name = "btnRemISO";
            this.btnRemISO.UseVisualStyleBackColor = true;
            this.btnRemISO.Click += new System.EventHandler(this.btnRemISO_Click);
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
            this.lvIsos.BackgroundColor = System.Drawing.SystemColors.Window;
            this.lvIsos.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lvIsos.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleVertical;
            this.lvIsos.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.lvIsos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.lvIsos.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.clmnName,
            this.clmnSize,
            this.clmnCate,
            this.clmnDescr,
            this.clmnFilePath,
            this.clmnCode});
            this.lvIsos.GridColor = System.Drawing.SystemColors.Window;
            this.lvIsos.MultiSelect = false;
            this.lvIsos.Name = "lvIsos";
            this.lvIsos.RowHeadersVisible = false;
            this.lvIsos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.lvIsos.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.lvIsos_CellValueChanged);
            this.lvIsos.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.lvIsos_RowsAdded);
            this.lvIsos.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.lvIsos_RowsRemoved);
            this.lvIsos.SelectionChanged += new System.EventHandler(this.lvIsos_SelectionChanged);
            this.lvIsos.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.lvIsos_UserDeletingRow);
            this.lvIsos.DragDrop += new System.Windows.Forms.DragEventHandler(this.lvIsos_DragDrop);
            this.lvIsos.DragEnter += new System.Windows.Forms.DragEventHandler(this.lvIsos_DragEnter);
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
            // clmnCode
            // 
            resources.ApplyResources(this.clmnCode, "clmnCode");
            this.clmnCode.Name = "clmnCode";
            // 
            // menuStrip
            // 
            resources.ApplyResources(this.menuStrip, "menuStrip");
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sharpBootToolStripMenuItem,
            this.mniUpdate,
            this.updateAvailableToolStripMenuItem,
            this.languageToolStripMenuItem,
            this.addFilesToolStripMenuItem,
            this.installBootloaderOnUSBKeyToolStripMenuItem});
            this.menuStrip.Name = "menuStrip";
            // 
            // sharpBootToolStripMenuItem
            // 
            resources.ApplyResources(this.sharpBootToolStripMenuItem, "sharpBootToolStripMenuItem");
            this.sharpBootToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
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
            this.openToolStripMenuItem.Image = global::SharpBoot.Properties.Resources.folder;
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            resources.ApplyResources(this.saveToolStripMenuItem, "saveToolStripMenuItem");
            this.saveToolStripMenuItem.Image = global::SharpBoot.Properties.Resources.file_save_as;
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            // 
            // addISOToolStripMenuItem
            // 
            resources.ApplyResources(this.addISOToolStripMenuItem, "addISOToolStripMenuItem");
            this.addISOToolStripMenuItem.Image = global::SharpBoot.Properties.Resources.cd_add;
            this.addISOToolStripMenuItem.Name = "addISOToolStripMenuItem";
            this.addISOToolStripMenuItem.Click += new System.EventHandler(this.addISOToolStripMenuItem_Click);
            // 
            // automaticallyAddISOInfoToolStripMenuItem
            // 
            resources.ApplyResources(this.automaticallyAddISOInfoToolStripMenuItem, "automaticallyAddISOInfoToolStripMenuItem");
            this.automaticallyAddISOInfoToolStripMenuItem.Checked = true;
            this.automaticallyAddISOInfoToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.automaticallyAddISOInfoToolStripMenuItem.Name = "automaticallyAddISOInfoToolStripMenuItem";
            this.automaticallyAddISOInfoToolStripMenuItem.Click += new System.EventHandler(this.automaticallyAddISOInfoToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            // 
            // exitToolStripMenuItem
            // 
            resources.ApplyResources(this.exitToolStripMenuItem, "exitToolStripMenuItem");
            this.exitToolStripMenuItem.Image = global::SharpBoot.Properties.Resources.door_out;
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // mniUpdate
            // 
            resources.ApplyResources(this.mniUpdate, "mniUpdate");
            this.mniUpdate.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.mniUpdate.Image = global::SharpBoot.Properties.Resources.update_anim;
            this.mniUpdate.Name = "mniUpdate";
            // 
            // updateAvailableToolStripMenuItem
            // 
            resources.ApplyResources(this.updateAvailableToolStripMenuItem, "updateAvailableToolStripMenuItem");
            this.updateAvailableToolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.updateAvailableToolStripMenuItem.Image = global::SharpBoot.Properties.Resources.package_go;
            this.updateAvailableToolStripMenuItem.Name = "updateAvailableToolStripMenuItem";
            this.updateAvailableToolStripMenuItem.Click += new System.EventHandler(this.updateAvailableToolStripMenuItem_Click);
            // 
            // languageToolStripMenuItem
            // 
            resources.ApplyResources(this.languageToolStripMenuItem, "languageToolStripMenuItem");
            this.languageToolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.languageToolStripMenuItem.Name = "languageToolStripMenuItem";
            // 
            // addFilesToolStripMenuItem
            // 
            resources.ApplyResources(this.addFilesToolStripMenuItem, "addFilesToolStripMenuItem");
            this.addFilesToolStripMenuItem.Image = global::SharpBoot.Properties.Resources.page_white_add;
            this.addFilesToolStripMenuItem.Name = "addFilesToolStripMenuItem";
            this.addFilesToolStripMenuItem.Click += new System.EventHandler(this.addFilesToolStripMenuItem_Click);
            // 
            // installBootloaderOnUSBKeyToolStripMenuItem
            // 
            resources.ApplyResources(this.installBootloaderOnUSBKeyToolStripMenuItem, "installBootloaderOnUSBKeyToolStripMenuItem");
            this.installBootloaderOnUSBKeyToolStripMenuItem.Image = global::SharpBoot.Properties.Resources.compile1;
            this.installBootloaderOnUSBKeyToolStripMenuItem.Name = "installBootloaderOnUSBKeyToolStripMenuItem";
            this.installBootloaderOnUSBKeyToolStripMenuItem.Click += new System.EventHandler(this.btnInstBoot_Click);
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
            // lblDragHere
            // 
            resources.ApplyResources(this.lblDragHere, "lblDragHere");
            this.lblDragHere.BackColor = System.Drawing.Color.White;
            this.lblDragHere.ForeColor = System.Drawing.Color.DarkGray;
            this.lblDragHere.Name = "lblDragHere";
            // 
            // MainWindow
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblDragHere);
            this.Controls.Add(this.lvIsos);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pnlBottom);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.Name = "MainWindow";
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.SizeChanged += new System.EventHandler(this.MainWindow_SizeChanged);
            this.pnlBottom.ResumeLayout(false);
            this.pnlBottom.PerformLayout();
            this.gbxTest.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.cmsChecksum.ResumeLayout(false);
            this.gbxBckd.ResumeLayout(false);
            this.gbxBckd.PerformLayout();
            this.gbxTitle.ResumeLayout(false);
            this.gbxTitle.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lvIsos)).EndInit();
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
        private Button btnInstBoot2;
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
        private ToolStripMenuItem mniUpdate;
        private ToolStripMenuItem updateAvailableToolStripMenuItem;
        private Label lblDragHere;
        private ToolStripMenuItem languageToolStripMenuItem;
        private Button btnCustomCode;
        private DataGridViewTextBoxColumn clmnCode;
        private DataGridViewTextBoxColumn clmnFilePath;
        private DataGridViewTextBoxColumn clmnDescr;
        private DataGridViewTextBoxColumn clmnCate;
        private DataGridViewTextBoxColumn clmnSize;
        private DataGridViewTextBoxColumn clmnName;
        private ToolStripMenuItem addFilesToolStripMenuItem;
        private ToolStripMenuItem installBootloaderOnUSBKeyToolStripMenuItem;
        private Button button1;
    }
}

