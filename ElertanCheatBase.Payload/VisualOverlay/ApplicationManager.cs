using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using ElertanCheatBase.Payload.InputHooks;
using ElertanCheatBase.Payload.Rendering;
using ElertanCheatBase.Payload.VisualOverlay.Applications.Terminal;

namespace ElertanCheatBase.Payload.VisualOverlay
{
    public class ApplicationManager
    {
        public ApplicationManager()
        {
            AvailableApplications.Add(typeof(App));
        }

        public List<Type> AvailableApplications { get; set; } = new List<Type>();
        public List<Application> RunningApplications { get; set; } = new List<Application>();

        public Application StartApplication(Type appType)
        {
            var app = (Application) Activator.CreateInstance(appType);
            RunningApplications.Add(app);
            return app;
        }

        public void Draw(IRenderDevice desktopRenderDevice)
        {
            foreach (var app in RunningApplications)
            foreach (var window in app.Windows.Where(w => w.Visible))
            {
                var windowRenderDevice = new PartialRenderDevice(desktopRenderDevice,
                    new Rectangle(window.Position.X, window.Position.Y, window.Size.Width, window.Size.Height));
                window.Draw(windowRenderDevice);
            }
        }

        public void HandleMouseInput(Point mousePosition, MouseMessages mouseMessage)
        {
            foreach (var app in RunningApplications)
            foreach (var window in app.Windows.Where(w => w.Visible))
                if (mousePosition.X >= window.Position.X &&
                    mousePosition.Y >= window.Position.Y &&
                    mousePosition.X <= window.Position.X + window.Size.Width &&
                    mousePosition.Y <= window.Position.Y + window.Size.Height)
                {
                    var partialPosition = new Point(mousePosition.X - window.Position.X,
                        mousePosition.Y - window.Position.Y);
                    window.HandleMouseInput(partialPosition, mouseMessage);
                }
        }
    }
}