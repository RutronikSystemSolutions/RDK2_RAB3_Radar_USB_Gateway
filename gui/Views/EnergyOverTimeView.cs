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
    public partial class EnergyOverTimeView : UserControl
    {
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
            Unit = "Magnitude",
            Key = "Amp",
        };

        private LineSeries energyOverTimeAntenna0LineSeries = new LineSeries();
        private LineSeries energyOverTimeAntenna1LineSeries = new LineSeries();
        private LineSeries dynamicThresholdLineSeries = new LineSeries();

        private double timeIndex = 0;

        public EnergyOverTimeView()
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
            energyOverTimeAntenna0LineSeries.Title = "Antenna 0";
            energyOverTimeAntenna0LineSeries.YAxisKey = yAxisEnergyOverTime.Key;

            energyOverTimeAntenna1LineSeries.Title = "Antenna 1";
            energyOverTimeAntenna1LineSeries.YAxisKey = yAxisEnergyOverTime.Key;

            dynamicThresholdLineSeries.Title = "Threshold";
            dynamicThresholdLineSeries.YAxisKey = yAxisEnergyOverTime.Key;

            timeModel.Series.Add(energyOverTimeAntenna0LineSeries);
            timeModel.Series.Add(energyOverTimeAntenna1LineSeries);
            timeModel.Series.Add(dynamicThresholdLineSeries);

            plotView.Model = timeModel;
            plotView.InvalidatePlot(true);
        }

        public void updateData(double energy, double threshold, int antennaIndex)
        {
            if (antennaIndex == 0)
            {
                energyOverTimeAntenna0LineSeries.Points.Add(new DataPoint(timeIndex, energy));
                dynamicThresholdLineSeries.Points.Add(new DataPoint(timeIndex, threshold));
                if (energyOverTimeAntenna0LineSeries.Points.Count > 500)
                {
                    energyOverTimeAntenna0LineSeries.Points.RemoveAt(0);
                    dynamicThresholdLineSeries.Points.RemoveAt(0);
                }
            }
            else if (antennaIndex == 1)
            {
                energyOverTimeAntenna1LineSeries.Points.Add(new DataPoint(timeIndex, energy));
                if (energyOverTimeAntenna1LineSeries.Points.Count > 500) energyOverTimeAntenna1LineSeries.Points.RemoveAt(0);
                timeIndex += 1;
                plotView.InvalidatePlot(true);
            }
        }
    }
}
