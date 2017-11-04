using System;
using SharpDX.Direct3D9;

namespace ElertanCheatBase.Payload.CommonCheats
{
    public class Cham
    {
        public bool Enabled { get; set; } = true;
        public bool VisibleThroughWalls { get; set; } = true;

        public virtual bool WillChamBeUsed(int numVertices, int primitiveCount)
        {
            throw new NotImplementedException("Will cham be used not implemented");
        }

        public virtual Texture DetermineTexture(Direct3D9ChamsController controller)
        {
            throw new NotImplementedException("Determine texture method is not set for cham");
        }
    }
}