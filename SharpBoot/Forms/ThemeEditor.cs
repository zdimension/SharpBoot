using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using SharpBoot.Controls;
using SharpBoot.Properties;

namespace SharpBoot.Forms
{
    public partial class ThemeEditor : Form
    {
        public ThemeEditor()
        {
            InitializeComponent();

            /*TheBootloader = new Syslinux();
            SetSyslinux();*/

            //fakeVGA1.BackgroundImage = Utils.DrawFilledRectangle(640, 480, Color.White);
            fakeVGA1.BackgroundImage = Image.FromStream(new MemoryStream(Resources.sharpboot));
        }

        private void AddString(int x, int y, string s, Color f, Color? b= null)
        {
            fakeVGA1.Write(10 + 8 * x, 10 + 16 * y, s, f, b ?? Color.Transparent);
        }

        private void InitGrub2()
        {
            var text = Color.FromArgb(0xa8, 0xa8, 0xa8);

            grub2Strings = new List<VGAString>
            {
                new VGAString(new Point(2, 3), Bind(cbxMenuFG), Bind(cbxMenuBG), "┌"),
                new VGAString(new Point(3, 3), Bind(cbxMenuFG), Bind(cbxMenuBG), new string('─', 68)),
                new VGAString(new Point(80, 3), Bind(cbxMenuFG), Bind(cbxMenuBG), "┐")
            };
        }

        private List<VGAString> grub2Strings = null;

        private static Func<Color> Bind(ColorComboBox cbx)
        {
            return () => cbx.SelectedColor;
        }

        private void ThemeEditor_Load(object sender, EventArgs e)
        {
            InitGrub2();
        }

        //public IBootloader TheBootloader { get; set; }

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