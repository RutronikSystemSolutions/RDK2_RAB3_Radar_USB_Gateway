using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDK2_Radar_SignalProcessing_GUI
{
    public class RDK2
    {
        public enum ConnectionState
        {
            Iddle,
            Connected,
            Error
        }

        #region "Constants"

        private const int WorkerReportFrame = 1;

        #endregion

        #region "Events"

        public delegate void OnNewConnectionStateEventHandler(object sender, ConnectionState state);
        public event OnNewConnectionStateEventHandler? OnNewConnectionState;

        public delegate void OnNewFrameEventHandler(object sender, ushort[] frame);
        public event OnNewFrameEventHandler? OnNewFrame;

        #endregion

        #region "Members"

        /// <summary>
        /// Serial port used for the communication
        /// </summary>
        private SerialPort port = null;

        /// <summary>
        /// Background worker enabling background operations
        /// </summary>
        private BackgroundWorker worker;

        /// <summary>
        /// Object used for synchronizsation purpose
        /// </summary>
        private object sync = new object();

        #endregion

        /// <summary>
        /// Set the serial port name
        /// </summary>
        /// <param name="portName"></param>
        public void SetPortName(string portName)
        {
            try
            {
                port = new SerialPort
                {
                    BaudRate = 921600,
                    DataBits = 8,
                    Handshake = Handshake.None,
                    Parity = Parity.None,
                    PortName = portName,
                    StopBits = StopBits.One,
                    ReadTimeout = 500,
                    WriteTimeout = 2000
                };
                port.Open();
            }
            catch (Exception)
            {
                OnNewConnectionState?.Invoke(this, ConnectionState.Error);
                return;
            }

            OnNewConnectionState?.Invoke(this, ConnectionState.Connected);

            CreateBackgroundWorker();
            worker.RunWorkerAsync();
        }

        private void CreateBackgroundWorker()
        {
            if (this.worker != null)
            {
                this.worker.CancelAsync();
            }

            this.worker = new BackgroundWorker();
            this.worker.WorkerReportsProgress = true;
            this.worker.WorkerSupportsCancellation = true;
            this.worker.DoWork += Worker_DoWork;
            this.worker.ProgressChanged += Worker_ProgressChanged;
            this.worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
        }

        private void Worker_DoWork(object? sender, DoWorkEventArgs e)
        {
            if (sender == null) return;
            BackgroundWorker worker = (BackgroundWorker)sender;
            port.ReadExisting();

            int samplesPerChirp = RadarConfiguration.SAMPLES_PER_CHIRP;
            int chirpsPerFrame = RadarConfiguration.CHIRPS_PER_FRAME;
            int antennaCount = RadarConfiguration.ANTENNA_COUNT;

            int samplesPerFrame = samplesPerChirp * chirpsPerFrame * antennaCount;
            int bytesPerFrame = samplesPerFrame * 2;

            byte[] readBuffer = new byte[bytesPerFrame];

            int offset = 0;
            int remaining = bytesPerFrame;

            var watch = System.Diagnostics.Stopwatch.StartNew();

            for (; ;)
            {
                int available = port.BytesToRead;
                if (available > 0)
                {
                    if (remaining == bytesPerFrame)
                    {
                        watch.Restart();
                    }
                    int toRead = (available > remaining) ? remaining: available;
                    port.Read(readBuffer, offset, toRead);

                    offset += toRead;
                    remaining -= toRead;
                }

                if (remaining == 0)
                {
                    // Frame is available
                    var samples = new ushort[samplesPerFrame];
                    for (int i = 0; i < samplesPerFrame; i++)
                    {
                        samples[i] = BitConverter.ToUInt16(readBuffer, i * 2);
                    }

                    worker.ReportProgress(WorkerReportFrame, samples);

                    offset = 0;
                    remaining = bytesPerFrame;
                }

                if ((watch.Elapsed.TotalMilliseconds > 500) && (offset != 0))
                {
                    // Problem...
                    System.Diagnostics.Debug.WriteLine("Timeout 500,s");
                    offset = 0;
                    remaining = bytesPerFrame;
                }

                System.Threading.Thread.Sleep(1);
            }
        }

        private void Worker_ProgressChanged(object? sender, ProgressChangedEventArgs e)
        {
            switch(e.ProgressPercentage)
            {
                case WorkerReportFrame:
                    ushort[]? samples = e.UserState as ushort[];
                    if (samples != null)
                    {
                        OnNewFrame?.Invoke(this, samples);
                    }
                    break;
            }
        }

        private void Worker_RunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs e)
        {
            OnNewConnectionState?.Invoke(this, ConnectionState.Iddle);
        }
    }
}
