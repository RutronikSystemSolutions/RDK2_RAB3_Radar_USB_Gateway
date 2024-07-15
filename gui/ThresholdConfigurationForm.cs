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
    public partial class ThresholdConfigurationForm : Form
    {
        private RadarSignalProcessor radarSignalProcessor;

        public ThresholdConfigurationForm()
        {
            InitializeComponent();
            radarSignalProcessor = new RadarSignalProcessor(new RadarConfiguration());
        }

        public ThresholdConfigurationForm(RadarSignalProcessor radarSignalProcessor)
        {
            InitializeComponent();
            this.radarSignalProcessor = radarSignalProcessor;
        }

        private void setThresholdValueButton_Click(object sender, EventArgs e)
        {
            try
            {
                string text = thresholdValueTextBox.Text.Replace(',', '.');
                double threshold = double.Parse(text, System.Globalization.CultureInfo.InvariantCulture);
                radarSignalProcessor.setThresholdValue(threshold);
            }
            catch(Exception)
            {
                MessageBox.Show("Wrong threshold value, please correct.");
                return;
            }
        }
    }
}
