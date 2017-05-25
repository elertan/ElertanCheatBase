using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using ElertanCheatBase.Payload.InputHooks;
using ElertanCheatBase.Payload.Rendering;

namespace ElertanCheatBase.Payload.VisualOverlay.Interactables
{
    public class Window
    {
        private bool _isMovingWindow;
        private Point _movingWindowOffsetPoint = Point.Empty;

        public Window(Application app)
        {
            Application = app;
            Title = Application.Name;
        }

        public Application Application { get; set; }
        public Point Position { get; set; } = Point.Empty;
        public Size Size { get; set; } = new Size(640, 480);
        public Color BackgroundColor { get; set; } = Color.FromArgb(20, 20, 20);
        public bool Visible { get; set; } = true;
        public string Title { get; set; }

        public int BarHeight { get; set; } = 30;

        public List<Control> Controls { get; set; } = new List<Control>();

        public void Draw(IRenderDevice device)
        {
            device.DrawRectangle(Point.Empty, Size, BackgroundColor);
            var invertedBackgroundColor = Color.FromArgb(BackgroundColor.ToArgb() ^ 0xffffff);
            device.DrawRectangle(Point.Empty, new Size(Size.Width, BarHeight), invertedBackgroundColor);

            device.DrawText(Title, 18, new Point(15, BarHeight / 2 - 9), BackgroundColor);

            foreach (var control in Controls)
            {
                var controlRenderDevice = new PartialRenderDevice(device,
                    new Rectangle(control.Position.X, control.Position.Y, control.Size.Width, control.Size.Height));
                control.Draw(controlRenderDevice);
            }
        }

        public void HandleMouseInput(Point mousePosition, MouseMessages mouseMessage)
        {
            if (mousePosition.Y <= BarHeight)
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
                    }
            }
        }
    }
}