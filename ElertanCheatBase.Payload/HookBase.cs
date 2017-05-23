using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using ElertanCheatBase.Payload.CommonCheats;
using ElertanCheatBase.Payload.InputHooks;
using ElertanCheatBase.Payload.Rendering;
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

            var renderDevice = new D3D9RenderDevice(device);
            Overlay.Draw(renderDevice);
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

            if (ev.Key == Overlay.ToggleKey)
            {
                Overlay.Enabled = !Overlay.Enabled;
                Main.KeyboardHook.BlockInput = Overlay.Enabled;
                Main.MouseHook.BlockInput = Overlay.Enabled;
            }
        }

        public void HandleMouseChanges(MouseHookEventArgs ev)
        {
            ev.MouseInfo.Point = new Point(ev.MouseInfo.Point.X, ev.MouseInfo.Point.Y - 25);
            if (Overlay.Enabled)
                Overlay.HandleMouseInput(ev);
            else
                Overlay.ListenMouseInput(ev);
        }
    }
}