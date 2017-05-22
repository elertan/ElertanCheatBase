using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using ElertanCheatBase.Payload.InputHooks;
using ElertanCheatBase.Payload.Interfaces;
using ElertanCheatBase.Payload.VisualRenderHooks;

namespace ElertanCheatBase.Payload
{
    public class Core
    {
        private static readonly List<IHook> _hooks = new List<IHook>();
        public static string AssemblyPath => Assembly.GetExecutingAssembly().Location;
        public static HookBase HookBase { get; set; }
        public static VisualRenderType VisualRenderType { get; set; }

        public static void Install(Process p, HookBase hb)
        {
            Memory.Initialize(p);

            HookBase = hb;
            HookBase.Initialize(p);

            switch (VisualRenderType)
            {
                case VisualRenderType.Direct3D9:
                    // ReSharper disable once RedundantNameQualifier
                    _hooks.Add(new DirectD3D9());
                    break;
                case VisualRenderType.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            // Input Hooks
            var keyboardHook = new KeyboardHook();
            keyboardHook.KeyDownOccured += KeyboardHook_KeyDownOccured;
            var mouseHook = new MouseHook();
            mouseHook.MouseChangesOccured += MouseHook_MouseChangesOccured;
            _hooks.Add(keyboardHook);
            _hooks.Add(mouseHook);

            Main.KeyboardHook = keyboardHook;
            Main.MouseHook = mouseHook;

            foreach (var hook in _hooks)
                hook.Install(HookBase);
        }

        private static void MouseHook_MouseChangesOccured(object sender, MouseHookEventArgs e)
        {
            WinApi.RECT windowRect;
            WinApi.GetWindowRect(Main.Process.MainWindowHandle, out windowRect);

            // Dont handle messages outside our window frame
            if (e.MouseInfo.Point.X < windowRect.Left ||
                e.MouseInfo.Point.X > windowRect.Right ||
                e.MouseInfo.Point.Y < windowRect.Top ||
                e.MouseInfo.Point.Y > windowRect.Bottom) return;

            e.MouseInfo.Point = new Point(e.MouseInfo.Point.X - windowRect.Left, e.MouseInfo.Point.Y - windowRect.Top);
            HookBase.HandleMouseChanges(e);
        }

        private static void KeyboardHook_KeyDownOccured(object sender, KeyboardHookKeyDown e)
        {
            HookBase.HandleKeyDown(e);
        }

        public static void Uninstall()
        {
            foreach (var hook in _hooks)
                hook.Uninstall();
        }
    }
}