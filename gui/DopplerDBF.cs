using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDK2_Radar_SignalProcessing_GUI
{
    public class DopplerDBF
    {
        private int antennaCount;
        private int beamCount;
        private double maxAngleDegree;
        private double lamda;

        /// <summary>
        /// Store the weights
        /// Dimensions: antennaCount x beamCount
        /// </summary>
        private System.Numerics.Complex[,] weights;

        /// <summary>
        /// Typical lambda: 0.5
        /// </summary>
        /// <param name="antennaCount"></param>
        /// <param name="beamCount"></param>
        /// <param name="maxAngleDegree"></param>
        /// <param name="lamda"></param>
        public DopplerDBF(int antennaCount, int beamCount, double maxAngleDegree, double lamda)
        {
            this.antennaCount = antennaCount;
            this.beamCount = beamCount;
            this.maxAngleDegree = maxAngleDegree;
            this.lamda = lamda;

            // Array storing angles of the beams in radian
            double[] angleVector = new double[beamCount];

            double increment = MathUtils.ConvertDegreesToRadians(maxAngleDegree) * 2 / (beamCount - 1);

            for (int i = 0; i < beamCount; i++)
            {
                angleVector[i] = MathUtils.ConvertDegreesToRadians(-maxAngleDegree) + (increment * i);
            }

            // Compute the weights
            weights = new System.Numerics.Complex[antennaCount, beamCount];
            for (int beamIndex = 0; beamIndex < beamCount; ++beamIndex)
            {
                double angle = angleVector[beamIndex];
                for (int antennaIndex = 0; antennaIndex < antennaCount; ++antennaIndex)
                {
                    weights[antennaIndex, beamIndex] = 
                        System.Numerics.Complex.Exp(System.Numerics.Complex.ImaginaryOne * 2 * Math.PI * antennaIndex * lamda * Math.Sin(angle));
                }
            }
        }

        public System.Numerics.Complex[,,] Run(System.Numerics.Complex[,] rangeDoppler0, System.Numerics.Complex[,] rangeDoppler1)
        {
            int numRanges = rangeDoppler0.GetLength(0);
            int numChirps = rangeDoppler0.GetLength(1);

            System.Numerics.Complex[,,] retval = new System.Numerics.Complex[numRanges, numChirps, beamCount];

            for (int ibeam = 0; ibeam < beamCount; ++ibeam)
            {
                //System.Numerics.Complex[,] acc = new System.Numerics.Complex[numRanges, numChirps];
                for (int irange = 0; irange < numRanges; ++irange)
                {
                    for(int ichirp = 0; ichirp < numChirps; ++ichirp)
                    {
                        retval[irange, ichirp, ibeam] += rangeDoppler0[irange, ichirp] * weights[0, ibeam];
                        retval[irange, ichirp, ibeam] += rangeDoppler1[irange, ichirp] * weights[1, ibeam];
                        //acc[irange, ichirp] += rangeDoppler0[irange, ichirp] * weights[0, ibeam];
                        //acc[irange, ichirp] += rangeDoppler1[irange, ichirp] * weights[1, ibeam];
                    }
                }
            }

            return retval;
        }
    }
}
