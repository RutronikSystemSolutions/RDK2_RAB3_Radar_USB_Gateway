using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDK2_Radar_SignalProcessing_GUI
{
    public class DataCollector
    {
        public delegate void OnNewDatasetAvailableEventHandler(object sender, List<DataSet> dataset);
        public event OnNewDatasetAvailableEventHandler? OnNewDatasetAvailable;

        public class DataSet
        {
            public System.Numerics.Complex[,] dopplerFFTRx1;
            public System.Numerics.Complex[,] dopplerFFTRx2;
            public System.Numerics.Complex[,] dopplerFFTRx3;

            public DataSet(System.Numerics.Complex[,] dopplerFFTRx1, 
                System.Numerics.Complex[,] dopplerFFTRx2, 
                System.Numerics.Complex[,] dopplerFFTRx3)
            { 
                this.dopplerFFTRx1 = dopplerFFTRx1;
                this.dopplerFFTRx2 = dopplerFFTRx2;
                this.dopplerFFTRx3 = dopplerFFTRx3;
            }
        }

        private int MinCount = 10;
        private int MaxCount = 32;
        private int ResetAfter = 2;
        private double Threshold = 0.1;

        private List<DataSet> collectedData = new List<DataSet>();
        private int NullSince = 0;

        public void Feed(System.Numerics.Complex[,] dopplerFFTRx1, 
            System.Numerics.Complex[,] dopplerFFTRx2, 
            System.Numerics.Complex[,] dopplerFFTRx3)
        {
            int maxRange = 0;
            double maxMag = 0;
            int maxSpeed = 0;

            getMaxAmplitudeRange(dopplerFFTRx1, out maxRange, out maxMag, out maxSpeed);

            // Amplitude bigger than threshold?
            if (maxMag < Threshold)
            {
                if (collectedData.Count == 0)
                {
                    // nothing collected so far...
                    return;
                }
                
                NullSince++;
                
                if (NullSince >= ResetAfter)
                {
                    NullSince = 0;
                    if (collectedData.Count < MinCount)
                    {
                        // Not enough data
                        System.Diagnostics.Debug.WriteLine("Not enough data: " + collectedData.Count.ToString());
                        collectedData.Clear();
                    }
                    else if (collectedData.Count > MaxCount)
                    {
                        // Too much
                        System.Diagnostics.Debug.WriteLine("Too much data: " + collectedData.Count.ToString());
                        collectedData.Clear();
                    }
                    else
                    {
                        // Enough data!
                        System.Diagnostics.Debug.WriteLine("Enough collected data: " + collectedData.Count.ToString());
                        OnNewDatasetAvailable?.Invoke(this, collectedData);
                        // Create new list
                        collectedData = new List<DataSet>();
                    }
                }

            }
            else
            {
                NullSince = 0;
                // Too much? -> avoid overflow
                if (collectedData.Count > MaxCount)
                {
                    // Nothing to do, already too much
                }
                else
                {
                    if (collectedData.Count == MaxCount)
                    {
                        // TODO - generate event?
                        System.Diagnostics.Debug.WriteLine("Already collected enough data... " + collectedData.Count.ToString());
                    }

                    collectedData.Add(new DataSet(dopplerFFTRx1, dopplerFFTRx2, dopplerFFTRx3));
                }
            }
        }

        /// <summary>
        /// From a doppler FFT matrix, extract biggest range (index can be converted to meter) and the biggest magnitude
        /// </summary>
        /// <param name="dopplerFFTMatrix"></param>
        /// <param name="maxDetectedRange"></param>
        /// <param name="maxDetectedMag"></param>
        private void getMaxAmplitudeRange(System.Numerics.Complex[,] dopplerFFTMatrix,
            out int maxDetectedRange, out double maxDetectedMag, out int maxDetectedSpeed)
        {
            maxDetectedMag = 0;
            maxDetectedRange = 0;
            maxDetectedSpeed = 0;

            for (int i = 0; i < dopplerFFTMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < dopplerFFTMatrix.GetLength(1); j++)
                {
                    double magnitude = dopplerFFTMatrix[i, j].Magnitude;
                    if (magnitude > maxDetectedMag)
                    {
                        maxDetectedMag = magnitude;
                        maxDetectedRange = i;
                        maxDetectedSpeed = j;
                    }
                }
            }

        }
    }
}
