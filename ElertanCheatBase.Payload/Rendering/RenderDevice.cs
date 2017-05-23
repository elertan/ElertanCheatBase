using System;
using System.Drawing;

namespace ElertanCheatBase.Payload.Rendering
{
    public abstract class RenderDevice
    {
        protected RenderDevice(Size viewport)
        {
            Viewport = viewport;
        }

        public Size Viewport { get; set; }

        public virtual PartialRenderDevice CreatePartialDevice(Rectangle area)
        {
            return new PartialRenderDevice(this, area);
        }

        public virtual void DrawRectangle(Point position, Size size, Color color)
        {
            throw new NotImplementedException();
        }

        public virtual void DrawText(string text, int fontSize, Point position, Color color)
        {
            throw new NotImplementedException();
        }
    }
}