using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OxyPlot.Axes;
using OxyPlot;
using OxyPlot.Series;

namespace RDK2_Radar_SignalProcessing_GUI.Views
{
    public partial class DBFDopplerView : UserControl
    {
        private double threshold = 0.1;
        private int minRange = 0;
        private int maxRange = (RadarConfiguration.SAMPLES_PER_CHIRP / 2) + 1;

        private const int BEAM_COUNT = 27;
        private const double ANGLE_MAX = 90;
        private const double INVALID_VALUE = -10;

        private DopplerDBF dopplerDBF = new DopplerDBF(2, BEAM_COUNT, ANGLE_MAX, 0.5);


        private class DataWithTimeStamp
        {
            public int x;
            public int y;
            public int range;
            public double magnitude;
            public double timestamp;

            public DataWithTimeStamp(int x, int y, int range, double magnitude)
            {
                this.x = x;
                this.y = y;
                this.range = range;
                this.magnitude = magnitude;
                timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            }
        }

        private List<DataWithTimeStamp> data = new List<DataWithTimeStamp>();
        private System.Timers.Timer timer = new System.Timers.Timer();


        #region "Scatter series"

        private const double maxColorValue = 100;
        private const double memoryDurationMs = 1000;

        /// <summary>
        /// X Axis
        /// </summary>
        LinearAxis xAxis = new LinearAxis
        {
            MajorGridlineStyle = LineStyle.Dot,
            Position = AxisPosition.Bottom,
            AxislineStyle = LineStyle.Solid,
            AxislineColor = OxyColors.Gray,
            FontSize = 10,
            Minimum = 0,
            Maximum = BEAM_COUNT,
            AbsoluteMinimum = 0,
            AbsoluteMaximum = BEAM_COUNT,
            PositionAtZeroCrossing = true,
            IsPanEnabled = false,
            IsZoomEnabled = true,
            Unit = "X"
        };

        /// <summary>
        /// Y Axis
        /// </summary>
        private LinearAxis yAxis = new LinearAxis
        {
            MajorGridlineStyle = LineStyle.Dot,
            AxislineStyle = LineStyle.Solid,
            AxislineColor = OxyColors.Gray,
            FontSize = 10,
            TextColor = OxyColors.Gray,
            Position = AxisPosition.Left,
            IsPanEnabled = false,
            IsZoomEnabled = true,
            Minimum = 0,
            Maximum = BEAM_COUNT,
            AbsoluteMinimum = 0,
            AbsoluteMaximum = BEAM_COUNT,
            Unit = "Y"
        };

        private ScatterSeries detectedPresenceSeries = new ScatterSeries();

        #endregion

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
            Minimum = 0,
            Maximum = BEAM_COUNT,
            AbsoluteMinimum = 0,
            AbsoluteMaximum = BEAM_COUNT
        };

        private LineSeries hLineSeries = new LineSeries();
        private LineSeries vLineSeries = new LineSeries();
        private double timeIndex = 0;

        public DBFDopplerView()
        {
            InitializeComponent();
            InitTimePlot();
            InitScatterPlot();

            timer.Interval = 100;
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        private void InitScatterPlot()
        {
            // Raw signals plot
            var timeModel = new PlotModel
            {
                PlotType = PlotType.XY,
                PlotAreaBorderThickness = new OxyThickness(0),
            };

            // Set the axes
            timeModel.Axes.Add(xAxis);
            timeModel.Axes.Add(yAxis);

            var axis1 = new LinearColorAxis();
            axis1.Key = "ColorAxis";
            axis1.Maximum = maxColorValue;
            axis1.Minimum = 0;
            axis1.Position = AxisPosition.Top;
            timeModel.Axes.Add(axis1);

            // Add series
            detectedPresenceSeries.Title = "Presence";
            detectedPresenceSeries.ColorAxisKey = "ColorAxis";

            timeModel.Series.Add(detectedPresenceSeries);

            scatterPlotView.Model = timeModel;
            scatterPlotView.InvalidatePlot(true);
        }

        private void Timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            // Only show the last x seconds
            double timeMs = DateTimeOffset.Now.ToUnixTimeMilliseconds();

            double deadline = timeMs - memoryDurationMs;

            // First delete (points are sorted)
            int deleteCount = data.Count;
            for (int i = 0; i < data.Count; ++i)
            {
                if (data[i].timestamp > deadline)
                {
                    deleteCount = i;
                    break;
                }
            }
            data.RemoveRange(0, deleteCount);


            detectedPresenceSeries.Points.Clear();
            for (int i = 0; i < data.Count; ++i)
            {
                // 0 -> 0ms
                // maxColorValue -> memoryDurationMs

                double zvalue = memoryDurationMs - (data[i].timestamp - deadline);
                zvalue = (zvalue * maxColorValue) / memoryDurationMs;

                detectedPresenceSeries.Points.Add(new ScatterPoint(x: data[i].x, y: data[i].y, value: zvalue));
            }

            scatterPlotView.InvalidatePlot(true);
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

            hLineSeries.LineStyle = LineStyle.None;
            hLineSeries.MarkerType = MarkerType.Circle;

            vLineSeries.Title = "Antenna 1";
            vLineSeries.YAxisKey = yAxisTimeDomain.Key;

            vLineSeries.LineStyle = LineStyle.None;
            vLineSeries.MarkerType = MarkerType.Circle;

            timeModel.Series.Add(hLineSeries);
            timeModel.Series.Add(vLineSeries);

            plotView.Model = timeModel;
            plotView.InvalidatePlot(true);
        }

        private void getMaxAmplitudeRange(System.Numerics.Complex[,] dopplerFFTMatrix,
            out int maxDetectedRange, out double maxDetectedMag)
        {
            maxDetectedMag = 0;
            maxDetectedRange = 0;

            for (int i = 0; i < dopplerFFTMatrix.GetLength(0); i++)
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
        public void UpdateData(System.Numerics.Complex[,] dopplerFFTMatrixRx1, System.Numerics.Complex[,] dopplerFFTMatrixRx2, System.Numerics.Complex[,] dopplerFFTMatrixRx3)
        {
            // Should never happen but let be sure of it
            if (dopplerFFTMatrixRx1 == null) return;
            if (dopplerFFTMatrixRx2 == null) return;
            if (dopplerFFTMatrixRx3 == null) return;

            double maxMag = 0;
            int maxDetectedRange = 0;

            getMaxAmplitudeRange(dopplerFFTMatrixRx1, out maxDetectedRange, out maxMag);

            if (maxDetectedRange < minRange || maxDetectedRange > maxRange)
            {
                maxMag = 0;
            }

            if (maxMag > threshold)
            {
                // Ok, compute DBF
                // horizontal
                System.Numerics.Complex[,,] dbfRetH = dopplerDBF.Run(dopplerFFTMatrixRx1, dopplerFFTMatrixRx3);

                System.Numerics.Complex[,,] dbfRetV = dopplerDBF.Run(dopplerFFTMatrixRx2, dopplerFFTMatrixRx3);

                int maxBeamIndexH = 0;
                double maxBeamValueH = 0;

                int maxBeamIndexV = 0;
                double maxBeamValueV = 0;

                // Results with dbfRet[numRanges, numChirps, beamCount]
                int numRanges = dopplerFFTMatrixRx1.GetLength(0);
                int numChirps = dopplerFFTMatrixRx1.GetLength(1);
                for(int ibeam = 0; ibeam < BEAM_COUNT; ++ibeam)
                {
                    double sumH = 0;
                    double sumV = 0;
                    for (int irange = 0; irange < numRanges; ++irange)
                    {
                        for(int ichirp = 0; ichirp < numChirps; ++ichirp)
                        {
                            sumH += dbfRetH[irange, ichirp, ibeam].Magnitude;
                            sumV += dbfRetV[irange, ichirp, ibeam].Magnitude;
                        }
                    }
                    if (sumH > maxBeamValueH)
                    {
                        maxBeamValueH = sumH;
                        maxBeamIndexH = ibeam;
                    }

                    if (sumV > maxBeamValueV)
                    {
                        maxBeamValueV = sumV;
                        maxBeamIndexV = ibeam;
                    }
                }

                hLineSeries.Points.Add(new DataPoint(timeIndex, maxBeamIndexH));
                vLineSeries.Points.Add(new DataPoint(timeIndex, maxBeamIndexV));

                data.Add(new DataWithTimeStamp(maxBeamIndexH, maxBeamIndexV, 0, 0));
            }
            else
            {
                // Below threshold
                hLineSeries.Points.Add(new DataPoint(timeIndex, INVALID_VALUE));
                vLineSeries.Points.Add(new DataPoint(timeIndex, INVALID_VALUE));
            }

            timeIndex++;
            if (hLineSeries.Points.Count > 50)
            {
                hLineSeries.Points.RemoveAt(0);
                vLineSeries.Points.RemoveAt(0);
            }

            plotView.InvalidatePlot(true);
        }
    }
}
