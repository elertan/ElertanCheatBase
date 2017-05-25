using System.Drawing;
using SharpDX.Direct3D9;
using SharpDX.Mathematics.Interop;
using Font = SharpDX.Direct3D9.Font;

namespace ElertanCheatBase.Payload.Rendering
{
    public class D3D9RenderDevice : RenderDevice
    {
        private readonly Device _device;

        public D3D9RenderDevice(Device device) : base(new Size(device.Viewport.Width, device.Viewport.Height))
        {
            _device = device;
        }

        public RawColorBGRA GetColor(Color color)
        {
            return new RawColorBGRA(color.B, color.G, color.R, color.A);
        }

        public override void DrawRectangle(Point position, Size size, Color color)
        {
            var rect = new RawRectangle
            {
                Left = position.X,
                Top = position.Y,
                Right = position.X + size.Width,
                Bottom = position.Y + size.Height
            };
            _device.Clear(ClearFlags.Target, GetColor(color), 0, 0, new[] {rect});
        }

        public override void DrawText(string text, int fontSize, Point position, Color color)
        {
            if (text.Length == 0) text = "ERROR: EMPTY STRING GIVEN";

            using (
                var font = new Font(_device,
                    new FontDescription
                    {
                        FaceName = "Verdana",
                        OutputPrecision = FontPrecision.TrueTypeOnly,
                        Quality = FontQuality.Antialiased,
                        Weight = FontWeight.Bold,
                        Height = fontSize
                    }))
            {
                font.DrawText(null, text, position.X, position.Y, GetColor(color));
            }
        }

        public override void DrawText(string text, int fontSize, Rectangle area, FontDrawOptions options, Color color)
        {
            if (text.Length == 0) text = "ERROR: EMPTY STRING GIVEN";

            using (
                var font = new Font(_device,
                    new FontDescription
                    {
                        FaceName = "Verdana",
                        OutputPrecision = FontPrecision.TrueTypeOnly,
                        Quality = FontQuality.Antialiased,
                        Weight = FontWeight.Bold,
                        Height = fontSize
                    }))
            {
                var rawRectangle = new RawRectangle(area.Left, area.Top, area.Right, area.Bottom);
                font.DrawText(null, text, rawRectangle, FontDrawOptionsToFontDrawFlags(options), GetColor(color));
            }
        }

        private static FontDrawFlags FontDrawOptionsToFontDrawFlags(FontDrawOptions options)
        {
            var flags = FontDrawFlags.Top;
            if (options.HasFlag(FontDrawOptions.Bottom)) flags |= FontDrawFlags.Bottom;
            //if (options.HasFlag(FontDrawOptions.Top)) flags |= FontDrawFlags.Top;
            if (options.HasFlag(FontDrawOptions.Center)) flags |= FontDrawFlags.Center;
            if (options.HasFlag(FontDrawOptions.Right)) flags |= FontDrawFlags.Right;
            if (options.HasFlag(FontDrawOptions.Left)) flags |= FontDrawFlags.Left;
            if (options.HasFlag(FontDrawOptions.VerticalCenter)) flags |= FontDrawFlags.VerticalCenter;
            return flags;
        }
    }
}