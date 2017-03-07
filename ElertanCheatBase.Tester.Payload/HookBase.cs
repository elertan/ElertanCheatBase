using System.Diagnostics;
using System.IO;
using SharpDX.Direct3D9;

//#define ctLegs  ((NumVertices == 2118 && PrimitiveCount == 3354))
//#define ctBody  ((NumVertices == 4523 && PrimitiveCount == 6700 ))
//#define ctHead  ((NumVertices == 1677 && PrimitiveCount == 2713)||(NumVertices == 1761 && PrimitiveCount == 2681))

//#define tLegs  ((NumVertices == 1986 && PrimitiveCount == 3044)||(NumVertices == 1889 && PrimitiveCount == 3088)||(NumVertices == 2138 && PrimitiveCount == 3424)||(NumVertices == 2531 && PrimitiveCount == 3888)||(NumVertices == 2416 && PrimitiveCount == 3778))
//#define tBody  ((NumVertices == 3162 && PrimitiveCount == 5182)||(NumVertices == 3816 && PrimitiveCount == 5930)||(NumVertices == 2587 && PrimitiveCount == 4334)||(NumVertices == 3606 && PrimitiveCount == 5856))
//#define tHead  ((NumVertices == 1372 && PrimitiveCount == 2286)||(NumVertices == 1310 && PrimitiveCount == 2302)||(NumVertices == 1352 && PrimitiveCount == 2268)||(NumVertices == 1134 && PrimitiveCount == 2024)||(NumVertices == 1430 && PrimitiveCount == 2422))

//#define CounterTerrorist ((ctLegs)||(ctBody)||(ctHead))
//#define Terrorist ((tLegs)||(tBody)||(tHead))

namespace ElertanCheatBase.Tester.Payload
{
    public class HookBase : ElertanCheatBase.Payload.HookBase
    {
        private readonly byte[] bBlue =
        {
            0x42, 0x4D, 0x3C, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x36, 0x00, 0x00, 0x00, 0x28, 0x00, 0x00, 0x00,
            0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01,
            0x00, 0x20, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x12, 0x0B, 0x00, 0x00, 0x12, 0x0B, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0xFF, 0x00, 0x00, 0x00, 0x00, 0x00
        };

        private readonly byte[] bRed =
        {
            0x42, 0x4D, 0x3A, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x36, 0x00, 0x00, 0x00, 0x28, 0x00,
            0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x18, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xFF, 0x00
        };

        private Texture _blueTexture;


        private Texture _redTexture;

        public override void Initialize(Process p)
        {
            base.Initialize(p);
        }

        public override void Direct3D9_EndScene(Device device)
        {
            if (_redTexture == null) _redTexture = Texture.FromStream(device, new MemoryStream(bRed));
            if (_blueTexture == null) _blueTexture = Texture.FromStream(device, new MemoryStream(bBlue));
        }

        public override void Direct3D9_DrawIndexedPrimitive(Device device, PrimitiveType primitiveType,
            int baseVertexIndex, int minVertexIndex,
            int numVertices, int startIndex, int primCount)
        {
            if (IsTerrorist(numVertices, primCount))
            {
                device.SetRenderState(RenderState.ZEnable, false);
                device.SetRenderState(RenderState.FillMode, 3);
                device.SetTexture(0, _redTexture);
                device.DrawIndexedPrimitive(primitiveType, baseVertexIndex, minVertexIndex, numVertices, startIndex,
                    primCount);
                device.SetRenderState(RenderState.ZEnable, true);
                //device.SetRenderState(RenderState.FillMode, 3);
                //device.SetTexture(0, _redTexture);
            }
            if (IsCounterTerrorist(numVertices, primCount))
            {
                device.SetRenderState(RenderState.ZEnable, false);
                device.SetRenderState(RenderState.FillMode, 3);
                device.SetTexture(0, _blueTexture);
                device.DrawIndexedPrimitive(primitiveType, baseVertexIndex, minVertexIndex, numVertices, startIndex,
                    primCount);
                device.SetRenderState(RenderState.ZEnable, true);
                //device.SetRenderState(RenderState.FillMode, 3);
                //device.SetTexture(0, _redTexture);
            }
        }

        /*
         #define ctLegs  ((NumVertices == 2118 && PrimitiveCount == 3354))
#define ctBody  ((NumVertices == 4523 && PrimitiveCount == 6700 ))
#define ctHead  ((NumVertices == 1677 && PrimitiveCount == 2713)||(NumVertices == 1761 && PrimitiveCount == 2681))

#define tLegs  ((NumVertices == 1986 && PrimitiveCount == 3044)||(NumVertices == 1889 && PrimitiveCount == 3088)||(NumVertices == 2138 && PrimitiveCount == 3424)||(NumVertices == 2531 && PrimitiveCount == 3888)||(NumVertices == 2416 && PrimitiveCount == 3778))
#define tBody  ((NumVertices == 3162 && PrimitiveCount == 5182)||(NumVertices == 3816 && PrimitiveCount == 5930)||(NumVertices == 2587 && PrimitiveCount == 4334)||(NumVertices == 3606 && PrimitiveCount == 5856))
#define tHead  ((NumVertices == 1372 && PrimitiveCount == 2286)||(NumVertices == 1310 && PrimitiveCount == 2302)||(NumVertices == 1352 && PrimitiveCount == 2268)||(NumVertices == 1134 && PrimitiveCount == 2024)||(NumVertices == 1430 && PrimitiveCount == 2422))

#define CounterTerrorist ((ctLegs)||(ctBody)||(ctHead))
#define Terrorist ((tLegs)||(tBody)||(tHead))

#define Bomb (NumVertices == 4329 && PrimitiveCount == 3779)
             */

        private bool IsTerrorist(int NumVertices, int PrimitiveCount)
        {
            return NumVertices == 2118 && PrimitiveCount == 3354 || // LEGS
                   NumVertices == 4523 && PrimitiveCount == 6700 || // BODY (NOT WORKING)
                   NumVertices == 1677 && PrimitiveCount == 2713 || NumVertices == 1761 && PrimitiveCount == 2681;
            // HEAD
        }

        private bool IsCounterTerrorist(int NumVertices, int PrimitiveCount)
        {
            // LEGS BODY NOT WORKING EITHER
            return NumVertices == 1986 && PrimitiveCount == 3044 || NumVertices == 1889 && PrimitiveCount == 3088 ||
                   NumVertices == 2138 && PrimitiveCount == 3424 || NumVertices == 2531 && PrimitiveCount == 3888 ||
                   NumVertices == 2416 && PrimitiveCount == 3778 || NumVertices == 3162 && PrimitiveCount == 5182 ||
                   NumVertices == 3816 && PrimitiveCount == 5930 || NumVertices == 2587 && PrimitiveCount == 4334 ||
                   NumVertices == 3606 && PrimitiveCount == 5856 || NumVertices == 1372 && PrimitiveCount == 2286 ||
                   NumVertices == 1310 && PrimitiveCount == 2302 || NumVertices == 1352 && PrimitiveCount == 2268 ||
                   NumVertices == 1134 && PrimitiveCount == 2024 || NumVertices == 1430 && PrimitiveCount == 2422;
        }
    }
}