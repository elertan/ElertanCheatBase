using System;
using System.Diagnostics;
using ElertanCheatBase.Payload.CommonCheats;
using SharpDX.Direct3D9;

namespace ElertanCheatBase.Payload
{
    public class HookBase
    {
        public HookBase(string secretKey)
        {
            if (secretKey != "alavon") throw new Exception("->PrivateAssembly<-");
            Main.KeepRunning = false;
        }

        public Direct3D9ChamsController Direct3D9ChamsController { get; set; }
        public Process Process { get; private set; }

        public void Exit()
        {
            Main.KeepRunning = false;
        }

        public virtual void Initialize(Process p)
        {
            Process = p;

            if (Core.VisualRenderType == VisualRenderType.Direct3D9)
            {
                Direct3D9ChamsController = new Direct3D9ChamsController();
            }
        }

        public virtual void Direct3D9_EndScene(Device device)
        {
            Direct3D9ChamsController.Direct3D9_EndScene(device);
        }

        public virtual void Direct3D9_DrawIndexedPrimitive(Device device,
            PrimitiveType primitiveType,
            int baseVertexIndex,
            int minVertexIndex,
            int numVertices,
            int startIndex,
            int primCount)
        {
            Direct3D9ChamsController.Direct3D9_DrawIndexedPrimitive(device, primitiveType, baseVertexIndex, minVertexIndex,
                numVertices,
                startIndex, primCount);
        }

        public virtual void OpenGL_GlBegin(OpenGL.DeviceContext ctx, OpenGL.PrimitiveType mode)
        {
            
        }
    }
}