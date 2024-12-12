using OxyPlot.Axes;
using OxyPlot;
using System.IO.Ports;
using OxyPlot.Series;
using OxyPlot.WindowsForms;
using System.Windows.Forms;

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

            dbfDopplerView.UpdateData(dopplerFFTMatrixRx1, dopplerFFTMatrixRx2, dopplerFFTMatrixRx3);
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

        private void RadarSignalProcessor_OnNewEnergyOverTime(object sender, double energy, int antennaIndex)
        {
            energyOverTimeView.updateData(energy, antennaIndex);
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
            //radarSignalProcessor.feedDopplerFFT(frame);
            radarSignalProcessor.feedBackground(frame);
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
            // Update settings if necessary
            if (Properties.Settings.Default.minRange == -1 || Properties.Settings.Default.maxRange == -1)
            {
                Properties.Settings.Default.minRange = 0;
                Properties.Settings.Default.maxRange = (RadarConfiguration.SAMPLES_PER_CHIRP / 2) + 1;
                Properties.Settings.Default.Save();
            }

            // Load the possible com ports
            string[] serialPorts = SerialPort.GetPortNames();
            comPortComboBox.DataSource = serialPorts;

            // Set range everywhere
            int minRange = Properties.Settings.Default.minRange;
            int maxRange = Properties.Settings.Default.maxRange;
            clickDetector.SetRange(minRange, maxRange);
            distanceView.SetRange(minRange, maxRange);
            gestureViewScatter.SetRange(minRange, maxRange);
            userFeedbackView.SetRange(minRange, maxRange);
            gestureViewTime.SetRange(minRange, maxRange);
            dbfDopplerView.SetRange(minRange, maxRange);
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

        private void thresholdToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ThresholdConfigurationForm form = new ThresholdConfigurationForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                // Set threshold everywhere
                clickDetector.SetThreshold(form.threshold);
                energyOverTimeView.SetThreshold(form.threshold);
                gestureViewTime.SetThreshold(form.threshold);
                distanceView.SetThreshold(form.threshold);
                gestureViewScatter.SetThreshold(form.threshold);
                userFeedbackView.SetThreshold(form.threshold);
                dbfDopplerView.SetThreshold(form.threshold);
            }
        }

        private void rangeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TargetDetectionConfigurationForm form = new TargetDetectionConfigurationForm(Properties.Settings.Default.minRange,
                Properties.Settings.Default.maxRange);
            if (form.ShowDialog() == DialogResult.OK)
            {
                // Store
                Properties.Settings.Default.minRange = form.minRange;
                Properties.Settings.Default.maxRange = form.maxRange;
                Properties.Settings.Default.Save();

                logView.AddLog(string.Format("Set range to: {0} - {1}", form.minRange, form.maxRange));
                // Set range everywhere
                clickDetector.SetRange(form.minRange, form.maxRange);
                distanceView.SetRange(form.minRange, form.maxRange);
                gestureViewScatter.SetRange(form.minRange, form.maxRange);
                userFeedbackView.SetRange(form.minRange, form.maxRange);
                gestureViewTime.SetRange(form.minRange, form.maxRange);
                dbfDopplerView.SetRange(form.minRange, form.maxRange);
            }
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
