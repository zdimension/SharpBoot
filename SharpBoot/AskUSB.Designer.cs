using System.ComponentModel;
using System.Windows.Forms;

namespace SharpBoot
{
    partial class AskUSB
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AskUSB));
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnAnnul = new System.Windows.Forms.Button();
            this.ofpISO = new System.Windows.Forms.SaveFileDialog();
            this.cbxUSB = new System.Windows.Forms.ComboBox();
            this.cbxFS = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
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
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.Name = "btnOK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnAnnul
            // 
            this.btnAnnul.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.btnAnnul, "btnAnnul");
            this.btnAnnul.Name = "btnAnnul";
            this.btnAnnul.UseVisualStyleBackColor = true;
            // 
            // ofpISO
            // 
            this.ofpISO.DefaultExt = "iso";
            resources.ApplyResources(this.ofpISO, "ofpISO");
            this.ofpISO.SupportMultiDottedExtensions = true;
            // 
            // cbxUSB
            // 
            this.cbxUSB.DisplayMember = "Disp";
            this.cbxUSB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxUSB.FormattingEnabled = true;
            resources.ApplyResources(this.cbxUSB, "cbxUSB");
            this.cbxUSB.Name = "cbxUSB";
            this.cbxUSB.ValueMember = "Value";
            this.cbxUSB.SelectedIndexChanged += new System.EventHandler(this.cbxUSB_SelectedIndexChanged);
            // 
            // cbxFS
            // 
            this.cbxFS.DisplayMember = "Disp";
            this.cbxFS.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxFS.FormattingEnabled = true;
            this.cbxFS.Items.AddRange(new object[] {
            resources.GetString("cbxFS.Items"),
            resources.GetString("cbxFS.Items1")});
            resources.ApplyResources(this.cbxFS, "cbxFS");
            this.cbxFS.Name = "cbxFS";
            this.cbxFS.ValueMember = "Value";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // AskUSB
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ControlBox = false;
            this.Controls.Add(this.cbxFS);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbxUSB);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "AskUSB";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label label1;
        private Panel panel1;
        private Button btnOK;
        private Button btnAnnul;
        private SaveFileDialog ofpISO;
        private ComboBox cbxUSB;
        private ComboBox cbxFS;
        private Label label2;
    }
}