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

            timeModel.Series.Add(hLineSeries);
            timeModel.Series.Add(vLineSeries);

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

        public void UpdateData(System.Numerics.Complex[,] dopplerFFTMatrixRx1, System.Numerics.Complex[,] dopplerFFTMatrixRx2, System.Numerics.Complex[,] dopplerFFTMatrixRx3)
        {
            // System.Numerics.Complex[,] dopplerFFTMatrix = new System.Numerics.Complex[spectrumLen, radarConfiguration.ChirpsPerFrame];
            // so x is range and y is the 

            // Rx1 - Rx3 -> horizontal
            // Rx2 - Rx3 -> vertical



            int maxX = 0;
            int maxY = 0;
            double maxVal = getMaxXMaxY(dopplerFFTMatrixRx1, out maxX, out maxY);

            if (maxVal > 0.1)
            {
                /*
                // horizontal angle index (1 - 3)
                double horizontalPhase = dopplerFFTMatrixRx1[maxX, maxY].Phase - dopplerFFTMatrixRx3[maxX, maxY].Phase;
                double verticalPhase = dopplerFFTMatrixRx2[maxX, maxY].Phase - dopplerFFTMatrixRx3[maxX, maxY].Phase;

                double delta = 0;
                if (double.IsNaN(lastH) == false)
                {
                    delta = horizontalPhase - lastH;
                    if (delta > Math.PI) offsetH = offsetH - 2 * Math.PI;
                    else if (delta < -Math.PI) offsetH = offsetH + 2 * Math.PI;
                }
                lastH = horizontalPhase;

                delta = 0;
                if (double.IsNaN(lastV) == false)
                {
                    delta = verticalPhase - lastV;
                    if (delta > Math.PI) offsetV = offsetV - 2 * Math.PI;
                    else if (delta < -Math.PI) offsetV = offsetV + 2 * Math.PI;
                }
                lastV = verticalPhase;


                hLineSeries.Points.Add(new DataPoint(timeIndex, horizontalPhase + offsetH));
                vLineSeries.Points.Add(new DataPoint(timeIndex, verticalPhase + offsetV));
                */
                double rx1 = dopplerFFTMatrixRx1[maxX, maxY].Phase;
                double rx2 = dopplerFFTMatrixRx2[maxX, maxY].Phase;
                double rx3 = dopplerFFTMatrixRx3[maxX, maxY].Phase;

                double hPhase = getAngleDiff(rx1, rx3);
                double vPhase = getAngleDiff(rx2, rx3);

                hLineSeries.Points.Add(new DataPoint(timeIndex, hPhase));
                vLineSeries.Points.Add(new DataPoint(timeIndex, vPhase));

                fftInH.Add(hPhase);
                fftInV.Add(vPhase);

                //if (hLineSeries.Points.Count > 64)
                if (hLineSeries.Points.Count > 32)
                {
                    int fftPointCount = 32;

                    if (hLineSeries.Points.Count > 64)
                    {
                        fftPointCount = 64;
                        hLineSeries.Points.RemoveAt(0);
                        vLineSeries.Points.RemoveAt(0);

                        fftInH.RemoveAt(0);
                        fftInV.RemoveAt(0);
                    }

                    int startIndex = 0;
                    startIndex = fftInH.Count - fftPointCount;

                    // Compute FFT
                    // Compute real FFT
                    // Size of spectrum is (SamplesPerChirp / 2) + 1
                    //double[] fftIn = new double[fftInH.Count];
                    double[] fftIn = new double[fftPointCount];

                    //for (int i = 0; i < fftInH.Count; i++)
                    for (int i = startIndex; i < (startIndex + fftPointCount); i++)
                    {
                        fftIn[i - startIndex] = fftInH[i];
                    }

                    // Compute the average and remove it
                    double average = ArrayUtils.getAverage(fftIn);
                    ArrayUtils.offsetInPlace(fftIn, -average);

                    System.Numerics.Complex[] spectrum = FftSharp.FFT.ForwardReal(fftIn);

                    phaseHLineSeries.Points.Clear();
                    for(int i = 0; i  < spectrum.Length; i++)
                    {
                        phaseHLineSeries.Points.Add(new DataPoint(i, spectrum[i].Magnitude));
                    }

                    //for (int i = 0; i < fftInV.Count; i++)
                    for (int i = startIndex; i < (startIndex + fftPointCount); i++)
                    {
                        fftIn[i - startIndex] = fftInV[i];
                    }

                    // Compute the average and remove it
                    average = ArrayUtils.getAverage(fftIn);
                    ArrayUtils.offsetInPlace(fftIn, -average);

                    spectrum = FftSharp.FFT.ForwardReal(fftIn);

                    phaseVLineSeries.Points.Clear();
                    for (int i = 0; i < spectrum.Length; i++)
                    {
                        phaseVLineSeries.Points.Add(new DataPoint(i, spectrum[i].Magnitude));
                    }
                }
            }
            else
            {
                hLineSeries.Points.Clear();
                vLineSeries.Points.Clear();

                phaseVLineSeries.Points.Clear();
                phaseHLineSeries.Points.Clear();

                fftInH.Clear();
                fftInV.Clear();
            }

            timeIndex += 1;
            plotView.InvalidatePlot(true);
            fftPlotView.InvalidatePlot(true);
        }

    }
}
