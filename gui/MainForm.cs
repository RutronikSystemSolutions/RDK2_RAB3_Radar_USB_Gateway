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

        private RDK2 rdk2 = new RDK2();

        public MainForm()
        {
            InitializeComponent();

            rdk2.OnNewFrame += Rdk2_OnNewFrame;
            rdk2.OnNewConnectionState += Rdk2_OnNewConnectionState;

            radarSignalProcessor.OnNewFormattedTimeSignal += RadarSignalProcessor_OnNewFormattedTimeSignal;
            radarSignalProcessor.OnNewFrameSpectrum += RadarSignalProcessor_OnNewFrameSpectrum;
            radarSignalProcessor.OnNewEnergyOverTime += RadarSignalProcessor_OnNewEnergyOverTime;
            radarSignalProcessor.OnNewDBFOutput += RadarSignalProcessor_OnNewDBFOutput;
            radarSignalProcessor.OnNewTargetDetected += RadarSignalProcessor_OnNewTargetDetected;
            radarSignalProcessor.OnNewDopplerFFTMatrix += RadarSignalProcessor_OnNewDopplerFFTMatrix;
            //radarSignalProcessor.OnNew2DBFOutput += RadarSignalProcessor_OnNew2DBFOutput;

            radarSignalProcessor.OnNewDopplerFFTMatrix3 += RadarSignalProcessor_OnNewDopplerFFTMatrix3;
        }

        private void RadarSignalProcessor_OnNewDopplerFFTMatrix3(object sender, System.Numerics.Complex[,] dopplerFFTMatrixRx1, System.Numerics.Complex[,] dopplerFFTMatrixRx2, System.Numerics.Complex[,] dopplerFFTMatrixRx3)
        {
            // Let's go! -> compute angle and display max
            gestureView.UpdateData(dopplerFFTMatrixRx1, dopplerFFTMatrixRx2, dopplerFFTMatrixRx3);
            gestureViewTime.UpdateData(dopplerFFTMatrixRx1, dopplerFFTMatrixRx2, dopplerFFTMatrixRx3);
        }

        private void RadarSignalProcessor_OnNew2DBFOutput(object sender, System.Numerics.Complex[,] dbfOutputH, System.Numerics.Complex[,] dbfOutputV)
        {
            // Convert for DBF view
            int beamCount = dbfOutputH.GetLength(0);
            int freqBinCount = dbfOutputH.GetLength(1);

            double[,] dbfMagnitudeH = new double[beamCount, freqBinCount];
            double[,] dbfMagnitudeV = new double[beamCount, freqBinCount];
            for (int i = 0; i < beamCount; ++i)
            {
                for (int j = 0; j < freqBinCount; ++j)
                {
                    dbfMagnitudeH[i, j] = dbfOutputH[i, j].Magnitude;
                    dbfMagnitudeV[i, j] = dbfOutputV[i, j].Magnitude;
                }
            }

            gestureView.UpdateData(dbfMagnitudeH, dbfMagnitudeV);
            gestureViewTime.UpdateData(dbfMagnitudeH, dbfMagnitudeV);
        }

        private void RadarSignalProcessor_OnNewDopplerFFTMatrix(object sender, System.Numerics.Complex[,] dopplerFFTMatrix, int antennaIndex)
        {
            if (antennaIndex == 0) dopplerfftView.UpdateData(dopplerFFTMatrix);
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

        private void RadarSignalProcessor_OnNewTargetDetected(object sender, bool detected, double angle, double range)
        {
            anglePresenceView.SignalPresenceDetected(detected, angle, range);
            //System.Diagnostics.Debug.WriteLine("Detected: " + angle.ToString() + "° range: " + range.ToString());
        }

        private void RadarSignalProcessor_OnNewDBFOutput(object sender, System.Numerics.Complex[,] dbfOutput)
        {
            // Convert for DBF view
            int beamCount = dbfOutput.GetLength(0);
            int freqBinCount = dbfOutput.GetLength(1);
            double[,] dbfMagnitude = new double[beamCount, freqBinCount];
            for (int i = 0; i < beamCount; ++i)
            {
                for (int j = 0; j < freqBinCount; ++j)
                {
                    dbfMagnitude[i, j] = dbfOutput[i, j].Magnitude;
                }
            }
            dbfView.UpdateData(dbfMagnitude);
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
            //radarSignalProcessor.feed(frame);
            radarSignalProcessor.feedDopplerFFT(frame);
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
    }
}
