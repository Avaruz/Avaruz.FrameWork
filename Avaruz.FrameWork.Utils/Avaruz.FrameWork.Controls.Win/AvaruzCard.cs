using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Avaruz.FrameWork.Controls.Win
{
    public class AvaruzCard : Panel
    {
        private int _borderRadius = 8;

        public AvaruzCard()
        {
            BackColor = Color.White;
            Padding = new Padding(24);
            DoubleBuffered = true;
            SetStyle(ControlStyles.SupportsTransparentBackColor | 
                     ControlStyles.UserPaint | 
                     ControlStyles.AllPaintingInWmPaint, true);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            UpdateRegion();
        }

        private void UpdateRegion()
        {
            if (Width > 0 && Height > 0)
            {
                var rect = ClientRectangle;
                rect.Inflate(-1, -1);
                using var path = CreateRoundedRect(rect, _borderRadius);
                Region = new Region(path);
            }
        }

        [Category("Appearance")]
        [DefaultValue(8)]
        [Description("Gets or sets the radius of the rounded corners.")]
        public int BorderRadius
        {
            get => _borderRadius;
            set
            {
                if (_borderRadius != value)
                {
                    _borderRadius = value;
                    UpdateRegion();
                    Invalidate();
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            var rect = ClientRectangle;
            rect.Inflate(-1, -1);

            using var path = CreateRoundedRect(rect, _borderRadius);
            using var shadowBrush = new SolidBrush(Color.FromArgb(24, 0, 0, 0));
            using var backBrush = new SolidBrush(BackColor);

            var shadowRect = rect;
            shadowRect.Offset(0, 2);
            using (var shadowPath = CreateRoundedRect(shadowRect, _borderRadius))
            {
                g.FillPath(shadowBrush, shadowPath);
            }

            g.FillPath(backBrush, path);
        }

        private GraphicsPath CreateRoundedRect(Rectangle r, int radius)
        {
            var path = new GraphicsPath();
            int d = radius * 2;

            path.AddArc(r.X, r.Y, d, d, 180, 90);
            path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            path.CloseFigure();

            return path;
        }
    }
}
