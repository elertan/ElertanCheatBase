using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ElertanCheatBase.Payload.Interfaces;

namespace ElertanCheatBase.Payload.InputHooks
{
    public class MouseHook : IHook
    {
        private const int WH_MOUSE_LL = 14;
        private static WinApi.HookProc _hookProc;

        private int _hookId;
        public bool BlockInput { get; set; } = false;

        public void Install(HookBase hookBase)
        {
            _hookProc = HookCallback;
            _hookId = WinApi.SetWindowsHookEx(WH_MOUSE_LL, _hookProc, (IntPtr) 0, 0);
        }

        public void Uninstall()
        {
            WinApi.UnhookWindowsHookEx((IntPtr) _hookId);
        }

        public event EventHandler<MouseHookEventArgs> MouseChangesOccured;

        private int HookCallback(int code, IntPtr wParam, IntPtr lParam)
        {
            if (code >= 0)
                if (WinApi.GetForegroundWindow() == Main.Process.MainWindowHandle)
                {
                    var hookStruct = (MSLLHOOKSTRUCT) Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT));
                    var mouseInfo = new MouseInfo {Point = new Point(hookStruct.pt.x, hookStruct.pt.y)};
                    MouseChangesOccured?.Invoke(this,
                        new MouseHookEventArgs {MouseInfo = mouseInfo, MouseMessage = (MouseMessages) wParam});
                    if (BlockInput) return 1;
                }
            return WinApi.CallNextHookEx((IntPtr) _hookId, code, wParam, lParam);
        }

        protected virtual void OnMouseChangesOccured(MouseHookEventArgs e)
        {
            MouseChangesOccured?.Invoke(this, e);
        }
    }

    public enum MouseMessages
    {
        WM_LBUTTONDOWN = 0x0201,
        WM_LBUTTONUP = 0x0202,
        WM_MOUSEMOVE = 0x0200,
        WM_MOUSEWHEEL = 0x020A,
        WM_RBUTTONDOWN = 0x0204,
        WM_RBUTTONUP = 0x0205
    }


    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public readonly int x;
        public readonly int y;
    }


    [StructLayout(LayoutKind.Sequential)]
    public struct MSLLHOOKSTRUCT
    {
        public readonly POINT pt;
        public readonly uint mouseData;
        public readonly uint flags;
        public readonly uint time;
        public readonly IntPtr dwExtraInfo;
    }

    public class MouseHookEventArgs : EventArgs
    {
        public MouseMessages MouseMessage { get; set; }
        public MouseInfo MouseInfo { get; set; }
    }

    public class MouseInfo
    {
        public Point Point { get; set; }
    }
}