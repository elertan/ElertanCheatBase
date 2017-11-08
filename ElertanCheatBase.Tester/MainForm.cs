using System;
using System.Windows.Forms;
using ElertanCheatBase.Csgo.Properties;
using ElertanCheatBase.Payload;

namespace ElertanCheatBase.Csgo
{
    public partial class MainForm : Form
    {
        private ProcessHelper _processHelper;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            _processHelper = new ProcessHelper("csgo");
            if (_processHelper.Process == null)
            {
                StatusLabel.Text = Resources.StatusText_WaitingForCsgo;
                _processHelper.ProcessStarted += ProcessHelper_ProcessStarted;
            }
            else StartPayloadInjection();
        }

        private void ProcessHelper_ProcessStarted(object sender, EventArgs e)
        {
            // Invoke on mainthread
            Invoke(new MethodInvoker(StartPayloadInjection));
        }

        private void StartPayloadInjection()
        {
            StatusLabel.Text = Resources.StatusText_Injecting;
            // Setup cheatbase
            var cheatBase = new CheatBase("csgo")
            {
                InternalMode = true,
                InternalPayloadPath = Payload.Main.AssemblyPath
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
            Close();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _processHelper?.Dispose();
        }

        private void StatusLabel_Click(object sender, EventArgs e)
        {

        }
    }
}
