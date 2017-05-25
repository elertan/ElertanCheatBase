using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using ElertanCheatBase.Payload.Rendering;
using ElertanCheatBase.Payload.VisualOverlay.EventArguments;
using ElertanCheatBase.Payload.VisualOverlay.Interactables;

namespace ElertanCheatBase.Payload.VisualOverlay.Applications.Dock.Windows
{
    internal class MainWindow : Window
    {
        private readonly App _app;
        private readonly Timer _updateTimer = new Timer();
        private readonly List<Button> _windowButtons = new List<Button>();
        public Button HomeButton = new Button();

        public MainWindow(Application app) : base(app)
        {
            _app = (App) app;
            BorderStyle = WindowBorderStyle.None;
            Load += MainWindow_Load;
        }

        public Color BaseColor { get; set; } = Color.FromArgb(255, 20, 20, 100);

        private void MainWindow_Load(object sender, RenderDeviceEventArgs e)
        {
            Size = new Size(e.RenderDevice.Viewport.Width, 50);
            Position = new Point(0, e.RenderDevice.Viewport.Height - Size.Height - 70);

            HomeButton.TextLabel.Text = "Home";
            var dimensions = Size.Height - 6;
            HomeButton.Size = new Size(dimensions * 2, dimensions);
            HomeButton.Position = new Point(3, 3);
            HomeButton.Clicked += HomeButton_Clicked;

            Controls.Add(HomeButton);

            //_updateTimer.Interval = 100;
            //_updateTimer.Ticked += _updateTimer_Ticked;
            //_updateTimer.Start();

            UpdateDock();
        }

        private void _updateTimer_Ticked(object sender, EventArgs e)
        {
            UpdateDock();
        }

        private void UpdateDock()
        {
            foreach (var windowButton in _windowButtons)
                Controls.Remove(windowButton);

            _windowButtons.Clear();
            foreach (var app in _app.AppManager.RunningApplications.Where(app => app != _app))
            {
                var i = 0;
                foreach (var window in app.Windows)
                {
                    var button = new Button();
                    button.TextLabel.Text = window.Title;
                    var dimensions = Size.Height - 6;
                    button.Size = new Size(dimensions * 2, dimensions);
                    button.Position = new Point(3 + button.Size.Width + 10 + i * button.Size.Width + 5, 3);
                    i++;
                    _windowButtons.Add(button);
                }
            }

            foreach (var windowButton in _windowButtons)
                Controls.Add(windowButton);
        }

        private void HomeButton_Clicked(object sender, EventArgs e)
        {
            DebugTool.App.Logs.Add("Home button pressed");
        }

        public override void Draw(IRenderDevice device)
        {
            base.Draw(device);
        }
    }
}