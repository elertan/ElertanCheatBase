using System;
using System.Drawing;
using ElertanCheatBase.Payload.Rendering;

namespace ElertanCheatBase.Payload.VisualOverlay.Interactables
{
    public class Button : Control
    {
        public Button()
        {
            Size = new Size(60, 20);
        }

        public string Text { get; set; } = "Button";
        public Color BackgroundColor { get; set; } = Color.DarkGray;
        public Color TextColor { get; set; } = Color.Black;
        public Color BorderColor { get; set; } = Color.Black;

        public event EventHandler Clicked;
        public event EventHandler Hovered;
        public event EventHandler Unhovered;

        protected virtual void OnClicked()
        {
            Clicked?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnHovered()
        {
            Hovered?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnUnhovered()
        {
            Unhovered?.Invoke(this, EventArgs.Empty);
        }

        public override void Draw(PartialRenderDevice device)
        {
            device.DrawRectangle(Point.Empty, new Size(Size.Width, Size.Height), BackgroundColor);
            device.DrawText(Text, 12, new Point(5, 5), TextColor);
        }
    }
}