using System;
using System.IO;
using System.Windows.Forms;

namespace SharpBoot
{
    public partial class CustomEntryFrm : Form
    {
        public CustomEntryFrm()
        {
            InitializeComponent();
            btnOK.Text = Strings.OK;
            btnAnnul.Text = Strings.Cancel;
            lblHeader.Text = Strings.AddCustomEntry;
        }

        public EntryType SelectedType { get; set; } = EntryType.Nope;

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (ofp.ShowDialog(this) == DialogResult.OK)
            {
                tbxDest.Text = ofp.FileName;
            }
        }

        public string FilePath => ofp.FileName;

        private void cbxEntryType_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedType = (EntryType) cbxEntryType.SelectedIndex;
        }

        private void tbxDest_TextChanged(object sender, EventArgs e)
        {
            btnOK.Enabled = File.Exists(tbxDest.Text);
        }

        /*public void LoadTypes()
        {
            var lst = new[]
            {
                
            }.ToList();
        }*/
    }
}