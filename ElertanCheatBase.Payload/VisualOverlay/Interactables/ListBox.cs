using System.Collections.Generic;
using System.Drawing;
using ElertanCheatBase.Payload.Rendering;

namespace ElertanCheatBase.Payload.VisualOverlay.Interactables
{
    public class ListBox : Control
    {
        public Color BackgroundColor { get; set; } = Color.WhiteSmoke;
        public Color TextColor { get; set; } = Color.Black;
        public List<string> Items { get; set; } = new List<string>();
        public int Padding { get; set; } = 5;
        public int ItemHeight { get; set; } = 13;
        public int FontSize { get; set; } = 12;
        public int Index { get; set; }

        public override void Draw(IRenderDevice device)
        {
            device.DrawRectangle(Point.Empty, Size, BackgroundColor);
            for (var i = 0; i < Items.Count && (i + 1) * ItemHeight < Size.Height; i++)
                device.DrawText(Items[i], FontSize,
                    new Rectangle(Padding, Padding + i * ItemHeight, Size.Width - Padding * 2, ItemHeight),
                    FontDrawOptions.Left | FontDrawOptions.VerticalCenter, Color.Black);

            base.Draw(device);
        }
    }
}