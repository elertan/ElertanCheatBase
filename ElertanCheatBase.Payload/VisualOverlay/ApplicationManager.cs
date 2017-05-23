using System;
using System.Collections.Generic;
using System.Linq;
using ElertanCheatBase.Payload.Rendering;
using ElertanCheatBase.Payload.VisualOverlay.Applications;

namespace ElertanCheatBase.Payload.VisualOverlay
{
    public class ApplicationManager
    {
        public ApplicationManager()
        {
            AvailableApplications.Add(typeof(Terminal));
        }

        public List<Type> AvailableApplications { get; set; } = new List<Type>();
        public List<Application> RunningApplications { get; set; } = new List<Application>();

        public Application StartApplication(Type appType)
        {
            var app = (Application) Activator.CreateInstance(appType);
            RunningApplications.Add(app);
            return app;
        }

        public void Draw(PartialRenderDevice desktopRenderDevice)
        {
            foreach (var app in RunningApplications)
            foreach (var window in app.Windows.Where(w => w.Visible))
                window.Draw(desktopRenderDevice);
        }
    }
}