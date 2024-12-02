using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RDK2_Radar_SignalProcessing_GUI.UIControls
{
    public partial class VerticalProgressBar : UserControl
    {
        private int min = 0;
        private int max = 100;

        public VerticalProgressBar()
        {
            InitializeComponent();
        }

        public void SetMinMax(int min, int max)
        {
            this.min = min;
            this.max = max;
        }

        public void SetValue(int value)
        {
            if (value < min) value = min;
            if (value > max) value = max;

            double componentHeight = this.Height;
            double ratio = (double)(value - min) / (double)(max - min);

            ratio = 1 - ratio;

            valuePanel.Height = (int)(ratio * this.Height);

        }
    }
}
