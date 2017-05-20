using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SharpDX.Direct3D9;

namespace ElertanCheatBase.Payload.CommonCheats
{
    public class ChamsController
    {
        private readonly Dictionary<string, byte[]> _colorBuffersForTextures = new Dictionary<string, byte[]>();
        public bool Enabled { get; set; } = true;
        private Dictionary<string, Texture> Textures { get; } = new Dictionary<string, Texture>();
        public List<Cham> Chams { get; set; } = new List<Cham>();

        public void CreateTexture(string key, byte[] colorBuffer)
        {
            _colorBuffersForTextures.Add(key, colorBuffer);
        }

        public Texture ResolveTexture(string key)
        {
            if (!Textures.ContainsKey(key))
                throw new Exception($"Texture with name '{key}' not created in chamscontroller");
            return Textures[key];
        }

        public void Direct3D9_EndScene(Device device)
        {
            if (!Enabled) return;

            foreach (var pair in _colorBuffersForTextures)
            {
                var texture = Texture.FromStream(device, new MemoryStream(pair.Value));
                Textures.Add(pair.Key, texture);
            }
            if (_colorBuffersForTextures.Any()) _colorBuffersForTextures.Clear();
        }

        public void Direct3D9_DrawIndexedPrimitive(Device device,
            PrimitiveType primitiveType,
            int baseVertexIndex,
            int minVertexIndex,
            int numVertices,
            int startIndex,
            int primCount)
        {
            if (!Enabled) return;

            foreach (var cham in Chams)
            {
                if (!cham.Enabled || !cham.WillChamBeUsed(numVertices, primCount)) continue;

                if (cham.VisibleThroughWalls) device.SetRenderState(RenderState.ZEnable, false);
                device.SetRenderState(RenderState.FillMode, 3);
                device.SetTexture(0, cham.DetermineTexture(this));
                device.DrawIndexedPrimitive(primitiveType, baseVertexIndex, minVertexIndex, numVertices, startIndex,
                    primCount);
                if (cham.VisibleThroughWalls) device.SetRenderState(RenderState.ZEnable, true);
            }
        }
    }
}