using System.Windows.Forms;

namespace SharpBoot
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