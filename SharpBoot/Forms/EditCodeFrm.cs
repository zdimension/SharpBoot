using System;
using System.Windows.Forms;

namespace SharpBoot.Forms
{
    public partial class EditCodeFrm : Form
    {
        public EditCodeFrm()
        {
            InitializeComponent();
            lblHeader.Text = Program.editcode;
            btnOK.Text = Strings.OK;
            btnAnnul.Text = Strings.Cancel;
        }

        public EditCodeFrm(string fname) : this()
        {
            lblFilePath.Text = Program.fpath + ": " + fname;
        }

        public string Code
        {
            get => rtbCode.Text;
            set => rtbCode.Text = value;
        }

        private void rtbCode_TextChanged(object sender, EventArgs e)
        {
            btnOK.Enabled = !string.IsNullOrWhiteSpace(rtbCode.Text);
        }
    }
}