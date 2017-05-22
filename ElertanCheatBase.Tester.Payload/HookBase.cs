using System.Diagnostics;
using ElertanCheatBase.Csgo.Payload.Chams;
using ElertanCheatBase.Payload.InputHooks;
using SharpDX.Direct3D9;

namespace ElertanCheatBase.Csgo.Payload
{
    public class HookBase : ElertanCheatBase.Payload.HookBase
    {
        public override void Initialize(Process p)
        {
            base.Initialize(p);
            InitializeChams();
        }

        private void InitializeChams()
        {
            var blueColor = new byte[]
            {
                0x42, 0x4D, 0x3C, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x36, 0x00, 0x00, 0x00, 0x28, 0x00, 0x00, 0x00,
                0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01,
                0x00, 0x20, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x12, 0x0B, 0x00, 0x00, 0x12, 0x0B, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0xFF, 0x00, 0x00, 0x00, 0x00, 0x00
            };
            var redColor = new byte[]
            {
                0x42, 0x4D, 0x3A, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x36, 0x00, 0x00, 0x00, 0x28, 0x00,
                0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x18, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xFF, 0x00
            };

            ChamsController.CreateTexture("blue", blueColor);
            ChamsController.CreateTexture("red", redColor);

            ChamsController.Chams.Add(new TerroristCham {Enabled = true});
            ChamsController.Chams.Add(new CounterTerroristCham {Enabled = true});
        }

        public override void Direct3D9_EndScene(Device device)
        {
            base.Direct3D9_EndScene(device);
        }

        public override void Direct3D9_DrawIndexedPrimitive(Device device, PrimitiveType primitiveType,
            int baseVertexIndex, int minVertexIndex,
            int numVertices, int startIndex, int primCount)
        {
            base.Direct3D9_DrawIndexedPrimitive(device, primitiveType, baseVertexIndex, minVertexIndex, numVertices,
                startIndex, primCount);
        }

        public override void HandleKeyDown(KeyboardHookKeyDown ev)
        {
            base.HandleKeyDown(ev);
        }
    }
}