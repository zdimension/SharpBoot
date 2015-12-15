namespace SharpBoot
{
    partial class TestFrm
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
            this.customTextBox1 = new SharpBoot.CustomTextBox();
            this.SuspendLayout();
            // 
            // customTextBox1
            // 
            this.customTextBox1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.customTextBox1.Location = new System.Drawing.Point(53, 104);
            this.customTextBox1.Name = "customTextBox1";
            this.customTextBox1.Size = new System.Drawing.Size(180, 23);
            this.customTextBox1.TabIndex = 0;
            // 
            // TestFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.customTextBox1);
            this.Name = "TestFrm";
            this.Text = "TestFrm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CustomTextBox customTextBox1;
    }
}