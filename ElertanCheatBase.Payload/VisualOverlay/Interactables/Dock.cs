using System.Drawing;
using ElertanCheatBase.Payload.Rendering;

namespace ElertanCheatBase.Payload.VisualOverlay.Interactables
{
    public class Dock
    {
        public Color BaseColor { get; set; } = Color.FromArgb(255, 20, 20, 100);

        public void Draw(IRenderDevice device)
        {
            device.DrawRectangle(Point.Empty, new Size(device.Area.Width, device.Area.Height), BaseColor);
            device.DrawText("Start", 21, new Point(20, 10), Color.White);
        }
    }
}