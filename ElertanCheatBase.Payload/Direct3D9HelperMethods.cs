using System;
using SharpDX;
using SharpDX.Direct3D9;
using SharpDX.Mathematics.Interop;

namespace ElertanCheatBase.Payload
{
    public static class Direct3D9HelperMethods
    {
        public static void DrawRectangle(this Device device, RawPoint pt, Size2 size, RawColorBGRA color)
        {
            var rect = new RawRectangle
            {
                Left = pt.X,
                Top = pt.Y,
                Right = pt.X + size.Width,
                Bottom = pt.Y + size.Height
            };
            device.Clear(ClearFlags.Target, color, 0, 0, new[] { rect });

            //using (var line = new Line(device))
            //{
            //    line.Width = size.Height;
            //    //line.Antialias = true;
            //    var extraHeight = (float)size.Width / 2;
            //    var vertices = new[]
            //    {
            //        new RawVector2(pt.X, pt.Y + extraHeight),
            //        new RawVector2(pt.X + size.Width, pt.Y + extraHeight)
            //    };
            //    line.Draw(vertices, color);
            //}
        }

        public static void DrawText(this Device device, string text, int fontSize, RawPoint pt, RawColorBGRA color)
        {
            if (text.Length == 0) text = "ERROR: EMPTY STRING GIVEN";
            var fontWidth = device.Viewport.Width * fontSize / 1250;
            var fontHeight = Convert.ToInt32(fontWidth * 1.7);
            using (
                var font = new Font(device,
                    new FontDescription
                    {
                        FaceName = "Arial",
                        OutputPrecision = FontPrecision.TrueTypeOnly,
                        Quality = FontQuality.Antialiased,
                        Width = fontWidth,
                        Height = fontHeight
                    }))
            {
                font.DrawText(null, text, pt.X, pt.Y, color);
            }
        }
    }
}