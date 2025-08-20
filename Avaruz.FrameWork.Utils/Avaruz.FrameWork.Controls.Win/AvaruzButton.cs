using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Avaruz.FrameWork.Controls.Win;

public class AvaruzButton : Button
{
    //Fields
    private int _borderSize = 0;
    private int _borderRadius = 0;
    private Color _borderColor = Color.PaleVioletRed;

    //Properties
    [Category("Avaruz Code Advance")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public int BorderSize
    {
        get { return _borderSize; }
        set
        {
            _borderSize = value;
            this.Invalidate();
        }
    }

    [Category("Avaruz Code Advance")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public int BorderRadius
    {
        get { return _borderRadius; }
        set
        {
            _borderRadius = value;
            this.Invalidate();
        }
    }

    [Category("Avaruz Code Advance")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public Color BorderColor
    {
        get { return _borderColor; }
        set
        {
            _borderColor = value;
            this.Invalidate();
        }
    }

    [Category("Avaruz Code Advance")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public Color BackgroundColor
    {
        get { return this.BackColor; }
        set { this.BackColor = value; }
    }

    [Category("Avaruz Code Advance")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public Color TextColor
    {
        get { return this.ForeColor; }
        set { this.ForeColor = value; }
    }

    //Constructor
    public AvaruzButton()
    {
        this.FlatStyle = FlatStyle.Flat;
        this.FlatAppearance.BorderSize = 0;
        this.Size = new Size(150, 40);
        this.BackColor = Color.MediumSlateBlue;
        this.ForeColor = Color.White;
        this.Resize += Button_Resize;
    }

    //Methods
    private static GraphicsPath GetFigurePath(Rectangle rect, int radius)
    {
        GraphicsPath path = new();
        float curveSize = radius * 2F;

        path.StartFigure();
        path.AddArc(rect.X, rect.Y, curveSize, curveSize, 180, 90);
        path.AddArc(rect.Right - curveSize, rect.Y, curveSize, curveSize, 270, 90);
        path.AddArc(rect.Right - curveSize, rect.Bottom - curveSize, curveSize, curveSize, 0, 90);
        path.AddArc(rect.X, rect.Bottom - curveSize, curveSize, curveSize, 90, 90);
        path.CloseFigure();
        return path;
    }

    protected override void OnPaint(PaintEventArgs pevent)
    {
        base.OnPaint(pevent);

        Rectangle rectSurface = this.ClientRectangle;
        Rectangle rectBorder = Rectangle.Inflate(rectSurface, -_borderSize, -_borderSize);
        int smoothSize = 2;
        if (_borderSize > 0)
            smoothSize = _borderSize;

        if (_borderRadius > 2) //Rounded button
        {
            using GraphicsPath pathSurface = GetFigurePath(rectSurface, _borderRadius);
            using GraphicsPath pathBorder = GetFigurePath(rectBorder, _borderRadius - _borderSize);
            using Pen penSurface = new(this.Parent.BackColor, smoothSize);
            using Pen penBorder = new(_borderColor, _borderSize);
            pevent.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            //Button surface
            this.Region = new Region(pathSurface);
            //Draw surface border for HD result
            pevent.Graphics.DrawPath(penSurface, pathSurface);

            //Button border
            if (_borderSize >= 1)
            {
                //Draw control border
                pevent.Graphics.DrawPath(penBorder, pathBorder);
            }
        }
        else //Normal button
        {
            pevent.Graphics.SmoothingMode = SmoothingMode.None;
            //Button surface
            this.Region = new Region(rectSurface);
            //Button border
            if (_borderSize >= 1)
            {
                using Pen penBorder = new(_borderColor, _borderSize);
                penBorder.Alignment = PenAlignment.Inset;
                pevent.Graphics.DrawRectangle(penBorder, 0, 0, this.Width - 1, this.Height - 1);
            }
        }
    }
    protected override void OnHandleCreated(EventArgs e)
    {
        base.OnHandleCreated(e);
        this.Parent.BackColorChanged += Container_BackColorChanged;
    }

    private void Container_BackColorChanged(object sender, EventArgs e)
    {
        this.Invalidate();
    }
    private void Button_Resize(object sender, EventArgs e)
    {
        if (_borderRadius > this.Height)
            _borderRadius = this.Height;
    }
}
