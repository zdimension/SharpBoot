using System.Windows.Forms;
using SharpBoot.Utilities;

namespace SharpBoot.Controls
{
    // http://stackoverflow.com/a/27027922/2196124
    public class DisabledRichTextBox : RichTextBox
    {
        protected override void WndProc(ref Message m)
        {
            if (!(m.Msg == WinApiConstants.WM_SETFOCUS || m.Msg == WinApiConstants.WM_ENABLE || m.Msg == WinApiConstants.WM_SETCURSOR))
                base.WndProc(ref m);
        }
    }
}