using OxyPlot.Axes;
using OxyPlot;
using OxyPlot.Series;
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
    public partial class DopplerFFTView : UserControl
    {
        private HeatMapSeries? heatMapSeries;

        public DopplerFFTView()
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
                Palette = OxyPalettes.Viridis(100),
                Position = AxisPosition.Right,
                Minimum = 0,
                Maximum = 10
            });

            var linearAxis = new LinearAxis();
            linearAxis.Position = AxisPosition.Left;
            linearAxis.Key = "linearAxis";
            linearAxis.Title = "Range";
            model.Axes.Add(linearAxis);

            linearAxis = new LinearAxis();
            linearAxis.Position = AxisPosition.Bottom;
            linearAxis.Title = "Velocity";
            model.Axes.Add(linearAxis);

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
            plotView.Model = model;
            plotView.InvalidatePlot(true);
        }

        /// <summary>
        /// [bin, velocity]
        /// </summary>
        /// <param name="dopplerFFTMatrix"></param>
        public void UpdateData(System.Numerics.Complex[,] dopplerFFTMatrix )
        {
            if (heatMapSeries == null) return;

            // Rotate and convert to double
            // We want X axis to be the velocity, and Y axis to be the range
            double[,] data = new double[dopplerFFTMatrix.GetLength(1), dopplerFFTMatrix.GetLength(0)];
            for (int i = 0; i < data.GetLength(0); i++)
            {
                for (int j = 0; j <  data.GetLength(1); j++)
                {
                    data[i, j] = dopplerFFTMatrix[j, i].Magnitude;
                }
            }

            // TODO -> should depend on the radar configuration :-)inte
            heatMapSeries.Data = data;
            heatMapSeries.X0 = -16;
            heatMapSeries.X1 = 16;
            heatMapSeries.Y0 = 0;
            //heatMapSeries.Y1 = data.GetLength(1) - 1;
            heatMapSeries.Y1 = 20;
            plotView.InvalidatePlot(true);
        }
    }
}
