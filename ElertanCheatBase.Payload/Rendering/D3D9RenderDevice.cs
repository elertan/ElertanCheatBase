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
                        Height = fontSize
                    }))
            {
                font.DrawText(null, text, position.X, position.Y, GetColor(color));
            }
        }
    }
}