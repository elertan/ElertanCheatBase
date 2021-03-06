﻿using System.Collections.Generic;
using ElertanCheatBase.Payload.VisualOverlayItems;
using SharpDX.Direct3D9;
using SharpDX.Mathematics.Interop;

namespace ElertanCheatBase.Payload
{
    public static class VisualOverlay
    {
        public static bool OverlayVisible { get; set; } = true;
        public static List<Window> Windows { get; set; } = new List<Window>();

        public static void Draw(Device device)
        {
            if (!OverlayVisible) return;

            foreach (var window in Windows)
                device.DrawRectangle(window.Position, window.Size, new RawColorBGRA(100, 100, 100, 200));
        }
    }
}