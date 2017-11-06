using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

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

            _uiThread = new Thread(UIThread);
            _uiThread.Start();
        }

        [STAThread]
        private void UIThread()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
            Application.Exit();
        }
    }
}
