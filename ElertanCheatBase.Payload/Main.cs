using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using EasyHook;

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

        public static Process Process { get; set; }

        [DllImport("user32.dll")]
        private static extern int PeekMessage(ref Message msg, IntPtr handle, uint Something, uint whoCares,
            uint whocares2);

        [DllImport("user32.dll")]
        private static extern int TranslateMessage(ref Message msg);

        [DllImport("user32.dll")]
        private static extern int DispatchMessage(ref Message msg);

        public void Run(RemoteHooking.IContext context, string channelName, VisualRenderType visualRenderType)
        {
#if DEBUG
            // Instant launch debugger on debug build (does cause crash when csgo is not already running)
            Debugger.Launch();

            // Create console for testing
            //WinApi.AllocConsole();
            //var stdHandle = WinApi.GetStdHandle(WinApi.STD_OUTPUT_HANDLE);
            //var safeFileHandle = new SafeFileHandle(stdHandle, true);
            //var fileStream = new FileStream(safeFileHandle, FileAccess.Write);
            //var encoding = Encoding.GetEncoding(WinApi.MY_CODE_PAGE);
            //var standardOutput = new StreamWriter(fileStream, encoding);
            //standardOutput.AutoFlush = true;
            //Console.SetOut(standardOutput);

            //Console.WriteLine("Debug Console Elertan Cheatbase\n-------------------------------");
#endif
            Process = Process.GetProcessById(RemoteHooking.GetCurrentProcessId());
            if (HookBase == null) throw new Exception("HookBase must be set");
            // Install

            Core.VisualRenderType = visualRenderType;
            Core.Install(Process, HookBase);

            InitializeAction?.Invoke();

            try
            {
                var msg = new Message();
                while (KeepRunning)
                {
                    // When debugging, exit the dll when the debugging had stopped, this is not applicable for a release build
#if DEBUG
                    if (!Debugger.IsAttached && _debuggerHadBeenAttached) KeepRunning = false;
                    if (Debugger.IsAttached && !_debuggerHadBeenAttached) _debuggerHadBeenAttached = true;
#endif
                    if (PeekMessage(ref msg, (IntPtr) 0, 0, 0, 0) != 0)
                    {
                        TranslateMessage(ref msg);
                        DispatchMessage(ref msg);
                    }
                    Thread.Sleep(2);
                }
            }
            catch
            {
            }

#if DEBUG
            //WinApi.FreeConsole();
#endif
            Core.Uninstall();

            // Finalise cleanup of hooks
            LocalHook.Release();
        }
    }

    public class Message
    {
        public int message { get; set; }
    }
}