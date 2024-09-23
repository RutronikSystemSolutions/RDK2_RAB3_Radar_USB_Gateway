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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RDK2_Radar_SignalProcessing_GUI.Views
{
    public partial class GestureView : UserControl
    {
        private HeatMapSeries? heatMapSeries;

        private int splitCount = 128;

        private class HistoryPoint
        {
            public int x;
            public int y;
            public HistoryPoint(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }

        private List<HistoryPoint> history = new List<HistoryPoint>();


        public GestureView()
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
                //Palette = OxyPalettes.Rainbow(100),
                Palette = OxyPalettes.Viridis(100),
                Minimum = 0,
                Maximum = 40
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
            plotView.Model = model;
            plotView.InvalidatePlot(true);
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

            phase = phase + Math.PI;
            phase = (phase * splitCount) / (2 * Math.PI);

            return (int)phase;
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
            // System.Numerics.Complex[,] dopplerFFTMatrix = new System.Numerics.Complex[spectrumLen, radarConfiguration.ChirpsPerFrame];
            // so x is range and y is the 

            // Rx1 - Rx3 -> horizontal
            // Rx2 - Rx3 -> vertical

            int maxX = 0;
            int maxY = 0;
            double maxVal = getMaxXMaxY(dopplerFFTMatrixRx1, out maxX, out maxY);


            double[,] data = new double[splitCount, splitCount];

            for (int i = 0; i < splitCount; i++)
            {
                for (int j = 0; j < splitCount; ++j)
                {
                    data[i, j] = 0;
                }
            }

            if (maxVal > 0.1)
            {
                /*
                // horizontal angle index (1 - 3)
                double horizontalPhase = dopplerFFTMatrixRx1[maxX, maxY].Phase - dopplerFFTMatrixRx3[maxX, maxY].Phase;
                double verticalPhase = dopplerFFTMatrixRx2[maxX, maxY].Phase - dopplerFFTMatrixRx3[maxX, maxY].Phase;

                int xindex = scalePhase(horizontalPhase);
                int yindex = scalePhase(verticalPhase);


                if (xindex < 0) 
                    xindex = 0;
                if (xindex > 31) 
                    xindex = 31;

                if (yindex < 0) 
                    yindex = 0;
                if (yindex > 31) 
                    yindex = 31;

                data[xindex, yindex] = 1;*/

                double rx1 = dopplerFFTMatrixRx1[maxX, maxY].Phase;
                double rx2 = dopplerFFTMatrixRx2[maxX, maxY].Phase;
                double rx3 = dopplerFFTMatrixRx3[maxX, maxY].Phase;

                double hPhase = getAngleDiff(rx1, rx3);
                double vPhase = getAngleDiff(rx2, rx3);


                int xindex = scalePhase(hPhase);
                int yindex = scalePhase(vPhase);

                /*
                if (xindex < 0)
                    xindex = 0;
                if (xindex > 31)
                    xindex = 31;

                if (yindex < 0)
                    yindex = 0;
                if (yindex > 31)
                    yindex = 31;*/

                history.Add(new HistoryPoint(xindex, yindex));
                if (history.Count > 30) history.RemoveAt(0);

                // data[xindex, yindex] = 1;
            }
            else
            {
                //if (history.Count > 0)
                //    history.RemoveAt(0);
            }

            for(int i = 0; i < history.Count; i++)
            {
                data[history[i].x, history[i].y] = (history.Count - i) + 10;
            }


            if (heatMapSeries == null) return;
            heatMapSeries.Data = data;
            heatMapSeries.X0 = 0;
            heatMapSeries.X1 = splitCount;
            heatMapSeries.Y0 = 0;
            heatMapSeries.Y1 = splitCount;
            plotView.InvalidatePlot(true);
        }

        public void UpdateData(double[,] dataH, double[,] dataV)
        {
            // data we want to plot is 32x32 (because the DBF algo has been configured like that)

            double[,] data = new double[32, 32];

            for(int i  = 0; i < 32; i++)
            {
                for(int j = 0; j < 32; ++j)
                {
                    data[i, j] = 0;
                }
            }

            int maxH = getMaxValueX(dataH);
            int maxY = getMaxValueX(dataV);

            data[maxH, maxY] = 1;

            if (heatMapSeries == null) return;
            heatMapSeries.Data = data;
            heatMapSeries.X0 = 0;
            heatMapSeries.X1 = 32;
            heatMapSeries.Y0 = 0;
            heatMapSeries.Y1 = 32;
            plotView.InvalidatePlot(true);

            /*
            // only keep the maximum point
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

            for (int x = 0; x < beamCount; ++x)
            {
                for (int y = 0; y < freqBinCount; ++y)
                {
                    if (x == maxx && y == maxy)
                    {
                        if (maxvalue > 0.05)
                            data[x, y] = 1;
                        else
                            data[x, y] = 0;
                    }
                    else
                    {
                        data[x, y] = 0;
                    }
                }
            }


            // TODO make it depends on the radar configuration
            if (heatMapSeries == null) return;
            heatMapSeries.Data = data;
            heatMapSeries.X0 = -45;
            heatMapSeries.X1 = 45;
            heatMapSeries.Y0 = 0;
            heatMapSeries.Y1 = 20;
            plotView.InvalidatePlot(true);*/
        }
    }
}
