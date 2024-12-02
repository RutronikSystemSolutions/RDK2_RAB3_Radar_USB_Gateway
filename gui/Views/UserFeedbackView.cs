using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OxyPlot;

namespace RDK2_Radar_SignalProcessing_GUI.Views
{
    public partial class UserFeedbackView : UserControl
    {
        private const int MAX_DETECTED_RANGE = 14;
        private const int MIN_DETECTED_RANGE = 1;
        private const int NONE = -1;
        private double threshold = 0.1;

        private object sync = new object();
        private int lastMaxRange = NONE;

        public UserFeedbackView()
        {
            InitializeComponent();
            verticalProgressBar.SetMinMax(MIN_DETECTED_RANGE, MAX_DETECTED_RANGE);
            guiUpdateTime.Start();
        }

        private void getMaxAmplitudeRange(System.Numerics.Complex[,] dopplerFFTMatrix, out int maxRange, out double maxMag)
        {
            maxMag = 0;
            maxRange = 0;

            for (int i = 0; i < dopplerFFTMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < dopplerFFTMatrix.GetLength(1); j++)
                {
                    double magnitude = dopplerFFTMatrix[i, j].Magnitude;
                    if (magnitude > maxMag)
                    {
                        maxMag = magnitude;
                        maxRange = i;
                    }
                }
            }

        }

        public void UpdateData(System.Numerics.Complex[,] dopplerFFTMatrixRx1, System.Numerics.Complex[,] dopplerFFTMatrixRx2, System.Numerics.Complex[,] dopplerFFTMatrixRx3)
        {
            // Should never happen but let be sure of it
            if (dopplerFFTMatrixRx1 == null) return;
            if (dopplerFFTMatrixRx2 == null) return;
            if (dopplerFFTMatrixRx3 == null) return;

            double maxMag = 0;
            int maxRange = 0;

            getMaxAmplitudeRange(dopplerFFTMatrixRx1, out maxRange, out maxMag);
            if (maxMag > threshold)
            {
                lock(sync)
                {
                    lastMaxRange = maxRange;
                }
            }
            else
            {
                lock(sync)
                {
                    lastMaxRange = NONE;
                }
            }
        }

        private void guiUpdateTime_Tick(object sender, EventArgs e)
        {
            int maxRange = NONE;
            lock(sync)
            {
                maxRange = lastMaxRange;
            }

            selectedNumber.Text = maxRange.ToString();

            if (maxRange == NONE)
            {
                verticalProgressBar.SetValue(MAX_DETECTED_RANGE);
            }
            else
            {
                verticalProgressBar.SetValue(maxRange);
            }
        }
    }
}
