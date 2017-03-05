using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EasyHook;

namespace ElertanCheatBase.Payload
{
    public class Main : IEntryPoint
    {
        public static bool KeepRunning = true;
        public HookBase HookBase;
        private readonly InjectorInterface _interface;

        public Main(RemoteHooking.IContext context, string channelName, VisualRenderType visualRenderType)
        {
            // Connect to server object using provided channel name
            _interface = RemoteHooking.IpcConnectClient<InjectorInterface>(channelName);

            // If Ping fails then the Run method will be not be called
            _interface.Ping();
        }

        public void Run(RemoteHooking.IContext context, string channelName, VisualRenderType visualRenderType)
        {
            // Injection is now complete and the server interface is connected
            _interface.IsInstalled(RemoteHooking.GetCurrentProcessId());

            var process = Process.GetProcessById(RemoteHooking.GetCurrentProcessId());
            if (HookBase == null) throw new Exception("HookBase must be set");
            // Install
            Core.VisualRenderType = visualRenderType;
            Core.Install(process, HookBase);

            try
            {
                while (KeepRunning)
                    Thread.Sleep(500);
            }
            catch
            {
            }

            Core.Uninstall();

            // Finalise cleanup of hooks
            LocalHook.Release();
        }
    }
}
