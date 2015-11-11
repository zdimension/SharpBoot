using System.Diagnostics;
using System.Windows.Forms;

namespace SharpBoot
{
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();
            lblAbout.Text = lblAbout.Text.Insert(9, " " + Program.GetVersion());
        }

        private void linkLabelClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(((LinkLabelEx)sender).Text);
        }
    }
}
