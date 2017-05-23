using System.Drawing;
using ElertanCheatBase.Payload.VisualOverlay.Interactables;

namespace ElertanCheatBase.Payload.VisualOverlay.Applications
{
    public class Terminal : Application
    {
        public Terminal()
        {
            Name = "Terminal";

            Setup();
        }

        private void Setup()
        {
            var window = new Window(this);
            window.Position = new Point(200, 200);
            Windows.Add(window);
        }
    }
}