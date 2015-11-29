// Windows 7 ToolStrip Renderer
//
// Andrea Martinelli
// http://at-my-window.blogspot.com/?page=windows7renderer
//
// Based on Office 2007 Renderer by Phil Wright
// http://www.componentfactory.com
//
//
// https://windows7renderer.codeplex.com/


using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace W7R
{
    internal class Windows7ColorTable : ProfessionalColorTable
    {
        #region Static Fixed Colors - Blue Color Scheme

        private static Color _contextMenuBack = Color.FromArgb(250, 250, 250);
        private static Color _buttonPressedBegin = Color.FromArgb(248, 181, 106);
        private static Color _buttonPressedEnd = Color.FromArgb(255, 208, 134);
        private static Color _buttonPressedMiddle = Color.FromArgb(251, 140, 60);
        private static Color _buttonSelectedBegin = Color.FromArgb(255, 255, 222);
        private static Color _buttonSelectedEnd = Color.FromArgb(255, 203, 136);
        private static Color _buttonSelectedMiddle = Color.FromArgb(255, 225, 172);
        private static Color _menuItemSelectedBegin = Color.FromArgb(255, 213, 103);
        private static Color _menuItemSelectedEnd = Color.FromArgb(255, 228, 145);
        private static Color _checkBack = Color.FromArgb(234, 242, 250);
        private static Color _gripDark = Color.FromArgb(111, 157, 217);
        private static Color _gripLight = Color.FromArgb(255, 255, 255);
        private static Color _imageMargin = Color.FromArgb(241, 241, 241);
        private static Color _menuBorder = Color.FromArgb(134, 134, 134);
        private static Color _overflowBegin = Color.FromArgb(167, 204, 251);
        private static Color _overflowEnd = Color.FromArgb(101, 147, 207);
        private static Color _overflowMiddle = Color.FromArgb(167, 204, 251);
        private static Color _menuToolBack = Color.FromArgb(191, 219, 255);
        private static Color _separatorDark = Color.FromArgb(154, 198, 255);
        private static Color _separatorLight = Color.FromArgb(255, 255, 255);
        private static Color _statusStripLight = Color.FromArgb(215, 229, 247);
        private static Color _statusStripDark = Color.FromArgb(172, 201, 238);
        private static Color _toolStripBorder = Color.FromArgb(111, 157, 217);
        private static Color _toolStripContentEnd = Color.FromArgb(164, 195, 235);
        private static Color _toolStripBegin = Color.FromArgb(227, 239, 255);
        private static Color _toolStripEnd = Color.FromArgb(152, 186, 230);
        private static Color _toolStripMiddle = Color.FromArgb(222, 236, 255);
        private static Color _buttonBorder = Color.FromArgb(121, 153, 194);

        #endregion

        #region Identity

        #endregion

        #region ButtonPressed

        /// <summary>
        ///     Gets the starting color of the gradient used when the button is pressed down.
        /// </summary>
        public override Color ButtonPressedGradientBegin => _buttonPressedBegin;

        /// <summary>
        ///     Gets the end color of the gradient used when the button is pressed down.
        /// </summary>
        public override Color ButtonPressedGradientEnd => _buttonPressedEnd;

        /// <summary>
        ///     Gets the middle color of the gradient used when the button is pressed down.
        /// </summary>
        public override Color ButtonPressedGradientMiddle => _buttonPressedMiddle;

        #endregion

        #region ButtonSelected

        /// <summary>
        ///     Gets the starting color of the gradient used when the button is selected.
        /// </summary>
        public override Color ButtonSelectedGradientBegin => _buttonSelectedBegin;

        /// <summary>
        ///     Gets the end color of the gradient used when the button is selected.
        /// </summary>
        public override Color ButtonSelectedGradientEnd => _buttonSelectedEnd;

        /// <summary>
        ///     Gets the middle color of the gradient used when the button is selected.
        /// </summary>
        public override Color ButtonSelectedGradientMiddle => _buttonSelectedMiddle;

        /// <summary>
        ///     Gets the border color to use with ButtonSelectedHighlight.
        /// </summary>
        public override Color ButtonSelectedHighlightBorder => _buttonBorder;

        #endregion

        #region Check

        /// <summary>
        ///     Gets the solid color to use when the check box is selected and gradients are being used.
        /// </summary>
        public override Color CheckBackground => _checkBack;

        #endregion

        #region Grip

        /// <summary>
        ///     Gets the color to use for shadow effects on the grip or move handle.
        /// </summary>
        public override Color GripDark => _gripDark;

        /// <summary>
        ///     Gets the color to use for highlight effects on the grip or move handle.
        /// </summary>
        public override Color GripLight => _gripLight;

        #endregion

        #region ImageMargin

        /// <summary>
        ///     Gets the starting color of the gradient used in the image margin of a ToolStripDropDownMenu.
        /// </summary>
        public override Color ImageMarginGradientBegin => _imageMargin;

        #endregion

        #region MenuBorder

        /// <summary>
        ///     Gets the border color or a MenuStrip.
        /// </summary>
        public override Color MenuBorder => _menuBorder;

        #endregion

        #region MenuItem

        /// <summary>
        ///     Gets the starting color of the gradient used when a top-level ToolStripMenuItem is pressed down.
        /// </summary>
        public override Color MenuItemPressedGradientBegin => _toolStripBegin;

        /// <summary>
        ///     Gets the end color of the gradient used when a top-level ToolStripMenuItem is pressed down.
        /// </summary>
        public override Color MenuItemPressedGradientEnd => _toolStripEnd;

        /// <summary>
        ///     Gets the middle color of the gradient used when a top-level ToolStripMenuItem is pressed down.
        /// </summary>
        public override Color MenuItemPressedGradientMiddle => _toolStripMiddle;

        /// <summary>
        ///     Gets the starting color of the gradient used when the ToolStripMenuItem is selected.
        /// </summary>
        public override Color MenuItemSelectedGradientBegin => _menuItemSelectedBegin;

        /// <summary>
        ///     Gets the end color of the gradient used when the ToolStripMenuItem is selected.
        /// </summary>
        public override Color MenuItemSelectedGradientEnd => _menuItemSelectedEnd;

        #endregion

        #region MenuStrip

        /// <summary>
        ///     Gets the starting color of the gradient used in the MenuStrip.
        /// </summary>
        public override Color MenuStripGradientBegin => _menuToolBack;

        /// <summary>
        ///     Gets the end color of the gradient used in the MenuStrip.
        /// </summary>
        public override Color MenuStripGradientEnd => _menuToolBack;

        #endregion

        #region OverflowButton

        /// <summary>
        ///     Gets the starting color of the gradient used in the ToolStripOverflowButton.
        /// </summary>
        public override Color OverflowButtonGradientBegin => _overflowBegin;

        /// <summary>
        ///     Gets the end color of the gradient used in the ToolStripOverflowButton.
        /// </summary>
        public override Color OverflowButtonGradientEnd => _overflowEnd;

        /// <summary>
        ///     Gets the middle color of the gradient used in the ToolStripOverflowButton.
        /// </summary>
        public override Color OverflowButtonGradientMiddle => _overflowMiddle;

        #endregion

        #region RaftingContainer

        /// <summary>
        ///     Gets the starting color of the gradient used in the ToolStripContainer.
        /// </summary>
        public override Color RaftingContainerGradientBegin => _menuToolBack;

        /// <summary>
        ///     Gets the end color of the gradient used in the ToolStripContainer.
        /// </summary>
        public override Color RaftingContainerGradientEnd => _menuToolBack;

        #endregion

        #region Separator

        /// <summary>
        ///     Gets the color to use to for shadow effects on the ToolStripSeparator.
        /// </summary>
        public override Color SeparatorDark => _separatorDark;

        /// <summary>
        ///     Gets the color to use to for highlight effects on the ToolStripSeparator.
        /// </summary>
        public override Color SeparatorLight => _separatorLight;

        #endregion

        #region StatusStrip

        /// <summary>
        ///     Gets the starting color of the gradient used on the StatusStrip.
        /// </summary>
        public override Color StatusStripGradientBegin => _statusStripLight;

        /// <summary>
        ///     Gets the end color of the gradient used on the StatusStrip.
        /// </summary>
        public override Color StatusStripGradientEnd => _statusStripDark;

        #endregion

        #region ToolStrip

        /// <summary>
        ///     Gets the border color to use on the bottom edge of the ToolStrip.
        /// </summary>
        public override Color ToolStripBorder => _toolStripBorder;

        /// <summary>
        ///     Gets the starting color of the gradient used in the ToolStripContentPanel.
        /// </summary>
        public override Color ToolStripContentPanelGradientBegin => _toolStripContentEnd;

        /// <summary>
        ///     Gets the end color of the gradient used in the ToolStripContentPanel.
        /// </summary>
        public override Color ToolStripContentPanelGradientEnd => _menuToolBack;

        /// <summary>
        ///     Gets the solid background color of the ToolStripDropDown.
        /// </summary>
        public override Color ToolStripDropDownBackground => _contextMenuBack;

        /// <summary>
        ///     Gets the starting color of the gradient used in the ToolStrip background.
        /// </summary>
        public override Color ToolStripGradientBegin => _toolStripBegin;

        /// <summary>
        ///     Gets the end color of the gradient used in the ToolStrip background.
        /// </summary>
        public override Color ToolStripGradientEnd => _toolStripEnd;

        /// <summary>
        ///     Gets the middle color of the gradient used in the ToolStrip background.
        /// </summary>
        public override Color ToolStripGradientMiddle => _toolStripMiddle;

        /// <summary>
        ///     Gets the starting color of the gradient used in the ToolStripPanel.
        /// </summary>
        public override Color ToolStripPanelGradientBegin => _menuToolBack;

        /// <summary>
        ///     Gets the end color of the gradient used in the ToolStripPanel.
        /// </summary>
        public override Color ToolStripPanelGradientEnd => _menuToolBack;

        #endregion
    }

    internal sealed class Utils
    {
        private static bool _installed;

        public static void Install()
        {
            if (_installed) return;
            _installed = true;
            ToolStripManager.Renderer = new Windows7Renderer();
        }
    }

    /// <summary>
    ///     Set the SmoothingMode=AntiAlias until instance disposed.
    /// </summary>
    internal class UseAntiAlias : IDisposable
    {
        #region Instance Fields

        private Graphics _g;
        private SmoothingMode _old;

        #endregion

        #region Identity

        /// <summary>
        ///     Initialize a new instance of the UseAntiAlias class.
        /// </summary>
        /// <param name="g">Graphics instance.</param>
        public UseAntiAlias(Graphics g)
        {
            _g = g;
            _old = _g.SmoothingMode;
            _g.SmoothingMode = SmoothingMode.AntiAlias;
        }

        /// <summary>
        ///     Revert the SmoothingMode back to original setting.
        /// </summary>
        public void Dispose()
        {
            _g.SmoothingMode = _old;
        }

        #endregion
    }

    /// <summary>
    ///     Set the TextRenderingHint.ClearTypeGridFit until instance disposed.
    /// </summary>
    internal class UseClearTypeGridFit : IDisposable
    {
        #region Instance Fields

        private Graphics _g;
        private TextRenderingHint _old;

        #endregion

        #region Identity

        /// <summary>
        ///     Initialize a new instance of the UseClearTypeGridFit class.
        /// </summary>
        /// <param name="g">Graphics instance.</param>
        public UseClearTypeGridFit(Graphics g)
        {
            _g = g;
            _old = _g.TextRenderingHint;
            _g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
        }

        /// <summary>
        ///     Revert the TextRenderingHint back to original setting.
        /// </summary>
        public void Dispose()
        {
            _g.TextRenderingHint = _old;
        }

        #endregion
    }

    /// <summary>
    ///     Set the clipping region until instance disposed.
    /// </summary>
    internal class UseClipping : IDisposable
    {
        #region Instance Fields

        private Graphics _g;
        private Region _old;

        #endregion

        #region Identity

        /// <summary>
        ///     Initialize a new instance of the UseClipping class.
        /// </summary>
        /// <param name="g">Graphics instance.</param>
        /// <param name="path">Clipping path.</param>
        public UseClipping(Graphics g, GraphicsPath path)
        {
            _g = g;
            _old = g.Clip;
            Region clip = _old.Clone();
            clip.Intersect(path);
            _g.Clip = clip;
        }

        /// <summary>
        ///     Initialize a new instance of the UseClipping class.
        /// </summary>
        /// <param name="g">Graphics instance.</param>
        /// <param name="region">Clipping region.</param>
        public UseClipping(Graphics g, Region region)
        {
            _g = g;
            _old = g.Clip;
            Region clip = _old.Clone();
            clip.Intersect(region);
            _g.Clip = clip;
        }

        /// <summary>
        ///     Revert clipping back to origina setting.
        /// </summary>
        public void Dispose()
        {
            _g.Clip = _old;
        }

        #endregion
    }

    [SuppressMessage("ReSharper", "MemberCanBeMadeStatic.Local")]
    [SuppressMessage("ReSharper", "SuggestBaseTypeForParameter")]
    public class Windows7Renderer : ToolStripProfessionalRenderer
    {
        private static Windows7Renderer _instance;

        public static Windows7Renderer Instance => _instance ?? (_instance = new Windows7Renderer());

        #region GradientItemColors

        private class GradientItemColors
        {
            #region Public Fields

            public Color InsideTop1;
            public Color InsideTop2;
            public Color InsideBottom1;
            public Color InsideBottom2;
            public Color FillTop1;
            public Color FillTop2;
            public Color FillBottom1;
            public Color FillBottom2;
            public Color Border1;
            public Color Border2;

            #endregion

            #region Identity

            public GradientItemColors(Color insideTop1, Color insideTop2,
                Color insideBottom1, Color insideBottom2,
                Color fillTop1, Color fillTop2,
                Color fillBottom1, Color fillBottom2,
                Color border1, Color border2)
            {
                InsideTop1 = insideTop1;
                InsideTop2 = insideTop2;
                InsideBottom1 = insideBottom1;
                InsideBottom2 = insideBottom2;
                FillTop1 = fillTop1;
                FillTop2 = fillTop2;
                FillBottom1 = fillBottom1;
                FillBottom2 = fillBottom2;
                Border1 = border1;
                Border2 = border2;
            }

            #endregion
        }

        #endregion

        #region Static Metrics

        private const int _gripOffset = 1;
        private const int _gripSquare = 2;
        private const int _gripSize = 3;
        private const int _gripMove = 4;
        private const int _gripLines = 3;
        private const int _checkInset = 0;
        private const int _marginInset = 2;
        private const int _separatorInset = 25;
        private const float _cutToolItemMenu = 1.0f;
        private const float _cutContextMenu = 0f;
        private const float _cutMenuItemBack = 1.2f;
        private const float _contextCheckTickThickness = 1.6f;
        private static Blend _statusStripBlend;

        #endregion

        #region Static Colors

        private static Color _c1 = Color.FromArgb(167, 167, 167);
        private static Color _c2 = Color.FromArgb(21, 66, 139);
        private static Color _c3 = Color.FromArgb(76, 83, 92);
        private static Color _c4 = Color.FromArgb(250, 250, 250);
        private static Color _c5 = Color.FromArgb(248, 248, 248);
        private static Color _c6 = Color.FromArgb(243, 243, 243);
        private static Color _r1 = Color.FromArgb(255, 255, 251);
        private static Color _r2 = Color.FromArgb(255, 249, 227);
        private static Color _r3 = Color.FromArgb(255, 242, 201);
        private static Color _r4 = Color.FromArgb(248, 251, 254);

        private static Color _r5 = Color.FromArgb(248, 251, 254);
        private static Color _r6 = Color.FromArgb(237, 242, 250);

        private static Color _r7 = Color.FromArgb(215, 228, 244);
        private static Color _r8 = Color.FromArgb(193, 210, 232);


        private static Color _r9 = Color.FromArgb(187, 202, 219);
        private static Color _rA = Color.FromArgb(177, 195, 216);

        private static Color _rB = Color.FromArgb(182, 190, 192);
        private static Color _rC = Color.FromArgb(155, 163, 167);

        private static Color _rD = Color.FromArgb(201, 212, 228);
        private static Color _rE = Color.FromArgb(177, 195, 216);
        private static Color _rF = Color.FromArgb(170, 188, 211);
        private static Color _rG = Color.FromArgb(207, 220, 237);

        private static Color _rH = Color.FromArgb(221, 232, 241);
        private static Color _rI = Color.FromArgb(216, 228, 241);
        private static Color _rJ = Color.FromArgb(207, 220, 237);
        private static Color _rK = Color.FromArgb(207, 219, 236);

        private static Color _rL = Color.FromArgb(249, 192, 103);
        private static Color _rM = Color.FromArgb(250, 195, 93);
        private static Color _rN = Color.FromArgb(248, 190, 81);
        private static Color _rO = Color.FromArgb(255, 208, 49);
        private static Color _rP = Color.FromArgb(254, 214, 168);
        private static Color _rQ = Color.FromArgb(252, 180, 100);
        private static Color _rR = Color.FromArgb(252, 161, 54);
        private static Color _rS = Color.FromArgb(254, 238, 170);
        private static Color _rT = Color.FromArgb(249, 202, 113);
        private static Color _rU = Color.FromArgb(250, 205, 103);
        private static Color _rV = Color.FromArgb(248, 200, 91);
        private static Color _rW = Color.FromArgb(255, 218, 59);
        private static Color _rX = Color.FromArgb(254, 185, 108);
        private static Color _rY = Color.FromArgb(252, 161, 54);
        private static Color _rZ = Color.FromArgb(254, 238, 170);


        //private static Color clrBGTop1 = Color.FromArgb(255, 253, 255, 254);
        //private static Color clrBGTop2 = Color.FromArgb(255, 222, 231, 240);
        //private static Color clrBGBottom1 = Color.FromArgb(255, 231, 240, 249);
        //private static Color clrBGBottom2 = Color.FromArgb(255, 224, 232, 243);
        //private static Color clrBGBorder = Color.FromArgb(255, 235, 243, 254);
        //private static Color clrBGGreen = Color.FromArgb(0, 255, 255, 255);
        //private static Color clrBtnDarkBorder = Color.FromArgb(200, 3, 50, 81);
        //private static Color clrBtnLightBorder = Color.FromArgb(200, 216, 228, 236);


        private static Color clrWindows7text = Color.FromArgb(30, 57, 91);
        private static Color clrWindows7topBegin = Color.FromArgb(250, 252, 253);
        private static Color clrWindows7topEnd = Color.FromArgb(230, 240, 250);
        private static Color clrWindows7bottomBegin = Color.FromArgb(220, 230, 244);
        private static Color clrWindows7bottomEnd = Color.FromArgb(221, 233, 247);
        private static Color clrWindows7bottomBorder3 = Color.FromArgb(228, 239, 251);
        private static Color clrWindows7bottomBorder2 = Color.FromArgb(205, 218, 234);
        private static Color clrWindows7bottomBorder1 = Color.FromArgb(160, 175, 195);


        // Color scheme values
        private static Color _textDisabled = _c1;

        private static Color _textMenuStripItem = Color.Black;
        private static Color _textStatusStripItem = clrWindows7text;
        private static Color _textContextMenuItem = clrWindows7text;

        private static Color _arrowDisabled = _c1;
        private static Color _arrowLight = Color.FromArgb(106, 126, 197);
        private static Color _arrowDark = clrWindows7text;
        private static Color _separatorMenuDark = Color.FromArgb(226, 227, 227);
        private static Color _separatorMenuLight = Color.FromArgb(255, 255, 255);
        private static Color _contextMenuBack = Color.FromArgb(240, 240, 240);
        private static Color _contextCheckBorder = Color.FromArgb(175, 208, 247);
        private static Color _contextCheckTick = Color.FromArgb(12, 18, 161);
        private static Color _statusStripBorderDark = Color.FromArgb(86, 125, 176);
        private static Color _statusStripBorderLight = Color.White;
        private static Color _gripDark = Color.FromArgb(114, 152, 204);
        private static Color _gripLight = _c5;


        private static Color _statusBarDark = Color.FromArgb(204, 217, 234);
        private static Color _statusBarLight = Color.FromArgb(241, 245, 251);

        private static GradientItemColors _itemContextItemEnabledColors = new GradientItemColors(
            Color.FromArgb(127, 242, 244, 246),
            Color.FromArgb(127, 236, 241, 247),
            Color.FromArgb(127, 231, 238, 247),
            Color.FromArgb(127, 236, 241, 247),
            Color.Transparent,
            Color.Transparent,
            Color.Transparent,
            Color.Transparent,
            Color.FromArgb(174, 207, 247), Color.FromArgb(174, 207, 247));

        private static GradientItemColors _itemDisabledColors = new GradientItemColors(
            Color.FromArgb(127, 242, 244, 246),
            Color.FromArgb(127, 236, 241, 247),
            Color.FromArgb(127, 231, 238, 247),
            Color.FromArgb(127, 236, 241, 247),
            Color.Transparent,
            Color.Transparent,
            Color.Transparent,
            Color.Transparent,
            Color.FromArgb(212, 212, 212), Color.FromArgb(195, 195, 195)
            );

        private static GradientItemColors _itemToolItemSelectedColors = new GradientItemColors( /****/
            _r1, _r2, _r3, _r4, _r5, _r6, _r7, _r8, _r9, _rA);

        private static GradientItemColors _itemToolItemPressedColors = new GradientItemColors( /*****/
            _rD, _rE, _rF, _rG, _rH, _rI, _rJ, _rK, _r9, _rA);

        private static GradientItemColors _itemToolItemCheckedColors = new GradientItemColors( /*****/
            _rD, _rE, _rF, _rG, _rH, _rI, _rJ, _rK, _r9, _rA);

        //new GradientItemColors(/*****/ _rL, _rM, _rN, _rO, _rP, _rQ, _rR, _rS, _r9, _rA);
        private static GradientItemColors _itemToolItemCheckPressColors = new GradientItemColors( /*****/
            _rD, _rE, _rF, _rG, _rH, _rI, _rJ, _rK, _r9, _rA);

        //new GradientItemColors(/**/ _rT, _rU, _rV, _rW, _rX, _rI, _rY, _rZ, _r9, _rA);

        #endregion

        #region Identity

        static Windows7Renderer()
        {
            // One time creation of the blend for the status strip gradient brush
            _statusStripBlend = new Blend
            {
                Positions = new[] {0.0f, 0.25f, 0.25f, 0.57f, 0.86f, 1.0f},
                Factors = new[] {0.1f, 0.6f, 1.0f, 0.4f, 0.0f, 0.95f}
            };
        }


        protected override void InitializeItem(ToolStripItem item)
        {
            base.InitializeItem(item);

            if (item.DisplayStyle == ToolStripItemDisplayStyle.Image)
            {
                var m = item.Margin;
                m.Left += 4;
                m.Right += 4;
                item.Margin = m;
            }

            {
                var a = item as ToolStripSplitButton;
                if (a != null)
                {
                    a.DropDownButtonWidth = 17;

                    switch (a.DisplayStyle)
                    {
                        case ToolStripItemDisplayStyle.Image:
                            a.Padding = new Padding(3, 0, 3, 0);
                            break;
                        case ToolStripItemDisplayStyle.Text:
                            a.Padding = new Padding(14, 3, 15, 3);
                            break;
                        default:
                            a.Padding = new Padding(25, 3, 0, 3);
                            a.TextAlign = ContentAlignment.MiddleRight;
                            break;
                    }
                }
            }


            {
                var a = item as ToolStripDropDownButton;
                if (a != null)
                {
                    switch (a.DisplayStyle)
                    {
                        case ToolStripItemDisplayStyle.Image:
                            a.Padding = new Padding(7, 0, 7, 0);
                            break;
                        // a.Margin = new Padding(4, 0, 4, 0);
                        case ToolStripItemDisplayStyle.Text:
                            a.Padding = new Padding(14, 3, 15, 3);
                            break;
                        default:
                            a.Padding = new Padding(25, 3, 0, 3);
                            a.TextAlign = ContentAlignment.MiddleRight;
                            break;
                    }
                }
            }


            {
                var a = item as ToolStripButton;
                if (a != null)
                {
                    switch (a.DisplayStyle)
                    {
                        case ToolStripItemDisplayStyle.Image:
                            a.Padding = new Padding(4, 0, 4, 0);
                            break;
                        // a.Margin = new Padding(4, 0, 4, 0);
                        case ToolStripItemDisplayStyle.Text:
                            a.Padding = new Padding(15, 3, 15, 3);
                            break;
                        default:
                            a.Padding = new Padding(25, 3, 0, 3);
                            a.TextAlign = ContentAlignment.MiddleRight;
                            break;
                    }
                }
            }

            if (item is ToolStripSeparator)
            {
                item.Height++;
            }

            if (item is ToolStripOverflowButton)
            {
                item.Width += 25;
            }
        }


        public Windows7Renderer()
            : base(new Windows7ColorTable())
        {
            RoundedEdges = false;
        }

        #endregion

        #region OnRenderArrow

        /// <summary>
        ///     Raises the RenderArrow event.
        /// </summary>
        /// <param name="e">An ToolStripArrowRenderEventArgs containing the event data.</param>
        protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e)
        {
            // Cannot paint a zero sized area
            if ((e.ArrowRectangle.Width > 0) &&
                (e.ArrowRectangle.Height > 0))
            {
                // Create a path that is used to fill the arrow
                using (GraphicsPath arrowPath = CreateArrowPath(e.Item,
                    e.ArrowRectangle,
                    e.Direction))
                {
                    // Get the rectangle that encloses the arrow and expand slightly
                    // so that the gradient is always within the expanding bounds
                    //RectangleF boundsF = arrowPath.GetBounds();
                    //boundsF.Inflate(5f, 5f);

                    //  Color color1 = (e.Item.Enabled ? _arrowLight : _arrowDisabled);

                    Color color2;
                    if (!e.Item.Enabled)
                    {
                        color2 = _arrowDisabled;
                    }
                    else
                    {
                        var parent = e.Item.GetCurrentParent();
                        if (parent is ToolStripDropDown || parent is MenuStrip || parent is ContextMenuStrip)
                        {
                            color2 = Color.Black;
                        }
                        else
                        {
                            color2 = _arrowDark;
                        }
                    }


                    /*
                    float angle = 0;

                    // Use gradient angle to match the arrow direction
                    switch (e.Direction)
                    {
                        case ArrowDirection.Right:
                            angle = 0;
                            break;
                        case ArrowDirection.Left:
                            angle = 180f;
                            break;
                        case ArrowDirection.Down:
                            angle = 90f;
                            break;
                        case ArrowDirection.Up:
                            angle = 270f;
                            break;
                    }*/


                    // Draw the actual arrow using a gradient
                    //  using (LinearGradientBrush arrowBrush = new LinearGradientBrush(boundsF, color1, color2, angle))
                    //  e.Graphics.FillPath(arrowBrush, arrowPath);
                    //     e.Graphics.CompositingQuality=SmoothingMode.None;
                    using (var arrowBrush = new SolidBrush(color2))
                        e.Graphics.FillPath(arrowBrush, arrowPath);
                }
            }
        }

        #endregion

        protected override void Initialize(ToolStrip toolStrip)
        {
            base.Initialize(toolStrip);

            if (toolStrip is MenuStrip)
            {
                toolStrip.CanOverflow = false;
            }
            else if (toolStrip is ContextMenuStrip)
            {
                // No nothing
            }
            else
            {
                toolStrip.AutoSize = false;
                toolStrip.Height = 32;

                toolStrip.Padding = new Padding(5, 2, 5, 2);
                toolStrip.CanOverflow = false;
                toolStrip.GripStyle = ToolStripGripStyle.Hidden;
            }

            toolStrip.Font = new Font("Segoe UI", 9);
        }

        #region OnRenderButtonBackground

        /// <summary>
        ///     Raises the RenderButtonBackground event.
        /// </summary>
        /// <param name="e">An ToolStripItemRenderEventArgs containing the event data.</param>
        protected override void OnRenderButtonBackground(ToolStripItemRenderEventArgs e)
        {
            // Cast to correct type
            ToolStripButton button = (ToolStripButton) e.Item;
            if (button.Selected || button.Pressed || button.Checked)
                RenderToolButtonBackground(e.Graphics, button, e.ToolStrip);
        }

        #endregion

        #region OnRenderDropDownButtonBackground

        /// <summary>
        ///     Raises the RenderDropDownButtonBackground event.
        /// </summary>
        /// <param name="e">An ToolStripItemRenderEventArgs containing the event data.</param>
        protected override void OnRenderDropDownButtonBackground(ToolStripItemRenderEventArgs e)
        {
            if (e.Item.Selected || e.Item.Pressed)
                RenderToolDropButtonBackground(e.Graphics, e.Item, e.ToolStrip);
        }

        #endregion

        #region OnRenderItemCheck

        /// <summary>
        ///     Raises the RenderItemCheck event.
        /// </summary>
        /// <param name="e">An ToolStripItemImageRenderEventArgs containing the event data.</param>
        protected override void OnRenderItemCheck(ToolStripItemImageRenderEventArgs e)
        {
            // Staring size of the checkbox is the image rectangle
            Rectangle checkBox = e.ImageRectangle;

            // Make the border of the check box 1 pixel bigger on all sides, as a minimum
            checkBox.Inflate(2, 2);

            // Can we extend upwards?
            if (checkBox.Top > _checkInset)
            {
                int diff = checkBox.Top - _checkInset;
                checkBox.Y -= diff;
                checkBox.Height += diff;
            }

            // Can we extend downwards?
            if (checkBox.Height <= (e.Item.Bounds.Height - (_checkInset * 2)))
            {
                int diff = e.Item.Bounds.Height - (_checkInset * 2) - checkBox.Height;
                checkBox.Height += diff;
            }

            checkBox.Width = checkBox.Height;


            // Drawing with anti aliasing to create smoother appearance
            using (UseAntiAlias uaa = new UseAntiAlias(e.Graphics))
            {
                // Create border path for the check box
                using (GraphicsPath borderPath = CreateBorderPath(checkBox, _cutMenuItemBack))
                {
                    // Fill the background in a solid color
                    using (SolidBrush fillBrush = new SolidBrush(ColorTable.CheckBackground))
                        e.Graphics.FillPath(fillBrush, borderPath);

                    // Draw the border around the check box
                    using (Pen borderPen = new Pen(_contextCheckBorder))
                        e.Graphics.DrawPath(borderPen, borderPath);

                    // If there is not an image, then we can draw the tick, square etc...
                    if (e.Image != null)
                    {
                        CheckState checkState = CheckState.Unchecked;

                        // Extract the check state from the item
                        if (e.Item is ToolStripMenuItem)
                        {
                            ToolStripMenuItem item = (ToolStripMenuItem) e.Item;
                            checkState = item.CheckState;
                        }

                        // Decide what graphic to draw
                        switch (checkState)
                        {
                            case CheckState.Checked:
                                // Create a path for the tick
                                using (GraphicsPath tickPath = CreateTickPath(checkBox))
                                {
                                    // Draw the tick with a thickish brush
                                    using (Pen tickPen = new Pen(_contextCheckTick, _contextCheckTickThickness))
                                        e.Graphics.DrawPath(tickPen, tickPath);
                                }
                                break;
                            case CheckState.Indeterminate:
                                // Create a path for the indeterminate diamond
                                using (GraphicsPath tickPath = CreateIndeterminatePath(checkBox))
                                {
                                    // Draw the tick with a thickish brush
                                    using (SolidBrush tickBrush = new SolidBrush(_contextCheckTick))
                                        e.Graphics.FillPath(tickBrush, tickPath);
                                }
                                break;
                        }
                    }
                }
            }
        }

        #endregion

        #region OnRenderItemText

        /// <summary>
        ///     Raises the RenderItemText event.
        /// </summary>
        /// <param name="e">An ToolStripItemTextRenderEventArgs containing the event data.</param>
        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
        {
            if ((e.ToolStrip is MenuStrip) ||
                (e.ToolStrip != null) ||
                (e.ToolStrip is ContextMenuStrip) ||
                (e.ToolStrip is ToolStripDropDownMenu))
            {
                // We set the color depending on the enabled state
                if (!e.Item.Enabled)
                    e.TextColor = _textDisabled;
                else
                {
                    /*    if ((e.ToolStrip is MenuStrip) && !e.Item.Pressed && !e.Item.Selected)
                            e.TextColor = _textMenuStripItem;*/
                    if ((e.ToolStrip is StatusStrip) && !e.Item.Pressed && !e.Item.Selected)
                        e.TextColor = _textStatusStripItem;
                    else if ((e.ToolStrip is MenuStrip))
                        e.TextColor = _textStatusStripItem;
                    else if (e.ToolStrip is ToolStripDropDown)
                        e.TextColor = Color.Black;
                    else
                        e.TextColor = _textContextMenuItem;
                }

                e.TextRectangle = AdjustDrawRectangle(e.Item, e.TextRectangle);


                // All text is draw using the ClearTypeGridFit text rendering hint
                using (UseClearTypeGridFit clearTypeGridFit = new UseClearTypeGridFit(e.Graphics))
                    base.OnRenderItemText(e);
            }
            else
            {
                base.OnRenderItemText(e);
            }
        }

        private Rectangle AdjustDrawRectangle(ToolStripItem item, Rectangle rectangle)
        {
            if (LooksPressed(item))
            {
                var r = rectangle;
                r.X++;
                r.Y++;
                return r;
            }

            return rectangle;
        }

        #endregion

        private static bool LooksPressed(ToolStripItem item)
        {
            var parent = item.GetCurrentParent();
            if (parent is MenuStrip) return false;
            if (parent is ContextMenuStrip) return false;
            if (parent is ToolStripDropDown) return false;

            if (item.Pressed) return true;


            {
                var a = item as ToolStripDropDownButton;
                if (a != null && a.IsOnDropDown) return true;
            }

            {
                var a = item as ToolStripSplitButton;
                if (a != null && a.IsOnDropDown) return true;
            }


            return false;
        }

        #region OnRenderItemImage

        /// <summary>
        ///     Raises the RenderItemImage event.
        /// </summary>
        /// <param name="e">An ToolStripItemImageRenderEventArgs containing the event data.</param>
        protected override void OnRenderItemImage(ToolStripItemImageRenderEventArgs e)
        {
            // We only override the image drawing for context menus
            /* if ((e.ToolStrip is ContextMenuStrip) ||
                 (e.ToolStrip is ToolStripDropDownMenu))
             {*/


            if (e.Image != null)
            {
                var checkbox = e.Item as ToolStripMenuItem;
                if (checkbox == null || checkbox.CheckState == CheckState.Unchecked)
                {
                    var newrect = AdjustDrawRectangle(e.Item, e.ImageRectangle);
                    if (e.Item.Enabled)
                        e.Graphics.DrawImage(e.Image, newrect);
                    else
                        ControlPaint.DrawImageDisabled(e.Graphics, e.Image,
                            newrect.X,
                            newrect.Y,
                            Color.Transparent);
                }
            }


            /*  }
              else
              {
               



                 // base.OnRenderItemImage(e);
              }*/
        }

        #endregion

        #region OnRenderMenuItemBackground

        /// <summary>
        ///     Raises the RenderMenuItemBackground event.
        /// </summary>
        /// <param name="e">An ToolStripItemRenderEventArgs containing the event data.</param>
        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            if ((e.ToolStrip is MenuStrip) ||
                (e.ToolStrip is ContextMenuStrip) ||
                (e.ToolStrip is ToolStripDropDownMenu))
            {
                /* if ((e.Item.Pressed) && (e.ToolStrip is MenuStrip))
                 {
                     // Draw the menu/tool strip as a header for a context menu item
                     DrawContextMenuHeader(e.Graphics, e.Item);
                 }
                 else*/
                {
                    // We only draw a background if the item is selected and enabled
                    if (e.Item.Selected)
                    {
                        if (e.Item.Enabled)
                        {
                            // Do we draw as a menu strip or context menu item?
                            /*  if (e.ToolStrip is MenuStrip)
                                  DrawGradientToolItem(e.Graphics, e.Item, _itemToolItemSelectedColors);
                              else*/
                            DrawGradientContextMenuItem(e.Graphics, e.Item, _itemContextItemEnabledColors);
                        }
                        else
                        {
                            // Get the mouse position in tool strip coordinates
                            Point mousePos = e.ToolStrip.PointToClient(Control.MousePosition);

                            // If the mouse is not in the item area, then draw disabled
                            //if (!e.Item.Bounds.Contains(mousePos))
                            //{
                            // Do we draw as a menu strip or context menu item?
                            if (e.ToolStrip is MenuStrip)
                                DrawGradientToolItem(e.Graphics, e.Item, _itemDisabledColors);
                            else
                                DrawGradientContextMenuItem(e.Graphics, e.Item, _itemDisabledColors);
                            //}
                        }
                    }
                }
            }
            else
            {
                base.OnRenderMenuItemBackground(e);
            }
        }

        #endregion

        #region OnRenderSplitButtonBackground

        /// <summary>
        ///     Raises the RenderSplitButtonBackground event.
        /// </summary>
        /// <param name="e">An ToolStripItemRenderEventArgs containing the event data.</param>
        protected override void OnRenderSplitButtonBackground(ToolStripItemRenderEventArgs e)
        {
            if (e.Item.Selected || e.Item.Pressed)
            {
                // Cast to correct type
                ToolStripSplitButton splitButton = (ToolStripSplitButton) e.Item;

                // Draw the border and background
                RenderToolSplitButtonBackground(e.Graphics, splitButton, e.ToolStrip);

                // Get the rectangle that needs to show the arrow
                Rectangle arrowBounds = splitButton.DropDownButtonBounds;

                // Draw the arrow on top of the background
                OnRenderArrow(new ToolStripArrowRenderEventArgs(e.Graphics,
                    splitButton,
                    arrowBounds,
                    SystemColors.ControlText,
                    ArrowDirection.Down));
            }
            else
            {
                base.OnRenderSplitButtonBackground(e);
            }
        }

        #endregion

        #region OnRenderStatusStripSizingGrip

        /// <summary>
        ///     Raises the RenderStatusStripSizingGrip event.
        /// </summary>
        /// <param name="e">An ToolStripRenderEventArgs containing the event data.</param>
        protected override void OnRenderStatusStripSizingGrip(ToolStripRenderEventArgs e)
        {
            using (SolidBrush darkBrush = new SolidBrush(_gripDark),
                lightBrush = new SolidBrush(_gripLight))
            {
                // Do we need to invert the drawing edge?
                bool rtl = (e.ToolStrip.RightToLeft == RightToLeft.Yes);

                // Find vertical position of the lowest grip line
                int y = e.AffectedBounds.Bottom - _gripSize * 2 + 1;

                // Draw three lines of grips
                for (int i = _gripLines; i >= 1; i--)
                {
                    // Find the rightmost grip position on the line
                    int x = (rtl
                        ? e.AffectedBounds.Left + 1
                        : e.AffectedBounds.Right - _gripSize * 2 + 1);

                    // Draw grips from right to left on line
                    for (int j = 0; j < i; j++)
                    {
                        // Just the single grip glyph
                        DrawGripGlyph(e.Graphics, x, y, darkBrush, lightBrush);

                        // Move left to next grip position
                        x -= (rtl ? -_gripMove : _gripMove);
                    }

                    // Move upwards to next grip line
                    y -= _gripMove;
                }
            }
        }

        #endregion

        #region OnRenderToolStripContentPanelBackground

        /// <summary>
        ///     Raises the RenderToolStripContentPanelBackground event.
        /// </summary>
        /// <param name="e">An ToolStripContentPanelRenderEventArgs containing the event data.</param>
        protected override void OnRenderToolStripContentPanelBackground(ToolStripContentPanelRenderEventArgs e)
        {
            // Must call base class, otherwise the subsequent drawing does not appear!
            base.OnRenderToolStripContentPanelBackground(e);

            // Cannot paint a zero sized area
            if ((e.ToolStripContentPanel.Width > 0) &&
                (e.ToolStripContentPanel.Height > 0))
            {
                using (LinearGradientBrush backBrush = new LinearGradientBrush(e.ToolStripContentPanel.ClientRectangle,
                    ColorTable.ToolStripContentPanelGradientEnd,
                    ColorTable.ToolStripContentPanelGradientBegin,
                    90f))
                {
                    e.Graphics.FillRectangle(backBrush, e.ToolStripContentPanel.ClientRectangle);
                }
            }
        }

        #endregion

        #region OnRenderSeparator

        /// <summary>
        ///     Raises the RenderSeparator event.
        /// </summary>
        /// <param name="e">An ToolStripSeparatorRenderEventArgs containing the event data.</param>
        protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
        {
            if ((e.ToolStrip is ContextMenuStrip) ||
                (e.ToolStrip is ToolStripDropDownMenu))
            {
                // Create the light and dark line pens
                using (Pen lightPen = new Pen(_separatorMenuLight),
                    darkPen = new Pen(_separatorMenuDark))
                {
                    DrawSeparator(e.Graphics, e.Vertical, e.Item.Bounds,
                        lightPen, darkPen, _separatorInset,
                        (e.ToolStrip.RightToLeft == RightToLeft.Yes));
                }
            }
            else if (e.ToolStrip is StatusStrip)
            {
                // Create the light and dark line pens
                using (Pen lightPen = new Pen(ColorTable.SeparatorLight),
                    darkPen = new Pen(ColorTable.SeparatorDark))
                {
                    DrawSeparator(e.Graphics, e.Vertical, e.Item.Bounds,
                        lightPen, darkPen, 0, false);
                }
            }
            else
            {
                base.OnRenderSeparator(e);
            }
        }

        #endregion

        #region OnRenderToolStripBackground

        /// <summary>
        ///     Raises the RenderToolStripBackground event.
        /// </summary>
        /// <param name="e">An ToolStripRenderEventArgs containing the event data.</param>
        protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
        {
            if ((e.ToolStrip is ContextMenuStrip) ||
                (e.ToolStrip is ToolStripDropDownMenu))
            {
                // Create border and clipping paths
                using (GraphicsPath borderPath = CreateBorderPath(e.AffectedBounds, _cutContextMenu),
                    clipPath = CreateClipBorderPath(e.AffectedBounds, _cutContextMenu))
                {
                    // Clip all drawing to within the border path
                    using (UseClipping clipping = new UseClipping(e.Graphics, clipPath))
                    {
                        // Create the background brush
                        using (SolidBrush backBrush = new SolidBrush(_contextMenuBack))
                            e.Graphics.FillPath(backBrush, borderPath);
                    }
                }
            }
            else if (e.ToolStrip is StatusStrip)
            {
                // We do not paint the top two pixel lines, so are drawn by the status strip border render method
                RectangleF backRect = new RectangleF(0, 1.5f, e.ToolStrip.Width, e.ToolStrip.Height - 2);

                // Cannot paint a zero sized area
                if ((backRect.Width > 0) && (backRect.Height > 0))
                {
                    using (var backBrush = new SolidBrush(_statusBarLight))
                    {
                        //backBrush.Blend = _statusStripBlend;
                        e.Graphics.FillRectangle(backBrush, backRect);
                    }
                    var topRect = new Rectangle(0, 0, (int) backRect.Width, 5);
                    using (
                        var backBrush = new LinearGradientBrush(topRect, _statusBarDark, _statusBarLight,
                            LinearGradientMode.Vertical))
                    {
                        //backBrush.Blend = _statusStripBlend;
                        e.Graphics.FillRectangle(backBrush, topRect);
                    }
                }
            }
            else
            {
                //  base.OnRenderToolStripBackground(e);
                ToolStrip toolStrip = e.ToolStrip;

                if ((toolStrip.Height > 0) && (toolStrip.Width > 0))
                {
                    var height = toolStrip.Height;
                    var center = height / 2;
                    var width = toolStrip.Width;

                    var topRect = new Rectangle(0, 0, width, center);
                    var bottomRect = new Rectangle(0, center, width, height - center - 3);


                    using (
                        var topBrush = new LinearGradientBrush(topRect, clrWindows7topBegin, clrWindows7topEnd,
                            LinearGradientMode.Vertical))
                        e.Graphics.FillRectangle(topBrush, topRect);

                    using (
                        var bottomBrush = new LinearGradientBrush(bottomRect, clrWindows7bottomBegin,
                            clrWindows7bottomEnd, LinearGradientMode.Vertical))
                        e.Graphics.FillRectangle(bottomBrush, bottomRect);

                    using (var pen3 = new Pen(clrWindows7bottomBorder3))
                        e.Graphics.DrawLine(pen3, 0, height - 3, width, height - 3);

                    using (var pen2 = new Pen(clrWindows7bottomBorder2))
                        e.Graphics.DrawLine(pen2, 0, height - 2, width, height - 2);

                    using (var pen1 = new Pen(clrWindows7bottomBorder1))
                        e.Graphics.DrawLine(pen1, 0, height - 1, width, height - 1);


                    // e.Graphics.FillRectangle(new SolidBrush(clrBGTop2), e.AffectedBounds);


                    // e.Graphics.FillRectangle(bottomGradBrush, bottomGradRect);
                    //    e.Graphics.FillRectangle(horGradBrush, e.AffectedBounds);

                    // e.Graphics.DrawRectangle(new Pen(clrBGBorder, 1), new Rectangle(0, 1, e.ToolStrip.Width - 1, e.ToolStrip.Height - 4));


                    /* using (LinearGradientBrush brush = new LinearGradientBrush(toolStrip.ClientRectangle, Color.FromArgb(253, 253, 253), Color.FromArgb(229, 233, 238), LinearGradientMode.Vertical))
                     {
                         e.Graphics.FillRectangle(brush, 0, 0, toolStrip.Width, toolStrip.Height);
                     }*/
                }
            }
        }

        #endregion

        #region OnRenderImageMargin

        /// <summary>
        ///     Raises the RenderImageMargin event.
        /// </summary>
        /// <param name="e">An ToolStripRenderEventArgs containing the event data.</param>
        protected override void OnRenderImageMargin(ToolStripRenderEventArgs e)
        {
            if ((e.ToolStrip is ContextMenuStrip) ||
                (e.ToolStrip is ToolStripDropDownMenu))
            {
                // Start with the total margin area
                Rectangle marginRect = e.AffectedBounds;

                // Do we need to draw with separator on the opposite edge?
                bool rtl = (e.ToolStrip.RightToLeft == RightToLeft.Yes);

                marginRect.Y += _marginInset;
                marginRect.Height -= _marginInset * 2;

                // Reduce so it is inside the border
                if (!rtl)
                    marginRect.X += _marginInset + 3;
                else
                    marginRect.X += _marginInset / 2;


                // Draw the entire margine area in a solid color
                using (SolidBrush backBrush = new SolidBrush(ColorTable.ImageMarginGradientBegin))
                    e.Graphics.FillRectangle(backBrush, marginRect);

                // Create the light and dark line pens
                using (Pen lightPen = new Pen(_separatorMenuLight),
                    darkPen = new Pen(_separatorMenuDark))
                {
                    if (!rtl)
                    {
                        // Draw the light and dark lines on the right hand side
                        e.Graphics.DrawLine(lightPen, marginRect.Right, marginRect.Top, marginRect.Right,
                            marginRect.Bottom);
                        e.Graphics.DrawLine(darkPen, marginRect.Right - 1, marginRect.Top, marginRect.Right - 1,
                            marginRect.Bottom);
                    }
                    else
                    {
                        // Draw the light and dark lines on the left hand side
                        e.Graphics.DrawLine(lightPen, marginRect.Left - 1, marginRect.Top, marginRect.Left - 1,
                            marginRect.Bottom);
                        e.Graphics.DrawLine(darkPen, marginRect.Left, marginRect.Top, marginRect.Left, marginRect.Bottom);
                    }
                }
            }
            else
            {
                base.OnRenderImageMargin(e);
            }
        }

        #endregion

        #region OnRenderToolStripBorder

        /// <summary>
        ///     Raises the RenderToolStripBorder event.
        /// </summary>
        /// <param name="e">An ToolStripRenderEventArgs containing the event data.</param>
        protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
        {
            if ((e.ToolStrip is ContextMenuStrip) ||
                (e.ToolStrip is ToolStripDropDownMenu))
            {
                // If there is a connected area to be drawn
                if (!e.ConnectedArea.IsEmpty)
                    using (SolidBrush excludeBrush = new SolidBrush(_contextMenuBack))
                        e.Graphics.FillRectangle(excludeBrush, e.ConnectedArea);
                /* var k = e.ToolStrip as ToolStripDropDownMenu;
                 var q = k.Parent;*/
                var connectedArea = Rectangle.Empty;

                // Create border and clipping paths
                using (GraphicsPath borderPath = CreateBorderPath(e.AffectedBounds, connectedArea, _cutContextMenu),
                    insidePath = CreateInsideBorderPath(e.AffectedBounds, connectedArea, _cutContextMenu),
                    clipPath = CreateClipBorderPath(e.AffectedBounds, connectedArea, _cutContextMenu))
                {
                    // Create the different pen colors we need
                    using (Pen borderPen = new Pen(ColorTable.MenuBorder),
                        insidePen = new Pen(_separatorMenuLight))
                    {
                        // Clip all drawing to within the border path
                        using (UseClipping clipping = new UseClipping(e.Graphics, clipPath))
                        {
                            // Drawing with anti aliasing to create smoother appearance
                            using (UseAntiAlias uaa = new UseAntiAlias(e.Graphics))
                            {
                                // Draw the inside area first
                                e.Graphics.DrawPath(insidePen, insidePath);

                                // Draw the border area second, so any overlapping gives it priority
                                e.Graphics.DrawPath(borderPen, borderPath);
                            }

                            // Draw the pixel at the bottom right of the context menu
                            e.Graphics.DrawLine(borderPen, e.AffectedBounds.Right, e.AffectedBounds.Bottom,
                                e.AffectedBounds.Right - 1, e.AffectedBounds.Bottom - 1);
                        }
                    }
                }
            }
            else if (e.ToolStrip is StatusStrip)
            {
                // Draw two lines at top of the status strip
                /*   using (Pen darkBorder = new Pen(_statusStripBorderDark),
                             lightBorder = new Pen(_statusStripBorderLight))
                   {
                       e.Graphics.DrawLine(darkBorder, 0, 0, e.ToolStrip.Width, 0);
                       e.Graphics.DrawLine(lightBorder, 0, 1, e.ToolStrip.Width, 1);
                   }*/
            }
            else
            {
                //   base.OnRenderToolStripBorder(e);
            }
        }

        #endregion

        #region Implementation

        private void RenderToolButtonBackground(Graphics g,
            ToolStripButton button,
            ToolStrip toolstrip)
        {
            // We only draw a background if the item is selected or being pressed
            if (button.Enabled)
            {
                if (button.Checked)
                {
                    if (button.Pressed)
                        DrawGradientToolItem(g, button, _itemToolItemPressedColors);
                    else if (button.Selected)
                        DrawGradientToolItem(g, button, _itemToolItemCheckPressColors);
                    else
                        DrawGradientToolItem(g, button, _itemToolItemCheckedColors);
                }
                else
                {
                    if (button.Pressed)
                        DrawGradientToolItem(g, button, _itemToolItemPressedColors);
                    else if (button.Selected)
                        DrawGradientToolItem(g, button, _itemToolItemSelectedColors);
                }
            }
            else
            {
                if (button.Selected)
                {
                    // Get the mouse position in tool strip coordinates
                    Point mousePos = toolstrip.PointToClient(Control.MousePosition);

                    // If the mouse is not in the item area, then draw disabled
                    if (!button.Bounds.Contains(mousePos))
                        DrawGradientToolItem(g, button, _itemDisabledColors);
                }
            }
        }

        private void RenderToolDropButtonBackground(Graphics g,
            ToolStripItem item,
            ToolStrip toolstrip)
        {
            // We only draw a background if the item is selected or being pressed
            if (item.Selected || item.Pressed)
            {
                if (item.Enabled)
                {
                    DrawGradientToolItem(g, item,
                        item.Pressed ? _itemToolItemPressedColors : _itemToolItemSelectedColors);
                }
                else
                {
                    // Get the mouse position in tool strip coordinates
                    Point mousePos = toolstrip.PointToClient(Control.MousePosition);

                    // If the mouse is not in the item area, then draw disabled
                    if (!item.Bounds.Contains(mousePos))
                        DrawGradientToolItem(g, item, _itemDisabledColors);
                }
            }
        }

        private void RenderToolSplitButtonBackground(Graphics g,
            ToolStripSplitButton splitButton,
            ToolStrip toolstrip)
        {
            // We only draw a background if the item is selected or being pressed
            if (splitButton.Selected || splitButton.Pressed)
            {
                if (splitButton.Enabled)
                {
                    if (!splitButton.Pressed && splitButton.ButtonPressed)
                    {
                        // large
                        //DrawGradientToolItem(g, splitButton, _itemToolItemCheckPressColors);
                        DrawGradientToolSplitItem(g, splitButton,
                            _itemToolItemPressedColors,
                            _itemToolItemPressedColors,
                            _itemContextItemEnabledColors);
                    }
                    else if (splitButton.Pressed && !splitButton.ButtonPressed)
                    {
                        // small
                        //DrawContextMenuHeader(g, splitButton);
                        DrawGradientToolSplitItem(g, splitButton,
                            _itemToolItemPressedColors,
                            _itemToolItemPressedColors,
                            _itemContextItemEnabledColors);
                    }
                    else
                    {
                        DrawGradientToolSplitItem(g, splitButton,
                            _itemToolItemSelectedColors,
                            _itemToolItemSelectedColors,
                            _itemContextItemEnabledColors);
                    }
                }
                else
                {
                    // Get the mouse position in tool strip coordinates
                    Point mousePos = toolstrip.PointToClient(Control.MousePosition);

                    // If the mouse is not in the item area, then draw disabled
                    if (!splitButton.Bounds.Contains(mousePos))
                        DrawGradientToolItem(g, splitButton, _itemDisabledColors);
                }
            }
        }

        private void DrawGradientToolItem(Graphics g,
            ToolStripItem item,
            GradientItemColors colors)
        {
            // Perform drawing into the entire background of the item
            DrawGradientItem(g, new Rectangle(Point.Empty, item.Bounds.Size), colors);
        }

        private void DrawGradientToolSplitItem(Graphics g,
            ToolStripSplitButton splitButton,
            GradientItemColors colorsButton,
            GradientItemColors colorsDrop,
            GradientItemColors colorsSplit)
        {
            // Create entire area and just the drop button area rectangles
            Rectangle backRect = new Rectangle(Point.Empty, splitButton.Bounds.Size);
            Rectangle backRectDrop = splitButton.DropDownButtonBounds;

            // Cannot paint zero sized areas
            if ((backRect.Width > 0) && (backRectDrop.Width > 0) &&
                (backRect.Height > 0) && (backRectDrop.Height > 0))
            {
                // Area that is the normal button starts as everything
                Rectangle backRectButton = backRect;

                // The X offset to draw the split line
                int splitOffset;

                // Is the drop button on the right hand side of entire area?
                if (backRectDrop.X > 0)
                {
                    backRectButton.Width = backRectDrop.Left;
                    backRectDrop.X -= 1;
                    backRectDrop.Width++;
                    splitOffset = backRectDrop.X;
                }
                else
                {
                    backRectButton.Width -= backRectDrop.Width - 2;
                    backRectButton.X = backRectDrop.Right - 1;
                    backRectDrop.Width++;
                    splitOffset = backRectDrop.Right - 1;
                }

                // Create border path around the item
                using (GraphicsPath borderPath = CreateBorderPath(backRect, _cutMenuItemBack))
                {
                    // Draw the normal button area background
                    DrawGradientBack(g, backRectButton, colorsButton);

                    // Draw the drop button area background
                    DrawGradientBack(g, backRectDrop, colorsDrop);

                    // Draw the split line between the areas
                    using (
                        LinearGradientBrush splitBrush =
                            new LinearGradientBrush(
                                new Rectangle(backRect.X + splitOffset, backRect.Top, 1, backRect.Height + 1),
                                colorsSplit.Border1, colorsSplit.Border2, 90f))
                    {
                        // Sigma curve, so go from color1 to color2 and back to color1 again
                        splitBrush.SetSigmaBellShape(0.5f);

                        // Convert the brush to a pen for DrawPath call
                        using (Pen splitPen = new Pen(splitBrush))
                            g.DrawLine(splitPen, backRect.X + splitOffset, backRect.Top + 1, backRect.X + splitOffset,
                                backRect.Bottom - 1);
                    }

                    // Draw the border of the entire item
                    DrawGradientBorder(g, backRect, colorsButton);
                }
            }
        }

        private void DrawContextMenuHeader(Graphics g, ToolStripItem item)
        {
            // Get the rectangle the is the items area
            Rectangle itemRect = new Rectangle(Point.Empty, item.Bounds.Size);

            // Create border and clipping paths
            using (GraphicsPath borderPath = CreateBorderPath(itemRect, _cutToolItemMenu),
                insidePath = CreateInsideBorderPath(itemRect, _cutToolItemMenu),
                clipPath = CreateClipBorderPath(itemRect, _cutToolItemMenu))
            {
                // Clip all drawing to within the border path
                using (UseClipping clipping = new UseClipping(g, clipPath))
                {
                    // Draw the entire background area first
                    using (SolidBrush backBrush = new SolidBrush(_separatorMenuLight))
                        g.FillPath(backBrush, borderPath);

                    // Draw the border
                    using (Pen borderPen = new Pen(ColorTable.MenuBorder))
                        g.DrawPath(borderPen, borderPath);
                }
            }
        }

        private void DrawGradientContextMenuItem(Graphics g,
            ToolStripItem item,
            GradientItemColors colors)
        {
            // Do we need to draw with separator on the opposite edge?
            Rectangle backRect = new Rectangle(2, 0, item.Bounds.Width - 3, item.Bounds.Height);

            // Perform actual drawing into the background

            backRect.X++;
            backRect.Width -= 2;

            if ((backRect.Width > 0) && (backRect.Height > 0))
            {
                // Draw the background of the entire item
                DrawGradientBack(g, backRect, colors);

                // Draw the border of the entire item
                DrawGradientBorder(g, backRect, colors);
                // g.Clear(Color.Red);
            }


            // DrawGradientItem(g, backRect, colors);
        }

        private void DrawGradientItem(Graphics g,
            Rectangle backRect,
            GradientItemColors colors)
        {
            // Cannot paint a zero sized area
            if ((backRect.Width > 0) && (backRect.Height > 0))
            {
                // Draw the background of the entire item
                DrawGradientBack(g, backRect, colors);

                // Draw the border of the entire item
                DrawGradientBorder(g, backRect, colors);
            }
        }

        private void DrawGradientBack(Graphics g,
            Rectangle backRect,
            GradientItemColors colors)
        {
            // Reduce rect draw drawing inside the border
            backRect.Inflate(-1, -1);

            int y2 = backRect.Height / 2;
            Rectangle backRect1 = new Rectangle(backRect.X, backRect.Y, backRect.Width, y2);
            Rectangle backRect2 = new Rectangle(backRect.X, backRect.Y + y2, backRect.Width, backRect.Height - y2);
            Rectangle backRect1I = backRect1;
            Rectangle backRect2I = backRect2;
            backRect1I.Inflate(1, 1);
            backRect2I.Inflate(1, 1);

            using (
                LinearGradientBrush insideBrush1 = new LinearGradientBrush(backRect1I, colors.InsideTop1,
                    colors.InsideTop2, 90f),
                    insideBrush2 = new LinearGradientBrush(backRect2I, colors.InsideBottom1, colors.InsideBottom2, 90f))
            {
                g.FillRectangle(insideBrush1, backRect1);
                g.FillRectangle(insideBrush2, backRect2);
            }

            y2 = backRect.Height / 2;
            backRect1 = new Rectangle(backRect.X, backRect.Y, backRect.Width, y2);
            backRect2 = new Rectangle(backRect.X, backRect.Y + y2, backRect.Width, backRect.Height - y2);
            backRect1I = backRect1;
            backRect2I = backRect2;
            backRect1I.Inflate(1, 1);
            backRect2I.Inflate(1, 1);

            using (
                LinearGradientBrush fillBrush1 = new LinearGradientBrush(backRect1I, colors.FillTop1, colors.FillTop2,
                    90f),
                    fillBrush2 = new LinearGradientBrush(backRect2I, colors.FillBottom1, colors.FillBottom2, 90f))
            {
                // Reduce rect one more time for the innermost drawing
                backRect.Inflate(-1, -1);

                y2 = (backRect.Height / 2) + 1;
                backRect1 = new Rectangle(backRect.X, backRect.Y, backRect.Width, y2);
                backRect2 = new Rectangle(backRect.X, backRect.Y + y2, backRect.Width, backRect.Height - y2);

                g.FillRectangle(fillBrush1, backRect1);
                g.FillRectangle(fillBrush2, backRect2);
            }
        }

        private void DrawGradientBorder(Graphics g,
            Rectangle backRect,
            GradientItemColors colors)
        {
            // Drawing with anti aliasing to create smoother appearance
            using (UseAntiAlias uaa = new UseAntiAlias(g))
            {
                Rectangle backRectI = backRect;
                backRectI.Inflate(1, 1);

                // Finally draw the border around the menu item
                using (
                    LinearGradientBrush borderBrush = new LinearGradientBrush(backRectI, colors.Border1, colors.Border2,
                        90f))
                {
                    // Sigma curve, so go from color1 to color2 and back to color1 again
                    borderBrush.SetSigmaBellShape(0.5f);

                    // Convert the brush to a pen for DrawPath call
                    using (Pen borderPen = new Pen(borderBrush))
                    {
                        // Create border path around the entire item
                        using (GraphicsPath borderPath = CreateBorderPath(backRect, _cutMenuItemBack))
                            g.DrawPath(borderPen, borderPath);
                    }
                }
            }
        }

        private void DrawGripGlyph(Graphics g,
            int x,
            int y,
            Brush darkBrush,
            Brush lightBrush)
        {
            g.FillRectangle(lightBrush, x + _gripOffset, y + _gripOffset, _gripSquare, _gripSquare);
            g.FillRectangle(darkBrush, x, y, _gripSquare, _gripSquare);
        }

        private void DrawSeparator(Graphics g,
            bool vertical,
            Rectangle rect,
            Pen lightPen,
            Pen darkPen,
            int horizontalInset,
            bool rtl)
        {
            if (vertical)
            {
                int l = rect.Width / 2;
                int t = rect.Y;
                int b = rect.Bottom;

                // Draw vertical lines centered
                g.DrawLine(darkPen, l, t, l, b);
                g.DrawLine(lightPen, l + 1, t, l + 1, b);
            }
            else
            {
                int y = rect.Height / 2;
                int l = rect.X + (rtl ? 0 : horizontalInset);
                int r = rect.Right - (rtl ? horizontalInset : 0);

                // Draw horizontal lines centered
                g.DrawLine(darkPen, l, y, r, y);
                g.DrawLine(lightPen, l, y + 1, r, y + 1);
            }
        }

        private GraphicsPath CreateBorderPath(Rectangle rect,
            Rectangle exclude,
            float cut)
        {
            // If nothing to exclude, then use quicker method
            if (exclude.IsEmpty)
                return CreateBorderPath(rect, cut);

            // Drawing lines requires we draw inside the area we want
            rect.Width--;
            rect.Height--;

            // Create an array of points to draw lines between
            List<PointF> pts = new List<PointF>();

            float l = rect.X;
            float t = rect.Y;
            float r = rect.Right;
            float b = rect.Bottom;
            float x0 = rect.X + cut;
            float x3 = rect.Right - cut;
            float y0 = rect.Y + cut;
            float y3 = rect.Bottom - cut;
            float cutBack = (cut == 0f ? 1 : cut);

            // Does the exclude intercept the top line
            if ((rect.Y >= exclude.Top) && (rect.Y <= exclude.Bottom))
            {
                float x1 = exclude.X - 1 - cut;
                float x2 = exclude.Right + cut;

                if (x0 <= x1)
                {
                    pts.Add(new PointF(x0, t));
                    pts.Add(new PointF(x1, t));
                    pts.Add(new PointF(x1 + cut, t - cutBack));
                }
                else
                {
                    x1 = exclude.X - 1;
                    pts.Add(new PointF(x1, t));
                    pts.Add(new PointF(x1, t - cutBack));
                }

                if (x3 > x2)
                {
                    pts.Add(new PointF(x2 - cut, t - cutBack));
                    pts.Add(new PointF(x2, t));
                    pts.Add(new PointF(x3, t));
                }
                else
                {
                    x2 = exclude.Right;
                    pts.Add(new PointF(x2, t - cutBack));
                    pts.Add(new PointF(x2, t));
                }
            }
            else
            {
                pts.Add(new PointF(x0, t));
                pts.Add(new PointF(x3, t));
            }

            pts.Add(new PointF(r, y0));
            pts.Add(new PointF(r, y3));
            pts.Add(new PointF(x3, b));
            pts.Add(new PointF(x0, b));
            pts.Add(new PointF(l, y3));
            pts.Add(new PointF(l, y0));

            // Create path using a simple set of lines that cut the corner
            GraphicsPath path = new GraphicsPath();

            // Add a line between each set of points
            for (int i = 1; i < pts.Count; i++)
                path.AddLine(pts[i - 1], pts[i]);

            // Add a line to join the last to the first
            path.AddLine(pts[pts.Count - 1], pts[0]);

            return path;
        }

        private GraphicsPath CreateBorderPath(Rectangle rect, float cut)
        {
            // Drawing lines requires we draw inside the area we want
            rect.Width--;
            rect.Height--;

            // Create path using a simple set of lines that cut the corner
            GraphicsPath path = new GraphicsPath();
            path.AddLine(rect.Left + cut, rect.Top, rect.Right - cut, rect.Top);
            path.AddLine(rect.Right - cut, rect.Top, rect.Right, rect.Top + cut);
            path.AddLine(rect.Right, rect.Top + cut, rect.Right, rect.Bottom - cut);
            path.AddLine(rect.Right, rect.Bottom - cut, rect.Right - cut, rect.Bottom);
            path.AddLine(rect.Right - cut, rect.Bottom, rect.Left + cut, rect.Bottom);
            path.AddLine(rect.Left + cut, rect.Bottom, rect.Left, rect.Bottom - cut);
            path.AddLine(rect.Left, rect.Bottom - cut, rect.Left, rect.Top + cut);
            path.AddLine(rect.Left, rect.Top + cut, rect.Left + cut, rect.Top);
            return path;
        }

        private GraphicsPath CreateInsideBorderPath(Rectangle rect, float cut)
        {
            // Adjust rectangle to be 1 pixel inside the original area
            rect.Inflate(-1, -1);

            // Now create a path based on this inner rectangle
            return CreateBorderPath(rect, cut);
        }

        private GraphicsPath CreateInsideBorderPath(Rectangle rect,
            Rectangle exclude,
            float cut)
        {
            // Adjust rectangle to be 1 pixel inside the original area
            rect.Inflate(-1, -1);

            // Now create a path based on this inner rectangle
            return CreateBorderPath(rect, exclude, cut);
        }

        private GraphicsPath CreateClipBorderPath(Rectangle rect, float cut)
        {
            // Clipping happens inside the rect, so make 1 wider and taller
            rect.Width++;
            rect.Height++;

            // Now create a path based on this inner rectangle
            return CreateBorderPath(rect, cut);
        }

        private GraphicsPath CreateClipBorderPath(Rectangle rect,
            Rectangle exclude,
            float cut)
        {
            // Clipping happens inside the rect, so make 1 wider and taller
            rect.Width++;
            rect.Height++;
            //rect.Inflate(10, 10);
            // Now create a path based on this inner rectangle
            return CreateBorderPath(rect, exclude, cut);
        }

        private GraphicsPath CreateArrowPath(ToolStripItem item,
            Rectangle rect,
            ArrowDirection direction)
        {
            int x, y;

            // Find the correct starting position, which depends on direction
            if ((direction == ArrowDirection.Left) ||
                (direction == ArrowDirection.Right))
            {
                x = rect.Right - (rect.Width - 4) / 2;
                y = rect.Y + rect.Height / 2;
            }
            else
            {
                x = rect.X + rect.Width / 2;
                y = rect.Bottom - (rect.Height - 3) / 2;

                // The drop down button is position 1 pixel incorrectly when in RTL
                if ((item is ToolStripDropDownButton) &&
                    (item.RightToLeft == RightToLeft.Yes))
                    x++;

                if (item is ToolStripDropDownButton)
                    if (item.DisplayStyle != ToolStripItemDisplayStyle.Image)
                        x -= 10;
                    else
                        x -= 1;
            }

            y--;

            if (LooksPressed(item))
            {
                x++;
                y++;
            }


            // Create triangle using a series of lines
            GraphicsPath path = new GraphicsPath();

            PointF p1, p2, p3;

            var mw = 3.5f;
            var t = 2f;
            var a = 2f;

            switch (direction)
            {
                case ArrowDirection.Right:
                    p1 = new PointF(x - t, y - mw);
                    p2 = new PointF(x - t, y + mw);
                    p3 = new PointF(x + a, y);
                    break;
                case ArrowDirection.Left:
                    p1 = new PointF(x + t, y - mw);
                    p2 = new PointF(x + t, y + mw);
                    p3 = new PointF(x - a, y);
                    break;
                case ArrowDirection.Down:
                    p1 = new PointF(x - mw, y - t);
                    p2 = new PointF(x + mw, y - t);
                    p3 = new PointF(x, y + a);
                    break;
                case ArrowDirection.Up:
                    p1 = new PointF(x - mw, y - t);
                    p2 = new PointF(x + mw, y - t);
                    p3 = new PointF(x, y + a);
                    break;
                default:
                    throw new Exception();
            }


            path.AddLine(p1, p2);
            path.AddLine(p2, p3);
            path.AddLine(p3, p1);

            return path;
        }

        private GraphicsPath CreateTickPath(Rectangle rect)
        {
            // Get the center point of the rect
            int x = rect.X + rect.Width / 2;
            int y = rect.Y + rect.Height / 2;
            x++;
            y -= 2;

            GraphicsPath path = new GraphicsPath();
            path.AddLine(x - 4, y, x - 2, y + 4);
            path.AddLine(x - 2, y + 4, x + 3, y - 5);
            return path;
        }

        private GraphicsPath CreateIndeterminatePath(Rectangle rect)
        {
            // Get the center point of the rect
            int x = rect.X + rect.Width / 2;
            int y = rect.Y + rect.Height / 2;


            GraphicsPath path = new GraphicsPath();
            path.AddLine(x - 3, y, x, y - 3);
            path.AddLine(x, y - 3, x + 3, y);
            path.AddLine(x + 3, y, x, y + 3);
            path.AddLine(x, y + 3, x - 3, y);
            return path;
        }

        #endregion

        //protected override void OnRenderOverflowButtonBackground(ToolStripItemRenderEventArgs e)
        //{
        //    var b=e.Item;
        //    //base.OnRenderOverflowButtonBackground(e);
        //}
    }
}