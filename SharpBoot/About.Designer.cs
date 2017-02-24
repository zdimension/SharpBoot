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
            this.label1 = new System.Windows.Forms.Label();
            this.lvTranslators = new SharpBoot.CustomListView();
            this.clmnName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clmnURL = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.rtbMyWebsite = new SharpBoot.DisabledRichTextBox();
            this.rbnHelpTranslate = new SharpBoot.DisabledRichTextBox();
            this.disabledRichTextBox1 = new SharpBoot.DisabledRichTextBox();
            this.pbLogo = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).BeginInit();
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
            this.ilTranslators.Images.SetKeyName(4, "flag_taiwan.png");
            this.ilTranslators.Images.SetKeyName(5, "flag_russia.png");
            this.ilTranslators.Images.SetKeyName(6, "flag_ukraine.png");
            this.ilTranslators.Images.SetKeyName(7, "flag_spain.png");
            this.ilTranslators.Images.SetKeyName(8, "flag_czech_republic.png");
            this.ilTranslators.Images.SetKeyName(9, "flag_italy.png");
            this.ilTranslators.Images.SetKeyName(10, "flag_portugal.png");
            this.ilTranslators.Images.SetKeyName(11, "flag_poland.png");
            this.ilTranslators.Images.SetKeyName(12, "flag_hungary.png");
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Name = "label1";
            // 
            // lvTranslators
            // 
            resources.ApplyResources(this.lvTranslators, "lvTranslators");
            this.lvTranslators.BackColor = System.Drawing.SystemColors.Control;
            this.lvTranslators.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lvTranslators.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.clmnName,
            this.clmnURL});
            this.lvTranslators.FullRowSelect = true;
            this.lvTranslators.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvTranslators.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            ((System.Windows.Forms.ListViewItem)(resources.GetObject("lvTranslators.Items"))),
            ((System.Windows.Forms.ListViewItem)(resources.GetObject("lvTranslators.Items1"))),
            ((System.Windows.Forms.ListViewItem)(resources.GetObject("lvTranslators.Items2"))),
            ((System.Windows.Forms.ListViewItem)(resources.GetObject("lvTranslators.Items3"))),
            ((System.Windows.Forms.ListViewItem)(resources.GetObject("lvTranslators.Items4"))),
            ((System.Windows.Forms.ListViewItem)(resources.GetObject("lvTranslators.Items5"))),
            ((System.Windows.Forms.ListViewItem)(resources.GetObject("lvTranslators.Items6"))),
            ((System.Windows.Forms.ListViewItem)(resources.GetObject("lvTranslators.Items7"))),
            ((System.Windows.Forms.ListViewItem)(resources.GetObject("lvTranslators.Items8"))),
            ((System.Windows.Forms.ListViewItem)(resources.GetObject("lvTranslators.Items9"))),
            ((System.Windows.Forms.ListViewItem)(resources.GetObject("lvTranslators.Items10"))),
            ((System.Windows.Forms.ListViewItem)(resources.GetObject("lvTranslators.Items11"))),
            ((System.Windows.Forms.ListViewItem)(resources.GetObject("lvTranslators.Items12")))});
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
            // rtbMyWebsite
            // 
            resources.ApplyResources(this.rtbMyWebsite, "rtbMyWebsite");
            this.rtbMyWebsite.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbMyWebsite.Cursor = System.Windows.Forms.Cursors.Hand;
            this.rtbMyWebsite.Name = "rtbMyWebsite";
            this.rtbMyWebsite.ReadOnly = true;
            this.rtbMyWebsite.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.richTextBox1_LinkClicked);
            // 
            // rbnHelpTranslate
            // 
            resources.ApplyResources(this.rbnHelpTranslate, "rbnHelpTranslate");
            this.rbnHelpTranslate.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rbnHelpTranslate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.rbnHelpTranslate.Name = "rbnHelpTranslate";
            this.rbnHelpTranslate.ReadOnly = true;
            this.rbnHelpTranslate.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.richTextBox1_LinkClicked);
            // 
            // disabledRichTextBox1
            // 
            resources.ApplyResources(this.disabledRichTextBox1, "disabledRichTextBox1");
            this.disabledRichTextBox1.BackColor = System.Drawing.SystemColors.Control;
            this.disabledRichTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.disabledRichTextBox1.Cursor = System.Windows.Forms.Cursors.Default;
            this.disabledRichTextBox1.Name = "disabledRichTextBox1";
            this.disabledRichTextBox1.ReadOnly = true;
            this.disabledRichTextBox1.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.richTextBox1_LinkClicked);
            // 
            // pbLogo
            // 
            resources.ApplyResources(this.pbLogo, "pbLogo");
            this.pbLogo.Name = "pbLogo";
            this.pbLogo.TabStop = false;
            this.pbLogo.Click += new System.EventHandler(this.pbLogo_Click);
            // 
            // About
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pbLogo);
            this.Controls.Add(this.lvTranslators);
            this.Controls.Add(this.disabledRichTextBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.rbnHelpTranslate);
            this.Controls.Add(this.rtbMyWebsite);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "About";
            this.ShowInTaskbar = false;
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        

        #endregion

        private Button btnOK;
        private CustomListView lvTranslators;
        private ColumnHeader clmnName;
        private ImageList ilTranslators;
        private DisabledRichTextBox rtbMyWebsite;
        private DisabledRichTextBox rbnHelpTranslate;
        private ColumnHeader clmnURL;
        private Label label1;
        private DisabledRichTextBox disabledRichTextBox1;
        private PictureBox pbLogo;
    }
}