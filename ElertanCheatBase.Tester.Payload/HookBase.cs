using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ElertanCheatBase.Payload;
using SharpDX.Direct3D9;
using SharpDX.Mathematics.Interop;

namespace ElertanCheatBase.Tester.Payload
{
    public class HookBase : ElertanCheatBase.Payload.HookBase
    {
        public override void Initialize(Process p)
        {
            base.Initialize(p);
        }

        public override void DirectD3D9_EndScene(Device device)
        {
            base.DirectD3D9_EndScene(device);

            using (var font = new Font(device, new FontDescription {FaceName = "Arial", Width = 21, Height = 13}))
            {
                font.DrawText(null, "Test", 50, 50, new RawColorBGRA(255, 255, 255, 255));
            }
        }
    }
}
