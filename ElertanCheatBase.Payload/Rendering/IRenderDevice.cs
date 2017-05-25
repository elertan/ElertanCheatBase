using System;
using System.Drawing;

namespace ElertanCheatBase.Payload.Rendering
{
    public interface IRenderDevice
    {
        int DeltaTime { get; set; }
        int Fps { get; set; }
        Rectangle Area { get; set; }
        Size Viewport { get; set; }

        void DrawRectangle(Point position, Size size, Color color);
        void DrawText(string text, int fontSize, Point position, Color color, FontWeight weight = FontWeight.Normal);

        void DrawText(string text, int fontSize, Rectangle area, FontDrawOptions options, Color color,
            FontWeight weight = FontWeight.Normal);
    }

    [Flags]
    public enum FontDrawOptions
    {
        Left = 0,
        Right = 1,
        Center = 2,
        Top = 4,
        Bottom = 8,
        VerticalCenter = 16
    }

    public enum FontWeight
    {
        Normal,
        Thin,
        Bold
    }
}