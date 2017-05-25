using System;
using System.Diagnostics;
using System.Drawing;
using ElertanCheatBase.Payload.VisualOverlay.Interactables;

namespace ElertanCheatBase.Payload.VisualOverlay.Applications.Terminal.Windows
{
    public class MainWindow : Window
    {
        public MainWindow(Application app) : base(app)
        {
            Size = new Size(700, 300);

            var button = new Button();
            button.TextLabel.Text = "Click Me!";
            button.Size = new Size(120, button.Size.Height);
            button.Position = new Point(Size.Width - button.Size.Width - 20, Size.Height - button.Size.Height - 20);
            button.Clicked += Button_Clicked;
            Controls.Add(button);
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            Debug.WriteLine("Button clicked!");
        }
    }
}