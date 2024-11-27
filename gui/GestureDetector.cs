using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDK2_Radar_SignalProcessing_GUI
{
    public class GestureDetector
    {
        private const int NONE_VALUE = -1;

        private double threshold = 0.1;

        private int observedDuration = 60;

        private List<int> detectedRangeOverTime = new List<int>();

        private ClickDetector clickDetector = new ClickDetector();

        #region "Click parameters"

        private int NOTHING_BEFORE_COUNT = 15; // 0.5 seconds
        private int NOTHING_AFTER_COUNT = 5;
        private int START_TO_MIN = 15;
        private int MIN_TO_STOP = 15;
        private int DECREASE_AMPLITUDE = 5; // 15 cm
        private int INCREASE_AMPLITUDE = 4; // 10cm
        #endregion

        /// <summary>
        /// From a doppler FFT matrix, extract biggest range (index can be converted to meter) and the biggest magnitude
        /// </summary>
        /// <param name="dopplerFFTMatrix"></param>
        /// <param name="maxRange"></param>
        /// <param name="maxMag"></param>
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

        private bool isClickDetected()
        {
            int startIndex = 0;

            for(; ;)
            {
                int remaining = detectedRangeOverTime.Count - startIndex;
                if ( remaining < (NOTHING_BEFORE_COUNT + NOTHING_AFTER_COUNT))
                {
                    // Not enough data anymore
                    return false;
                }

                int startPoint = NONE_VALUE;
                int stopPoint = NONE_VALUE;

                // Check for at least NOTHING_BEFORE_COUNT NONE values followed by a not NONE value
                int none_count = 0;
                for(int i = startIndex; i < (detectedRangeOverTime.Count - NOTHING_AFTER_COUNT); ++i)
                {
                    if (detectedRangeOverTime[i] == NONE_VALUE) none_count++;
                    else
                    {
                        if (none_count >= NOTHING_BEFORE_COUNT)
                        {
                            // store start point and update start index
                            startPoint = detectedRangeOverTime[i];
                            startIndex = i;
                            break;
                        }
                        else none_count = 0;
                    }
                }

                // Was able to find a start point?
                if (startPoint == NONE_VALUE) return false;

                int minValue = startPoint;

                // Get the min value within START_TO_MIN
                for(int i = startIndex; (i < (startIndex + START_TO_MIN) && (i < detectedRangeOverTime.Count)); ++i)
                {
                    if (detectedRangeOverTime[i] == NONE_VALUE) break;
                    if (detectedRangeOverTime[i] < minValue)
                    {
                        minValue = detectedRangeOverTime[i];
                        startIndex = i;
                    }

                    // Increase again from enough
                    if (detectedRangeOverTime[i] > (minValue + INCREASE_AMPLITUDE))
                    {
                        break;
                    }
                }

                // Check if delta is high enough
                if ((startPoint - minValue) < DECREASE_AMPLITUDE)
                {
                    // Not enough...
                    continue;
                }

                // Get the next NONE value within MIN_TO_STOP
                for (int i = startIndex; (i < (startIndex + MIN_TO_STOP) && (i < detectedRangeOverTime.Count)); ++i)
                {
                    if (detectedRangeOverTime[i] == NONE_VALUE)
                    {
                        startIndex = i;
                        stopPoint = detectedRangeOverTime[i - 1];
                        break;
                    }
                }

                // Was able to find a stop point?
                if (stopPoint == NONE_VALUE) return false;

                // Check if delta is high enough
                if ((stopPoint - minValue) < INCREASE_AMPLITUDE)
                {
                    // Not enough...
                    continue;
                }

                // Good!
                return true;
            }
        }

        public void UpdateData(System.Numerics.Complex[,] dopplerFFTMatrixRx1,
            System.Numerics.Complex[,] dopplerFFTMatrixRx2,
            System.Numerics.Complex[,] dopplerFFTMatrixRx3)
        {

            clickDetector.UpdateData(dopplerFFTMatrixRx1, dopplerFFTMatrixRx2, dopplerFFTMatrixRx3);
            return;

            int maxRange = 0;
            double maxMag = 0;

            getMaxAmplitudeRange(dopplerFFTMatrixRx1, out maxRange, out maxMag);

            // Amplitude bigger than threshold?
            if (maxMag > threshold)
            {
                detectedRangeOverTime.Add(maxRange);
            }
            else
            {
                detectedRangeOverTime.Add(NONE_VALUE);
            }

            // Remove if enough data
            if (detectedRangeOverTime.Count > observedDuration)
            {
                detectedRangeOverTime.RemoveAt(0);
            }

            if (isClickDetected())
            {
                System.Diagnostics.Debug.WriteLine("Click!");
                detectedRangeOverTime.Clear();
            }
        }
    }
}
