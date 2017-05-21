using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using ElertanCheatBase.Payload.Interfaces;

namespace ElertanCheatBase.Payload.InputHooks
{
    public class KeyboardHook : IHook
    {
        // from winuser.h:
        private const int GWL_WNDPROC = -4;
        private const int WM_LBUTTONDOWN = 0x0201;

        private const int WM_KEYDOWN = 0x100;

        // program variables
        private IntPtr oldWndProc = IntPtr.Zero;
        private Win32WndProc newWndProc = null;

        [DllImport("user32")]
        private static extern IntPtr SetWindowLong(IntPtr hWnd, int nIndex, Win32WndProc newProc);
        [DllImport("user32")]
        private static extern int CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hWnd, int Msg, int wParam, int lParam);

        // A delegate that matches Win32 WNDPROC:
        private delegate int Win32WndProc(IntPtr hWnd, int Msg, int wParam, int lParam);


        public void Install(HookBase hookBase)
        {
            // hWnd is the window you want to subclass..., create a new 
            // delegate for the new wndproc
            newWndProc = WinProc;
            // subclass
            oldWndProc = SetWindowLong(Main.Process.MainWindowHandle, GWL_WNDPROC, newWndProc);
        }

        public void Uninstall()
        {
            //WinApi.SetWindowLongPtr(new HandleRef(this, _p.MainWindowHandle), GWL_WNDPROC, _originalWinProcPtr);
        }

        private int WinProc(IntPtr hWnd, int Msg, int wParam, int lParam)
        {
            if (wParam == WM_KEYDOWN)
            {
                var vkCode = Marshal.ReadInt32(new IntPtr(lParam));

                Console.WriteLine("Key: " + (Keys) vkCode);
            }
            // Blocking call
            // return new IntPtr(1);

            return CallWindowProc(oldWndProc, hWnd, Msg, wParam, lParam);
        }
    }
}