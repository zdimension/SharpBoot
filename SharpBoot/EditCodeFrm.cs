using System;
using System.Windows.Forms;

namespace SharpBoot
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

        public string Code
        {
            get { return rtbCode.Text; }
            set { rtbCode.Text = value; }
        }

        public EditCodeFrm(string fname) : this()
        {
            lblFilePath.Text = Program.fpath + ": " + fname;
        }

        private void rtbCode_TextChanged(object sender, EventArgs e)
        {
            btnOK.Enabled = !string.IsNullOrWhiteSpace(rtbCode.Text);
        }
    }
}