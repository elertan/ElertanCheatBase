using System;
using System.Diagnostics;
using System.Windows.Forms;
using ElertanCheatBase.Payload.Interfaces;

namespace ElertanCheatBase.Payload.InputHooks
{
    public class KeyboardHook : IHook
    {
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_SYSKEYDOWN = 0x0104;
        private readonly Process _p;

        private IntPtr _hookId;

        public KeyboardHook(Process p)
        {
            _p = p;
        }

        public void Install(HookBase hookBase)
        {
            using (var module = _p.MainModule)
            {
                //var hInstance = WinApi.LoadLibrary("User32");
                var moduleHandle = WinApi.GetModuleHandle(module.ModuleName);
                _hookId = WinApi.SetWindowsHookEx(WH_KEYBOARD_LL, HookCallback, moduleHandle, 0);
                if (_hookId == IntPtr.Zero) throw new Exception("Failed to hook keyboard events");
            }
        }

        public void Uninstall()
        {
            WinApi.UnhookWindowsHookEx(_hookId);
        }

        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
                Console.WriteLine((Keys) wParam.ToInt32());

            // Blocking call
            // return new IntPtr(1);

            return WinApi.CallNextHookEx(_hookId, nCode, wParam, lParam);
        }
    }
}