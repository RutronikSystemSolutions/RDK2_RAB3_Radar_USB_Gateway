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

namespace RDK2_Radar_SignalProcessing_GUI.Views
{
    public partial class RangeFFTView : UserControl
    {
        private object sync = new object();

        private double startFrequency = RadarConfiguration.START_FREQUENCY;
        private double endFrequency = RadarConfiguration.END_FREQUENCY;
        private double samplingRate = RadarConfiguration.SAMPLING_RATE;
        private int samplesPerChirp = RadarConfiguration.SAMPLES_PER_CHIRP;

        /// <summary>
        /// X Axis
        /// </summary>
        LinearAxis xAxisSpectrum = new LinearAxis
        {
            MajorGridlineStyle = LineStyle.Dot,
            Position = AxisPosition.Bottom,
            AxislineStyle = LineStyle.Solid,
            AxislineColor = OxyColors.Gray,
            FontSize = 10,
            PositionAtZeroCrossing = true,
            IsPanEnabled = false,
            IsZoomEnabled = true,
            Unit = "Range (m)"
        };

        /// <summary>
        /// Y Axis for amplitude
        /// </summary>
        private LinearAxis yAxisSpectrum = new LinearAxis
        {
            MajorGridlineStyle = LineStyle.Dot,
            AxislineStyle = LineStyle.Solid,
            AxislineColor = OxyColors.Gray,
            FontSize = 10,
            Minimum = -120,
            Maximum = 0,
            TextColor = OxyColors.Gray,
            Position = AxisPosition.Left,
            IsPanEnabled = false,
            IsZoomEnabled = true,
            Unit = "Magnitude (dBFS)",
            Key = "Amp",
        };

        private LineSeries spectrumAntenna0LineSeries = new LineSeries();
        private LineSeries spectrumAntenna1LineSeries = new LineSeries();
        private LineSeries spectrumAntenna2LineSeries = new LineSeries();

        private System.Timers.Timer timer = new System.Timers.Timer();

        public RangeFFTView()
        {
            InitializeComponent();
            InitPlot();

            timer.Interval = 100;
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        private void Timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            lock (sync)
            {
                plotView.InvalidatePlot(true);
            }
        }

        public void setSpectrumDBFS(double[] spectrum, int antennaIndex)
        {
            double bandWidth = endFrequency - startFrequency;
            double celerity = 299792458;
            double fftLen = spectrum.Length;
            double slope = bandWidth / (samplesPerChirp * (1 / samplingRate));

            lock (sync)
            {
                if (antennaIndex == 0)
                {
                    spectrumAntenna0LineSeries.Points.Clear();
                    for (int i = 0; i < spectrum.Length; ++i)
                    {
                        double fractionFs = i / ((fftLen - 1) * 2);
                        double freq = fractionFs * samplingRate;
                        double rangeMeters = (celerity * freq) / (2 * slope);
                        spectrumAntenna0LineSeries.Points.Add(new DataPoint(rangeMeters, spectrum[i]));
                    }
                }
                else if (antennaIndex == 1)
                {
                    spectrumAntenna1LineSeries.Points.Clear();
                    for (int i = 0; i < spectrum.Length; ++i)
                    {
                        double fractionFs = i / ((fftLen - 1) * 2);
                        double freq = fractionFs * samplingRate;
                        double rangeMeters = (celerity * freq) / (2 * slope);
                        spectrumAntenna1LineSeries.Points.Add(new DataPoint(rangeMeters, spectrum[i]));
                    }
                }
                else if (antennaIndex == 2)
                {
                    spectrumAntenna2LineSeries.Points.Clear();
                    for (int i = 0; i < spectrum.Length; ++i)
                    {
                        double fractionFs = i / ((fftLen - 1) * 2);
                        double freq = fractionFs * samplingRate;
                        double rangeMeters = (celerity * freq) / (2 * slope);
                        spectrumAntenna2LineSeries.Points.Add(new DataPoint(rangeMeters, spectrum[i]));
                    }
                }
            }
        }

        private void InitPlot()
        {
            // Spectrum
            var timeModel = new PlotModel
            {
                PlotType = PlotType.XY,
                PlotAreaBorderThickness = new OxyThickness(0),
            };

            // Set the axes
            timeModel.Axes.Add(xAxisSpectrum);
            timeModel.Axes.Add(yAxisSpectrum);

            // Add series
            spectrumAntenna0LineSeries.Title = "Antenna 0";
            spectrumAntenna0LineSeries.YAxisKey = yAxisSpectrum.Key;

            spectrumAntenna1LineSeries.Title = "Antenna 1";
            spectrumAntenna1LineSeries.YAxisKey = yAxisSpectrum.Key;

            spectrumAntenna2LineSeries.Title = "Antenna 2";
            spectrumAntenna2LineSeries.YAxisKey = yAxisSpectrum.Key;

            timeModel.Series.Add(spectrumAntenna0LineSeries);
            timeModel.Series.Add(spectrumAntenna1LineSeries);
            timeModel.Series.Add(spectrumAntenna2LineSeries);

            plotView.Model = timeModel;
            plotView.InvalidatePlot(true);
        }
    }
}
