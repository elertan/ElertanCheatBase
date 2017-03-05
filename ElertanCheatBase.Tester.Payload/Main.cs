using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using EasyHook;
using ElertanCheatBase.Payload;

namespace ElertanCheatBase.Tester.Payload
{
    public class Main : ElertanCheatBase.Payload.Main
    {
        public static string AssemblyPath => Assembly.GetExecutingAssembly().Location;

        public Main(RemoteHooking.IContext context, string channelName, VisualRenderType visualRenderType) : base(context, channelName, visualRenderType)
        {
            HookBase = new HookBase();
        }
    }
}
