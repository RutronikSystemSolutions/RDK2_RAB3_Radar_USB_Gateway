using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDK2_Radar_SignalProcessing_GUI
{
    public class FormatterForNN
    {
        private double threshold = 0.1;
        private int maxMotionLength = 32;
        private int minMotionLength = 10;

        public float[] Format(List<DataCollector.DataSet> dataset)
        {
            const int dataCount = 96;
            const int MAGNITUDE_START = 0;
            const int RANGE_START = 32;
            const int AZIMUTH_START = 64;
            float[] retval = new float[dataCount];

            for(int i = 0; i < dataset.Count; ++i)
            {
                // Make it simple, only check for azimuth of the motion higher than threshold
                int maxRange = 0;
                double maxMag = 0;
                int maxSpeed = 0;

                getMaxAmplitudeRange(dataset[i].dopplerFFTRx1, out maxRange, out maxMag, out maxSpeed);

                if (maxMag < threshold)
                {
                    throw new Exception("Magnitude too small. Something wrong happened");
                }

                double rx1 = dataset[i].dopplerFFTRx1[maxRange, maxSpeed].Phase;
                double rx3 = dataset[i].dopplerFFTRx3[maxRange, maxSpeed].Phase;

                retval[MAGNITUDE_START + i] = (float) maxMag;
                retval[RANGE_START + i] = maxRange;
                retval[AZIMUTH_START + i] = (float)getAngleDiff(rx1, rx3);
            }

            return retval;
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
