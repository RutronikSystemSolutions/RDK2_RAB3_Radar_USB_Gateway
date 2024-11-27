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
    public partial class DistanceView : UserControl
    {
        private const double threshold = 0.1;
        private double index = 0;

        private double startFrequency = RadarConfiguration.START_FREQUENCY;
        private double endFrequency = RadarConfiguration.END_FREQUENCY;
        private double samplingRate = RadarConfiguration.SAMPLING_RATE;
        private int samplesPerChirp = RadarConfiguration.SAMPLES_PER_CHIRP;

        /// <summary>
        /// X Axis
        /// </summary>
        LinearAxis xAxisEnergyOverTime = new LinearAxis
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
        private LinearAxis yAxisEnergyOverTime = new LinearAxis
        {
            MajorGridlineStyle = LineStyle.Dot,
            AxislineStyle = LineStyle.Solid,
            AxislineColor = OxyColors.Gray,
            FontSize = 10,
            TextColor = OxyColors.Gray,
            Position = AxisPosition.Left,
            IsPanEnabled = false,
            IsZoomEnabled = true,
            Unit = "Distance",
            Key = "Amp",
            Minimum = 0,
            Maximum = 1
        };

        private LineSeries distanceLineSerieRx1 = new LineSeries();
        private LineSeries distanceLineSerieRx2 = new LineSeries();
        private LineSeries distanceLineSerieRx3 = new LineSeries();

        public DistanceView()
        {
            InitializeComponent();
            InitPlot();
        }

        private void InitPlot()
        {
            var timeModel = new PlotModel
            {
                PlotType = PlotType.XY,
                PlotAreaBorderThickness = new OxyThickness(0),
            };

            // Set the axes
            timeModel.Axes.Add(xAxisEnergyOverTime);
            timeModel.Axes.Add(yAxisEnergyOverTime);

            // Add series
            distanceLineSerieRx1.Title = "RX1";
            distanceLineSerieRx1.YAxisKey = yAxisEnergyOverTime.Key;

            distanceLineSerieRx2.Title = "RX2";
            distanceLineSerieRx2.YAxisKey = yAxisEnergyOverTime.Key;

            distanceLineSerieRx3.Title = "RX3";
            distanceLineSerieRx3.YAxisKey = yAxisEnergyOverTime.Key;

            timeModel.Series.Add(distanceLineSerieRx1);
            timeModel.Series.Add(distanceLineSerieRx2);
            timeModel.Series.Add(distanceLineSerieRx3);

            plotView.Model = timeModel;
            plotView.InvalidatePlot(true);
        }

        private double indexToRange(int index)
        {
            double bandWidth = endFrequency - startFrequency;
            double celerity = 299792458;
            double fftLen = (samplesPerChirp / 2) + 1;
            double slope = bandWidth / (samplesPerChirp * (1 / samplingRate));

            double fractionFs = index / ((fftLen - 1) * 2);
            double freq = fractionFs * samplingRate;
            double rangeMeters = (celerity * freq) / (2 * slope);

            return rangeMeters;
        }

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
                distanceLineSerieRx1.Points.Add(new DataPoint(index, indexToRange(maxRange)));
            }
            else
            {
                distanceLineSerieRx1.Points.Add(new DataPoint(index, double.NaN));
            }

            getMaxAmplitudeRange(dopplerFFTMatrixRx2, out maxRange, out maxMag);
            if (maxMag > threshold)
            {
                distanceLineSerieRx2.Points.Add(new DataPoint(index, indexToRange(maxRange)));
            }
            else
            {
                distanceLineSerieRx2.Points.Add(new DataPoint(index, double.NaN));
            }

            getMaxAmplitudeRange(dopplerFFTMatrixRx3, out maxRange, out maxMag);
            if (maxMag > threshold)
            {
                distanceLineSerieRx3.Points.Add(new DataPoint(index, indexToRange(maxRange)));
            }
            else
            {
                distanceLineSerieRx3.Points.Add(new DataPoint(index, double.NaN));
            }


            index += 1;

            if (distanceLineSerieRx1.Points.Count > 500)
            {
                distanceLineSerieRx1.Points.RemoveAt(0);
                distanceLineSerieRx2.Points.RemoveAt(0);
                distanceLineSerieRx3.Points.RemoveAt(0);
            }

            plotView.InvalidatePlot(true);
        }
    
    }
}
