using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using EasyHook;
using Microsoft.Win32.SafeHandles;

namespace ElertanCheatBase.Payload
{
    public class Main : IEntryPoint
    {
        public static bool KeepRunning = true;
        public static Process Process { get; set; }
        public static Dictionary<string, ModuleInfo> ModuleInfos { get; } = new Dictionary<string, ModuleInfo>();
#if DEBUG
        private static Thread _consoleThread;
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
            {
                ModuleInfos.Add(processModule.ModuleName, new ModuleInfo { Name = processModule.ModuleName, MemorySize = processModule.ModuleMemorySize, Address = processModule.BaseAddress });
            }

            // If Ping fails then the Run method will be not be called
            @interface.Ping();
        }

        public void Run(RemoteHooking.IContext context, string channelName, VisualRenderType visualRenderType)
        {
#if DEBUG
            // Instant launch debugger on debug build (does cause crash when csgo is not already running)
            Debugger.Launch();

            // Create console for testing
            CreateConsole();
#endif
            Process = Process.GetProcessById(RemoteHooking.GetCurrentProcessId());
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
            catch (Exception ex)
            {
                // ignored
            }

#if DEBUG
            _consoleThread.Abort();
#endif
            Core.Uninstall();

#if DEBUG
            WinApi.FreeConsole();
#endif

            // Finalise cleanup of hooks
            LocalHook.Release();
        }

        private static void CreateConsole()
        {
#if DEBUG
            _consoleThread = new Thread(() =>
            {
                WinApi.AllocConsole();
                var stdHandle = WinApi.GetStdHandle(WinApi.STD_OUTPUT_HANDLE);
                var safeFileHandle = new SafeFileHandle(stdHandle, true);
                var fileStream = new FileStream(safeFileHandle, FileAccess.Write);
                var encoding = Encoding.GetEncoding(WinApi.MY_CODE_PAGE);
                var standardOutput = new StreamWriter(fileStream, encoding) {AutoFlush = true};
                Console.SetOut(standardOutput);

                Console.WriteLine("Debug Console Elertan Cheatbase\n-------------------------------");
                while (true)
                    Console.Read();
            });
            _consoleThread.Start();
#endif
        }
    }
}