﻿using ElertanCheatBase.Payload.CommonCheats;
using SharpDX.Direct3D9;

namespace ElertanCheatBase.Csgo.Payload.Chams
{
    public class TerroristCham : ICham
    {
        public bool Enabled { get; set; }
        public bool VisibleThroughWalls { get; set; }

        public bool WillChamBeUsed(int numVertices, int primitiveCount)
        {
            return numVertices == 2118 && primitiveCount == 3354 || // LEGS
                   numVertices == 4523 && primitiveCount == 6700 || // BODY (NOT WORKING)
                   numVertices == 1677 && primitiveCount == 2713 || numVertices == 1761 && primitiveCount == 2681;
        }

        public Texture DetermineTexture(Direct3D9ChamsController controller)
        {
            return controller.ResolveTexture("red");
        }
    }
}