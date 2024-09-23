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
    public partial class AnglePresenceView : UserControl
    {
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
            Maximum = 31,
            PositionAtZeroCrossing = true,
            IsPanEnabled = false,
            IsZoomEnabled = true,
            Unit = "Angle"
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
            Maximum = 20,
            Unit = "Range"
        };

        
        private ScatterSeries detectedPresenceSeries = new ScatterSeries();
        private double index = 0;

        public AnglePresenceView()
        {
            InitializeComponent();
            InitPlot();
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
            axis1.Maximum = 32;
            axis1.Minimum = 0;
            axis1.Position = AxisPosition.Top;
            timeModel.Axes.Add(axis1);

            // Add series
            detectedPresenceSeries.Title = "Presence";
            detectedPresenceSeries.ColorAxisKey = "ColorAxis";

            timeModel.Series.Add(detectedPresenceSeries);

            plotView.Model = timeModel;
            plotView.InvalidatePlot(true);
        }

        public void SignalPresenceDetected(bool detected, double angle, double range)
        {
            if (detected == false)
            {
                angle = double.NaN;
                range = double.NaN;
            }
            detectedPresenceSeries.Points.Add(new ScatterPoint(x: angle, y: range, value: 15));
            if (detectedPresenceSeries.Points.Count > 1)
            {
                detectedPresenceSeries.Points.RemoveAt(0);
            }

            /*
            detectedPresenceSeries.Points.Add(new ScatterPoint(x: index, y: angle, value: range));
            if (detectedPresenceSeries.Points.Count > 500)
            {
                detectedPresenceSeries.Points.RemoveAt(0);
            }
            index++;*/
            plotView.InvalidatePlot(true);
        }
    }
}
