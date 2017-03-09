using System;
using System.Diagnostics;
using System.Linq;
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
            var clientModule = Memory.Modules.First(m => m.ModuleName == "client.dll");
            var scanner = new Memory.SignatureScanner
            {
                Address = clientModule.BaseAddress,
                Size = clientModule.ModuleMemorySize
            };

            var pattern =
                "8D 34 85 ?? ?? ?? ??" //lea esi, [eax*4+client.dll+xxxx] -> 3
                + " 89 15 ?? ?? ?? ??" //mov [client.dll+xxxx],edx
                + " 8B 41 08"          //mov eax,[ecx+08]
                + " 8B 48 ??";         //mov ecx,[eax+04] -> 18
            var address = scanner.Scan(pattern, 3); // [eax*4+client.dll+xxxx]
            var value1 = Memory.ReadInt32(address);
            var address2 = scanner.Scan(pattern, 18); // mov ecx,[eax+04]
            var value2 = Memory.ReadByte(address2);
            // This isnt right!
            var localPlayerAddress = new IntPtr(clientModule.BaseAddress.ToInt32() - value1 + value2);
        }
    }
}