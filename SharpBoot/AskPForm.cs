using System.Drawing;
using System.Windows.Forms;

namespace SharpBoot
{
    public class AskPForm : Form
    {
        public AskPForm()
        {
            InitializeComponent();
        }

        public virtual string FileName { get; }

        private void InitializeComponent()
        {
            SuspendLayout();
            // 
            // AskPForm
            // 
            ClientSize = new Size(284, 261);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Name = "AskPForm";
            ResumeLayout(false);

        }
    }
}
