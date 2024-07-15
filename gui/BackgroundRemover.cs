using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDK2_Radar_SignalProcessing_GUI
{
    public class BackgroundRemover
    {
        public enum Mode
        {
            ModeIIR,
            ModeFixed
        }

        private BackgroundFilterConfiguration configuration = new BackgroundFilterConfiguration();

        private System.Numerics.Complex[] background;
        private bool initialized = false;

        public BackgroundRemover(int freqCount)
        {
            background = new System.Numerics.Complex[freqCount];
        }

        public BackgroundFilterConfiguration getConfiguration()
        {
            return configuration.clone();
        }

        public void setConfiguration(BackgroundFilterConfiguration configuration)
        {
            this.configuration = configuration.clone();
            initialized = false;
        }

        public void feed(System.Numerics.Complex[] spectrum)
        {
            if (initialized == false)
            {
                for (int i = 0; i < spectrum.Length; i++)
                {
                    background[i] = spectrum[i];
                }
                initialized = true;
            }

            // In case of IIR mode, apply filter
            if (configuration.mode == Mode.ModeIIR)
            {
                double a0 = 1 - configuration.alpha;
                double b1 = configuration.alpha;

                for (int i = 0; i < spectrum.Length; i++)
                {
                    background[i] = spectrum[i] * a0 + background[i] * b1;
                }
            }
        }

        public void removeBackgroundInPlace(System.Numerics.Complex[] spectrum)
        {
            for (int i = 0; i < spectrum.Length; i++)
            {
                spectrum[i] = spectrum[i] - background[i];
            }
        }

        public void removeBackgroundInPlaceIfLower(System.Numerics.Complex[] spectrum)
        {
            for (int i = 0; i < spectrum.Length; i++)
            {
                if (spectrum[i].Magnitude > background[i].Magnitude)
                {
                    spectrum[i] = spectrum[i] - background[i];
                }
                else
                {
                    spectrum[i] = 0;
                }
            }
        }
    }
}
