using System;
using System.Drawing;
using System.Windows.Forms;
using ElertanCheatBase.Payload.InputHooks;
using ElertanCheatBase.Payload.VisualOverlay.Direct3D9;
using SharpDX;
using SharpDX.Direct3D9;
using SharpDX.Mathematics.Interop;

namespace ElertanCheatBase.Payload.VisualOverlay
{
    public class Direct3D9Overlay
    {
        private static bool _enabled;
        private static Point _mousePosition = new Point(200, 200);

        static Direct3D9Overlay()
        {
        }

        public static bool Enabled
        {
            get { return _enabled; }
            set
            {
                _enabled = value;
                PreMouseLockPoint = default(Point);
            }
        }

        public static Keys ToggleKey { get; set; } = Keys.Insert;

        private static Dock Dock { get; } = new Dock();

        public static Point PreMouseLockPoint { get; set; }

        private static Point MousePosition
        {
            get { return _mousePosition; }
            set
            {
                var x = 0;
                var y = 0;
                if (value.X < 0) x = 0;
                else if (value.X > ViewPort.Width) x = ViewPort.Width;
                else x = value.X;

                if (value.Y < 0) y = 0;
                else if (value.Y > ViewPort.Height) y = ViewPort.Height;
                else y = value.Y;

                _mousePosition = new Point(x, y);
            }
        }

        private static RawViewport ViewPort { get; set; }

        public static void Draw(Device device)
        {
            if (!Enabled) return;
            ViewPort = device.Viewport;
            DrawBackground(device);

            Dock.Area = new RawRectangle(0, device.Viewport.Height - 40, device.Viewport.Width, device.Viewport.Height);
            Dock.Draw(device);

            device.DrawRectangle(new RawPoint(MousePosition.X - 4, MousePosition.Y - 4), new Size2(8, 8),
                new RawColorBGRA(255, 255, 255, 255));
        }

        private static void DrawBackground(Device device)
        {
            device.DrawRectangle(new RawPoint(0, 0), new Size2(device.Viewport.Width, 70),
                new RawColorBGRA(50, 50, 50, 255));
            device.DrawText($"Elertan's Cheat for {Main.Process.MainWindowTitle}", 21, new RawPoint(20, 10),
                new RawColorBGRA(255, 255, 255, 255));
            device.DrawText(DateTime.Now.ToShortTimeString() + ":" + DateTime.Now.Second, 21, new RawPoint(35, 40),
                new RawColorBGRA(255, 255, 255, 255));
            //device.DrawRectangle(new RawPoint(0, 70),
            //    new Size2(device.Viewport.Width, device.Viewport.Height - 70),
            //    new RawColorBGRA(20, 20, 20, 255));
        }

        public static void HandleMouseInput(MouseHookEventArgs ev)
        {
            if (PreMouseLockPoint == default(Point))
            {
                PreMouseLockPoint = ev.MouseInfo.Point;
                return;
            }
            var horizontalChanges = PreMouseLockPoint.X - ev.MouseInfo.Point.X;
            var verticalChanges = PreMouseLockPoint.Y - ev.MouseInfo.Point.Y;
            MousePosition = new Point(MousePosition.X - horizontalChanges, MousePosition.Y - verticalChanges);
        }
    }
}