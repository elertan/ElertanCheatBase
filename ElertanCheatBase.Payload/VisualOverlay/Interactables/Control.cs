using System;
using System.Collections.Generic;
using System.Drawing;
using ElertanCheatBase.Payload.InputHooks;
using ElertanCheatBase.Payload.Rendering;

namespace ElertanCheatBase.Payload.VisualOverlay.Interactables
{
    public class Control
    {
        public bool Visible { get; set; } = true;
        public Size Size { get; set; }
        public Point Position { get; set; }
        public List<Control> InnerControls { get; set; } = new List<Control>();

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

        public virtual void Draw(IRenderDevice device)
        {
            foreach (var innerControl in InnerControls)
            {
                var innerControlRenderDevice = new PartialRenderDevice(device,
                    new Rectangle(innerControl.Position.X, innerControl.Position.Y, innerControl.Size.Width,
                        innerControl.Size.Height));
                innerControl.Draw(innerControlRenderDevice);
            }
        }

        public virtual void HandleMouseInput(Point mousePos, MouseMessages mouseMessage)
        {

        }
    }
}