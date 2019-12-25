using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SharpBoot.Controls
{
    public class ColorComboBox : ComboBox
    {
        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            Graphics g = e.Graphics;
            Rectangle rect = e.Bounds;

            if (e.Index >= 0)
            {
                var n = (ColorItem)Items[e.Index];
                Brush b = new SolidBrush(n.Color);
                g.DrawString(n.Name, this.Font, Brushes.Black, rect.X, rect.Top);
                g.FillRectangle(b, rect.X + 110, rect.Y + 5,
                    rect.Width - 10, rect.Height - 10);
            }
        }

        public Color SelectedColor => ((ColorItem) SelectedItem).Color;

        public class ColorItem
        {
            public string Name { get; set; }
            public Color Color { get; set; }
        }
    }
}
