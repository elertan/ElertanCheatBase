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
                var songTime = TimeSpan.FromMilliseconds(Game.SongTime);
                
                Console.WriteLine("Hi! This bot only shows time as of now.");
                Console.WriteLine($"Current song time: {songTime.ToString()}");
                Thread.Sleep(500);
                Console.Clear();
            }
        }
    }
}
