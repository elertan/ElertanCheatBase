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

#if DEBUG
        private bool _debuggerHadBeenAttached;
#endif
        public HookBase HookBase;
        public Action InitializeAction;

        public Main(RemoteHooking.IContext context, string channelName, VisualRenderType visualRenderType)
        {
            // Connect to server object using provided channel name
            var @interface = RemoteHooking.IpcConnectClient<InjectorInterface>(channelName);

            Process = Process.GetProcessById(RemoteHooking.GetCurrentProcessId());
            foreach (ProcessModule processModule in Process.Modules)
                ModuleInfos.Add(processModule.ModuleName,
                    new ModuleInfo
                    {
                        Name = processModule.ModuleName,
                        MemorySize = processModule.ModuleMemorySize,
                        Address = processModule.BaseAddress
                    });

            // If Ping fails then the Run method will be not be called
            @interface.Ping();
        }

        public static Process Process { get; set; }
        public static Dictionary<string, ModuleInfo> ModuleInfos { get; } = new Dictionary<string, ModuleInfo>();

        public void Run(RemoteHooking.IContext context, string channelName, VisualRenderType visualRenderType)
        {
#if DEBUG
            // Instant launch debugger on debug build (does cause crash when csgo is not already running)
            Debugger.Launch();
#endif
            Process = Process.GetProcessById(RemoteHooking.GetCurrentProcessId());
            if (HookBase == null) throw new Exception("HookBase must be set");
            // Install

            Core.VisualRenderType = visualRenderType;
            Core.Install(Process, HookBase);

            InitializeAction?.Invoke();

            try
            {
                //MSG msg;
                while (KeepRunning /*&& WinApi.GetMessage(out msg, IntPtr.Zero, 0, 0) != 0*/)
                {
                    // When debugging, exit the dll when the debugging had stopped, this is not applicable for a release build
#if DEBUG
                    if (!Debugger.IsAttached && _debuggerHadBeenAttached) KeepRunning = false;
                    if (Debugger.IsAttached && !_debuggerHadBeenAttached) _debuggerHadBeenAttached = true;
#endif
                    //WinApi.TranslateMessage(ref msg);
                    //WinApi.DispatchMessage(ref msg);
                    Thread.Sleep(500);
                }
            }
            catch (Exception ex)
            {
                // ignored
            }

            Core.Uninstall();

            // Finalise cleanup of hooks
            LocalHook.Release();
        }
    }
}