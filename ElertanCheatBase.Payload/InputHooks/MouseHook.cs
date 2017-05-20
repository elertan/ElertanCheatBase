using System;
using System.Runtime.InteropServices;
using SharpDX.Mathematics.Interop;

namespace ElertanCheatBase.Payload.InputHooks
{
    public class MouseHook
    {
        public static RawPoint MousePosition { get; private set; }
        public static bool PreventMousePassThru { get; set; }
        private static IntPtr HookProc { get; set; }

        public static void HookMouse()
        {
            throw new NotImplementedException();
            var windowHandle = Memory.Process.MainWindowHandle;
            //WinApi.GetWindowThreadProcessId(windowHandle, out uint windowThreadProcessId);
            HookProc = WinApi.SetWindowsHookEx(7 /*WH_MOUSE*/, Hook,
                WinApi.GetModuleHandle(Memory.Process.MainModule.ModuleName), 0);
        }

        private static IntPtr Hook(int code, IntPtr wParam, IntPtr lParam)
        {
            if (code > 0)
            {
                var mouseHookStruct = (MouseHookStruct) Marshal.PtrToStructure(lParam, typeof(MouseHookStruct));
                MousePosition = new RawPoint(mouseHookStruct.pt.x, mouseHookStruct.pt.y);
            }

            if (PreventMousePassThru) return new IntPtr(1);
            return WinApi.CallNextHookEx(HookProc, code, wParam, lParam);
        }

        [StructLayout(LayoutKind.Sequential)]
        // ReSharper disable once InconsistentNaming
        private class POINT
        {
            public int x;
            public int y;
        }

        [StructLayout(LayoutKind.Sequential)]
        private class MouseHookStruct
        {
            public int dwExtraInfo;
            public int hwnd;
            public POINT pt;
            public int wHitTestCode;
        }
    }
}