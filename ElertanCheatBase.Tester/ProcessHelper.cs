using System;
using System.Diagnostics;
using System.Linq;
using System.Management;

namespace ElertanCheatBase.Csgo
{
    class ProcessHelper : IDisposable
    {
        private readonly string _processName;
        public Process Process;
        private ManagementEventWatcher _processStartWatcher;

        public event EventHandler ProcessStarted;

        ~ProcessHelper()
        {
            Dispose(false);
        }

        public ProcessHelper(int pId) : this(Process.GetProcessById(pId))
        {
        }

        public ProcessHelper(Process process)
        {
            Process = process;
            _processName = process.ProcessName;
        }

        public ProcessHelper(string processName)
        {
            // Add .exe if not given
            if (processName.Contains(".exe")) processName = processName.Replace(".exe", "");
            _processName = processName;
            if (Process.GetProcessesByName(processName).Any())
            {
                Process = Process.GetProcessesByName(processName).First();
            }
            else
            {
                _processStartWatcher = WatchForProcessStart(processName);
            }
        }

        private ManagementEventWatcher WatchForProcessStart(string processName)
        {
            string queryString =
                "SELECT TargetInstance" +
                "  FROM __InstanceCreationEvent " +
                "WITHIN  10 " +
                " WHERE TargetInstance ISA 'Win32_Process' " +
                "   AND TargetInstance.Name = '" + processName + ".exe'";

            // The dot in the scope means use the current machine
            string scope = @"\\.\root\CIMV2";

            // Create a watcher and listen for events
            ManagementEventWatcher watcher = new ManagementEventWatcher(scope, queryString);
            watcher.EventArrived += WatcherProcessStarted;
            watcher.Start();
            return watcher;
        }

        private void WatcherProcessStarted(object sender, EventArrivedEventArgs e)
        {
            Process = Process.GetProcessesByName(_processName).First();
            OnProcessStarted(EventArgs.Empty);
        }

        protected virtual void OnProcessStarted(EventArgs e)
        {
            ProcessStarted?.Invoke(this, e);
        }

        private void ReleaseUnmanagedResources()
        {
            // TODO release unmanaged resources here
        }

        private void Dispose(bool disposing)
        {
            ReleaseUnmanagedResources();
            if (disposing)
            {
                Process?.Dispose();
                _processStartWatcher?.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
