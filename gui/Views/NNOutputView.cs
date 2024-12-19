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
using OxyPlot;
using OxyPlot.Axes;

namespace RDK2_Radar_SignalProcessing_GUI.Views
{
    public partial class NNOutputView : UserControl
    {
        private BarItem [] items = new BarItem[3];

        public NNOutputView()
        {
            InitializeComponent();
            InitPlot();
        }

        private void InitPlot()
        {
            var plotModel = new PlotModel
            {
                Background = OxyColors.White
            };

            var barSeries = new BarSeries();
            barSeries.IsStacked = false;

            items[0] = new BarItem(0);
            items[1] = new BarItem(0);
            items[2] = new BarItem(0);

            barSeries.Items.Add(items[0]);
            barSeries.Items.Add(items[1]);
            barSeries.Items.Add(items[2]);

            plotModel.Series.Add(barSeries);
            plotView.Model = plotModel;

            var categoryAxis = new CategoryAxis { Position = AxisPosition.Left };
            categoryAxis.Labels.Add("Click");
            categoryAxis.Labels.Add("Left-swipe");
            categoryAxis.Labels.Add("Right-swipe");

            var valueAxis = new LinearAxis { 
                Position = AxisPosition.Bottom, 
                Minimum = 0,
                Maximum = 1,
                AbsoluteMinimum = 0, 
                AbsoluteMaximum = 1 };

            plotModel.Axes.Add(categoryAxis);
            plotModel.Axes.Add(valueAxis);

            plotView.InvalidatePlot(true);
        }

        public void Update(float[] values)
        {
            for (int i = 0; i < items.Length; i++)
            {
                items[i].Value = values[i];
            }
            plotView.InvalidatePlot(true);
        }
    }
}
