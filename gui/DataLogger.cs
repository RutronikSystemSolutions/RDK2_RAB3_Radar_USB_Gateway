using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDK2_Radar_SignalProcessing_GUI
{
    public class DataLogger
    {
        #region "Events"

        public delegate void OnNewLoggerStateEventHandler(object sender, bool startedFlag);
        public event OnNewLoggerStateEventHandler? OnNewLoggerState;

        #endregion

        #region "Members"

        /// <summary>
        /// Background worker enabling background operations
        /// </summary>
        private BackgroundWorker? worker;

        /// <summary>
        /// True if logging the data into csv file
        /// </summary>
        private bool logging = false;

        /// <summary>
        /// Path to the CSV file
        /// </summary>
        private string? path;

        /// <summary>
        /// Used for synchronisation between UI and logging thread
        /// </summary>
        private object sync = new object();

        /// <summary>
        /// List that has to be stored into file
        /// </summary>
        private List<ushort[]> toStore = new List<ushort[]>();

        #endregion

        /// <summary>
        /// Start to log the data
        /// </summary>
        /// <param name="path">Path on the CSV file where the data will be stored</param>
        public void Start(string path)
        {
            if (logging) return;

            this.path = path;

            CreateBackgroundWorkerAndStart();

            logging = true;

            OnNewLoggerState?.Invoke(this, true);
        }

        /// <summary>
        /// Stop to log the data
        /// </summary>
        public void Stop()
        {
            if (this.worker == null) return;

            this.worker.CancelAsync();
            this.worker.DoWork -= Worker_DoWork;

            logging = false;

            OnNewLoggerState?.Invoke(this, false);
        }

        /// <summary>
        /// Add a data to the log file
        /// </summary>
        /// <param name="frame"></param>
        public void Log(ushort[] frame)
        {
            if (logging == false) return;

            lock(sync)
            {
                toStore.Add(frame);
            }
        }

        private void CreateBackgroundWorkerAndStart()
        {
            if (this.worker != null)
            {
                Stop();
            }

            this.worker = new BackgroundWorker();
            this.worker.WorkerReportsProgress = true;
            this.worker.WorkerSupportsCancellation = true;
            this.worker.DoWork += Worker_DoWork;
            this.worker.RunWorkerAsync();
        }

        private void Worker_DoWork(object? sender, DoWorkEventArgs e)
        {
            if (sender == null) return;
            if (path == null) return;

            BackgroundWorker? worker = (BackgroundWorker)sender;
            if (worker == null) return;

            TextWriter writer = new StreamWriter(this.path);

            for(; ;)
            {
                if (worker.CancellationPending)
                {
                    lock(sync)
                    {
                        toStore.Clear();
                    }
                    writer.Close();
                    return;
                }

                ushort[]? frame = null;
                lock(sync)
                {
                    if (toStore.Count > 0)
                    {
                        frame = toStore[0];
                        toStore.RemoveAt(0);
                    }
                }

                if (frame == null)
                {
                    System.Threading.Thread.Sleep(50);
                }
                else
                {
                    // Frame not null, write to CSV
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < frame.Length; i++)
                    {
                        sb.Append(string.Format("{0};", frame[i]));
                    }
                    writer.WriteLine(sb.ToString());
                }
            }
        }
    }
}
