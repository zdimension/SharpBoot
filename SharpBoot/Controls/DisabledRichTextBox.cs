using System.Windows.Forms;
using SharpBoot.Utilities;

namespace SharpBoot.Controls
{
    // http://stackoverflow.com/a/27027922/2196124
    public class DisabledRichTextBox : RichTextBox
    {
        protected override void WndProc(ref Message m)
        {
            if (!(m.Msg == WindowMessage.WM_SETFOCUS || m.Msg == WindowMessage.WM_ENABLE || m.Msg == WindowMessage.WM_SETCURSOR))
                base.WndProc(ref m);
        }
    }
}