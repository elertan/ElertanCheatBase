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
    public abstract class Base : IEntryPoint
    {
        public static bool KeepRunning = true;
        private readonly InjectorInterface _interface;

        protected Base(RemoteHooking.IContext context, string channelName)
        {
            // Connect to server object using provided channel name
            _interface = RemoteHooking.IpcConnectClient<InjectorInterface>(channelName);

            // If Ping fails then the Run method will be not be called
            _interface.Ping();
        }

        public void Run(RemoteHooking.IContext context, string channelName)
        {
            // Injection is now complete and the server interface is connected
            _interface.IsInstalled(RemoteHooking.GetCurrentProcessId());

            var process = Process.GetProcessById(RemoteHooking.GetCurrentProcessId());
            // Install
            //Core.Install(process);

            try
            {
                while (KeepRunning)
                    Thread.Sleep(500);
            }
            catch
            {
            }

            //Core.Uninstall();

            // Finalise cleanup of hooks
            LocalHook.Release();
        }
    }
}
