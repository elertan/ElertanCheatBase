using System.Drawing;

namespace ElertanCheatBase.Payload.Rendering
{
    public class PartialRenderDevice : IRenderDevice
    {
        private readonly IRenderDevice _renderDevice;

        public PartialRenderDevice(IRenderDevice renderDevice, Rectangle area)
        {
            _renderDevice = renderDevice;
            Area = area;
            Viewport = renderDevice.Viewport;
        }

        public Size Viewport { get; set; }

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

        public Rectangle Area { get; set; }

        public void DrawText(string text, int fontSize, Rectangle area, FontDrawOptions options, Color color)
        {
            var rec = new Rectangle(Area.Left + area.X, Area.Top + area.Y, area.Width, area.Height);
            _renderDevice.DrawText(text, fontSize, rec, options, color);
        }
    }
}