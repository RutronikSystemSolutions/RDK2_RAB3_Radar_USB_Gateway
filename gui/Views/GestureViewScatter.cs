using OxyPlot.Axes;
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
using OxyPlot.Series;
using System.Reflection;

namespace RDK2_Radar_SignalProcessing_GUI.Views
{
    public partial class GestureViewScatter : UserControl
    {
        private const int splitCount = 128;
        private double threshold = 0.1;
        private int minRange = 0;
        private int maxRange = (RadarConfiguration.SAMPLES_PER_CHIRP / 2) + 1;
        private const double maxColorValue = 100;
        private const double memoryDurationMs = 1000;

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
            Maximum = splitCount,
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
            Maximum = splitCount,
            Unit = "Y"
        };

        private ScatterSeries detectedPresenceSeries = new ScatterSeries();
        private ScatterSeries lastValueSeries = new ScatterSeries();

        public GestureViewScatter()
        {
            InitializeComponent();
            InitPlot();

            timer.Interval = 100;
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
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

        private void InitPlot()
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

            lastValueSeries.MarkerSize = 10;
            lastValueSeries.MarkerFill = OxyColors.Red;
            lastValueSeries.MarkerStroke = OxyColors.Black;

            timeModel.Series.Add(detectedPresenceSeries);
            timeModel.Series.Add(lastValueSeries);

            plotView.Model = timeModel;
            plotView.InvalidatePlot(true);
        }

        /// <summary>
        /// Scale the phase between 0 (-pi) -> splitCount - 1 (pi)
        /// </summary>
        /// <param name="phase"></param>
        /// <returns></returns>
        private int scalePhase(double phase)
        {
            if (phase > Math.PI) phase = Math.PI;
            if (phase < -Math.PI) phase = -Math.PI;

            phase = phase + Math.PI;
            phase = (phase * splitCount) / (2 * Math.PI);

            int retval = (int)phase;
            if (retval < 0) retval = 0;
            if (retval >= splitCount) retval = splitCount - 1;

            return retval;
        }

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

        public void UpdateData(System.Numerics.Complex[,] dopplerFFTMatrixRx1, System.Numerics.Complex[,] dopplerFFTMatrixRx2, System.Numerics.Complex[,] dopplerFFTMatrixRx3)
        {
            // Should never happen but let be sure of it
            if (dopplerFFTMatrixRx1 == null) return;
            if (dopplerFFTMatrixRx2 == null) return;
            if (dopplerFFTMatrixRx3 == null) return;

            double maxDetectedMag = 0;
            int maxX = 0;
            int maxY = 0;
            int maxDetectedRange = 0;

            // Check for all values above the threshold
            //for (int i = minRange; i < maxRange; i++)
            for (int i = minRange; i < dopplerFFTMatrixRx1.GetLength(0); i++)
            {
                for (int j = 0; j < dopplerFFTMatrixRx1.GetLength(1); j++)
                {
                    double magnitude = dopplerFFTMatrixRx1[i, j].Magnitude;
                    if (magnitude > threshold)
                    {
                        double rx1 = dopplerFFTMatrixRx1[i, j].Phase;
                        double rx2 = dopplerFFTMatrixRx2[i, j].Phase;
                        double rx3 = dopplerFFTMatrixRx3[i, j].Phase;

                        double hPhase = getAngleDiff(rx1, rx3);
                        double vPhase = getAngleDiff(rx2, rx3);

                        int xindex = scalePhase(hPhase);
                        int yindex = scalePhase(vPhase);

                        // Add to the history
                        //data.Add(new DataWithTimeStamp(xindex, yindex, i, magnitude));

                        if (magnitude > maxDetectedMag)
                        {
                            maxDetectedMag = magnitude;
                            maxX = xindex;
                            maxY = yindex;
                            maxDetectedRange = i;
                        }
                    }
                }
            }

            if (maxDetectedRange < minRange || maxDetectedRange > maxRange)
            {
                maxDetectedMag = 0;
            }

            if (maxDetectedMag > threshold)
            {
                if (maxY > 100)
                {
                    maxY = maxY - 100;
                }
                else
                {
                    maxY = maxY + 28;
                }
                data.Add(new DataWithTimeStamp(maxX, maxY, maxDetectedRange, maxDetectedMag));
            }
        }

        private void Timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            // Only show the last x seconds
            double timeMs = DateTimeOffset.Now.ToUnixTimeMilliseconds();

            double deadline = timeMs - memoryDurationMs;

            // First delele (points are sorted)
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
                //detectedPresenceSeries.Points.Add(new ScatterPoint(x: data[i].x, y: data[i].y, value: data[i].range));
                //detectedPresenceSeries.Points.Add(new ScatterPoint(x: data[i].x, y: data[i].y, value: data.Count - i));

                // 0 -> 0ms
                // maxColorValue -> memoryDurationMs

                double zvalue = memoryDurationMs -  (data[i].timestamp - deadline);
                zvalue = (zvalue * maxColorValue) / memoryDurationMs;

                detectedPresenceSeries.Points.Add(new ScatterPoint(x: data[i].x, y: data[i].y, value: zvalue));
            }

            lastValueSeries.Points.Clear();
            if (data.Count > 0)
            {
                //lastValueSeries.Points.Add(new ScatterPoint(x: data[data.Count - 1].x, y: data[data.Count - 1].y, value: data[data.Count - 1].range));
            }

            plotView.InvalidatePlot(true);
        }
    }
}
