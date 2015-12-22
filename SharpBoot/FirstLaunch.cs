using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SharpBoot
{
    public partial class FirstLaunch : Form
    {
        public FirstLaunch()
        {
            InitializeComponent();

            btnRight.NormalImage.RotateFlip(RotateFlipType.RotateNoneFlipX);
            btnRight.HoverImage.RotateFlip(RotateFlipType.RotateNoneFlipX);
            btnRight.DownImage.RotateFlip(RotateFlipType.RotateNoneFlipX);

            Properties.Settings.Default.FirstLaunch = false;
        }

        private void btnLeft_Click(object sender, EventArgs e)
        {
            tablessControl1.SelectedIndex--;
        }

        private void btnRight_Click(object sender, EventArgs e)
        {
            tablessControl1.SelectedIndex++;
        }

        private void btnSkip_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void tablessControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnLeft.Visible = tablessControl1.SelectedIndex > 0;
            btnRight.Visible = tablessControl1.SelectedIndex < tablessControl1.TabCount - 1;
        }
    }
}
