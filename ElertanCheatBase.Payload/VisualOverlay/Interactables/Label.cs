using System.Drawing;
using ElertanCheatBase.Payload.Rendering;

namespace ElertanCheatBase.Payload.VisualOverlay.Interactables
{
    public class Label : Control
    {
        public Label()
        {
            Size = new Size(80, 25);
        }

        public string Text { get; set; } = "Label";
        public int FontSize { get; set; } = 14;
        public Color TextColor { get; set; } = Color.Black;

        public override void Draw(IRenderDevice device)
        {
            device.DrawText(Text, FontSize, device.Area, FontDrawOptions.Center | FontDrawOptions.VerticalCenter,
                TextColor);

            base.Draw(device);
        }
    }
}