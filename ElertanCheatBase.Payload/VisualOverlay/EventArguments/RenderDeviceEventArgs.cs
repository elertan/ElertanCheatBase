using System;
using ElertanCheatBase.Payload.Rendering;

namespace ElertanCheatBase.Payload.VisualOverlay.EventArguments
{
    public class RenderDeviceEventArgs : EventArgs
    {
        public IRenderDevice RenderDevice { get; set; }
    }
}