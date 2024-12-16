using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDK2_Radar_SignalProcessing_GUI
{
    public class DatasetAnalyser
    {
        private double Threshold = 0.1;

        public const int ACTION_SWIPE_LEFT = 0;
        public const int ACTION_SWIPE_RIGHT = 1;
        public const int ACTION_CLICK = 2;

        public delegate void OnNewActionDetectedEventHandler(object sender, int action);
        public event OnNewActionDetectedEventHandler? OnNewActionDetected;

        public void Analyse(List<DataCollector.DataSet> dataset)
        {
            List<double> x = new List<double>();
            List<double> y = new List<double>();

            for (int i = 0; i < dataset.Count; i++) {

                // Make it simple, only check for azimuth of the motion higher than threshold
                int maxRange = 0;
                double maxMag = 0;
                int maxSpeed = 0;

                getMaxAmplitudeRange(dataset[i].dopplerFFTRx1, out maxRange, out maxMag, out maxSpeed);

                if (maxMag >= Threshold)
                {
                    double rx1 = dataset[i].dopplerFFTRx1[maxRange, maxSpeed].Phase;
                    double rx3 = dataset[i].dopplerFFTRx3[maxRange, maxSpeed].Phase;

                    x.Add(i);
                    y.Add(getAngleDiff(rx1, rx3));
                }
            }

            // regression and print
            LinearRegression.Compute(x, y, out double slope, out double intercept, out double s);

            double first = intercept;
            double last = slope * dataset.Count + intercept;
            double delta = first - last;

            System.Diagnostics.Debug.WriteLine("Delta is: " + delta.ToString());

            if (delta > Math.PI) OnNewActionDetected?.Invoke(this, ACTION_SWIPE_LEFT);
            if (delta < -Math.PI) OnNewActionDetected?.Invoke(this, ACTION_SWIPE_RIGHT);

            if (Math.Abs(delta) < (Math.PI / 2))
                OnNewActionDetected?.Invoke(this, ACTION_CLICK);
        }

        /// <summary>
        /// Get difference between two angles
        /// Result is always between -pi and pi
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <returns></returns>
        private double getAngleDiff(double a1, double a2)
        {
            double sign = -1;
            if (a1 > a2) sign = 1;

            double angle = a1 - a2;
            double k = -sign * Math.PI * 2;
            if (Math.Abs(k + angle) < Math.Abs(angle))
            {
                return k + angle;
            }
            return angle;
        }

        /// <summary>
        /// From a doppler FFT matrix, extract biggest range (index can be converted to meter) and the biggest magnitude
        /// </summary>
        /// <param name="dopplerFFTMatrix"></param>
        /// <param name="maxDetectedRange"></param>
        /// <param name="maxDetectedMag"></param>
        private void getMaxAmplitudeRange(System.Numerics.Complex[,] dopplerFFTMatrix,
            out int maxDetectedRange, out double maxDetectedMag, out int maxDetectedSpeed)
        {
            maxDetectedMag = 0;
            maxDetectedRange = 0;
            maxDetectedSpeed = 0;

            for (int i = 0; i < dopplerFFTMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < dopplerFFTMatrix.GetLength(1); j++)
                {
                    double magnitude = dopplerFFTMatrix[i, j].Magnitude;
                    if (magnitude > maxDetectedMag)
                    {
                        maxDetectedMag = magnitude;
                        maxDetectedRange = i;
                        maxDetectedSpeed = j;
                    }
                }
            }

        }
    }
}
