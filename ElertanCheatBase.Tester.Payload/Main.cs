using System.Collections.Generic;
using System.Reflection;
using EasyHook;
using ElertanCheatBase.Payload;
using ElertanCheatBase.Payload.VisualOverlayItems;

namespace ElertanCheatBase.Csgo.Payload
{
    public class Main : ElertanCheatBase.Payload.Main
    {
        public Main(RemoteHooking.IContext context, string channelName, VisualRenderType visualRenderType)
            : base(context, channelName, visualRenderType)
        {
            HookBase = new HookBase();
            InitializeAction = Initialize;
        }

        public static string AssemblyPath => Assembly.GetExecutingAssembly().Location;

        private void Initialize()
        {
            VisualOverlay.OverlayVisible = true;
            VisualOverlay.Windows = BuildVisualOverlay();
        }

        private List<Window> BuildVisualOverlay()
        {
            var windows = new List<Window>();
            var mainWindow = new Window();

            windows.Add(mainWindow);
            return windows;
        }
    }
}