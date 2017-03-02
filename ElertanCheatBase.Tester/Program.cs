using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElertanCheatBase.Payload;

namespace ElertanCheatBase.Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            // Setup cheatbase
            var cheatBase = new CheatBase("csgo")
            {
                InternalMode = true,
                InternalPayloadPath = Payload.Main.AssemblyPath,
                VisualRenderType = VisualRenderType.Direct3D9
            };
            // Run
            cheatBase.Run();
        }
    }
}
