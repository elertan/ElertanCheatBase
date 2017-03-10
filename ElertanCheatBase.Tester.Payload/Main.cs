using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using EasyHook;
using ElertanCheatBase.Payload;
using ElertanCheatBase.Tester.Payload.Models;

namespace ElertanCheatBase.Tester.Payload
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