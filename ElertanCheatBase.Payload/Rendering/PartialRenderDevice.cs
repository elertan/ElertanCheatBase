using System.Drawing;

namespace ElertanCheatBase.Payload.Rendering
{
    public class PartialRenderDevice
    {
        private readonly RenderDevice _renderDevice;
        public readonly Rectangle Area;

        public PartialRenderDevice(RenderDevice renderDevice, Rectangle area)
        {
            _renderDevice = renderDevice;
            Area = area;
        }

        public virtual void DrawRectangle(Point position, Size size, Color color)
        {
            var pos = new Point(Area.Left + position.X, Area.Top + position.Y);

            _renderDevice.DrawRectangle(pos, size, color);
        }

        public virtual void DrawText(string text, int fontSize, Point position, Color color)
        {
            var pos = new Point(Area.Left + position.X, Area.Top + position.Y);

            _renderDevice.DrawText(text, fontSize, pos, color);
        }
    }
}