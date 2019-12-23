using System.Windows.Forms;

namespace SharpBoot.Controls
{
    public class CustomListView : ListView
    {
        public CustomListView()
        {
            SetStyle(
                ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint,
                true);
            DoubleBuffered = true;
        }
    }
}