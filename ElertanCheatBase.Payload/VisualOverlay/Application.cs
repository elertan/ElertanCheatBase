using System.Collections.Generic;
using ElertanCheatBase.Payload.VisualOverlay.Interactables;

namespace ElertanCheatBase.Payload.VisualOverlay
{
    public class Application
    {
        public string Name { get; set; } = "Undefined Appname";
        public List<Window> Windows { get; set; } = new List<Window>();
    }
}