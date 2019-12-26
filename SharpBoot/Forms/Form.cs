using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SharpBoot.Forms
{
    public class Form : System.Windows.Forms.Form
    {
        public Form()
        {
            DoubleBuffered = true;
            Font = SystemFonts.MessageBoxFont;
            AutoScaleMode = AutoScaleMode.Dpi;
            //AutoScaleDimensions = new System.Drawing.SizeF(96, 96);
        }
    }
}
