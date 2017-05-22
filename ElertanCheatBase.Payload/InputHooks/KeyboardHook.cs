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
        private static HookProc _hookProc;

        private int _hookId;
        public bool BlockInput { get; set; } = false;

        public void Install(HookBase hookBase)
        {
            //var myThreadId = RemoteHooking.GetCurrentThreadId();
            //var processThreadId = Main.Process.Threads[0].Id;
            //var mName = Path.GetFileNameWithoutExtension(Main.Process.MainModule.ModuleName);
            //_hookId = SetWindowsHookEx(13, HookCallback, WinApi.GetModuleHandle(mName), processThreadId);

            _hookProc = HookCallback;
            _hookId = SetWindowsHookEx(13, _hookProc, (IntPtr) 0, 0);
        }

        public void Uninstall()
        {
            WinApi.UnhookWindowsHookEx((IntPtr) _hookId);
        }

        public event EventHandler<KeyboardHookKeyDown> KeyDownOccured;


        [DllImport("user32.dll")]
        private static extern int SetWindowsHookEx(int idHook, HookProc func, IntPtr mod, int threadId);

        [DllImport("user32.dll")]
        private static extern int CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        private int HookCallback(int code, IntPtr wParam, IntPtr lParam)
        {
            if (code >= 0 && wParam.ToInt32() == WM_KEYDOWN)
                if (GetForegroundWindow() == Main.Process.MainWindowHandle)
                {
                    var vkCode = Marshal.ReadInt32(lParam);
                    var key = (Keys) vkCode;

                    OnKeyDownOccured(new KeyboardHookKeyDown {Key = key, KeyboardHook = this});

                    if (BlockInput) return 1;
                }
            return CallNextHookEx((IntPtr) _hookId, code, wParam, lParam);
        }

        protected virtual void OnKeyDownOccured(KeyboardHookKeyDown e)
        {
            KeyDownOccured?.Invoke(this, e);
        }

        private delegate int HookProc(int code, IntPtr wParam, IntPtr lParam);
    }

    public class KeyboardHookKeyDown : EventArgs
    {
        public KeyboardHook KeyboardHook { get; set; }
        public Keys Key { get; set; }
    }
}