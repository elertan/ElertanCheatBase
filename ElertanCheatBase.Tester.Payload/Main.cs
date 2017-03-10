using System.Reflection;
using EasyHook;
using ElertanCheatBase.Payload;

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
            
        }
    }
}