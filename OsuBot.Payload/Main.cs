using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using EasyHook;
using ElertanCheatBase.Payload;

namespace OsuBot.Payload
{
    public class Main : ElertanCheatBase.Payload.Main
    {
        public Main(RemoteHooking.IContext context, string channelName)
            : base(context, channelName, VisualRenderType.None)
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
