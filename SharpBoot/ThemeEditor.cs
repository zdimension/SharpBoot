using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SharpBoot
{
    public partial class ThemeEditor : Form
    {
        public ThemeEditor()
        {
            InitializeComponent();

            TheBootloader = new Syslinux();
            SetSyslinux();

            //fakeVGA1.BackgroundImage = Utils.DrawFilledRectangle(640, 480, Color.White);
            fakeVGA1.BackgroundImage = Image.FromStream(new MemoryStream(Properties.Resources.sharpboot));
        }

        private void ThemeEditor_Load(object sender, EventArgs e)
        {
            fakeVGA1.Write(10, 10, "test", Color.Red, Color.Transparent);
        }

        public IBootloader TheBootloader { get; set; }

        public void DrawSyslinuxBG(Graphics g)
        {
            g.FillRectangle(new SolidBrush(Color.FromArgb(182, 0, 0, 0)), 40, 8, 560, 256);
        }

        public void DrawGrub4DOSBG(Graphics g)
        {

        }


        public void SetSyslinux()
        {
            fakeVGA1.SetAfterPaintBackgroundHandler(DrawSyslinuxBG);

            fakeVGA1.Write(40, 8, "┌", Color.White, Color.Transparent);
            fakeVGA1.Write(41, 8, new string('─', 68), Color.White, Color.Transparent);
            fakeVGA1.Write(108, 8, "┐", Color.White, Color.Transparent);
        }
    }
}
