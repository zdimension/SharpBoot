using SharpBoot.Controls;

namespace SharpBoot.Forms
{
    partial class ThemeEditor
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnAnnul = new System.Windows.Forms.Button();
            this.fakeVGA1 = new SharpBoot.Controls.FakeVGA();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbxMenuFG = new SharpBoot.Controls.ColorComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbxMenuBG = new SharpBoot.Controls.ColorComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cbxMenuHighBG = new SharpBoot.Controls.ColorComboBox();
            this.cbxMenuHighFG = new SharpBoot.Controls.ColorComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cbxOtherHighBG = new SharpBoot.Controls.ColorComboBox();
            this.cbxOtherHighFG = new SharpBoot.Controls.ColorComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cbxOtherBG = new SharpBoot.Controls.ColorComboBox();
            this.cbxOtherFG = new SharpBoot.Controls.ColorComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.Controls.Add(this.btnOK);
            this.panel1.Controls.Add(this.btnAnnul);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 504);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(979, 59);
            this.panel1.TabIndex = 13;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Enabled = false;
            this.btnOK.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnOK.Location = new System.Drawing.Point(804, 18);
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
            this.btnAnnul.Location = new System.Drawing.Point(885, 18);
            this.btnAnnul.Name = "btnAnnul";
            this.btnAnnul.Size = new System.Drawing.Size(75, 23);
            this.btnAnnul.TabIndex = 4;
            this.btnAnnul.Text = "Cancel";
            this.btnAnnul.UseVisualStyleBackColor = true;
            // 
            // fakeVGA1
            // 
            this.fakeVGA1.BackColor = System.Drawing.Color.Black;
            this.fakeVGA1.Location = new System.Drawing.Point(327, 12);
            this.fakeVGA1.Name = "fakeVGA1";
            this.fakeVGA1.ScreenMemoryLocation = ((ushort)(0));
            this.fakeVGA1.Size = new System.Drawing.Size(640, 480);
            this.fakeVGA1.TabIndex = 14;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.cbxMenuBG);
            this.groupBox1.Controls.Add(this.cbxMenuFG);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(309, 83);
            this.groupBox1.TabIndex = 15;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Menu items";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Foreground";
            // 
            // cbxMenuFG
            // 
            this.cbxMenuFG.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxMenuFG.FormattingEnabled = true;
            this.cbxMenuFG.Location = new System.Drawing.Point(130, 23);
            this.cbxMenuFG.Name = "cbxMenuFG";
            this.cbxMenuFG.Size = new System.Drawing.Size(173, 23);
            this.cbxMenuFG.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "Background";
            // 
            // cbxMenuBG
            // 
            this.cbxMenuBG.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxMenuBG.FormattingEnabled = true;
            this.cbxMenuBG.Location = new System.Drawing.Point(130, 52);
            this.cbxMenuBG.Name = "cbxMenuBG";
            this.cbxMenuBG.Size = new System.Drawing.Size(173, 23);
            this.cbxMenuBG.TabIndex = 1;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.cbxMenuHighBG);
            this.groupBox2.Controls.Add(this.cbxMenuHighFG);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Location = new System.Drawing.Point(12, 101);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(309, 83);
            this.groupBox2.TabIndex = 16;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Menu items (highlighted)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 55);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 15);
            this.label3.TabIndex = 2;
            this.label3.Text = "Background";
            // 
            // cbxMenuHighBG
            // 
            this.cbxMenuHighBG.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxMenuHighBG.FormattingEnabled = true;
            this.cbxMenuHighBG.Location = new System.Drawing.Point(130, 52);
            this.cbxMenuHighBG.Name = "cbxMenuHighBG";
            this.cbxMenuHighBG.Size = new System.Drawing.Size(173, 23);
            this.cbxMenuHighBG.TabIndex = 1;
            // 
            // cbxMenuHighFG
            // 
            this.cbxMenuHighFG.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxMenuHighFG.FormattingEnabled = true;
            this.cbxMenuHighFG.Location = new System.Drawing.Point(130, 23);
            this.cbxMenuHighFG.Name = "cbxMenuHighFG";
            this.cbxMenuHighFG.Size = new System.Drawing.Size(173, 23);
            this.cbxMenuHighFG.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 26);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(69, 15);
            this.label4.TabIndex = 0;
            this.label4.Text = "Foreground";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.cbxOtherHighBG);
            this.groupBox3.Controls.Add(this.cbxOtherHighFG);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Location = new System.Drawing.Point(12, 279);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(309, 83);
            this.groupBox3.TabIndex = 18;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Other text (highlighted)";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 55);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 15);
            this.label5.TabIndex = 2;
            this.label5.Text = "Background";
            // 
            // cbxOtherHighBG
            // 
            this.cbxOtherHighBG.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxOtherHighBG.FormattingEnabled = true;
            this.cbxOtherHighBG.Location = new System.Drawing.Point(130, 52);
            this.cbxOtherHighBG.Name = "cbxOtherHighBG";
            this.cbxOtherHighBG.Size = new System.Drawing.Size(173, 23);
            this.cbxOtherHighBG.TabIndex = 1;
            // 
            // cbxOtherHighFG
            // 
            this.cbxOtherHighFG.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxOtherHighFG.FormattingEnabled = true;
            this.cbxOtherHighFG.Location = new System.Drawing.Point(130, 23);
            this.cbxOtherHighFG.Name = "cbxOtherHighFG";
            this.cbxOtherHighFG.Size = new System.Drawing.Size(173, 23);
            this.cbxOtherHighFG.TabIndex = 1;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 26);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(69, 15);
            this.label6.TabIndex = 0;
            this.label6.Text = "Foreground";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label7);
            this.groupBox4.Controls.Add(this.cbxOtherBG);
            this.groupBox4.Controls.Add(this.cbxOtherFG);
            this.groupBox4.Controls.Add(this.label8);
            this.groupBox4.Location = new System.Drawing.Point(12, 190);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(309, 83);
            this.groupBox4.TabIndex = 17;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Other text";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 55);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(71, 15);
            this.label7.TabIndex = 2;
            this.label7.Text = "Background";
            // 
            // cbxOtherBG
            // 
            this.cbxOtherBG.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxOtherBG.FormattingEnabled = true;
            this.cbxOtherBG.Location = new System.Drawing.Point(130, 52);
            this.cbxOtherBG.Name = "cbxOtherBG";
            this.cbxOtherBG.Size = new System.Drawing.Size(173, 23);
            this.cbxOtherBG.TabIndex = 1;
            // 
            // cbxOtherFG
            // 
            this.cbxOtherFG.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxOtherFG.FormattingEnabled = true;
            this.cbxOtherFG.Location = new System.Drawing.Point(130, 23);
            this.cbxOtherFG.Name = "cbxOtherFG";
            this.cbxOtherFG.Size = new System.Drawing.Size(173, 23);
            this.cbxOtherFG.TabIndex = 1;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 26);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(69, 15);
            this.label8.TabIndex = 0;
            this.label8.Text = "Foreground";
            // 
            // ThemeEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(979, 563);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.fakeVGA1);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ThemeEditor";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Edit theme";
            this.Load += new System.EventHandler(this.ThemeEditor_Load);
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnAnnul;
        private FakeVGA fakeVGA1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private ColorComboBox cbxMenuBG;
        private ColorComboBox cbxMenuFG;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label3;
        private ColorComboBox cbxMenuHighBG;
        private ColorComboBox cbxMenuHighFG;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label5;
        private ColorComboBox cbxOtherHighBG;
        private ColorComboBox cbxOtherHighFG;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label7;
        private ColorComboBox cbxOtherBG;
        private ColorComboBox cbxOtherFG;
        private System.Windows.Forms.Label label8;
    }
}