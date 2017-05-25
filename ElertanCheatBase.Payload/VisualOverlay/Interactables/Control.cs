using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Windows.Forms;
using ElertanCheatBase.Payload.InputHooks;
using ElertanCheatBase.Payload.Rendering;

namespace ElertanCheatBase.Payload.VisualOverlay.Interactables
{
    public class Control
    {
        private bool _active;
        private Size _size = new Size(80, 25);

        public bool IsBeingHovered { get; private set; }
        public bool Visible { get; set; } = true;
        public Control Owner { get; set; }

        public bool Activatable { get; set; } = false;

        public bool Active
        {
            get { return _active; }
            set
            {
                _active = value;
                if (value) OnBecameActive();
                else OnBecameInactive();
            }
        }

        public Size Size
        {
            get { return _size; }
            set
            {
                _size = value;
                OnSizeChanged();
            }
        }

        public Point Position { get; set; }
        public ObservableCollection<Control> Controls { get; set; } = new ObservableCollection<Control>();

        public event EventHandler SizeChanged;
        public event EventHandler Clicked;
        public event EventHandler Hovered;
        public event EventHandler Unhovered;
        public event EventHandler BecameActive;
        public event EventHandler BecameInactive;
        public event EventHandler<ControlKeyDownEventArgs> KeyDown;

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
            foreach (var control in Controls)
            {
                var innerControlRenderDevice = new PartialRenderDevice(device,
                    new Rectangle(control.Position.X, control.Position.Y, control.Size.Width,
                        control.Size.Height));
                control.Draw(innerControlRenderDevice);
            }
        }

        public virtual void HandleMouseInput(Point mousePos, MouseMessages mouseMessage)
        {
            if (mouseMessage == MouseMessages.WM_LBUTTONDOWN && !Active && Activatable) PromoteToActive();

            if (!IsBeingHovered && mouseMessage == MouseMessages.WM_MOUSEMOVE)
            {
                IsBeingHovered = true;
                OnHovered();
            }

            if (IsBeingHovered && mouseMessage == MouseMessages.WM_LBUTTONDOWN)
                OnClicked();
        }

        private void PromoteToActive()
        {
            Active = true;
            var windowControl = Owner;
            while (!(windowControl is Window))
                windowControl = Owner.Owner;
            var window = (Window) windowControl;
            window.ActiveControl = this;
        }

        public void MouseUnhovered()
        {
            IsBeingHovered = false;
            OnUnhovered();
        }

        protected virtual void OnSizeChanged()
        {
            SizeChanged?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnBecameActive()
        {
            BecameActive?.Invoke(this, EventArgs.Empty);
        }

        public virtual void HandleKeyboardInput(KeyboardHookKeyDown ev)
        {
            OnKeyDown(new ControlKeyDownEventArgs {Keys = ev.Keys});
        }

        protected virtual void OnKeyDown(ControlKeyDownEventArgs e)
        {
            KeyDown?.Invoke(this, e);
        }

        protected virtual void OnBecameInactive()
        {
            BecameInactive?.Invoke(this, EventArgs.Empty);
        }
    }

    public class ControlKeyDownEventArgs : EventArgs
    {
        public Keys Keys { get; set; }
    }
}