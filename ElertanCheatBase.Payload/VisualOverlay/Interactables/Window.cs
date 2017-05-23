using System.Drawing;
using ElertanCheatBase.Payload.Rendering;

namespace ElertanCheatBase.Payload.VisualOverlay.Interactables
{
    public class Window
    {
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

        public void Draw(PartialRenderDevice device)
        {
            device.DrawRectangle(Position, Size, BackgroundColor);
            var invertedBackgroundColor = Color.FromArgb(BackgroundColor.ToArgb() ^ 0xffffff);
            device.DrawRectangle(Position, new Size(Size.Width, BarHeight), invertedBackgroundColor);

            device.DrawText(Title, 18, new Point(Position.X + 15, Position.Y + BarHeight / 2 - 9), BackgroundColor);
        }
    }
}