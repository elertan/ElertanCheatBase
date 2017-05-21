using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            _hooks.Add(new KeyboardHook());

            foreach (var hook in _hooks)
                hook.Install(HookBase);
        }

        public static void Uninstall()
        {
            foreach (var hook in _hooks)
                hook.Uninstall();
        }
    }
}