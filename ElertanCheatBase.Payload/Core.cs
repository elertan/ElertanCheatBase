using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ElertanCheatBase.Payload.Interfaces;
using ElertanCheatBase.Payload.VisualRenderHooks;

namespace ElertanCheatBase.Payload
{
    public class Core
    {
        public static string AssemblyPath => Assembly.GetExecutingAssembly().Location;
        public static HookBase HookBase { get; set; }
        public static VisualRenderType VisualRenderType { get; set; }
        private static readonly List<IHook> _hooks = new List<IHook>();

        public static void Install(Process p, HookBase hb)
        {
            Memory.Initialize(p);

            HookBase = hb;
            HookBase.Initialize(p);

            switch (VisualRenderType)
            {
                case VisualRenderType.Direct3D9:
                    // ReSharper disable once RedundantNameQualifier
                    _hooks.Add(new VisualRenderHooks.DirectD3D9());
                    break;
                case VisualRenderType.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            foreach (var hook in _hooks)
            {
                hook.Install(HookBase);
            }
        }

        public static void Uninstall()
        {
            foreach (var hook in _hooks)
            {
                hook.Uninstall();
            }
        }
    }
}
