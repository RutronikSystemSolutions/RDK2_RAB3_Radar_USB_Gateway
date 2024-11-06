using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDK2_Radar_SignalProcessing_GUI
{
    public class RadarConfiguration
    {
        public static double START_FREQUENCY = 58000000000;
        public static double END_FREQUENCY = 63500000000;
        public static double SAMPLING_RATE = 2000000;

        public static int SAMPLES_PER_CHIRP = 32;
        public static int CHIRPS_PER_FRAME = 64;
        public static int ANTENNA_COUNT = 3;

        private double startFrequency = START_FREQUENCY;
        private double endFrequency = END_FREQUENCY;
        private double samplingRate = SAMPLING_RATE;
        
        private int samplesPerChirp = SAMPLES_PER_CHIRP;
        private int chirpsPerFrame = CHIRPS_PER_FRAME;
        private int antennaCount = ANTENNA_COUNT;

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
