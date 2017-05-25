using ElertanCheatBase.Payload.VisualOverlay.Applications.Dock.Windows;

namespace ElertanCheatBase.Payload.VisualOverlay.Applications.Dock
{
    internal class App : Application
    {
        public App()
        {
            Name = "Dock";
            var window = new MainWindow(this);
            Windows.Add(window);
        }

        public ApplicationManager AppManager { get; set; }
    }
}