using System;
using System.Drawing;
using System.Windows.Forms;
using ElertanCheatBase.Payload.InputHooks;
using ElertanCheatBase.Payload.Rendering;
using ElertanCheatBase.Payload.VisualOverlay.Applications;
using ElertanCheatBase.Payload.VisualOverlay.Interactables;

namespace ElertanCheatBase.Payload.VisualOverlay
{
    public class Overlay
    {
        private static Point _mousePosition = new Point(200, 200);

        public static readonly ApplicationManager AppManager = new ApplicationManager();

        static Overlay()
        {
            var terminal = AppManager.StartApplication(typeof(Terminal));
        }

        public static bool Enabled { get; set; }

        public static Keys ToggleKey { get; set; } = Keys.Insert;

        private static Dock Dock { get; } = new Dock();

        public static Point PreMouseLockPoint { get; set; }

        public static Point MousePosition
        {
            get { return _mousePosition; }
            set
            {
                int x;
                int y;
                if (value.X < 0) x = 0;
                else if (value.X > ViewPort.Width) x = ViewPort.Width;
                else x = value.X;

                if (value.Y < 0) y = 0;
                else if (value.Y > ViewPort.Height) y = ViewPort.Height;
                else y = value.Y;

                _mousePosition = new Point(x, y);
            }
        }

        private static Size ViewPort { get; set; }

        public static void Draw(RenderDevice device)
        {
            if (!Enabled) return;
            ViewPort = device.Viewport;
            DrawMenuBar(device);
            DrawDock(device);

            var desktopRenderDevice = new PartialRenderDevice(device,
                new Rectangle(0, 90, device.Viewport.Width, device.Viewport.Height - 90 - 40));
            AppManager.Draw(desktopRenderDevice);

            DrawCursor(device);
        }

        private static void DrawDock(RenderDevice device)
        {
            var dockRenderDevice = new PartialRenderDevice(device,
                new Rectangle(0, device.Viewport.Height - 40, device.Viewport.Width, device.Viewport.Height));
            Dock.Draw(dockRenderDevice);
        }

        private static void DrawCursor(RenderDevice device)
        {
            device.DrawRectangle(new Point(MousePosition.X - 11, MousePosition.Y - 1), new Size(22, 2), Color.White);
            device.DrawRectangle(new Point(MousePosition.X - 1, MousePosition.Y - 11), new Size(2, 22), Color.White);
        }

        private static void DrawMenuBar(RenderDevice device)
        {
            var menuBarRenderDevice = new PartialRenderDevice(device, new Rectangle(0, 0, device.Viewport.Width, 70));

            menuBarRenderDevice.DrawRectangle(new Point(0, 0), new Size(menuBarRenderDevice.Area.Width, 70),
                Color.DarkSlateGray);
            menuBarRenderDevice.DrawText($"Elertan's Cheat for {Main.Process.MainWindowTitle}", 21, new Point(20, 10),
                Color.White);
            menuBarRenderDevice.DrawText(DateTime.Now.ToLongTimeString(), 21, new Point(35, 40), Color.White);
        }

        /// <summary>
        ///     Handles mouse input when the Overlay is active
        /// </summary>
        /// <param name="ev"></param>
        public static void HandleMouseInput(MouseHookEventArgs ev)
        {
            var horizontalChanges = PreMouseLockPoint.X - ev.MouseInfo.Point.X;
            var verticalChanges = PreMouseLockPoint.Y - ev.MouseInfo.Point.Y;
            MousePosition = new Point(MousePosition.X - horizontalChanges, MousePosition.Y - verticalChanges);

            
        }

        /// <summary>
        ///     Handles input when the overlay isnt active
        /// </summary>
        /// <param name="ev"></param>
        public static void ListenMouseInput(MouseHookEventArgs ev)
        {
            PreMouseLockPoint = ev.MouseInfo.Point;
            MousePosition = PreMouseLockPoint;
        }
    }
}