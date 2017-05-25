using System;
using System.Drawing;

namespace ElertanCheatBase.Payload.Rendering
{
    public abstract class RenderDevice : IRenderDevice
    {
        protected RenderDevice(Size viewport)
        {
            Viewport = viewport;
            Area = new Rectangle(Point.Empty, Viewport);
        }

        public Size Viewport { get; set; }

        public virtual void DrawRectangle(Point position, Size size, Color color)
        {
            throw new NotImplementedException();
        }

        public Rectangle Area { get; set; }

        public virtual void DrawText(string text, int fontSize, Point position, Color color,
            FontWeight weight = FontWeight.Normal)
        {
            throw new NotImplementedException();
        }

        public virtual void DrawText(string text, int fontSize, Rectangle area, FontDrawOptions options, Color color,
            FontWeight weight = FontWeight.Normal)
        {
            throw new NotImplementedException();
        }

        public int DeltaTime { get; set; }

        public int Fps { get; set; }

        public virtual PartialRenderDevice CreatePartialDevice(Rectangle area)
        {
            return new PartialRenderDevice(this, area);
        }
    }
}