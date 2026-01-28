using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Avaruz.FrameWork.Controls.Win
{
    public class AvaruzTextBox : Control
    {
        private readonly TextBox _textBox;
        private bool _hover;
        private bool _focused;
        private const int BorderPadding = 2;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public override string Text
        {
            get => _textBox.Text;
            set => _textBox.Text = value;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string Hint { get; set; } = string.Empty;

        [Category("Appearance")]
        [DefaultValue(8)]
        [Description("Gets or sets the radius of the rounded corners.")]
        public int BorderRadius
        {
            get;
            set
            {
                if (field != value)
                {
                    field = value;
                    UpdateRegion();
                    Invalidate();
                }
            }
        } = 8;

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool Multiline
        {
            get => _textBox.Multiline;
            set
            {
                _textBox.Multiline = value;
                UpdateTextBoxBounds();
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public char PasswordChar
        {
            get => _textBox.PasswordChar;
            set => _textBox.PasswordChar = value;
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool ReadOnly
        {
            get => _textBox.ReadOnly;
            set => _textBox.ReadOnly = value;
        }

        public AvaruzTextBox()
        {
            _textBox = new TextBox
            {
                BorderStyle = BorderStyle.None,
                BackColor = Color.White,
                Font = new Font("Segoe UI", 10F)
            };

            BackColor = Color.White;
            Font = new Font("Segoe UI", 10F);
            Margin = new Padding(0, 0, 0, 20);
            Size = new Size(200, 36);

            SetStyle(ControlStyles.UserPaint |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.SupportsTransparentBackColor |
                     ControlStyles.ContainerControl, true);

            Controls.Add(_textBox);

            _textBox.TextChanged += (s, e) => OnTextChanged(e);
            _textBox.Enter += TextBox_Enter;
            _textBox.Leave += TextBox_Leave;

            UpdateTextBoxBounds();
            UpdateRegion();
        }

        private void TextBox_Enter(object? sender, EventArgs e)
        {
            _focused = true;
            Invalidate();
        }

        private void TextBox_Leave(object? sender, EventArgs e)
        {
            _focused = false;
            Invalidate();
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            _hover = true;
            Invalidate();
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            _hover = false;
            Invalidate();
            base.OnMouseLeave(e);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            UpdateTextBoxBounds();
            UpdateRegion();
            Invalidate();
        }

        private void UpdateTextBoxBounds()
        {
            int padding = 10;
            int verticalPadding = Multiline ? 8 : (Height - _textBox.Height) / 2;
            _textBox.Location = new Point(padding, Math.Max(verticalPadding, 8));
            _textBox.Width = Width - (padding * 2);

            if (Multiline)
            {
                _textBox.Height = Height - (verticalPadding * 2);
            }
        }

        private void UpdateRegion()
        {
            if (Width > 0 && Height > 0)
            {
                int effectiveRadius = GetEffectiveRadius();
                using var path = CreateRoundedRect(new Rectangle(0, 0, Width, Height), effectiveRadius);
                Region = new Region(path);
            }
        }

        private int GetEffectiveRadius()
        {
            // El radio no puede ser mayor que la mitad de la dimensión más pequeña
            int maxRadius = Math.Min(Width, Height) / 2;
            return Math.Min(BorderRadius, maxRadius);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            int effectiveRadius = GetEffectiveRadius();

            // Dibujar fondo redondeado
            var rect = new Rectangle(0, 0, Width - 1, Height - 1);
            using (var path = CreateRoundedRect(rect, effectiveRadius))
            {
                using var backBrush = new SolidBrush(BackColor);
                g.FillPath(backBrush, path);
            }

            // Dibujar borde con ajuste para evitar artefactos
            var baseColor = Color.FromArgb(189, 189, 189);
            var focusColor = ColorTranslator.FromHtml("#2962FF");
            var borderColor = _focused ? focusColor : (_hover ? Color.FromArgb(120, 120, 120) : baseColor);
            var borderWidth = _focused ? 2f : 1f;

            // Ajustar rectángulo del borde para centrarlo en el trazo
            var borderRect = new Rectangle(
                (int)(borderWidth / 2),
                (int)(borderWidth / 2),
                Width - (int)borderWidth,
                Height - (int)borderWidth
            );

            using (var path = CreateRoundedRect(borderRect, effectiveRadius))
            using (var pen = new Pen(borderColor, borderWidth))
            {
                pen.Alignment = PenAlignment.Inset;
                g.DrawPath(pen, path);
            }

            // Dibujar hint si es necesario
            if (string.IsNullOrEmpty(_textBox.Text) && !string.IsNullOrEmpty(Hint) && !_focused)
            {
                using var hintBrush = new SolidBrush(Color.FromArgb(150, 117, 117, 117));
                var hintRect = new Rectangle(_textBox.Left, _textBox.Top, _textBox.Width, _textBox.Height);
                TextRenderer.DrawText(g, Hint, Font, hintRect, hintBrush.Color, TextFormatFlags.Left | TextFormatFlags.VerticalCenter);
            }

            base.OnPaint(e);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // No pintar el fondo aquí - lo hacemos en OnPaint
        }

        private GraphicsPath CreateRoundedRect(Rectangle r, int radius)
        {
            var path = new GraphicsPath();

            if (radius <= 0 || r.Width <= 0 || r.Height <= 0)
            {
                path.AddRectangle(r);
                return path;
            }

            int d = radius * 2;

            // Asegurar que el diámetro no sea mayor que las dimensiones del rectángulo
            if (d > r.Width)
            {
                d = r.Width;
            }

            if (d > r.Height)
            {
                d = r.Height;
            }

            if (d > 0)
            {
                path.AddArc(r.X, r.Y, d, d, 180, 90);
                path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
                path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
                path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
                path.CloseFigure();
            }
            else
            {
                path.AddRectangle(r);
            }

            return path;
        }

        public void SelectAll()
        {
            _textBox.SelectAll();
        }

        public void Focus()
        {
            _textBox.Focus();
        }
    }
}
