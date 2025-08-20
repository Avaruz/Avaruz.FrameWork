using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Avaruz.FrameWork.Controls.Win
{
    public partial class PaneCaption : UserControl
    {
        private static class Consts
        {
            public const int DefaultHeight = 26;
            public const string DefaultFontName = "Tahoma";
            public const int DefaultFontSize = 12;
            public const int PosOffset = 4;
        }

        private bool m_active = false;
        private bool m_antiAlias = false;
        private bool m_allowActive = false;
        private string m_text = string.Empty;

        private Color m_colorActiveText = Color.Black;
        private Color m_colorInactiveText = Color.White;

        private Color m_colorActiveLow = Color.FromArgb(255, 165, 78);
        private Color m_colorActiveHigh = Color.FromArgb(255, 225, 155);
        private Color m_colorInactiveLow = Color.FromArgb(3, 55, 145);
        private Color m_colorInactiveHigh = Color.FromArgb(90, 135, 215);

        private SolidBrush m_brushActiveText;
        private SolidBrush m_brushInactiveText;
        private LinearGradientBrush m_brushActive;
        private LinearGradientBrush m_brushInactive;
        private readonly StringFormat m_format = new();

        [CategoryAttribute("Appearance")]
        [DescriptionAttribute("If should draw the text as antialiased.")]
        [DefaultValueAttribute(true)]
        public bool AntiAlias
        {
            get
            {
                return m_antiAlias;
            }

            set
            {
                m_antiAlias = value;
                Invalidate();
            }
        }

        [DefaultValueAttribute(typeof(Color), "Black")]
        [DescriptionAttribute("Color of the text when active.")]
        [CategoryAttribute("Appearance")]
        public Color ActiveTextColor
        {
            get
            {
                return m_colorActiveText;
            }

            set
            {
                if (value == Color.Empty)
                {
                    value = Color.Black;
                }
                m_colorActiveText = value;
                m_brushActiveText = new SolidBrush(m_colorActiveText);
                Invalidate();
            }
        }

        [CategoryAttribute("Appearance")]
        [DefaultValueAttribute(typeof(Color), "White")]
        [DescriptionAttribute("Color of the text when inactive.")]
        public Color InactiveTextColor
        {
            get
            {
                return m_colorInactiveText;
            }

            set
            {
                if (value == Color.Empty)
                {
                    value = Color.White;
                }
                m_colorInactiveText = value;
                m_brushInactiveText = new SolidBrush(m_colorInactiveText);
                Invalidate();
            }
        }

        [DescriptionAttribute("Low color of the active gradient.")]
        [CategoryAttribute("Appearance")]
        [DefaultValueAttribute(typeof(Color), "255, 165, 78")]
        public Color ActiveGradientLowColor
        {
            get
            {
                return m_colorActiveLow;
            }

            set
            {
                if (value == Color.Empty)
                {
                    value = Color.FromArgb(255, 165, 78);
                }
                m_colorActiveLow = value;
                CreateGradientBrushes();
                Invalidate();
            }
        }

        [CategoryAttribute("Appearance")]
        [DescriptionAttribute("High color of the active gradient.")]
        [DefaultValueAttribute(typeof(Color), "255, 225, 155")]
        public Color ActiveGradientHighColor
        {
            get
            {
                return m_colorActiveHigh;
            }

            set
            {
                if (value == Color.Empty)
                {
                    value = Color.FromArgb(255, 225, 155);
                }
                m_colorActiveHigh = value;
                CreateGradientBrushes();
                Invalidate();
            }
        }

        [DefaultValueAttribute(typeof(Color), "3, 55, 145")]
        [DescriptionAttribute("Low color of the inactive gradient.")]
        [CategoryAttribute("Appearance")]
        public Color InactiveGradientLowColor
        {
            get
            {
                return m_colorInactiveLow;
            }

            set
            {
                if (value == Color.Empty)
                {
                    value = Color.FromArgb(3, 55, 145);
                }
                m_colorInactiveLow = value;
                CreateGradientBrushes();
                Invalidate();
            }
        }

        [CategoryAttribute("Appearance")]
        [DescriptionAttribute("High color of the inactive gradient.")]
        [DefaultValueAttribute(typeof(Color), "90, 135, 215")]
        public Color InactiveGradientHighColor
        {
            get
            {
                return m_colorInactiveHigh;
            }

            set
            {
                if (value == Color.Empty)
                {
                    value = Color.FromArgb(90, 135, 215);
                }
                m_colorInactiveHigh = value;
                CreateGradientBrushes();
                Invalidate();
            }
        }

        // brush used to draw the caption
        private SolidBrush TextBrush
        {
            get
            {
                return m_active && m_allowActive ? m_brushActiveText : m_brushInactiveText;
            }
        }

        // gradient brush for the background
        private LinearGradientBrush BackBrush
        {
            get
            {
                return m_active && m_allowActive ? m_brushActive : m_brushInactive;
            }
        }

        public override string Text
        {
            get
            {
                return Caption;
            }

            set
            {
                Caption = value;
            }
        }

        [DescriptionAttribute("Text displayed in the caption.")]
        [DefaultValueAttribute("")]
        [CategoryAttribute("Appearance")]
        public string Caption
        {
            get
            {
                return m_text;
            }

            set
            {
                m_text = value;
                Invalidate();
            }
        }

        [DescriptionAttribute("The active state of the caption, draws the caption with different gradient colors.")]
        [DefaultValueAttribute(false)]
        [CategoryAttribute("Appearance")]
        public bool Active
        {
            get
            {
                return m_active;
            }

            set
            {
                m_active = value;
                Invalidate();
            }
        }
        [DefaultValueAttribute(true)]
        [CategoryAttribute("Appearance")]
        [DescriptionAttribute("True always uses the inactive state colors, false maintains an active and inactive state.")]
        public bool AllowActive
        {
            get
            {
                return m_allowActive;
            }

            set
            {
                m_allowActive = value;
                Invalidate();
            }
        }

        public PaneCaption()
        {
            InitializeComponent();

            // set double buffer styles
            SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);

            // init the height
            Height = Consts.DefaultHeight;

            // format used when drawing the text
            m_format.FormatFlags = StringFormatFlags.NoWrap;
            m_format.LineAlignment = StringAlignment.Center;
            m_format.Trimming = StringTrimming.EllipsisCharacter;

            // init the font
            Font = new Font("Tahoma", 12.0F, FontStyle.Bold);

            // create gdi objects
            ActiveTextColor = m_colorActiveText;
            InactiveTextColor = m_colorInactiveText;

            // setting the height above actually does this, but leave
            // in incase change the code (and forget to init the 
            // gradient brushes)
            CreateGradientBrushes();
        }

        // the caption needs to be drawn
        protected override void OnPaint(PaintEventArgs e)
        {
            DrawCaption(e.Graphics);
            base.OnPaint(e);
        }

        // draw the caption
        private void DrawCaption(Graphics g)
        {
            // background
            g.FillRectangle(this.BackBrush, this.DisplayRectangle);

            if (m_antiAlias)
            {
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            }

            // need a rectangle when want to use ellipsis
            RectangleF bounds = new(Consts.PosOffset,
                                                0,
                                                this.DisplayRectangle.Width - Consts.PosOffset,
                                                this.DisplayRectangle.Height);

            g.DrawString(m_text, this.Font, this.TextBrush, bounds, m_format);
        }

        // clicking on the caption does not give focus,
        // handle the mouse down event and set focus to self
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (m_allowActive)
            {
                Focus();
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            // create the gradient brushes based on the new size
            CreateGradientBrushes();
        }

        private void CreateGradientBrushes()
        {
            // can only create brushes when have a width and height
            if (Width > 0 && Height > 0)
            {
                m_brushActive?.Dispose();

                m_brushActive = new LinearGradientBrush(DisplayRectangle, m_colorActiveHigh, m_colorActiveLow, LinearGradientMode.Vertical);

                m_brushInactive?.Dispose();

                m_brushInactive = new LinearGradientBrush(DisplayRectangle, m_colorInactiveHigh, m_colorInactiveLow, LinearGradientMode.Vertical);
            }
        }

        private void PaneCaption_Load(object sender, EventArgs e)
        {

        }
    }
}
