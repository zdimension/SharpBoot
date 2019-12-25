using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using SharpBoot.Properties;

namespace SharpBoot.Controls
{
    public class FakeVGA : UserControl
    {
        public List<VGAString> Strings = new List<VGAString>();

        public FakeVGA()
        {
            BackColor = Color.Black;

            Reset();
        }

        public ushort ScreenMemoryLocation { get; set; }

        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            // EDIT: ADD AN EXTRA HEIGHT VALIDATION TO AVOID INITIALIZATION PROBLEMS
            // BITWISE 'AND' OPERATION: IF ZERO THEN HEIGHT IS NOT INVOLVED IN THIS OPERATION
            base.SetBoundsCore(x, y, 640, 480, specified);
        }

        public void Reset()
        {
            Strings.Clear();

            Refresh();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var pf = new PrivateFontCollection();
            var fontdata = Resources.unifont;
            var il = fontdata.Length;
            var data = Marshal.AllocCoTaskMem(il);
            Marshal.Copy(fontdata, 0, data, il);
            pf.AddMemoryFont(data, il);
            Marshal.FreeCoTaskMem(data);

            var fnt = new Font(pf.Families[0], 16F);

            var bmp = new Bitmap(Width, Height);
            var bmpGraphics = Graphics.FromImage(bmp);

            foreach (var c in Strings)
            {
                bmpGraphics.FillRectangle(new SolidBrush(c.Background),
                    new Rectangle(c.Position, new Size(8 * c.Value.Length, 16)));
                bmpGraphics.CompositingQuality = CompositingQuality.HighSpeed;
                bmpGraphics.TextRenderingHint = TextRenderingHint.SingleBitPerPixel;
                bmpGraphics.DrawString(c.Value, fnt, new SolidBrush(c.Foreground), c.Position.X, c.Position.Y);
            }

            e.Graphics.DrawImage(bmp, new Point(0, 0));
            bmpGraphics.Dispose();
            bmp.Dispose();
        }


        public void SetAfterPaintBackgroundHandler(Action<Graphics> f)
        {
            AfterPaintBackground = null;
            AfterPaintBackground += (sender, args) => f(args.Graphics);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
            AfterPaintBackground(this, e);
        }

        [Browsable(true)] public event PaintEventHandler AfterPaintBackground = delegate { };


        public void Write(int x, int y, string s, Color f, Color b)
        {
            Strings.Add(new VGAString(new Point(x, y), f, b, s));
            Refresh();
        }
    }

    public class VGAString
    {
        private readonly Color? _foreground = null;
        private readonly Color? _background = null;
        private readonly Func<Color> _fgCallback;
        private readonly Func<Color> _bgCallback;
        public Point Position { get; }

        public Color Foreground => _foreground ?? _fgCallback();

        public Color Background => _background ?? _bgCallback();

        public string Value { get; }

        public VGAString(Point p, Color fg, Color bg, string val)
        {
            Position = p;
            _foreground = fg;
            _background = bg;
            Value = val;
        }

        public VGAString(Point p, Func<Color> fg, Func<Color> bg, string val)
        {
            Position = p;
            _fgCallback = fg;
            _bgCallback = bg;
            Value = val;
        }
    }

    public enum VGAColor : byte
    {
        Black = 0x00,
        Blue = 0x01,
        Green = 0x02,
        Cyan = 0x03,
        Red = 0x04,
        Magenta = 0x05,
        Brown = 0x06,
        LightGray = 0x07,
        DarkGray = 0x08,
        LightBlue = 0x09,
        LightGreen = 0x0A,
        LightCyan = 0x0B,
        LightRed = 0x0C,
        LightMagenta = 0x0D,
        Yellow = 0x0E,
        White = 0x0F
    }
}