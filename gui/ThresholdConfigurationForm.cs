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
        public double threshold = 0.1;

        public ThresholdConfigurationForm()
        {
            InitializeComponent();
        }

        private void setThresholdValueButton_Click(object sender, EventArgs e)
        {
            try
            {
                string text = thresholdValueTextBox.Text.Replace(',', '.');
                double threshold = double.Parse(text, System.Globalization.CultureInfo.InvariantCulture);
                this.threshold = threshold;
            }
            catch(Exception)
            {
                MessageBox.Show("Wrong threshold value, please correct.");
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
