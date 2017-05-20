using System;
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
        }

        public Process Process { get; set; }

        public void Run(RemoteHooking.IContext context, string channelName, VisualRenderType visualRenderType)
        {
#if DEBUG
            // Instant launch debugger on debug build (does cause crash when csgo is not already running)
            Debugger.Launch();

            // Create console for testing
            WinApi.AllocConsole();
            var stdHandle = WinApi.GetStdHandle(WinApi.STD_OUTPUT_HANDLE);
            var safeFileHandle = new SafeFileHandle(stdHandle, true);
            var fileStream = new FileStream(safeFileHandle, FileAccess.Write);
            var encoding = Encoding.GetEncoding(WinApi.MY_CODE_PAGE);
            var standardOutput = new StreamWriter(fileStream, encoding);
            standardOutput.AutoFlush = true;
            Console.SetOut(standardOutput);

            Console.WriteLine("Debug Console Elertan Cheatbase\n-------------------------------");
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
            catch
            {
            }

#if DEBUG
            WinApi.FreeConsole();
#endif
            Core.Uninstall();

            // Finalise cleanup of hooks
            LocalHook.Release();
        }
    }
}