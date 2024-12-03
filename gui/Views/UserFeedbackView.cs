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
        private int maxRange = (RadarConfiguration.SAMPLES_PER_CHIRP / 2) + 1;
        private int minRange = 0;
        private const int NONE = -1;
        private double threshold = 0.1;

        private object sync = new object();
        private int lastMaxRange = NONE;

        private int click = 0;
        private int leftMove = 0;
        private int rightMove = 0;
        private int readyForNew = 0;

        private const int ACTION_RESET = 5;

        public UserFeedbackView()
        {
            InitializeComponent();
            verticalProgressBar.SetMinMax(minRange, maxRange);
            guiUpdateTime.Start();
        }

        public void SetThreshold(double threshold)
        {
            this.threshold = threshold;
        }

        public void SetRange(int min, int max)
        {
            minRange = min;
            maxRange = max;
            verticalProgressBar.SetMinMax(minRange, maxRange);
        }

        private void getMaxAmplitudeRange(System.Numerics.Complex[,] dopplerFFTMatrix, out int maxDetectedRange, out double maxDetectedMag)
        {
            maxDetectedMag = 0;
            maxDetectedRange = 0;

            for (int i = minRange; i < maxRange; i++)
            {
                for (int j = 0; j < dopplerFFTMatrix.GetLength(1); j++)
                {
                    double magnitude = dopplerFFTMatrix[i, j].Magnitude;
                    if (magnitude > maxDetectedMag)
                    {
                        maxDetectedMag = magnitude;
                        maxDetectedRange = i;
                    }
                }
            }

        }

        public void SignalClick(int direction)
        {
            lock(sync)
            {
                if (direction == ClickDetector.DIRECTION_NONE)
                    click = ACTION_RESET;
                else if (direction == ClickDetector.DIRECTION_LEFT)
                    leftMove = ACTION_RESET;
                else if (direction == ClickDetector.DIRECTION_RIGHT)
                    rightMove = ACTION_RESET;
            }
        }

        public void SignalReadyForNextAction(bool status)
        {
            if (status)
                readyForNewMovePanel.BackColor = Color.LightGreen;
            else
                readyForNewMovePanel.BackColor = Color.LightGray;
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
            int currentMaxRange = NONE;
            int clickDo = 0;
            int leftMoveDo = 0;
            int rightMoveDo = 0;

            lock(sync)
            {
                currentMaxRange = lastMaxRange;
                clickDo = click;
                leftMoveDo = leftMove;
                rightMoveDo = rightMove;

                if (click >= 0) click--;
                if (leftMove >= 0) leftMove--;
                if (rightMove >= 0) rightMove--;
            }

            if (currentMaxRange == NONE)
            {
                verticalProgressBar.SetValue(maxRange);
            }
            else
            {
                verticalProgressBar.SetValue(currentMaxRange);
            }

            if (clickDo == ACTION_RESET)
            {
                clickPanel.BackColor = Color.Magenta;
            }
            else if (clickDo == 0)
            {
                clickPanel.BackColor = Color.LightGray;
            }

            if (leftMoveDo == ACTION_RESET)
            {
                leftMovePanel.BackColor = Color.Magenta;
            }
            else if (leftMoveDo == 0)
            {
                leftMovePanel.BackColor = Color.LightGray;
            }

            if (rightMoveDo == ACTION_RESET)
            {
                rightMovePanel.BackColor = Color.Magenta;
            }
            else if (rightMoveDo == 0)
            {
                rightMovePanel.BackColor = Color.LightGray;
            }
        }
    }
}
