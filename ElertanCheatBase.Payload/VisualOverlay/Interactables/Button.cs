using System;
using System.Drawing;
using ElertanCheatBase.Payload.InputHooks;
using ElertanCheatBase.Payload.Rendering;

namespace ElertanCheatBase.Payload.VisualOverlay.Interactables
{
    public class Button : Control
    {
        private Size _size = new Size(80, 25);

        public Button()
        {
            InnerControls.Add(TextLabel);
        }

        public new Size Size
        {
            get { return _size; }
            set
            {
                _size = value;
                TextLabel.Size = value;
            }
        }

        public Label TextLabel { get; set; } = new Label();
        public Color BackgroundColor { get; set; } = Color.DarkGray;
        public Color BorderColor { get; set; } = Color.Black;

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