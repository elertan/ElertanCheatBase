using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using ElertanCheatBase.Payload.Interfaces;

namespace ElertanCheatBase.Payload.InputHooks
{
    public class KeyboardHook : IHook
    {
        private const int GWL_WNDPROC = -4;

        private const int WM_KEYDOWN = 0x100;
        private readonly Process _p;
        private WinApi.WndProcDelegate _originalWinProc;
        private IntPtr _originalWinProcPtr;

        public KeyboardHook(Process p)
        {
            _p = p;
        }

        public void Install(HookBase hookBase)
        {
            var functionPtr = Marshal.GetFunctionPointerForDelegate(new WinApi.WndProcDelegate(WinProc));
            //WinApi.SetLastError(0);
            _originalWinProcPtr = WinApi.GetWindowLongPtr(_p.MainWindowHandle, GWL_WNDPROC);
            //var error = new Win32Exception(Marshal.GetLastWin32Error()).Message;
            WinApi.SetWindowLongPtr(new HandleRef(this, _p.MainWindowHandle), GWL_WNDPROC,
                functionPtr);
            //_originalWinProc = Marshal.GetDelegateForFunctionPointer<WinApi.WndProcDelegate>(_originalWinProcPtr);
        }

        public void Uninstall()
        {
            WinApi.SetWindowLongPtr(new HandleRef(this, _p.MainWindowHandle), GWL_WNDPROC, _originalWinProcPtr);
        }

        private IntPtr WinProc(IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam)
        {
            if (wParam == (IntPtr) WM_KEYDOWN)
            {
                var vkCode = Marshal.ReadInt32(lParam);

                Console.WriteLine("Key: " + (Keys) vkCode);
            }
            // Blocking call
            // return new IntPtr(1);

            return WinApi.CallWindowProc(_originalWinProc, hWnd, uMsg, wParam, lParam);
        }
    }
}