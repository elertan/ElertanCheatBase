using System.Diagnostics;
using ElertanCheatBase.Payload;
using SharpDX.Direct3D9;
using SharpDX.Mathematics.Interop;

namespace ElertanCheatBase.Tester.Payload
{
    public class HookBase : ElertanCheatBase.Payload.HookBase
    {
        private readonly int max = 30;
        private readonly int min = 5;
        private bool _increasing = true;
        private int counter;
        private int current = 11;

        public override void Initialize(Process p)
        {
            base.Initialize(p);
        }

        public override void Direct3D9_EndScene(Device device)
        {
            base.Direct3D9_EndScene(device);

            //device.DrawRectangle(new RawPoint(50, 50),
            //    new Size2(device.Viewport.Width - 100, device.Viewport.Height - 100),
            //    new RawColorBGRA(255, 255, 255, 100));
            device.DrawText("Menu", current, new RawPoint(70, 120), new RawColorBGRA(0, 0, 0, 255));

            if (counter > 60)
            {
                if (current > max || current < min) _increasing = !_increasing;
                if (_increasing) current++;
                else current--;
                counter = 0;
            }
            counter++;
        }
    }
}