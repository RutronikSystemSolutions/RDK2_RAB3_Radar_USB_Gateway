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
                    //BaudRate = 921600,
                    BaudRate = 1000000,
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



            // var watch = System.Diagnostics.Stopwatch.StartNew();

            for (; ; )
            {

                for (; ; )
                {
                    int available = port.BytesToRead;
                    if (available > 0) break;
                    System.Threading.Thread.Sleep(10);
                }

                for (; ; )
                {
                    int offset = 0;
                    int remaining = bytesPerFrame;
                    bool gotTimeout = false;

                    for (; ; )
                    {
                        int readSize = 0;
                        try
                        {
                            readSize = port.Read(readBuffer, offset, remaining);
                        }
                        catch (System.TimeoutException)
                        {
                            gotTimeout = true;
                            break;
                        }
                        offset += readSize;
                        remaining -= readSize;
                        if (remaining == 0) break;
                    }

                    if (gotTimeout)
                    {
                        break;
                    }

                    // Frame is available
                    var samples = new ushort[samplesPerFrame];
                    for (int i = 0; i < samplesPerFrame; i++)
                    {
                        samples[i] = BitConverter.ToUInt16(readBuffer, i * 2);
                    }



                    worker.ReportProgress(WorkerReportFrame, samples);
                }
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
