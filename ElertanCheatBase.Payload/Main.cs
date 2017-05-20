using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using EasyHook;

namespace ElertanCheatBase.Payload
{
    public class Main : IEntryPoint
    {
        public static bool KeepRunning = true;
        public static Process Process { get; set; }
        public static Dictionary<string, ModuleInfo> ModuleInfos { get; private set; } = new Dictionary<string, ModuleInfo>();
        private readonly InjectorInterface _interface;
#if DEBUG
        private bool _debuggerHadBeenAttached;
#endif
        public HookBase HookBase;
        public Action InitializeAction;

        public Main(RemoteHooking.IContext context, string channelName, VisualRenderType visualRenderType)
        {
            // Connect to server object using provided channel name
            _interface = RemoteHooking.IpcConnectClient<InjectorInterface>(channelName);

            // If Ping fails then the Run method will be not be called
            _interface.Ping();

            Process = Process.GetProcessById(RemoteHooking.GetCurrentProcessId());
            foreach (ProcessModule processModule in Process.Modules)
            {
                ModuleInfos.Add(processModule.ModuleName, new ModuleInfo { Name = processModule.ModuleName, MemorySize = processModule.ModuleMemorySize, Address = processModule.BaseAddress });
            }
        }

        public void Run(RemoteHooking.IContext context, string channelName, VisualRenderType visualRenderType)
        {
            // Injection is now complete and the server interface is connected
            //_interface.IsInstalled(RemoteHooking.GetCurrentProcessId());

#if DEBUG
            // Instant launch debugger on debug build
            Debugger.Launch();
#endif

            if (HookBase == null) throw new Exception("HookBase must be set");
            // Install
            Core.VisualRenderType = visualRenderType;
            Core.Install(Process, HookBase);

            InitializeAction?.Invoke();

            try
            {
                while (KeepRunning)
                {
                    // When debugging, exit the dll when the debugging had stopped, this is not applicable for a release build
#if DEBUG
                    if (!Debugger.IsAttached && _debuggerHadBeenAttached) KeepRunning = false;
                    if (Debugger.IsAttached && !_debuggerHadBeenAttached) _debuggerHadBeenAttached = true;
#endif
                    Thread.Sleep(500);
                }
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