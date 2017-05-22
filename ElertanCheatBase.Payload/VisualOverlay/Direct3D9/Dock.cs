using SharpDX;
using SharpDX.Direct3D9;
using SharpDX.Mathematics.Interop;

namespace ElertanCheatBase.Payload.VisualOverlay.Direct3D9
{
    public class Dock
    {
        public RawRectangle Area { get; set; }
        public RawColorBGRA BaseColor { get; set; } = new RawColorBGRA(100, 20, 20, 255);

        public void Draw(Device device)
        {
            device.DrawRectangle(new RawPoint(Area.Left, Area.Top),
                new Size2(Area.Right, Area.Bottom),
                BaseColor);
            device.DrawText("Start", 21, new RawPoint(Area.Left + 20, Area.Top + 10),
                new RawColorBGRA(255, 255, 255, 255));
        }
    }
}