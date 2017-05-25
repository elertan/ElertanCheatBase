using System.Drawing;
using ElertanCheatBase.Payload.InputHooks;
using ElertanCheatBase.Payload.VisualOverlay.Interactables;

namespace ElertanCheatBase.Payload.VisualOverlay.Applications.Terminal.Windows
{
    public class MainWindow : Window
    {
        public MainWindow(Application app) : base(app)
        {
            Size = new Size(700, 300);
        }

        public override void HandleMouseInput(Point mousePosition, MouseMessages mouseMessage)
        {
            base.HandleMouseInput(mousePosition, mouseMessage);
        }
    }
}