using System.Collections.Generic;
using System.Drawing;
using ElertanCheatBase.Payload.VisualOverlay.Applications.DebugTool.Windows;

namespace ElertanCheatBase.Payload.VisualOverlay.Applications.DebugTool
{
    public class App : Application
    {
        public App()
        {
            Name = "DebugTool";

            Setup();
        }

        public static List<string> Logs { get; set; } = new List<string>();

        private void Setup()
        {
            var window = new MainWindow(this) {Position = new Point(100, 100)};

            Windows.Add(window);
        }
    }
}