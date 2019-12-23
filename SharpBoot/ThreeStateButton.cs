using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SharpBoot
{
    public class ThreeStateButton : Button
    {
        private Image _normalImage;
        private Image _hoverImage;
        private Image _downImage;

        public ThreeStateButton()
        {
            DoubleBuffered = true;
            SetStyle(ControlStyles.AllPaintingInWmPaint
                     | ControlStyles.DoubleBuffer
                     | ControlStyles.OptimizedDoubleBuffer
                     | ControlStyles.ResizeRedraw, true);
            SetBackImage();
        }

        public Image NormalImage
        {
            get { return _normalImage; }
            set
            {
                _normalImage = value;
                SetBackImage();
            }
        }

        public Image HoverImage
        {
            get { return _hoverImage; }
            set
            {
                _hoverImage = value;
                SetBackImage();
            }
        }

        public Image DownImage
        {
            get { return _downImage; }
            set
            {
                _downImage = value;
                SetBackImage();
            }
        }

        private bool isHover = false;
        private bool isDown = false;

        private void SetBackImage()
        {
            if (isDown)
            {
                BackgroundImage = DownImage;
            }
            else
            {
                if (isHover)
                {
                    BackgroundImage = HoverImage;
                }
                else
                {
                    BackgroundImage = NormalImage;
                }
            }

            Invalidate();
        }


        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);

            isHover = true;

            SetBackImage();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            isHover = false;

            SetBackImage();
        }

        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            base.OnMouseDown(mevent);

            isDown = true;

            SetBackImage();
        }

        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            base.OnMouseUp(mevent);

            isDown = false;

            SetBackImage();
        }
    }
}