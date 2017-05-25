using System;
using System.Collections.Generic;
using System.Linq;

namespace ElertanCheatBase.Payload.VisualOverlay
{
    public class Timer : IDisposable
    {
        private static readonly List<Timer> Timers = new List<Timer>();

        private int _currentTicks;

        public Timer()
        {
            Timers.Add(this);
        }

        public int Interval { get; set; }
        public bool IsRunning { get; private set; }

        public void Dispose()
        {
            Timers.Remove(this);
        }

        public static void Update(int tickAmount)
        {
            foreach (var timer in Timers.Where(t => t.IsRunning))
                timer.AddTicks(tickAmount);
        }

        private void AddTicks(int tickAmount)
        {
            _currentTicks += tickAmount;
            if (_currentTicks >= Interval)
            {
                _currentTicks = 0;
                OnTicked();
            }
        }

        public void Start()
        {
            IsRunning = true;
        }

        public void Stop()
        {
            IsRunning = false;
            _currentTicks = 0;
        }

        public void Reset()
        {
            _currentTicks = 0;
        }

        public event EventHandler Ticked;

        protected virtual void OnTicked()
        {
            Ticked?.Invoke(this, EventArgs.Empty);
        }
    }
}