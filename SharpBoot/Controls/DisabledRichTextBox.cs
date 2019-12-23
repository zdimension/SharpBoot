using System.Windows.Forms;

namespace SharpBoot.Controls
{
    // http://stackoverflow.com/a/27027922/2196124
    public class DisabledRichTextBox : RichTextBox
    {
        // See: http://wiki.winehq.org/List_Of_Windows_Messages

        private const int WM_SETFOCUS = 0x07;
        private const int WM_ENABLE = 0x0A;
        private const int WM_SETCURSOR = 0x20;

        protected override void WndProc(ref Message m)
        {
            if (!(m.Msg == WM_SETFOCUS || m.Msg == WM_ENABLE || m.Msg == WM_SETCURSOR))
                base.WndProc(ref m);
        }
    }
}