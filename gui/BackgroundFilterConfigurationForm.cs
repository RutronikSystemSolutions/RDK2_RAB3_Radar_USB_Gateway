using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RDK2_Radar_SignalProcessing_GUI
{
    public partial class BackgroundFilterConfigurationForm : Form
    {
        private BackgroundFilterConfiguration configuration;
        private RadarSignalProcessor radarSignalProcessor;

        public BackgroundFilterConfigurationForm()
        {
            InitializeComponent();
            configuration = new BackgroundFilterConfiguration();
            radarSignalProcessor = new RadarSignalProcessor(new RadarConfiguration());
        }

        private int valueToTrackBarTick(double alpha)
        {
            int value = (int)(alpha * alphaTrackBar.Maximum);
            if (value < alphaTrackBar.Minimum) value = alphaTrackBar.Minimum;
            if (value > alphaTrackBar.Maximum) value = alphaTrackBar.Maximum;
            return value;
        }

        public BackgroundFilterConfigurationForm(RadarSignalProcessor radarSignalProcessor)
        {
            InitializeComponent();
            this.radarSignalProcessor = radarSignalProcessor;
            configuration = radarSignalProcessor.getBackgroundFilterConfiguration();
            if (configuration.mode == BackgroundRemover.Mode.ModeFixed)
            {
                fixedRadioButton.Checked = true;
            }
            else
            {
                iirRadioButton.Checked = true;
            }

            alphaTextBox.Text = configuration.alpha.ToString();
            alphaTrackBar.Value = valueToTrackBarTick(configuration.alpha);
        }

        private void alphaTrackBar_Scroll(object sender, EventArgs e)
        {
            // Convert to alpha
            configuration.alpha = (double)alphaTrackBar.Value / (double)alphaTrackBar.Maximum;
            // Update
            radarSignalProcessor.setBackgroundFilterConfiguration(configuration);
            // Update text box
            alphaTextBox.Text = configuration.alpha.ToString();
        }

        private void iirRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            updateFilterType();
        }

        private void fixedRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            updateFilterType();
        }

        private void updateFilterType()
        {
            bool iirMode = iirRadioButton.Checked;
            bool fixedMode = fixedRadioButton.Checked;

            if (iirMode == false && fixedMode == false)
            {
                // Nothing to do
                return;
            }

            if (iirMode)
            {
                configuration.mode = BackgroundRemover.Mode.ModeIIR;
            }
            else if (fixedMode)
            {
                configuration.mode = BackgroundRemover.Mode.ModeFixed;
            }
            radarSignalProcessor.setBackgroundFilterConfiguration(configuration);
        }
    }
}
