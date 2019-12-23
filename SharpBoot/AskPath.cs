using System;
using System.Drawing;
using System.Windows.Forms;

namespace SharpBoot
{
    public partial class AskPath
    {
        public AskPath()
        {
            InitializeComponent();
            btnOK.Text = Strings.OK;
            btnAnnul.Text = Strings.Cancel;
        }

        public string FileName => tbxDest.Text;

        private void tbxDest_TextChanged(object sender, EventArgs e)
        {
            btnOK.Enabled = !string.IsNullOrWhiteSpace(tbxDest.Text);
        }

        public void SetTextMode(string title, string message, string def = "")
        {
            Text = title;
            label1.Text = message;
            btnBrowse.Visible = false;
            tbxDest.Text = def;
            tbxDest.Size = new Size(431, tbxDest.Height);
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            var g = new SaveFileDialog
            {
                Filter = Strings.ISOImg + " (*.iso)|*.iso",
                DefaultExt = "iso"
            };
            if (g.ShowDialog() == DialogResult.OK) tbxDest.Text = g.FileName;
        }
    }
}