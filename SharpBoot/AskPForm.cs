using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SharpBoot
{
    public class AskPForm : Form
    {
        public virtual string FileName { get; }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // AskPForm
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "AskPForm";
            this.ResumeLayout(false);

        }
    }
}
