using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RDK2_Radar_SignalProcessing_GUI
{
    public partial class TargetDetectionConfigurationForm : Form
    {
        public int minRange = 0;
        public int maxRange = 0;

        public TargetDetectionConfigurationForm()
        {
            InitializeComponent();
            minRange = 0;
            maxRange = GetFreqBinCount() - 1;
            initTrackBars();
        }

        public TargetDetectionConfigurationForm(int min, int max)
        {
            InitializeComponent();
            minRange = min;
            maxRange = max;
            initTrackBars();
        }



        private int GetFreqBinCount()
        {
            // Since we use no zero padding
            return (RadarConfiguration.SAMPLES_PER_CHIRP / 2) + 1;
        }

        private void initTrackBars()
        {
            minRangeTrackBar.Maximum = GetFreqBinCount();
            maxRangeTrackBar.Maximum = GetFreqBinCount();

            minRangeTrackBar.Value = minRange;
            maxRangeTrackBar.Value = maxRange;

            updateMinRangeText();
            updateMaxRangeText();
        }

        private double freqIndexToMeters(int index)
        {
            double bandWidth = RadarConfiguration.END_FREQUENCY - RadarConfiguration.START_FREQUENCY;
            double celerity = 299792458;
            double fftLen = GetFreqBinCount();
            double slope = bandWidth / (RadarConfiguration.SAMPLES_PER_CHIRP * (1 / RadarConfiguration.SAMPLING_RATE));

            double fractionFs = index / ((fftLen - 1) * 2);
            double freq = fractionFs * RadarConfiguration.SAMPLING_RATE;
            double rangeMeters = (celerity * freq) / (2 * slope);

            return rangeMeters;
        }

        private void updateMinRangeText()
        {
            int freqIndex = minRangeTrackBar.Value;
            minRangeMetersTextBox.Text = string.Format("{0:0.0} cm", freqIndexToMeters(freqIndex) * 100);
        }

        private void minRangeTrackBar_Scroll(object sender, EventArgs e)
        {
            minRange = minRangeTrackBar.Value;
            updateMinRangeText();
        }

        private void updateMaxRangeText()
        {
            int freqIndex = maxRangeTrackBar.Value;
            maxRangeMetersTextBox.Text = string.Format("{0:0.0} cm", freqIndexToMeters(freqIndex) * 100);
        }

        private void maxRangeTrackBar_Scroll(object sender, EventArgs e)
        {
            maxRange = maxRangeTrackBar.Value;
            updateMaxRangeText();
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
