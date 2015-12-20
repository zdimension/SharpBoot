using System;
using System.Windows.Forms;

namespace SharpBoot
{
    public partial class PathSelector : SelectorCtrl
    {
        public PathSelector()
        {
            InitializeComponent();
        }

        public new string Value => tbxDest.Text;

        public EventHandler ValueChanged = delegate { };

        private void tbxDest_TextChanged(object sender, EventArgs e)
        {
            ValueChanged(sender, e);
            IsValid = !(string.IsNullOrWhiteSpace(tbxDest.Text));
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