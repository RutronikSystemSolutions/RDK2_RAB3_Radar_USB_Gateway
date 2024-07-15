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
        private RadarSignalProcessor radarSignalProcessor;

        public TargetDetectionConfigurationForm()
        {
            InitializeComponent();
            radarSignalProcessor = new RadarSignalProcessor(new RadarConfiguration());
        }

        public TargetDetectionConfigurationForm(RadarSignalProcessor radarSignalProcessor)
        {
            InitializeComponent();
            this.radarSignalProcessor = radarSignalProcessor;
            initTrackBars();
        }

        private void initTrackBars()
        {
            minRangeTrackBar.Maximum = radarSignalProcessor.getFreqBinCount() - 1;
            maxRangeTrackBar.Maximum = radarSignalProcessor.getFreqBinCount() - 1;
        }

        private double freqIndexToMeters(int index)
        {
            RadarConfiguration configuration = radarSignalProcessor.getRadarConfiguration();
            double bandWidth = configuration.EndFrequency - configuration.StartFrequency;
            double celerity = 299792458;
            double fftLen = radarSignalProcessor.getFreqBinCount();
            double slope = bandWidth / (configuration.SamplesPerChirp * (1 / configuration.SamplingRate));

            double fractionFs = index / ((fftLen - 1) * 2);
            double freq = fractionFs * configuration.SamplingRate;
            double rangeMeters = (celerity * freq) / (2 * slope);

            return rangeMeters;
        }

        private void updateRadarProcessor()
        {
            this.radarSignalProcessor.setObservedRange(minRangeTrackBar.Value, maxRangeTrackBar.Value);
        }

        private void minRangeTrackBar_Scroll(object sender, EventArgs e)
        {
            int freqIndex = minRangeTrackBar.Value;
            minRangeMetersTextBox.Text = string.Format("{0:0.0} meters", freqIndexToMeters(freqIndex));
            updateRadarProcessor();
        }

        private void maxRangeTrackBar_Scroll(object sender, EventArgs e)
        {
            int freqIndex = maxRangeTrackBar.Value;
            maxRangeMetersTextBox.Text = string.Format("{0:0.0} meters", freqIndexToMeters(freqIndex));
            updateRadarProcessor();
        }
    }
}
