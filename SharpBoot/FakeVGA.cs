using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SharpBoot
{
    public class FakeVGA : UserControl
    {
        public const long MEM_SIZE = 5120;

        private readonly byte[] m_ScreenMemory;

        public List<Tuple<Point, Color, Color, string>> Strings = new List<Tuple<Point, Color, Color, string>>();

        public FakeVGA()
        {
            BackColor = Color.Black;

            m_ScreenMemory = new byte[MEM_SIZE];

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
            for (var i = 0; i < MEM_SIZE; i += 2)
            {
                m_ScreenMemory[i] = 32;
                m_ScreenMemory[i + 1] = 7;
            }

            Refresh();
        }

        public void Poke(ushort Address, byte Value)
        {
            ushort MemLoc;

            try
            {
                MemLoc = (ushort) (Address - ScreenMemoryLocation);
            }
            catch (Exception)
            {
                return;
            }

            if (MemLoc < 0 || MemLoc > MEM_SIZE - 1)
                return;

            m_ScreenMemory[MemLoc] = Value;
            Refresh();
        }

        public byte Peek(ushort Address)
        {
            ushort MemLoc;

            try
            {
                MemLoc = (ushort) (Address - ScreenMemoryLocation);
            }
            catch (Exception)
            {
                return 0;
            }

            if (MemLoc < 0 || MemLoc > MEM_SIZE - 1)
                return 0;

            return m_ScreenMemory[MemLoc];
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var pf = new PrivateFontCollection();
            var fs = Assembly.GetExecutingAssembly().GetManifestResourceStream("SharpBoot.MorePerfectDOSVGA.ttf");
            var il = (int) fs.Length;
            var data = Marshal.AllocCoTaskMem(il);
            var fontdata = new byte[fs.Length];
            fs.Read(fontdata, 0, il);
            Marshal.Copy(fontdata, 0, data, il);
            pf.AddMemoryFont(data, il);
            fs.Close();
            Marshal.FreeCoTaskMem(data);
            var fnt = new Font(pf.Families[0], 6F);

            /*var cline = 0;
            foreach(var line in Text.Wrap(640))
            {
                TextRenderer.DrawText(e.Graphics, line, fnt, new Point(0, cline * 16), Color.Gray);
                cline++;
            }*/

            var bmp = new Bitmap(Width, Height);
            var bmpGraphics = Graphics.FromImage(bmp);

            foreach (var c in Strings)
            {
                bmpGraphics.FillRectangle(new SolidBrush(c.Item3),
                    new Rectangle(c.Item1, TextRenderer.MeasureText(c.Item4, fnt)));
                bmpGraphics.CompositingQuality = CompositingQuality.HighSpeed;
                bmpGraphics.TextRenderingHint = TextRenderingHint.SingleBitPerPixel;
                bmpGraphics.DrawString(c.Item4, fnt, new SolidBrush(c.Item2), c.Item1.X, c.Item1.Y);
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
            Strings.Add(new Tuple<Point, Color, Color, string>(new Point(x, y), f, b, s));
            Refresh();
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