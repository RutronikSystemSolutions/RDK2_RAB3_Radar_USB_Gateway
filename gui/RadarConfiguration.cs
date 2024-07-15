using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDK2_Radar_SignalProcessing_GUI
{
    public class RadarConfiguration
    {
        private double startFrequency = 61020000000;
        private double endFrequency = 61480000000;
        private double samplingRate = 2352941;
        private int samplesPerChirp = 128;
        private int chirpsPerFrame = 64;
        private int antennaCount = 2;

        public RadarConfiguration()
        {

        }

        public double StartFrequency
            { get { return startFrequency; } }

        public double EndFrequency
            { get { return endFrequency; } }

        public double SamplingRate 
            { get { return samplingRate; } }

        public int SamplesPerChirp
            { get { return samplesPerChirp; } }

        public int ChirpsPerFrame 
            { get { return chirpsPerFrame; } }

        public int AntennaCount
            { get { return antennaCount; } }
    }
}
