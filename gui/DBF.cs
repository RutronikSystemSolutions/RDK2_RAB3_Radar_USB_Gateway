using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDK2_Radar_SignalProcessing_GUI
{
    public class DBF
    {
        private int beamCount;
        private double maxAngleDegree;
        private double radarLambda;

        /// <summary>
        /// Store the weights
        /// Dimensions: 2 x beamCount
        /// Why 2 -> becuase 2 antennas
        /// </summary>
        private System.Numerics.Complex[,] weights;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="beamCount">Standard: 32</param>
        /// <param name="maxAngleDegree">Standard: 45 (+/-45°)</param>
        /// <param name="radarLambda">0.5mm for BGT60TR13C</param>
        public DBF(int beamCount, double maxAngleDegree, double radarLambda)
        {
            this.beamCount = beamCount;
            this.maxAngleDegree = maxAngleDegree;
            this.radarLambda = radarLambda;

            // Array storing angles of the beams in radian
            double[] angleVector = new double[beamCount];

            double increment = MathUtils.ConvertDegreesToRadians(maxAngleDegree) * 2 / (beamCount - 1);

            for (int i = 0; i < beamCount; i++)
            {
                angleVector[i] = MathUtils.ConvertDegreesToRadians(-maxAngleDegree) + (increment * i);
            }

            // Compute the weights
            weights = new System.Numerics.Complex[2, beamCount];
            for (int i = 0; i < beamCount; ++i)
            {
                double angle = angleVector[i];
                for (int antennaIndex = 0; antennaIndex < 2; ++antennaIndex)
                {
                    weights[antennaIndex, i] = System.Numerics.Complex.Exp(System.Numerics.Complex.ImaginaryOne * 2 * Math.PI * antennaIndex * radarLambda * Math.Sin(angle));
                }
            }
        }

        /// <summary>
        /// Compute digital beam forming array
        /// Range FFT contains the spectrum of antenna 0 and 1
        /// </summary>
        /// <param name="rangeFFT0"></param>
        /// <param name="rangeFFT1"></param>
        /// <returns>Array of dimension [beamCount, freq bin count]</returns>
        public System.Numerics.Complex[,] compute(System.Numerics.Complex[] rangeFFT0, System.Numerics.Complex[] rangeFFT1)
        {
            if (rangeFFT0 == null || rangeFFT1 == null)
            {
                throw new Exception("Range FFT cannot be null");
            }

            if (rangeFFT0.Length != rangeFFT1.Length)
            {
                throw new Exception("Range FFT must have the same size");
            }

            int rangeFFTLen = rangeFFT0.Length;

            System.Numerics.Complex[,] retval = new System.Numerics.Complex[beamCount, rangeFFTLen];
            for (int beamIndex = 0; beamIndex < beamCount; ++beamIndex)
            {
                for (int freqIndex = 0; freqIndex < rangeFFTLen; ++freqIndex)
                {
                    System.Numerics.Complex antenna0 = rangeFFT0[freqIndex] * weights[0, beamIndex];
                    System.Numerics.Complex antenna1 = rangeFFT1[freqIndex] * weights[1, beamIndex];
                    retval[beamIndex, freqIndex] = antenna0 + antenna1;
                }
            }

            return retval;
        }
    }
}
