using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDK2_Radar_SignalProcessing_GUI
{
    public class CFAR
    {
        private double threshold = double.NaN;
        private double dynamicThreshold = double.NaN;
        private int guardCellCount;
        private int refCellCount;

        /// <summary>
        /// Store the history of values (enable to compute the dynamic threshold)
        /// </summary>
        private List<double> values = new List<double>();

        public CFAR(int guardCellCount, int refCellCount, double threshold)
        {
            this.threshold = threshold;
            this.guardCellCount = guardCellCount;
            this.refCellCount = refCellCount;
        }

        public double DynamicThreshold
        {
            get { return dynamicThreshold; }
        }

        public void feed(double value)
        {
            // Add to history
            values.Add(value);

            int requiredCellCount = (guardCellCount + refCellCount + 1);

            if (values.Count == requiredCellCount)
            {
                // Search for maximum value
                double maxValue = values[0];
                for (int i = 1; i < refCellCount; ++i)
                {
                    if (values[i] > maxValue) maxValue = values[i];
                }

                // Compute dynamic threshold
                dynamicThreshold = maxValue + threshold;

                values.RemoveAt(0);
            }
            else if (values.Count > requiredCellCount)
            {
                // Something went wrong...
                values.Clear();
                dynamicThreshold = double.NaN;
            }
            else
            {
                // Not enough value yet
                dynamicThreshold = double.NaN;
            }
        }
    }
}
