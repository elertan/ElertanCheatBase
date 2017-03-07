using System;
using ElertanCheatBase.Payload;

namespace ElertanCheatBase.Tester
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // Setup cheatbase
            var cheatBase = new CheatBase("csgo")
            {
                InternalMode = true,
                InternalPayloadPath = Payload.Main.AssemblyPath,
                VisualRenderType = VisualRenderType.Direct3D9
            };
            // Run
            try
            {
                cheatBase.Run();
            }
            catch (InjectPayloadFailedException ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
    }
}