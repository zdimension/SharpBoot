using System.Windows.Forms;

namespace SharpBoot
{
    public class CustomTextBox : TextBox
    {
        public CustomTextBox()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer,
                true);
            DoubleBuffered = true;
        }
    }
}