using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ElertanCheatBase.Payload;
using Microsoft.Win32.SafeHandles;
using OsuBot.Payload.Models;

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

            OpenConsole();
            while (true)
            {
                Console.WriteLine(Game.SongTime);
                Thread.Sleep(50);
            }
        }
    }
}
