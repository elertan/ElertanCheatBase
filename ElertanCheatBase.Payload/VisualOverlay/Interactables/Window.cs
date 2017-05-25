using System;
using System.Collections.Specialized;
using System.Drawing;
using System.Linq;
using ElertanCheatBase.Payload.InputHooks;
using ElertanCheatBase.Payload.Rendering;
using ElertanCheatBase.Payload.VisualOverlay.EventArguments;

namespace ElertanCheatBase.Payload.VisualOverlay.Interactables
{
    public class Window : Control
    {
        private Control _activeControl;
        private bool _hasLoaded;
        private bool _isMovingWindow;
        private Point _movingWindowOffsetPoint = Point.Empty;

        public Window(Application app)
        {
            Application = app;
            Title = Application.Name;
            Size = new Size(500, 300);

            Controls.CollectionChanged += Controls_CollectionChanged;
        }

        public Application Application { get; set; }
        // public Point Position { get; set; } = Point.Empty;
        //public Size Size { get; set; } = new Size(640, 480);
        public Color BackgroundColor { get; set; } = Color.FromArgb(20, 20, 20);
        //public bool Visible { get; set; } = true;
        public string Title { get; set; }

        public WindowBorderStyle BorderStyle { get; set; } = WindowBorderStyle.FixedSingle;

        public int BarHeight { get; set; } = 30;

        public int ZIndex { get; set; }

        public Control ActiveControl
        {
            get { return _activeControl; }
            set
            {
                var control = Controls.FirstOrDefault(c => c.Active && c != value);
                if (control != null) control.Active = false;
                _activeControl = value;
            }
        }

        public event EventHandler<RenderDeviceEventArgs> Load;

        private void Controls_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            foreach (var control in e.NewItems.Cast<Control>())
                control.Owner = this;
        }

        //public List<Control> Controls { get; set; } = new List<Control>();

        public override void Draw(IRenderDevice device)
        {
            if (!_hasLoaded)
            {
                _hasLoaded = true;
                OnLoad(new RenderDeviceEventArgs {RenderDevice = device});
            }

            device.DrawRectangle(Point.Empty, Size, BackgroundColor);

            if (BorderStyle == WindowBorderStyle.FixedSingle)
            {
                var invertedBackgroundColor = Color.FromArgb(BackgroundColor.ToArgb() ^ 0xffffff);
                device.DrawRectangle(Point.Empty, new Size(Size.Width, BarHeight), invertedBackgroundColor);

                device.DrawText(Title, 18, new Point(15, BarHeight / 2 - 9), BackgroundColor);
            }

            base.Draw(device);
            //foreach (var control in Controls)
            //{
            //    var controlRenderDevice = new PartialRenderDevice(device,
            //        new Rectangle(control.Position.X, control.Position.Y, control.Size.Width, control.Size.Height));
            //    control.Draw(controlRenderDevice);
            //}
        }

        public override void HandleMouseInput(Point mousePosition, MouseMessages mouseMessage)
        {
            if (BorderStyle == WindowBorderStyle.FixedSingle && mousePosition.Y <= BarHeight)
            {
                if (mouseMessage == MouseMessages.WM_LBUTTONDOWN)
                {
                    _isMovingWindow = true;
                    _movingWindowOffsetPoint = new Point(mousePosition.X, mousePosition.Y);
                }
                else if (mouseMessage == MouseMessages.WM_MOUSEMOVE)
                {
                    if (!_isMovingWindow) return;

                    var xDiff = mousePosition.X - _movingWindowOffsetPoint.X;
                    var yDiff = mousePosition.Y - _movingWindowOffsetPoint.Y;
                    Position = new Point(Position.X + xDiff, Position.Y + yDiff);
                }
                else if (mouseMessage == MouseMessages.WM_LBUTTONUP)
                {
                    _isMovingWindow = false;
                    _movingWindowOffsetPoint = Point.Empty;
                }
            }
            else
            {
                var controlsHandled = 0;
                _isMovingWindow = false;
                foreach (var control in Controls.Where(c => c.Visible))
                    if (mousePosition.X >= control.Position.X &&
                        mousePosition.Y >= control.Position.Y &&
                        mousePosition.X <= control.Position.X + control.Size.Width &&
                        mousePosition.Y <= control.Position.Y + control.Size.Height)
                    {
                        // TODO: LEFT HERE, WAS THINKING OF HOW TO HOVER CONTROLS AND SUCH
                        var mousePos = new Point(mousePosition.X - control.Position.X,
                            mousePosition.Y - control.Position.Y);
                        control.HandleMouseInput(mousePos, mouseMessage);
                        controlsHandled++;
                    }
                    else if (control.IsBeingHovered)
                    {
                        control.MouseUnhovered();
                    }

                if (controlsHandled == 0 && mouseMessage == MouseMessages.WM_LBUTTONDOWN)
                {
                    foreach (var control in Controls)
                        if (control.Active) control.Active = false;
                    ActiveControl = null;
                }
            }
        }

        public new void HandleKeyboardInput(KeyboardHookKeyDown ev)
        {
            ActiveControl?.HandleKeyboardInput(ev);
        }

        protected virtual void OnLoad(RenderDeviceEventArgs e)
        {
            Load?.Invoke(this, e);
        }
    }

    public enum WindowBorderStyle
    {
        None,
        FixedSingle
    }
}