using System;
using System.Drawing;
using System.Windows.Forms;
using ElertanCheatBase.Payload.Rendering;

namespace ElertanCheatBase.Payload.VisualOverlay.Interactables
{
    public class Textbox : Control
    {
        private readonly Timer _blinkTimer = new Timer();
        private readonly Label _label = new Label();
        private bool _shouldBlink;
        private string _text;

        public Textbox()
        {
            _label.FontWeight = FontWeight.Thin;
            Controls.Add(_label);
            Text = "";

            Activatable = true;
            BecameInactive += Textbox_BecameInactive;
            BecameActive += Textbox_BecameActive;
            SizeChanged += Textbox_SizeChanged;
            KeyDown += Textbox_KeyDown;

            _blinkTimer.Interval = 750;
            _blinkTimer.Ticked += _blinkTimer_Ticked;
        }

        public Color BackgroundColor { get; set; } = Color.WhiteSmoke;
        public Color BorderColor { get; set; } = Color.Black;
        public Color TextColor { get; set; } = Color.Black;
        public Color PlaceholderColor { get; set; } = Color.Gray;
        public int TextBlinkDelay { get; set; } = 1000;

        public string Placeholder { get; set; } = "Enter some text here";

        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                UpdateLabel();
            }
        }

        private void UpdateLabel()
        {
            if (Text == "" && !Active)
            {
                _label.Text = Placeholder;
                _label.TextColor = PlaceholderColor;
            }
            else
            {
                _label.Text = Text;
                _label.TextColor = TextColor;
            }
        }

        private void Textbox_BecameActive(object sender, EventArgs e)
        {
            _blinkTimer.Start();
            _shouldBlink = true;
        }

        private void Textbox_BecameInactive(object sender, EventArgs e)
        {
            _blinkTimer.Stop();
            _shouldBlink = false;
        }

        private void _blinkTimer_Ticked(object sender, EventArgs e)
        {
            _shouldBlink = !_shouldBlink;
        }

        private void Textbox_KeyDown(object sender, ControlKeyDownEventArgs e)
        {
            if (e.Keys == Keys.Back)
            {
                if (Text.Length > 0) Text = Text.Substring(0, Text.Length - 1);
                return;
            }
            var key = (char) e.Keys;
            Text += key.ToString().ToLower();
        }

        public event EventHandler TextChanged;

        private void Textbox_SizeChanged(object sender, EventArgs e)
        {
            _label.Position = new Point(4, 0);
            _label.Size = new Size(Size.Width - 8, Size.Height);
        }

        protected virtual void OnTextChanged()
        {
            _blinkTimer.Reset();
            _shouldBlink = true;

            TextChanged?.Invoke(this, EventArgs.Empty);
        }

        public override void Draw(IRenderDevice device)
        {
            UpdateLabel();
            device.DrawRectangle(Point.Empty, Size, BorderColor);
            device.DrawRectangle(new Point(1, 1), new Size(Size.Width - 2, Size.Height - 2), BackgroundColor);

            if (Active && _shouldBlink) _label.AdditionalCharactersToAppend += "|";
            base.Draw(device);
            _label.AdditionalCharactersToAppend = "";
        }
    }
}