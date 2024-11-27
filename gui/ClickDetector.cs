using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDK2_Radar_SignalProcessing_GUI
{
    public class ClickDetector
    {
        private double threshold = 0.1;
        private const int NONE_VALUE = -1;

        private int NOTHING_BEFORE_COUNT = 5;
        private int START_TO_MIN = 15;
        private int MIN_TO_STOP = 15;
        private int DECREASE_AMPLITUDE = 5; // ~15 cm
        private int INCREASE_AMPLITUDE = 4; // 10cm

        private int clickCount = 0;

        private int lastState = NONE_VALUE;

        public delegate void OnNewClickEventHandler(object sender);
        public event OnNewClickEventHandler? OnNewClick;

        public delegate void OnHandDetectedEventHandler(object sender, bool status);
        public event OnHandDetectedEventHandler? OnHandDetected;

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

        private int noneCount = 0;
        private int startPoint = NONE_VALUE;
        private int startTime = 0;
        private int minValue = 0;

        private void Debug(string text)
        {
            System.Diagnostics.Debug.WriteLine(text);
        }

        public void UpdateData(System.Numerics.Complex[,] dopplerFFTMatrixRx1,
            System.Numerics.Complex[,] dopplerFFTMatrixRx2,
            System.Numerics.Complex[,] dopplerFFTMatrixRx3)
        {
            int maxRange = 0;
            double maxMag = 0;

            getMaxAmplitudeRange(dopplerFFTMatrixRx1, out maxRange, out maxMag);

            // Amplitude bigger than threshold?
            if (maxMag < threshold)
            {
                maxRange = NONE_VALUE;
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
                    }
                    break;

                case ClickDetectorStatus.WAIT_START_POINT:
                    if (maxRange != NONE_VALUE)
                    {
                        startPoint = maxRange;
                        minValue = startPoint;
                        startTime = 0;
                        status = ClickDetectorStatus.WAIT_DECREASE;
                        Debug("WAIT_DECREASE");
                    }
                    break;

                case ClickDetectorStatus.WAIT_DECREASE:
                    {
                        if (maxRange == NONE_VALUE)
                        {
                            status = ClickDetectorStatus.WAIT_NOTHING_BEFORE;
                            Debug("WAIT_NOTHING_BEFORE - none value");
                            break;
                        }

                        if (maxRange < minValue) minValue = maxRange;

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

                        if (maxRange < minValue) minValue = maxRange;

                        int diff = maxRange - minValue;
                        if (diff >= INCREASE_AMPLITUDE)
                        {
                            status = ClickDetectorStatus.WAIT_NOTHING_AFTER;
                            startTime = 0;
                            Debug("WAIT_NOTHING_AFTER");
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
                            // Click detected
                            System.Diagnostics.Debug.WriteLine("Click detected " + clickCount.ToString());
                            OnNewClick?.Invoke(this);
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
