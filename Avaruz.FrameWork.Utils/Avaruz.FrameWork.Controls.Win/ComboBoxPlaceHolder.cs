using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Avaruz.FrameWork.Controls.Win;

// ComboBox con soporte real de placeholder en el área de edición
public class ComboBoxPlaceHolder : ComboBox
{
    private string _placeholderText = string.Empty;
    private Color _placeholderColor = Color.Gray;

    [Description("Texto para el placeholder")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public string PlaceholderText
    {
        get => _placeholderText;
        set { _placeholderText = value; Invalidate(); }
    }

    [Description("Color Placeholder")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public Color PlaceholderColor
    {
        get => _placeholderColor;
        set { _placeholderColor = value; Invalidate(); }
    }

    protected override void WndProc(ref Message m)
    {
        base.WndProc(ref m);
        if (m.Msg == 0xF /* WM_PAINT */ && !Focused && string.IsNullOrEmpty(Text) && !string.IsNullOrEmpty(_placeholderText))
        {
            using Graphics g = Graphics.FromHwnd(Handle);
            using Brush b = new SolidBrush(_placeholderColor);
            Rectangle rect = ClientRectangle;
            rect.Offset(2, 2);
            g.DrawString(_placeholderText, Font, b, rect.Location);
        }
    }
}
