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
    }
}