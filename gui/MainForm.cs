using OxyPlot.Axes;
using OxyPlot;
using System.IO.Ports;
using OxyPlot.Series;
using OxyPlot.WindowsForms;

namespace RDK2_Radar_SignalProcessing_GUI
{
    public partial class MainForm : Form
    {
        /// <summary>
        /// TODO: read parameters from the RDK2
        /// </summary>
        private RadarSignalProcessor radarSignalProcessor = new RadarSignalProcessor(new RadarConfiguration());

        private GestureDetector gestureDetector = new GestureDetector();
        private ClickDetector clickDetector = new ClickDetector();

        private RDK2 rdk2 = new RDK2();

        private DataLogger logger = new DataLogger();

        private PlaybackReader playbackReader = new PlaybackReader();

        public MainForm()
        {
            InitializeComponent();

            rdk2.OnNewFrame += Rdk2_OnNewFrame;
            rdk2.OnNewConnectionState += Rdk2_OnNewConnectionState;

            radarSignalProcessor.OnNewFormattedTimeSignal += RadarSignalProcessor_OnNewFormattedTimeSignal;
            radarSignalProcessor.OnNewFrameSpectrum += RadarSignalProcessor_OnNewFrameSpectrum;
            radarSignalProcessor.OnNewEnergyOverTime += RadarSignalProcessor_OnNewEnergyOverTime;
            radarSignalProcessor.OnNewDopplerFFTMatrix3 += RadarSignalProcessor_OnNewDopplerFFTMatrix3;

            clickDetector.OnNewClick += ClickDetector_OnNewClick;
            clickDetector.OnHandDetected += ClickDetector_OnHandDetected;
            clickDetector.OnReadyForNextAction += ClickDetector_OnReadyForNextAction;

            logger.OnNewLoggerState += Logger_OnNewLoggerState;

            playbackReader.OnNewFrameEvent += Rdk2_OnNewFrame;
        }

        private void ClickDetector_OnReadyForNextAction(object sender, bool status)
        {
            userFeedbackView.SignalReadyForNextAction(status);
        }

        private void Logger_OnNewLoggerState(object sender, bool startedFlag)
        {
            if (startedFlag)
            {
                dataLoggerToolStripStatusLabel.Text = "Logger started";
                startToolStripMenuItem.Enabled = false;
                stopToolStripMenuItem.Enabled = true;
            }
            else
            {
                dataLoggerToolStripStatusLabel.Text = "Logger stopped";
                startToolStripMenuItem.Enabled = true;
                stopToolStripMenuItem.Enabled = false;
            }
        }

        private void ClickDetector_OnHandDetected(object sender, bool status)
        {
            if (status)
            {
                handDetectedToolStripStatusLabel.Text = "Hand detected";
            }
            else
            {
                handDetectedToolStripStatusLabel.Text = string.Empty;
            }
        }

        private void ClickDetector_OnNewClick(object sender, int direction)
        {
            logView.AddLog("On Click " + direction);
            System.Media.SystemSounds.Asterisk.Play();

            userFeedbackView.SignalClick(direction);
        }

        private void RadarSignalProcessor_OnNewDopplerFFTMatrix3(object sender, System.Numerics.Complex[,] dopplerFFTMatrixRx1, System.Numerics.Complex[,] dopplerFFTMatrixRx2, System.Numerics.Complex[,] dopplerFFTMatrixRx3)
        {
            // Let's go! -> compute angle and display max
            //gestureView.UpdateData(dopplerFFTMatrixRx1, dopplerFFTMatrixRx2, dopplerFFTMatrixRx3);
            gestureViewScatter.UpdateData(dopplerFFTMatrixRx1, dopplerFFTMatrixRx2, dopplerFFTMatrixRx3);
            gestureViewTime.UpdateData(dopplerFFTMatrixRx1, dopplerFFTMatrixRx2, dopplerFFTMatrixRx3);
            distanceView.UpdateData(dopplerFFTMatrixRx1, dopplerFFTMatrixRx2, dopplerFFTMatrixRx3);

            //gestureDetector.UpdateData(dopplerFFTMatrixRx1, dopplerFFTMatrixRx2, dopplerFFTMatrixRx3);
            clickDetector.UpdateData(dopplerFFTMatrixRx1, dopplerFFTMatrixRx2, dopplerFFTMatrixRx3);
            userFeedbackView.UpdateData(dopplerFFTMatrixRx1, dopplerFFTMatrixRx2, dopplerFFTMatrixRx3);
        }

        private void Rdk2_OnNewConnectionState(object sender, RDK2.ConnectionState state)
        {
            switch (state)
            {
                case RDK2.ConnectionState.Connected:
                    rdk2ConnectionStateTextBox.Text = "Connected";
                    rdk2ConnectionStateTextBox.BackColor = Color.Green;
                    connectButton.Enabled = false;
                    break;

                case RDK2.ConnectionState.Error:
                    rdk2ConnectionStateTextBox.Text = "Error";
                    rdk2ConnectionStateTextBox.BackColor = Color.Red;
                    connectButton.Enabled = true;
                    break;
            }
        }

        private void RadarSignalProcessor_OnNewEnergyOverTime(object sender, double energy, double threshold, int antennaIndex)
        {
            energyOverTimeView.updateData(energy, threshold, antennaIndex);
        }

        private void RadarSignalProcessor_OnNewFrameSpectrum(object sender, System.Numerics.Complex[] spectrum, int antennaIndex)
        {
            double[] dbfs = radarSignalProcessor.computeAsDBFS(spectrum);
            rangefftView.setSpectrumDBFS(dbfs, antennaIndex);
        }

        private void RadarSignalProcessor_OnNewFormattedTimeSignal(object sender, double[] signal, int antennaIndex)
        {
            timeSignalView.updateData(signal, antennaIndex);
        }

        private void Rdk2_OnNewFrame(object sender, ushort[] frame)
        {
            radarSignalProcessor.feedDopplerFFT(frame);
            logger.Log(frame);
        }

        /// <summary>
        /// Event handler: happens once at first load
        /// Populate the list of possible COM ports
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            // Load the possible com ports
            string[] serialPorts = SerialPort.GetPortNames();
            comPortComboBox.DataSource = serialPorts;
        }

        /// <summary>
        /// Event handler: user clicked on the "connect" button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void connectButton_Click(object sender, EventArgs e)
        {
            if ((comPortComboBox.SelectedIndex < 0) || (comPortComboBox.SelectedIndex >= comPortComboBox.Items.Count)) return;

            var selectedItem = comPortComboBox.Items[comPortComboBox.SelectedIndex];
            if (selectedItem != null)
            {
                string? portName = selectedItem.ToString();
                if (portName != null) rdk2.SetPortName(portName);
            }

        }

        /// <summary>
        /// Background filter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundFilterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BackgroundFilterConfigurationForm form = new BackgroundFilterConfigurationForm(radarSignalProcessor);
            form.ShowDialog();
        }

        private void thresholdToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ThresholdConfigurationForm form = new ThresholdConfigurationForm(radarSignalProcessor);
            form.ShowDialog();
        }

        private void rangeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TargetDetectionConfigurationForm form = new TargetDetectionConfigurationForm(radarSignalProcessor);
            form.ShowDialog();
        }

        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "CSV Files | *.csv";
            dialog.DefaultExt = "csv";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                logger.Start(dialog.FileName);
            }
        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            logger.Stop();
        }

        private void playbackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "CSV Files | *.csv";
            dialog.DefaultExt = "csv";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                playbackReader.Start(dialog.FileName);
            }
        }
    }
}
