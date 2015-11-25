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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(About));
            this.btnOK = new System.Windows.Forms.Button();
            this.ilTranslators = new System.Windows.Forms.ImageList(this.components);
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.rtbMyWebsite = new System.Windows.Forms.RichTextBox();
            this.rbnHelpTranslate = new System.Windows.Forms.RichTextBox();
            this.lvTranslators = new SharpBoot.CustomListView();
            this.clmnName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clmnURL = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // btnOK
            // 
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Name = "btnOK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // ilTranslators
            // 
            this.ilTranslators.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilTranslators.ImageStream")));
            this.ilTranslators.TransparentColor = System.Drawing.Color.Transparent;
            this.ilTranslators.Images.SetKeyName(0, "flag_germany.png");
            this.ilTranslators.Images.SetKeyName(1, "flag_france.png");
            this.ilTranslators.Images.SetKeyName(2, "flag_romania.png");
            this.ilTranslators.Images.SetKeyName(3, "flag_china.png");
            // 
            // richTextBox1
            // 
            resources.ApplyResources(this.richTextBox1, "richTextBox1");
            this.richTextBox1.BackColor = System.Drawing.SystemColors.Control;
            this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox1.Cursor = System.Windows.Forms.Cursors.Default;
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.richTextBox1_LinkClicked);
            // 
            // rtbMyWebsite
            // 
            resources.ApplyResources(this.rtbMyWebsite, "rtbMyWebsite");
            this.rtbMyWebsite.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbMyWebsite.Cursor = System.Windows.Forms.Cursors.Default;
            this.rtbMyWebsite.Name = "rtbMyWebsite";
            this.rtbMyWebsite.ReadOnly = true;
            // 
            // rbnHelpTranslate
            // 
            resources.ApplyResources(this.rbnHelpTranslate, "rbnHelpTranslate");
            this.rbnHelpTranslate.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rbnHelpTranslate.Cursor = System.Windows.Forms.Cursors.Default;
            this.rbnHelpTranslate.Name = "rbnHelpTranslate";
            this.rbnHelpTranslate.ReadOnly = true;
            // 
            // lvTranslators
            // 
            resources.ApplyResources(this.lvTranslators, "lvTranslators");
            this.lvTranslators.BackColor = System.Drawing.SystemColors.Control;
            this.lvTranslators.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lvTranslators.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.clmnName,
            this.clmnURL});
            this.lvTranslators.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvTranslators.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            ((System.Windows.Forms.ListViewItem)(resources.GetObject("lvTranslators.Items"))),
            ((System.Windows.Forms.ListViewItem)(resources.GetObject("lvTranslators.Items1"))),
            ((System.Windows.Forms.ListViewItem)(resources.GetObject("lvTranslators.Items2"))),
            ((System.Windows.Forms.ListViewItem)(resources.GetObject("lvTranslators.Items3")))});
            this.lvTranslators.LargeImageList = this.ilTranslators;
            this.lvTranslators.MultiSelect = false;
            this.lvTranslators.Name = "lvTranslators";
            this.lvTranslators.SmallImageList = this.ilTranslators;
            this.lvTranslators.StateImageList = this.ilTranslators;
            this.lvTranslators.UseCompatibleStateImageBehavior = false;
            this.lvTranslators.View = System.Windows.Forms.View.Details;
            this.lvTranslators.DoubleClick += new System.EventHandler(this.lvTranslators_DoubleClick);
            // 
            // clmnName
            // 
            resources.ApplyResources(this.clmnName, "clmnName");
            // 
            // clmnURL
            // 
            resources.ApplyResources(this.clmnURL, "clmnURL");
            // 
            // About
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lvTranslators);
            this.Controls.Add(this.rbnHelpTranslate);
            this.Controls.Add(this.rtbMyWebsite);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.richTextBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "About";
            this.ShowInTaskbar = false;
            this.ResumeLayout(false);

        }

        #endregion

        private Button btnOK;
        private CustomListView lvTranslators;
        private ColumnHeader clmnName;
        private ImageList ilTranslators;
        private ColumnHeader clmnURL;
        private RichTextBox richTextBox1;
        private RichTextBox rtbMyWebsite;
        private RichTextBox rbnHelpTranslate;
    }
}