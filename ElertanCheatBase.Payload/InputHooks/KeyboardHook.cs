using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using ElertanCheatBase.Payload.Interfaces;

namespace ElertanCheatBase.Payload.InputHooks
{
    public class KeyboardHook : IHook
    {
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_SYSKEYDOWN = 0x0104;
        private static WinApi.HookProc _hookProc;

        private int _hookId;
        public bool BlockInput { get; set; } = false;

        public void Install(HookBase hookBase)
        {
            //var myThreadId = RemoteHooking.GetCurrentThreadId();
            //var processThreadId = Main.Process.Threads[0].Id;
            //var mName = Path.GetFileNameWithoutExtension(Main.Process.MainModule.ModuleName);
            //_hookId = SetWindowsHookEx(13, HookCallback, WinApi.GetModuleHandle(mName), processThreadId);

            _hookProc = HookCallback;
            _hookId = WinApi.SetWindowsHookEx(WH_KEYBOARD_LL, _hookProc, (IntPtr) 0, 0);
        }

        public void Uninstall()
        {
            WinApi.UnhookWindowsHookEx((IntPtr) _hookId);
        }

        public event EventHandler<KeyboardHookKeyDown> KeyDownOccured;


        private int HookCallback(int code, IntPtr wParam, IntPtr lParam)
        {
            if (code >= 0 && wParam.ToInt32() == WM_KEYDOWN)
                if (WinApi.GetForegroundWindow() == Main.Process.MainWindowHandle)
                {
                    var vkCode = Marshal.ReadInt32(lParam);
                    var key = (Keys) vkCode;

                    OnKeyDownOccured(new KeyboardHookKeyDown {Keys = key});

                    if (BlockInput) return 1;
                }
            return WinApi.CallNextHookEx((IntPtr) _hookId, code, wParam, lParam);
        }

        protected virtual void OnKeyDownOccured(KeyboardHookKeyDown e)
        {
            KeyDownOccured?.Invoke(this, e);
        }
    }

    public class KeyboardHookKeyDown : EventArgs
    {
        public Keys Keys { get; set; }
    }
}