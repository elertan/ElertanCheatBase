using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Mathematics.Interop;

namespace ElertanCheatBase.Payload.VisualOverlayItems
{
    public class Window
    {
        private bool _isFocused;
        public Size2 Size { get; set; } = new Size2(800, 600);
        public RawPoint Position { get; set; } = new RawPoint(0, 0);
    }
}
