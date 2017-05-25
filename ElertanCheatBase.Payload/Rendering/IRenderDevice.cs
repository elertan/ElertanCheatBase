using System;
using System.Drawing;

namespace ElertanCheatBase.Payload.Rendering
{
    public interface IRenderDevice
    {
        Rectangle Area { get; set; }
        Size Viewport { get; set; }

        void DrawRectangle(Point position, Size size, Color color);
        void DrawText(string text, int fontSize, Point position, Color color);
        void DrawText(string text, int fontSize, Rectangle area, FontDrawOptions options, Color color);
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
}