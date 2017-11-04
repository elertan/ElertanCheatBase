namespace ElertanCheatBase.Csgo
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.MarqueeProgressBar = new System.Windows.Forms.ProgressBar();
            this.StatusLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // MarqueeProgressBar
            // 
            this.MarqueeProgressBar.Location = new System.Drawing.Point(24, 23);
            this.MarqueeProgressBar.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.MarqueeProgressBar.MarqueeAnimationSpeed = 25;
            this.MarqueeProgressBar.Name = "MarqueeProgressBar";
            this.MarqueeProgressBar.Size = new System.Drawing.Size(520, 44);
            this.MarqueeProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.MarqueeProgressBar.TabIndex = 0;
            // 
            // StatusLabel
            // 
            this.StatusLabel.Font = new System.Drawing.Font("Comic Sans MS", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StatusLabel.Location = new System.Drawing.Point(0, 83);
            this.StatusLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(568, 44);
            this.StatusLabel.TabIndex = 1;
            this.StatusLabel.Text = "Status";
            this.StatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.StatusLabel.Click += new System.EventHandler(this.StatusLabel_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(568, 140);
            this.Controls.Add(this.StatusLabel);
            this.Controls.Add(this.MarqueeProgressBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Elertan CSGO";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ProgressBar MarqueeProgressBar;
        private System.Windows.Forms.Label StatusLabel;
    }
}

