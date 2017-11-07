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

namespace OsuBot.Payload
{
    public class HookBase : ElertanCheatBase.Payload.HookBase
    {
        private Thread _uiThread;

        public HookBase() : base("alavon")
        {
            
        }

        public override void Initialize(Process p)
        { 
            base.Initialize(p);

            OpenConsole();
            Console.WriteLine("Initialized");
            //_uiThread = new Thread(UIThread);
            //_uiThread.Start();
            new Thread(() =>
            {
                var signatureScanner = new Memory.SignatureScanner();
                var address = signatureScanner.Scan("DD 45 EC DD 1D", 5);
                var timeAddress = Memory.ReadIntPtr(address);
                while (true)
                {
                    var ingameTime = Memory.ReadDouble(timeAddress);
                    Console.WriteLine("Time: " + ingameTime);
                    Thread.Sleep(200);
                }
            }).Start();
        }

        [STAThread]
        private void UIThread()
        {
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new MainForm());
            //Application.Exit();
        }
    }
}
