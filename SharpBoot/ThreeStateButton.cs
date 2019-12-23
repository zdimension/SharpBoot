using System;
using System.Drawing;
using System.Windows.Forms;

namespace SharpBoot
{
    public class ThreeStateButton : Button
    {
        private Image _downImage;
        private Image _hoverImage;
        private Image _normalImage;
        private bool isDown;

        private bool isHover;

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
            get => _normalImage;
            set
            {
                _normalImage = value;
                SetBackImage();
            }
        }

        public Image HoverImage
        {
            get => _hoverImage;
            set
            {
                _hoverImage = value;
                SetBackImage();
            }
        }

        public Image DownImage
        {
            get => _downImage;
            set
            {
                _downImage = value;
                SetBackImage();
            }
        }

        private void SetBackImage()
        {
            if (isDown)
            {
                BackgroundImage = DownImage;
            }
            else
            {
                BackgroundImage = isHover ? HoverImage : NormalImage;
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