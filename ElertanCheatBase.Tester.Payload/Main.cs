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
            var scanner = new Memory.SignatureScanner
            {
                Address = ModuleInfos["client.dll"].Address,
                ScanSize = ModuleInfos["client.dll"].MemorySize
            };
            var address = scanner.Scan("A3 ? ? ? ? C7 05 ? ? ? ? ? ? ? ? E8 ? ? ? ? 59 C3 6A") + 0x1;
            address = Memory.ReadIntPtr(address, 0x2C);
        }
    }
}