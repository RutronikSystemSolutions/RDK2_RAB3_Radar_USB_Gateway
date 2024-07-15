using OxyPlot.Axes;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.WindowsForms;
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
    public partial class DBFView : UserControl
    {
        private HeatMapSeries? heatMapSeries;

        public DBFView()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            var model = new PlotModel { };

            // Color axis (the X and Y axes are generated automatically)
            model.Axes.Add(new LinearColorAxis
            {
                //Palette = OxyPalettes.Rainbow(100)
                Palette = OxyPalettes.Viridis(100),
                Minimum = 0,
                Maximum = 1
            });

            // generate 1d normal distributionradaradar
            var singleData = new double[100];
            for (int x = 0; x < 100; ++x)
            {
                singleData[x] = Math.Exp((-1.0 / 2.0) * Math.Pow(((double)x - 50.0) / 20.0, 2.0));
            }

            // generate 2d normal distribution
            var data = new double[100, 100];
            for (int x = 0; x < 100; ++x)
            {
                for (int y = 0; y < 100; ++y)
                {
                    data[y, x] = singleData[x] * singleData[(y + 30) % 100] * 100;
                }
            }

            heatMapSeries = new HeatMapSeries
            {
                Interpolate = false,
                RenderMethod = HeatMapRenderMethod.Bitmap,
                Data = data
            };

            model.Series.Add(heatMapSeries);
            dbfPlotView.Model = model;
            dbfPlotView.InvalidatePlot(true);
        }

        public void UpdateData(double[,] data)
        {
            // TODO make it depends on the radar configuration
            if (heatMapSeries == null) return;
            heatMapSeries.Data = data;
            heatMapSeries.X0 = -45;
            heatMapSeries.X1 = 45;
            heatMapSeries.Y0 = 0;
            heatMapSeries.Y1 = 20;
            dbfPlotView.InvalidatePlot(true);
        }
    }
}
