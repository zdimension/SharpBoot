using System;
using System.Windows.Forms;

namespace SharpBoot
{
    public partial class AskPath : AskPForm
    {
        public AskPath() 
        {
            InitializeComponent();
        }

        public override string FileName => tbxDest.Text;

        private void tbxDest_TextChanged(object sender, EventArgs e)
        {
            btnOK.Enabled = !(string.IsNullOrWhiteSpace(tbxDest.Text));
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            var g = new SaveFileDialog
            {
                Filter = Strings.ISOImg + " (*.iso)|*.iso",
                DefaultExt = "iso"
            };
            if (g.ShowDialog() == DialogResult.OK)
            {
                tbxDest.Text = g.FileName;
            }
        }
    }
}