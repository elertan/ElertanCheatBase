using System;
using SharpDX.Direct3D9;

namespace ElertanCheatBase.Payload.CommonCheats
{
    public interface ICham
    {
        bool Enabled { get; set; }
        bool VisibleThroughWalls { get; set; }

        bool WillChamBeUsed(int numVertices, int primitiveCount);
        //{
        //    throw new NotImplementedException("Will cham be used not implemented");
        //}

        Texture DetermineTexture(Direct3D9ChamsController controller);
        //{
        //    throw new NotImplementedException("Determine texture method is not set for cham");
        //}
    }
}