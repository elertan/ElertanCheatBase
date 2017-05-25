using System;
using System.Drawing;
using ElertanCheatBase.Payload.VisualOverlay.Interactables;

namespace ElertanCheatBase.Payload.VisualOverlay.Applications.DebugTool.Windows
{
    internal class MainWindow : Window
    {
        public MainWindow(Application app) : base(app)
        {
            LogListBox.Items = App.Logs;
            LogListBox.Position = new Point(20, 20 + BarHeight);
            LogListBox.Size = new Size(Size.Width - 40, Size.Height - 40 - 55 - BarHeight);
            Controls.Add(LogListBox);

            ExitButton.Size = new Size(120, 30);
            ExitButton.Position = new Point(Size.Width - ExitButton.Size.Width - 20,
                Size.Height - ExitButton.Size.Height - 20);
            ExitButton.TextLabel.Text = "Exit Cheat";
            ExitButton.Clicked += ExitButton_Clicked;
            Controls.Add(ExitButton);

            ClearButton.Size = new Size(120, 30);
            ClearButton.Position = new Point(20, Size.Height - ClearButton.Size.Height - 20);
            ClearButton.TextLabel.Text = "Clear Logs";
            ClearButton.Clicked += ClearButton_Clicked;
            Controls.Add(ClearButton);
        }

        public Button ClearButton { get; set; } = new Button();
        public Button ExitButton { get; set; } = new Button();
        public ListBox LogListBox { get; set; } = new ListBox();

        private void ClearButton_Clicked(object sender, EventArgs e)
        {
            App.Logs.Clear();
        }

        private void ExitButton_Clicked(object sender, EventArgs e)
        {
            Main.KeepRunning = false;
        }
    }
}