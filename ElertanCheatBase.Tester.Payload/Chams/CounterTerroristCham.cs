using ElertanCheatBase.Payload.CommonCheats;
using SharpDX.Direct3D9;

namespace ElertanCheatBase.Csgo.Payload.Chams
{
    public class CounterTerroristCham : Cham
    {
        public override bool WillChamBeUsed(int numVertices, int primitiveCount)
        {
            // LEGS BODY NOT WORKING EITHER
            return numVertices == 1986 && primitiveCount == 3044 || numVertices == 1889 && primitiveCount == 3088 ||
                   numVertices == 2138 && primitiveCount == 3424 || numVertices == 2531 && primitiveCount == 3888 ||
                   numVertices == 2416 && primitiveCount == 3778 || numVertices == 3162 && primitiveCount == 5182 ||
                   numVertices == 3816 && primitiveCount == 5930 || numVertices == 2587 && primitiveCount == 4334 ||
                   numVertices == 3606 && primitiveCount == 5856 || numVertices == 1372 && primitiveCount == 2286 ||
                   numVertices == 1310 && primitiveCount == 2302 || numVertices == 1352 && primitiveCount == 2268 ||
                   numVertices == 1134 && primitiveCount == 2024 || numVertices == 1430 && primitiveCount == 2422;
        }

        public override Texture DetermineTexture(Direct3D9ChamsController controller)
        {
            return controller.ResolveTexture("blue");
        }
    }
}