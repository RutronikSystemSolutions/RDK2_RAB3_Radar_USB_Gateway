using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDK2_Radar_SignalProcessing_GUI
{
    public class LinearRegression
    {
        public static void Compute(List<double> x, List<double> y, out double a, out double b, out double s)
        {
            double[] xa = new double[x.Count];
            double[] ya = new double[y.Count];

            for (int i = 0; i < x.Count; i++)
            {
                xa[i] = x[i];
            }
            for (int i = 0; i < y.Count; i++)
            {
                ya[i] = y[i];
            }

            Compute(xa, ya, out a, out b, out s);
        }

        /// <summary>
        /// Compute the linear regression of the points given in parameters
        /// The list x and y must have the same length
        /// </summary>
        /// <param name="x">X coordinates</param>
        /// <param name="y">Y coordinates</param>
        /// <param name="a">Output: Slope of the linear regression</param>
        /// <param name="b">Output: Intercept of the linear regression</param>
        /// <param name="s">Output: Quality of the linear regression</param>
        public static void Compute(double[] x, double[] y, out double a, out double b, out double s)
        {
            double xsum = 0;
            double ysum = 0;
            double xysum = 0;
            double xxsum = 0;
            double yysum = 0;
            double count = 0;

            // Default
            a = 0;
            b = 0;
            s = 0;

            // Safety
            if ((x == null) || (y == null)) return;
            if (x.Length != y.Length) return;
            if (x.Length <= 1) return;

            count = (double)x.Length;

            for (int i = 0; i < x.Length; i++)
            {
                xsum += x[i];
                ysum += y[i];
                xysum += x[i] * y[i];
                xxsum += x[i] * x[i];
                yysum += y[i] * y[i];
            }

            a = (count * xysum - xsum * ysum) / (count * xxsum - xsum * xsum);
            b = (ysum - a * xsum) / count;

            // Correlation coefficient
            double avgx = xsum / count;
            double avgy = ysum / count;

            s = (xysum - count * avgx * avgy) / Math.Sqrt((xxsum - count * avgx * avgx) * (yysum - count * avgy * avgy));
        }
    }
}
