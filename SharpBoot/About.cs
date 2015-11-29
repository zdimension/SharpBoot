using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SharpBoot
{
    public partial class About : Form
    {
        [DllImport("user32.dll")]
        static extern bool HideCaret(IntPtr hWnd);

        public About()
        {
            InitializeComponent();
            richTextBox1.Text = richTextBox1.Text.Insert(9, " " + Program.GetVersion()).Replace("{0}", Strings.SharpBootUsesSoft);
            if(Program.IsWin) Utils.SetWindowTheme(lvTranslators.Handle, "explorer", null);
            rtbMyWebsite.SelectAll();
            rtbMyWebsite.SelectionAlignment = HorizontalAlignment.Right;
            rtbMyWebsite.DeselectAll();
            HideCaret(richTextBox1.Handle);
            HideCaret(rbnHelpTranslate.Handle);
            HideCaret(rtbMyWebsite.Handle);
        }

        private void RichTextBox1OnGotFocus(object sender, EventArgs eventArgs)
        {
            btnOK.Focus();
            HideCaret(richTextBox1.Handle);
            HideCaret(rbnHelpTranslate.Handle);
            HideCaret(rtbMyWebsite.Handle);
        }

        private void lvTranslators_DoubleClick(object sender, EventArgs e)
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

        private void richTextBox1_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            if(e.LinkText != "")
                Process.Start(e.LinkText);
        }
    }
}
