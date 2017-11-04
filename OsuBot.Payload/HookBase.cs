using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsuBot.Payload
{
    public class HookBase : ElertanCheatBase.Payload.HookBase
    {
        public HookBase() : base("alavon")
        {
            
        }

        public override void Initialize(Process p)
        {
            base.Initialize(p);

            Debug.WriteLine($"Hey there! I'm inside of {p.ProcessName}");
        }
    }
}
