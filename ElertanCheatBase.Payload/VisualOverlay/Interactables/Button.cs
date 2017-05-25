using System;
using System.Drawing;
using ElertanCheatBase.Payload.InputHooks;
using ElertanCheatBase.Payload.Rendering;

namespace ElertanCheatBase.Payload.VisualOverlay.Interactables
{
    public class Button : Control
    {
        public Button()
        {
            TextLabel.FontDrawOptions |= FontDrawOptions.Center;

            Controls.Add(TextLabel);

            Hovered += Button_Hovered;
            Unhovered += Button_Unhovered;
            SizeChanged += Button_SizeChanged;
        }

        public Label TextLabel { get; set; } = new Label();
        public Color BackgroundColor { get; set; } = Color.DarkGray;
        public Color BorderColor { get; set; } = Color.Black;

        private void Button_Unhovered(object sender, EventArgs e)
        {
            BackgroundColor = Color.DarkGray;
        }

        private void Button_SizeChanged(object sender, EventArgs e)
        {
            TextLabel.Size = Size;
        }

        private void Button_Hovered(object sender, EventArgs e)
        {
            BackgroundColor = Color.LightGray;
        }

        public override void HandleMouseInput(Point mousePos, MouseMessages mouseMessage)
        {
            base.HandleMouseInput(mousePos, mouseMessage);
        }

        public override void Draw(IRenderDevice device)
        {
            device.DrawRectangle(Point.Empty, Size, BackgroundColor);

            base.Draw(device);
        }
    }
}