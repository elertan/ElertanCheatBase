using System.Drawing;
using ElertanCheatBase.Payload.VisualOverlay.Applications.Terminal.Windows;

namespace ElertanCheatBase.Payload.VisualOverlay.Applications.Terminal
{
    public class App : Application
    {
        public App()
        {
            Name = "Terminal";

            Setup();
        }

        private void Setup()
        {
            var window = new MainWindow(this) {Position = new Point(50, 50)};

            Windows.Add(window);
        }
    }
}