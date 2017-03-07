using System;
using System.Diagnostics;
using System.Reflection;
using EasyHook;
using ElertanCheatBase.Payload;

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
            //var clientModuleAddress = IntPtr.Zero;
            //var size = 0;
            //foreach (ProcessModule module in Process.Modules)
            //    switch (module.ModuleName)
            //    {
            //        case "engine.dll":

            //            break;
            //        case "client.dll":
            //            clientModuleAddress = module.BaseAddress;
            //            size = module.ModuleMemorySize;
            //            break;
            //    }
            //var scanner = new Memory.SignatureScanner
            //{
            //    Address = clientModuleAddress,
            //    Size = size
            //};
            //var address = scanner.Scan("A3 ? ? ? ? C7 05 ? ? ? ? ? ? ? ? E8 ? ? ? ? 59 C3 6A");
        }
    }
}