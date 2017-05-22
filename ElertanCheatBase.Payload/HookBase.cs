using System.Diagnostics;
using System.Windows.Forms;
using ElertanCheatBase.Payload.CommonCheats;
using ElertanCheatBase.Payload.InputHooks;
using ElertanCheatBase.Payload.VisualOverlay;
using SharpDX.Direct3D9;

namespace ElertanCheatBase.Payload
{
    public class HookBase
    {
        public ChamsController ChamsController { get; set; }

        public void Exit()
        {
            Main.KeepRunning = false;
        }

        public virtual void Initialize(Process p)
        {
            ChamsController = new ChamsController();
        }

        public virtual void Direct3D9_EndScene(Device device)
        {
            ChamsController.Direct3D9_EndScene(device);
            Direct3D9Overlay.Draw(device);
        }

        public virtual void Direct3D9_DrawIndexedPrimitive(Device device,
            PrimitiveType primitiveType,
            int baseVertexIndex,
            int minVertexIndex,
            int numVertices,
            int startIndex,
            int primCount)
        {
            ChamsController.Direct3D9_DrawIndexedPrimitive(device, primitiveType, baseVertexIndex, minVertexIndex,
                numVertices,
                startIndex, primCount);
        }

        public virtual void HandleKeyDown(KeyboardHookKeyDown ev)
        {
            Debug.WriteLine("Key: " + ev.Key);
#if DEBUG
            // Exit program
            if (ev.Key == Keys.Pause) Main.KeepRunning = false;
#endif

            if (Core.VisualRenderType == VisualRenderType.Direct3D9 && ev.Key == Direct3D9Overlay.ToggleKey)
            {
                Direct3D9Overlay.Enabled = !Direct3D9Overlay.Enabled;
                ev.KeyboardHook.BlockInput = Direct3D9Overlay.Enabled;
            }
        }
    }
}