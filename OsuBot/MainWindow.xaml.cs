using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ElertanCheatBase;
using ElertanCheatBase.Payload;

namespace OsuBot
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ProcessHelper _processHelper;

        public MainWindow()
        {
            InitializeComponent();
        }

        ~MainWindow()
        {
            _processHelper?.Dispose();
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            _processHelper = new ProcessHelper("osu!");
            if (_processHelper.Process == null)
            {
                _processHelper.ProcessStarted += _processHelper_ProcessStarted;
            }
            else
            {
                // Osu is already running
                StartPayloadInjection();
            }
        }

        private void _processHelper_ProcessStarted(object sender, EventArgs e)
        {
            // Osu started running
            StartPayloadInjection();
        }

        private void StartPayloadInjection()
        {
            // Setup cheatbase
            var cheatBase = new CheatBase("osu!")
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
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
            Dispatcher.Invoke(Close);
        }
    }
}
