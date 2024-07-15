using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDK2_Radar_SignalProcessing_GUI
{
    public class BackgroundFilterConfiguration
    {
        public BackgroundRemover.Mode mode;
        public double alpha;

        public BackgroundFilterConfiguration()
        {
            mode = BackgroundRemover.Mode.ModeIIR;
            alpha = 0.85;
        }

        public BackgroundFilterConfiguration clone()
        {
            BackgroundFilterConfiguration retval = new BackgroundFilterConfiguration();
            retval.mode = mode;
            retval.alpha = alpha;
            return retval;
        }
    }
}
