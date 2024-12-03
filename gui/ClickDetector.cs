using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDK2_Radar_SignalProcessing_GUI
{
    public class ClickDetector
    {
        public const int DIRECTION_LEFT = 0;
        public const int DIRECTION_RIGHT = 1;
        public const int DIRECTION_NONE = 2;

        private double threshold = 0.1;
        private const int NONE_VALUE = -1;

        private int NOTHING_BEFORE_COUNT = 5;
        private int START_TO_MIN = 15;
        private int MIN_TO_STOP = 15;
        private int DECREASE_AMPLITUDE = 5; // ~15 cm 5 * 2.7cm
        private int INCREASE_AMPLITUDE = 4; // 10cm
        private int NOTHING_DURING_DECREASE_MAX = 2;

        private int clickCount = 0;

        private int lastState = NONE_VALUE;

        public delegate void OnNewClickEventHandler(object sender, int direction);
        public event OnNewClickEventHandler? OnNewClick;

        public delegate void OnHandDetectedEventHandler(object sender, bool status);
        public event OnHandDetectedEventHandler? OnHandDetected;

        public delegate void OnReadyForNextActionEventHandler(object sender, bool status);
        public event OnReadyForNextActionEventHandler? OnReadyForNextAction;

        private int minRange = 0;
        private int maxRange = (RadarConfiguration.SAMPLES_PER_CHIRP / 2) + 1;

        private enum ClickDetectorStatus
        {
            WAIT_NOTHING_BEFORE,
            WAIT_START_POINT,
            WAIT_DECREASE,
            WAIT_INCREASE,
            WAIT_NOTHING_AFTER
        };

        private ClickDetectorStatus status = ClickDetectorStatus.WAIT_NOTHING_BEFORE;

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

            //for (int i = 0; i < dopplerFFTMatrix.GetLength(0); i++)
            //for (int i = 0; i < 8; i++) // Reduce the range to 8 * 2.7cm = 21.6 cm
            for (int i = minRange; i < maxRange; ++i)
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

        private int noneCount = 0;
        private int startPoint = NONE_VALUE;
        private int startTime = 0;
        private int minValue = 0;

        private double hAtStart = 0;
        private double hAtMin = 0;
        private double hAtLast = 0;

        private int noneDuringDecreaseCount = 0;

        private void Debug(string text)
        {
            System.Diagnostics.Debug.WriteLine(text);
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

        public void SetThreshold(double threshold)
        {
            this.threshold = threshold;
        }

        public void SetRange(int min, int max)
        {
            minRange = min;
            maxRange = max;
        }

        public void UpdateData(System.Numerics.Complex[,] dopplerFFTMatrixRx1,
            System.Numerics.Complex[,] dopplerFFTMatrixRx2,
            System.Numerics.Complex[,] dopplerFFTMatrixRx3)
        {
            int maxRange = 0;
            double maxMag = 0;
            int maxSpeed = 0;

            double hdiff = 0;

            getMaxAmplitudeRange(dopplerFFTMatrixRx1, out maxRange, out maxMag, out maxSpeed);

            // Amplitude bigger than threshold?
            if (maxMag < threshold)
            {
                maxRange = NONE_VALUE;
            }
            else
            {
                double rx1 = dopplerFFTMatrixRx1[maxRange, maxSpeed].Phase;
                double rx3 = dopplerFFTMatrixRx3[maxRange, maxSpeed].Phase;

                hdiff = getAngleDiff(rx1, rx3);
            }

            if (lastState == NONE_VALUE && maxRange != NONE_VALUE)
            {
                OnHandDetected?.Invoke(this, true);
            }
            else if (lastState != NONE_VALUE && maxRange == NONE_VALUE)
            {
                OnHandDetected?.Invoke(this, false);
            }
            lastState = maxRange;

            switch(status)
            {
                case ClickDetectorStatus.WAIT_NOTHING_BEFORE:
                    if (maxRange == NONE_VALUE) noneCount++;
                    else noneCount = 0;

                    if (noneCount >= NOTHING_BEFORE_COUNT)
                    {
                        noneCount = 0;
                        status = ClickDetectorStatus.WAIT_START_POINT;
                        OnReadyForNextAction?.Invoke(this, true);
                    }
                    break;

                case ClickDetectorStatus.WAIT_START_POINT:
                    if (maxRange != NONE_VALUE)
                    {
                        startPoint = maxRange;
                        minValue = startPoint;
                        startTime = 0;
                        status = ClickDetectorStatus.WAIT_DECREASE;
                        noneDuringDecreaseCount = 0;
                        hAtStart = hdiff;
                        Debug("WAIT_DECREASE");
                        OnReadyForNextAction?.Invoke(this, false);
                    }
                    break;

                case ClickDetectorStatus.WAIT_DECREASE:
                    {
                        if (maxRange == NONE_VALUE)
                        {
                            noneDuringDecreaseCount++;
                            if (noneDuringDecreaseCount > NOTHING_DURING_DECREASE_MAX)
                            {
                                status = ClickDetectorStatus.WAIT_NOTHING_BEFORE;
                                Debug("WAIT_NOTHING_BEFORE - none value");
                                break;
                            }
                            else
                            {
                                // Nothing to do
                                break;
                            }
                        }

                        noneDuringDecreaseCount = 0;

                        if (maxRange < minValue)
                        {
                            hAtMin = hdiff;
                            minValue = maxRange;
                        }

                        int diff = startPoint - minValue;
                        if (diff >= DECREASE_AMPLITUDE)
                        {
                            status = ClickDetectorStatus.WAIT_INCREASE;
                            Debug("WAIT_INCREASE");
                            startTime = 0;
                            break;
                        }

                        startTime++;
                        if (startTime > START_TO_MIN)
                        {
                            // Timeout
                            status = ClickDetectorStatus.WAIT_NOTHING_BEFORE;
                            Debug("Timeout WAIT_NOTHING_BEFORE");
                            break;
                        }
                    }
                    break;

                case ClickDetectorStatus.WAIT_INCREASE:
                    {
                        if (maxRange == NONE_VALUE)
                        {
                            status = ClickDetectorStatus.WAIT_NOTHING_BEFORE;
                            Debug("WAIT_NOTHING_BEFORE - none value");
                            break;
                        }

                        if (maxRange < minValue)
                        {
                            hAtMin = hdiff;
                            minValue = maxRange;
                        }

                        int diff = maxRange - minValue;
                        if (diff >= INCREASE_AMPLITUDE)
                        {
                            status = ClickDetectorStatus.WAIT_NOTHING_AFTER;
                            startTime = 0;
                            hAtLast = hdiff;
                            Debug("WAIT_NOTHING_AFTER");
                            Debug(string.Format("Start: {0}", hAtStart));
                            Debug(string.Format("Min: {0}", hAtMin));
                            Debug(string.Format("Last: {0}", hAtLast));
                            break;
                        }

                        startTime++;
                        if (startTime > MIN_TO_STOP)
                        {
                            // Timeout
                            status = ClickDetectorStatus.WAIT_NOTHING_BEFORE;
                            Debug("Timeout WAIT_NOTHING_BEFORE");
                            break;
                        }
                    }
                    break;

                case ClickDetectorStatus.WAIT_NOTHING_AFTER:
                    {
                        if (maxRange == NONE_VALUE)
                        {
                            int direction = DIRECTION_NONE;
                            // Click detected
                            System.Diagnostics.Debug.WriteLine("Click detected " + clickCount.ToString());

                            /*
                            if ((hAtStart < (-Math.PI / 4))
                                && (hAtLast > (0))
                                && (hAtMin > hAtStart)
                                && (hAtMin < hAtLast))
                            {
                                direction = DIRECTION_RIGHT;
                            }
                            else if ((hAtStart > (Math.PI / 4))
                                && (hAtLast < (0))
                                && (hAtMin < hAtStart)
                                && (hAtMin > hAtLast))
                            {
                                direction = DIRECTION_LEFT;
                            }
                            */
                            if (hAtLast > hAtStart)
                            {
                                double diff = hAtLast - hAtStart;
                                if (diff > (Math.PI / 2)) direction = DIRECTION_RIGHT;
                            }
                            else
                            {
                                double diff = hAtStart - hAtLast;
                                if (diff > (Math.PI / 2)) direction = DIRECTION_LEFT;
                            }

                            OnNewClick?.Invoke(this, direction);
                            clickCount++;
                            status = ClickDetectorStatus.WAIT_NOTHING_BEFORE;
                            break;
                        }

                        startTime++;
                        if (startTime > MIN_TO_STOP)
                        {
                            // Timeout
                            status = ClickDetectorStatus.WAIT_NOTHING_BEFORE;
                            Debug("Timeout WAIT_MOTHING_BEFORE");
                            break;
                        }

                        break;
                    }
            }
        }
    }
}
