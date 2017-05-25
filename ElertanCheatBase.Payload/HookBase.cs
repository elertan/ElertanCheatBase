using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using ElertanCheatBase.Payload.CommonCheats;
using ElertanCheatBase.Payload.InputHooks;
using ElertanCheatBase.Payload.Rendering;
using ElertanCheatBase.Payload.VisualOverlay;
using SharpDX.Direct3D9;
using Timer = ElertanCheatBase.Payload.VisualOverlay.Timer;

namespace ElertanCheatBase.Payload
{
    public class HookBase
    {
        private int _frames;
        private int _lastFps;
        private DateTime _lastFpsCheck = DateTime.Now;
        private int _renderDeltaTime;
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
            var startTime = DateTime.Now;
            ChamsController.Direct3D9_EndScene(device);

            var renderDevice = new D3D9RenderDevice(device);
            renderDevice.DeltaTime = _renderDeltaTime;
            renderDevice.Fps = _lastFps;

            Timer.Update(_renderDeltaTime);

            Overlay.Draw(renderDevice);

            _frames++;
            _renderDeltaTime = (DateTime.Now - startTime).Milliseconds;
            if ((DateTime.Now - _lastFpsCheck).TotalMilliseconds >= 1000)
            {
                _lastFps = _frames;

                _frames = 0;
                _lastFpsCheck = DateTime.Now;
            }
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
            Debug.WriteLine("Keys: " + ev.Keys);
#if DEBUG
            // Exit program
            if (ev.Keys == Keys.Pause) Main.KeepRunning = false;
#endif
            if (ev.Keys == Overlay.ToggleKey)
            {
                Overlay.Enabled = !Overlay.Enabled;
                Main.KeyboardHook.BlockInput = Overlay.Enabled;
                Main.MouseHook.BlockInput = Overlay.Enabled;
                WinApi.ShowCursor(false);
                return;
            }

            if (Overlay.Enabled)
                Overlay.HandleKeyboardInput(ev);
            else
                Overlay.ListenKeyboardInput(ev);
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