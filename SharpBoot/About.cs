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
            Utils.SetWindowTheme(lvTranslators.Handle, "explorer", null);
        }

        private void linkLabelClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(((LinkLabelEx)sender).Text);
        }

        private void lvTranslators_DoubleClick(object sender, System.EventArgs e)
        {
            if(lvTranslators.SelectedItems.Count == 1)
            {
                var it = lvTranslators.SelectedItems[0];
                var url = "";
                if (it.SubItems.Count > 1) url = it.SubItems[1].Text;
                if(!string.IsNullOrEmpty(url)) Process.Start(url);
                lvTranslators.SelectedIndices.Clear();
            }
        }
    }
}
