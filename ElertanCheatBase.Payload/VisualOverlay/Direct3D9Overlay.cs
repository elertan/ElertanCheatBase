using System;
using System.Windows.Forms;
using SharpDX;
using SharpDX.Direct3D9;
using SharpDX.Mathematics.Interop;

namespace ElertanCheatBase.Payload.VisualOverlay
{
    public class Direct3D9Overlay
    {
        public static bool Enabled { get; set; } = false;
        public static Keys ToggleKey { get; set; } = Keys.Insert;

        public static void Draw(Device device)
        {
            if (!Enabled) return;
            DrawBackground(device);
        }

        private static void DrawBackground(Device device)
        {
            device.DrawRectangle(new RawPoint(0, 0), new Size2(device.Viewport.Width, device.Viewport.Height),
                new RawColorBGRA(50, 50, 50, 255));
            device.DrawText($"Elertan's Cheat for {Main.Process.MainWindowTitle}", 21, new RawPoint(20, 10),
                new RawColorBGRA(255, 255, 255, 255));
            device.DrawText(DateTime.Now.ToShortTimeString() + ":" + DateTime.Now.Second, 21, new RawPoint(35, 40),
                new RawColorBGRA(255, 255, 255, 255));
            device.DrawRectangle(new RawPoint(0, 70), new Size2(device.Viewport.Width, device.Viewport.Height - 115),
                new RawColorBGRA(20, 20, 20, 255));
        }
    }
}