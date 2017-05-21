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

        private int _hookId;

        public void Install(HookBase hookBase)
        {
            //var myThreadId = RemoteHooking.GetCurrentThreadId();
            //var processThreadId = Main.Process.Threads[0].Id;
            //var mName = Path.GetFileNameWithoutExtension(Main.Process.MainModule.ModuleName);
            //_hookId = SetWindowsHookEx(13, HookCallback, WinApi.GetModuleHandle(mName), processThreadId);

            _hookId = SetWindowsHookEx(13, HookCallback, (IntPtr) 0, 0);
        }

        public void Uninstall()
        {
            WinApi.UnhookWindowsHookEx((IntPtr) _hookId);
        }


        [DllImport("user32.dll")]
        private static extern int SetWindowsHookEx(int idHook, HookProc func, IntPtr mod, int threadId);

        [DllImport("user32.dll")]
        private static extern int CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        private int HookCallback(int code, IntPtr wParam, IntPtr lParam)
        {
            if (code >= 0 && wParam.ToInt32() == WM_KEYDOWN)
            {
                var vkCode = Marshal.ReadInt32(lParam);
                var key = (Keys) vkCode;
                Console.WriteLine("Key: " + key);
            }

            // Blocking call
            //return 1;

            var ptr = new IntPtr(_hookId);
            return (int) WinApi.CallNextHookEx(ptr, code, wParam, lParam);
        }

        private delegate int HookProc(int code, IntPtr wParam, IntPtr lParam);
    }
}