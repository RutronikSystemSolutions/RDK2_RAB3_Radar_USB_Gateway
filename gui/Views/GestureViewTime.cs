using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RDK2_Radar_SignalProcessing_GUI.Views
{
    public partial class GestureViewTime : UserControl
    {
        private double threshold = 0.1;

        /// <summary>
        /// X Axis
        /// </summary>
        LinearAxis xAxisTimeDomain = new LinearAxis
        {
            MajorGridlineStyle = LineStyle.Dot,
            Position = AxisPosition.Bottom,
            AxislineStyle = LineStyle.Solid,
            AxislineColor = OxyColors.Gray,
            FontSize = 10,
            PositionAtZeroCrossing = true,
            IsPanEnabled = false,
            IsZoomEnabled = true,
            Unit = "Time"
        };

        /// <summary>
        /// Y Axis for amplitude
        /// </summary>
        private LinearAxis yAxisTimeDomain = new LinearAxis
        {
            MajorGridlineStyle = LineStyle.Dot,
            AxislineStyle = LineStyle.Solid,
            AxislineColor = OxyColors.Gray,
            FontSize = 10,
            TextColor = OxyColors.Gray,
            Position = AxisPosition.Left,
            IsPanEnabled = false,
            IsZoomEnabled = true,
            Unit = "Magnitude",
            Key = "Amp",
            Minimum = -Math.PI,
            Maximum = Math.PI
        };

        /// <summary>
        /// X Axis
        /// </summary>
        LinearAxis xAxisFreqDomain = new LinearAxis
        {
            MajorGridlineStyle = LineStyle.Dot,
            Position = AxisPosition.Bottom,
            AxislineStyle = LineStyle.Solid,
            AxislineColor = OxyColors.Gray,
            FontSize = 10,
            PositionAtZeroCrossing = true,
            IsPanEnabled = false,
            IsZoomEnabled = true,
            Unit = "Freq"
        };

        /// <summary>
        /// Y Axis for amplitude
        /// </summary>
        private LinearAxis yAxisFreqDomain = new LinearAxis
        {
            MajorGridlineStyle = LineStyle.Dot,
            AxislineStyle = LineStyle.Solid,
            AxislineColor = OxyColors.Gray,
            FontSize = 10,
            TextColor = OxyColors.Gray,
            Position = AxisPosition.Left,
            IsPanEnabled = false,
            IsZoomEnabled = true,
            Unit = "Magnitude",
            Key = "Amp",
        };

        private LineSeries hLineSeries = new LineSeries();
        private LineSeries vLineSeries = new LineSeries();
        private double timeIndex = 0;

        private LineSeries phaseHLineSeries = new LineSeries();
        private LineSeries phaseVLineSeries = new LineSeries();


        private LineSeries rotationDetectedLineSeries = new LineSeries();

        private LineSeries fftHPhaseLineSeries = new LineSeries();
        private LineSeries fftVPhaseLineSeries = new LineSeries();

        public GestureViewTime()
        {
            InitializeComponent();
            InitTimePlot();
            InitFreqPlot();
        }

        private void InitTimePlot()
        {
            var timeModel = new PlotModel
            {
                PlotType = PlotType.XY,
                PlotAreaBorderThickness = new OxyThickness(0),
            };

            // Set the axes
            timeModel.Axes.Add(xAxisTimeDomain);
            timeModel.Axes.Add(yAxisTimeDomain);

            // Add series
            hLineSeries.Title = "Antenna 0";
            hLineSeries.YAxisKey = yAxisTimeDomain.Key;

            vLineSeries.Title = "Antenna 1";
            vLineSeries.YAxisKey = yAxisTimeDomain.Key;

            rotationDetectedLineSeries.Title = "Rotation";
            rotationDetectedLineSeries.YAxisKey = yAxisTimeDomain.Key;

            fftHPhaseLineSeries.Title = "H";
            fftVPhaseLineSeries.Title = "V";

            timeModel.Series.Add(hLineSeries);
            timeModel.Series.Add(vLineSeries);
            //timeModel.Series.Add(rotationDetectedLineSeries);
            //timeModel.Series.Add(fftHPhaseLineSeries);
            //timeModel.Series.Add(fftVPhaseLineSeries);

            plotView.Model = timeModel;
            plotView.InvalidatePlot(true);
        }

        private void InitFreqPlot()
        {
            var timeModel = new PlotModel
            {
                PlotType = PlotType.XY,
                PlotAreaBorderThickness = new OxyThickness(0),
            };

            // Set the axes
            timeModel.Axes.Add(xAxisFreqDomain);
            timeModel.Axes.Add(yAxisFreqDomain);

            // Add series
            phaseHLineSeries.Title = "Horizontal";
            phaseHLineSeries.YAxisKey = yAxisFreqDomain.Key;

            phaseVLineSeries.Title = "Vertical";
            phaseVLineSeries.YAxisKey = yAxisFreqDomain.Key;


            timeModel.Series.Add(phaseHLineSeries);
            timeModel.Series.Add(phaseVLineSeries);

            fftPlotView.Model = timeModel;
            fftPlotView.InvalidatePlot(true);
        }

        private int getMaxValueX(double[,] data)
        {
            int maxx = 0;
            int maxy = 0;
            double maxvalue = data[maxx, maxy];
            int beamCount = data.GetLength(0);
            int freqBinCount = data.GetLength(1);

            for (int x = 0; x < beamCount; ++x)
            {
                for (int y = 0; y < freqBinCount; ++y)
                {
                    if (data[x, y] > maxvalue)
                    {
                        maxvalue = data[x, y];
                        maxx = x;
                        maxy = y;
                    }
                }
            }
            return maxx;
        }

        private double getMaxXMaxY(System.Numerics.Complex[,] data, out int maxx, out int maxy)
        {
            maxx = 0;
            maxy = 0;
            double maxVal = data[0, 0].Magnitude;

            for (int i = 0; i < data.GetLength(0); i++)
            {
                for (int j = 0; j < data.GetLength(1); j++)
                {
                    double mag = data[i, j].Magnitude;
                    if (mag > maxVal)
                    {
                        maxx = i;
                        maxy = j;
                        maxVal = mag;
                    }
                }
            }

            return maxVal;
        }

        // Scale the phase between 0 (-pi) -> 32 (pi)
        private int scalePhase(double phase)
        {
            if (phase > Math.PI) phase = Math.PI;
            if (phase < -Math.PI) phase = -Math.PI;

            return (int)((phase + Math.PI) * 31 / 2 * Math.PI);
        }

        private double offsetH = 0;
        private double offsetV = 0;
        private double lastH = double.NaN;
        private double lastV = double.NaN;

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



        private List<double> fftInH = new List<double>();
        private List<double> fftInV = new List<double>();
        private int belowThresholdCount = 0;

        /// <summary>
        /// Check if a rotation has been detected or not
        /// </summary>
        /// <param name="spectrumHorizontal"></param>
        /// <param name="spectrumVertical"></param>
        /// <returns>0: no rotation, -1: anti-clockwise, 1: clockwise</returns>
        private double DetectRotation(System.Numerics.Complex[] spectrumHorizontal,
            System.Numerics.Complex[] spectrumVertical,
            double[] timeH, double[] timeV)
        {
            // Only check for the first 5 values
            double biggestMagnitudeH = 0;
            int hIndex = 0;
            double biggestMagnitudeV = 0;
            int vIndex = 0;

            for(int i = 0; i < 5; ++i)
            {
                if (spectrumHorizontal[i].Magnitude > biggestMagnitudeH)
                {
                    biggestMagnitudeH = spectrumHorizontal[i].Magnitude;
                    hIndex = i;
                }

                if (spectrumVertical[i].Magnitude > biggestMagnitudeV)
                {
                    biggestMagnitudeV = spectrumVertical[i].Magnitude;
                    vIndex = i;
                }
            }

            // Same biggest index?
            int diffIndex = vIndex - hIndex;
            if (diffIndex < 0) diffIndex = -diffIndex;

            if (diffIndex <= 2)
            {
                // Check the ratio
                double biggest = (biggestMagnitudeH > biggestMagnitudeV)? biggestMagnitudeH : biggestMagnitudeV;
                double smallest = (biggestMagnitudeH < biggestMagnitudeV) ? biggestMagnitudeH : biggestMagnitudeV;

                double ratio = smallest / biggest;

                // Both bigger than threshold
                if ((smallest > 0.3) && (ratio > 0.6))
                {
                    double biggestTimeH = 0;
                    double biggestTimeV = 0;
                    int biggestTimeIndexH = 0;
                    int biggestTimeIndexV = 0;
                    for (int j = 0; j < timeH.Length; ++j)
                    {
                        if (timeH[j] > biggestTimeH)
                        {
                            biggestTimeH = timeH[j];
                            biggestTimeIndexH = j;
                        }

                        if (timeV[j] > biggestTimeV)
                        {
                            biggestTimeV = timeV[j];
                            biggestTimeIndexV = j;
                        }
                    }

                    // Between -16 and 16
                    int diff = biggestTimeIndexH - biggestTimeIndexV;
                    if (diff > 16)
                    {
                        diff = diff - 32;
                    }
                    if (diff < -16)
                    {
                        diff = diff + 32;
                    }

                    return ((double)diff / 16);
                    //return ((biggestTimeIndexH - biggestTimeIndexV));

                    //// Return the phase difference
                    //double phaseH = spectrumHorizontal[hIndex].Phase;
                    //double phaseV = spectrumVertical[vIndex].Phase;

                    //if (phaseH > phaseV) return -1;
                    //else return 1;
                }
            }

            return 0;
        }

        private void GetPhase(System.Numerics.Complex[] spectrumHorizontal,
            System.Numerics.Complex[] spectrumVertical,
            out double phaseH, out double phaseV, out double phaseDiff)
        {
            // Only check for the first 5 values
            double biggestMagnitudeH = 0;
            int hIndex = 0;
            double biggestMagnitudeV = 0;
            int vIndex = 0;

            for (int i = 0; i < 5; ++i)
            {
                if (spectrumHorizontal[i].Magnitude > biggestMagnitudeH)
                {
                    biggestMagnitudeH = spectrumHorizontal[i].Magnitude;
                    hIndex = i;
                }

                if (spectrumVertical[i].Magnitude > biggestMagnitudeV)
                {
                    biggestMagnitudeV = spectrumVertical[i].Magnitude;
                    vIndex = i;
                }
            }

            phaseH = spectrumHorizontal[hIndex].Phase;
            phaseV = spectrumVertical[vIndex].Phase;

            // In case diff bigger than Pi -> normalize
            double diff = phaseH - phaseV;
            if (diff > Math.PI)
            {
                phaseV = phaseV + 2 * Math.PI;
            }
            else if (diff < (-Math.PI))
            {
                phaseV = phaseV - 2 * Math.PI;
            }

            phaseDiff = phaseH - phaseV;
        }

        private int sameSignSince = 0;
        private bool isPositive = false;

        /// <summary>
        /// System.Numerics.Complex[,] dopplerFFTMatrix = new System.Numerics.Complex[spectrumLen, radarConfiguration.ChirpsPerFrame];
        /// so x is range and y is the velocity
        /// 
        /// 
        /// Rx1 - Rx3 -> horizontal
        /// Rx2 - Rx3 -> vertical
        /// </summary>
        /// <param name="dopplerFFTMatrixRx1"></param>
        /// <param name="dopplerFFTMatrixRx2"></param>
        /// <param name="dopplerFFTMatrixRx3"></param>
        public void UpdateData(System.Numerics.Complex[,] dopplerFFTMatrixRx1, System.Numerics.Complex[,] dopplerFFTMatrixRx2, System.Numerics.Complex[,] dopplerFFTMatrixRx3)
        {
            int maxX = 0;
            int maxY = 0;
            double maxVal = getMaxXMaxY(dopplerFFTMatrixRx1, out maxX, out maxY);

            if (maxVal > threshold)
            {
                belowThresholdCount = 0;

                double rx1 = dopplerFFTMatrixRx1[maxX, maxY].Phase;
                double rx2 = dopplerFFTMatrixRx2[maxX, maxY].Phase;
                double rx3 = dopplerFFTMatrixRx3[maxX, maxY].Phase;

                double hPhase = getAngleDiff(rx1, rx3);
                double vPhase = getAngleDiff(rx2, rx3);

                hLineSeries.Points.Add(new DataPoint(timeIndex, hPhase));
                vLineSeries.Points.Add(new DataPoint(timeIndex, vPhase));

                fftInH.Add(hPhase);
                fftInV.Add(vPhase);

                // Need at least 32 points (~ 1 second of data)
                if (hLineSeries.Points.Count > 32)
                {
                    int fftPointCount = 32;

                    // In case movement since more time -> use longer integration time (better resolution)
                    //if (hLineSeries.Points.Count > 64)
                    //{
                    //    fftPointCount = 64;
                    //    hLineSeries.Points.RemoveAt(0);
                    //    vLineSeries.Points.RemoveAt(0);

                    //    fftInH.RemoveAt(0);
                    //    fftInV.RemoveAt(0);
                    //}

                    // Let the last 5 seconds points
                    //if (hLineSeries.Points.Count > (5*30))
                    if (hLineSeries.Points.Count > (32))
                    {
                        hLineSeries.Points.RemoveAt(0);
                        vLineSeries.Points.RemoveAt(0);
                        rotationDetectedLineSeries.Points.RemoveAt(0);
                        fftHPhaseLineSeries.Points.RemoveAt(0);
                        fftVPhaseLineSeries.Points.RemoveAt(0);

                        fftInH.RemoveAt(0);
                        fftInV.RemoveAt(0);
                    }

                    int startIndex = 0;
                    startIndex = fftInH.Count - fftPointCount;

                    // Compute FFT
                    // Compute real FFT
                    // Size of spectrum is (SamplesPerChirp / 2) + 1
                    //double[] fftIn = new double[fftInH.Count];
                    double[] hFftIn = new double[fftPointCount];

                    //for (int i = 0; i < fftInH.Count; i++)
                    for (int i = startIndex; i < (startIndex + fftPointCount); i++)
                    {
                        hFftIn[i - startIndex] = fftInH[i];
                    }

                    // Compute the average and remove it
                    double average = ArrayUtils.getAverage(hFftIn);
                    ArrayUtils.offsetInPlace(hFftIn, -average);

                    System.Numerics.Complex[] spectrumHorizontal = FftSharp.FFT.ForwardReal(hFftIn);

                    phaseHLineSeries.Points.Clear();
                    for(int i = 0; i  < spectrumHorizontal.Length; i++)
                    {
                        double magnitude = spectrumHorizontal[i].Magnitude / fftPointCount;
                        phaseHLineSeries.Points.Add(new DataPoint(i, magnitude));
                    }


                    // --------------------------------------------------------------
                    // Vertical
                    double[] vFftIn = new double[fftPointCount];
                    for (int i = startIndex; i < (startIndex + fftPointCount); i++)
                    {
                        vFftIn[i - startIndex] = fftInV[i];
                    }

                    // Compute the average and remove it
                    average = ArrayUtils.getAverage(vFftIn);
                    ArrayUtils.offsetInPlace(vFftIn, -average);

                    System.Numerics.Complex[] spectrumVertical = FftSharp.FFT.ForwardReal(vFftIn);

                    phaseVLineSeries.Points.Clear();
                    for (int i = 0; i < spectrumVertical.Length; i++)
                    {
                        double magnitude = spectrumVertical[i].Magnitude / fftPointCount;
                        phaseVLineSeries.Points.Add(new DataPoint(i, magnitude));
                    }

                    GetPhase(spectrumHorizontal, spectrumVertical, out double phaseHFFT, out double phaseVFFT, out double phaseDiff);
                    fftHPhaseLineSeries.Points.Add(new DataPoint(timeIndex, phaseHFFT));
                    fftVPhaseLineSeries.Points.Add(new DataPoint(timeIndex, phaseVFFT));
                    //fftVPhaseLineSeries.Points.Add(new DataPoint(timeIndex, phaseDiff));
                    //fftHPhaseLineSeries.Points.Add(new DataPoint(timeIndex, double.NaN));
                    //fftVPhaseLineSeries.Points.Add(new DataPoint(timeIndex, double.NaN));

                    rotationDetectedLineSeries.Points.Add(new DataPoint(timeIndex, phaseDiff));

                    int rotDetected = 0;

                    if (phaseDiff > 0)
                    {
                        if (isPositive)
                        {
                            sameSignSince++;
                            if (sameSignSince > 8)
                            {
                                rotDetected = 1;
                            }
                        }
                        else
                        {
                            // Was negative before...
                            sameSignSince = 0;
                            rotDetected = 0;
                        }
                        isPositive = true;
                    }
                    else
                    {
                        if (isPositive == false)
                        {
                            sameSignSince++;
                            if (sameSignSince > 8)
                            {
                                rotDetected = -1;
                            }
                        }
                        else
                        {
                            // Was positive before...
                            sameSignSince = 0;
                            rotDetected = 0;
                        }
                        isPositive = false;
                    }

                    //rotationDetectedLineSeries.Points.Add(new DataPoint(timeIndex, rotDetected));

                    /*rotationDetectedLineSeries.Points.Add(new DataPoint
                        (timeIndex, DetectRotation(spectrumHorizontal, spectrumVertical, hFftIn, vFftIn)));*/
                }
                else
                {
                    // Not enough for FFT
                    rotationDetectedLineSeries.Points.Add(new DataPoint(timeIndex, 0));
                    fftHPhaseLineSeries.Points.Add(new DataPoint(timeIndex, 0));
                    fftVPhaseLineSeries.Points.Add(new DataPoint(timeIndex, 0));
                    sameSignSince = 0;
                }

                timeIndex += 1;
            }
            else
            {
                sameSignSince = 0;
                belowThresholdCount++;
                if (belowThresholdCount >= 2)
                {
                    hLineSeries.Points.Clear();
                    vLineSeries.Points.Clear();
                    rotationDetectedLineSeries.Points.Clear();
                    fftVPhaseLineSeries.Points.Clear();
                    fftHPhaseLineSeries.Points.Clear();

                    phaseVLineSeries.Points.Clear();
                    phaseHLineSeries.Points.Clear();

                    fftInH.Clear();
                    fftInV.Clear();

                    timeIndex = 0;
                }
            }

            plotView.InvalidatePlot(true);
            fftPlotView.InvalidatePlot(true);
        }

    }
}
