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
        public FontDrawOptions FontDrawOptions { get; set; } = FontDrawOptions.VerticalCenter;
        public FontWeight FontWeight { get; set; } = FontWeight.Normal;
        public string AdditionalCharactersToAppend { get; set; }

        public override void Draw(IRenderDevice device)
        {
            if (Text != string.Empty || AdditionalCharactersToAppend != string.Empty)
                device.DrawText(Text + AdditionalCharactersToAppend, FontSize, device.Area, FontDrawOptions, TextColor,
                    FontWeight);

            base.Draw(device);
        }
    }
}