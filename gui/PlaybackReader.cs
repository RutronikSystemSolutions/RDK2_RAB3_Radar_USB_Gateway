using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDK2_Radar_SignalProcessing_GUI
{
    public class PlaybackReader
    {
        public delegate void OnNewFrameEventHandler(object sender, ushort[] frame);
        public event OnNewFrameEventHandler? OnNewFrameEvent;

        /// <summary>
        /// Background worker enabling background operations
        /// </summary>
        private BackgroundWorker? worker;

        /// <summary>
        /// Store the path to the file to be read
        /// </summary>
        private string? path;

        public void Start(string path)
        {
            this.path = path;
            CreateBackgroundWorkerAndStart();
        }

        public void Stop()
        {
            if (this.worker == null) return;

            this.worker.CancelAsync();
            this.worker.DoWork -= Worker_DoWork;
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
            this.worker.ProgressChanged += Worker_ProgressChanged;
            this.worker.RunWorkerAsync();
        }

        private void Worker_ProgressChanged(object? sender, ProgressChangedEventArgs e)
        {
            if (e.UserState != null)
            {
                OnNewFrameEvent?.Invoke(this, (ushort[])e.UserState);
            }
        }

        private void Worker_DoWork(object? sender, DoWorkEventArgs e)
        {
            if (sender == null) return;
            if (path == null) return;

            BackgroundWorker worker = (BackgroundWorker)sender;

            bool firstLine = true;
            const Int32 BufferSize = 128;
            using (var fileStream = File.OpenRead(path))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
            {
                String? line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    if (firstLine)
                    {
                        firstLine = false;
                        continue;
                    }
                    // Process line
                    string[] content = line.Split(';');
                    if (content.Length == 0) return;

                    ushort[] frame = new ushort[content.Length - 1];

                    for(int i = 0; i < frame.Length; ++i)
                    {
                        frame[i] = Convert.ToUInt16(content[i]);
                    }

                    worker.ReportProgress(0, frame);

                    // Wait 33ms (30 Hz)
                    System.Threading.Thread.Sleep(33);
                }
            }
        }
    }
}
