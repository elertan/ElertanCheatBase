using System.Drawing;
using ElertanCheatBase.Payload.Rendering;

namespace ElertanCheatBase.Payload.VisualOverlay.Interactables
{
    public class Control
    {
        public Size Size { get; set; }
        public Point Position { get; set; }

        public virtual void Draw(PartialRenderDevice device)
        {
        }
    }
}