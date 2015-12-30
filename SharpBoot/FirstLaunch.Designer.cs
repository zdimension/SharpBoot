using System.Windows.Forms;

namespace SharpBoot
{
    partial class FirstLaunch
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
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.btnRight = new SharpBoot.ThreeStateButton();
            this.btnLeft = new SharpBoot.ThreeStateButton();
            this.btnSkip = new System.Windows.Forms.Button();
            this.tablessControl1 = new SharpBoot.TablessControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.pnlBottom.SuspendLayout();
            this.tablessControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblHeader
            // 
            this.lblHeader.BackColor = System.Drawing.Color.Blue;
            this.lblHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblHeader.Font = new System.Drawing.Font("Segoe UI", 15.75F);
            this.lblHeader.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblHeader.Location = new System.Drawing.Point(0, 0);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.lblHeader.Size = new System.Drawing.Size(684, 48);
            this.lblHeader.TabIndex = 12;
            this.lblHeader.Text = "Welcome to SharpBoot";
            this.lblHeader.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // pnlBottom
            // 
            this.pnlBottom.BackColor = System.Drawing.SystemColors.Control;
            this.pnlBottom.Controls.Add(this.btnRight);
            this.pnlBottom.Controls.Add(this.btnLeft);
            this.pnlBottom.Controls.Add(this.btnSkip);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 417);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(684, 60);
            this.pnlBottom.TabIndex = 14;
            // 
            // btnRight
            // 
            this.btnRight.BackgroundImage = global::SharpBoot.Properties.Resources.chevron_left;
            this.btnRight.DownImage = global::SharpBoot.Properties.Resources.chevron_left_d;
            this.btnRight.FlatAppearance.BorderSize = 0;
            this.btnRight.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnRight.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnRight.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRight.HoverImage = global::SharpBoot.Properties.Resources.chevron_left_h;
            this.btnRight.Location = new System.Drawing.Point(350, 14);
            this.btnRight.Name = "btnRight";
            this.btnRight.NormalImage = global::SharpBoot.Properties.Resources.chevron_left;
            this.btnRight.Size = new System.Drawing.Size(32, 32);
            this.btnRight.TabIndex = 2;
            this.btnRight.UseVisualStyleBackColor = true;
            this.btnRight.Click += new System.EventHandler(this.btnRight_Click);
            // 
            // btnLeft
            // 
            this.btnLeft.BackgroundImage = global::SharpBoot.Properties.Resources.chevron_left;
            this.btnLeft.DownImage = global::SharpBoot.Properties.Resources.chevron_left_d;
            this.btnLeft.FlatAppearance.BorderSize = 0;
            this.btnLeft.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnLeft.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnLeft.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLeft.HoverImage = global::SharpBoot.Properties.Resources.chevron_left_h;
            this.btnLeft.Location = new System.Drawing.Point(303, 14);
            this.btnLeft.Name = "btnLeft";
            this.btnLeft.NormalImage = global::SharpBoot.Properties.Resources.chevron_left;
            this.btnLeft.Size = new System.Drawing.Size(32, 32);
            this.btnLeft.TabIndex = 1;
            this.btnLeft.UseVisualStyleBackColor = true;
            this.btnLeft.Visible = false;
            this.btnLeft.Click += new System.EventHandler(this.btnLeft_Click);
            // 
            // btnSkip
            // 
            this.btnSkip.Location = new System.Drawing.Point(18, 18);
            this.btnSkip.Name = "btnSkip";
            this.btnSkip.Size = new System.Drawing.Size(75, 24);
            this.btnSkip.TabIndex = 0;
            this.btnSkip.Text = "Skip";
            this.btnSkip.UseVisualStyleBackColor = true;
            this.btnSkip.Click += new System.EventHandler(this.btnSkip_Click);
            // 
            // tablessControl1
            // 
            this.tablessControl1.Controls.Add(this.tabPage1);
            this.tablessControl1.Controls.Add(this.tabPage2);
            this.tablessControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tablessControl1.Location = new System.Drawing.Point(0, 48);
            this.tablessControl1.Name = "tablessControl1";
            this.tablessControl1.SelectedIndex = 0;
            this.tablessControl1.Size = new System.Drawing.Size(684, 369);
            this.tablessControl1.TabIndex = 13;
            this.tablessControl1.SelectedIndexChanged += new System.EventHandler(this.tablessControl1_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.Green;
            this.tabPage1.Location = new System.Drawing.Point(4, 24);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(676, 341);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 24);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(676, 341);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // FirstLaunch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(684, 477);
            this.Controls.Add(this.tablessControl1);
            this.Controls.Add(this.pnlBottom);
            this.Controls.Add(this.lblHeader);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FirstLaunch";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.pnlBottom.ResumeLayout(false);
            this.tablessControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblHeader;
        private TablessControl tablessControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Panel pnlBottom;
        private Button btnSkip;
        private ThreeStateButton btnLeft;
        private ThreeStateButton btnRight;
    }
}