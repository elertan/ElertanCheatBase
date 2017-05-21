using System;
using ElertanCheatBase.Payload.Interfaces;

namespace ElertanCheatBase.Payload.InputHooks
{
    public class KeyboardHook : IHook
    {
        private IntPtr _orgHook;

        public void Install(HookBase hookBase)
        {
            _orgHook = WinApi.SetWindowsHookEx(13, HookProc, IntPtr.Zero, 0);
        }

        public void Uninstall()
        {
            WinApi.UnhookWindowsHookEx(_orgHook);
        }

        private IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            //if (nCode > 0)

            return WinApi.CallNextHookEx(_orgHook, nCode, wParam, lParam);
        }
    }
}