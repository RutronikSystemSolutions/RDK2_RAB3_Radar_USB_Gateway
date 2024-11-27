using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RDK2_Radar_SignalProcessing_GUI.Views
{
    public partial class LogView : UserControl
    {
        private List<string> logs = new List<string>();
        private const int MAX_LOG_COUNT = 10;

        public LogView()
        {
            InitializeComponent();
        }

        public void AddLog(string log)
        {
            logs.Add(DateTime.Now.ToString("HH:mm:ss") + " > " + log + Environment.NewLine);
            if (logs.Count > MAX_LOG_COUNT) logs.RemoveAt(0);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < logs.Count; i++)
            {
                sb.Append(logs[logs.Count - 1 - i]);
            }
            logsTextBox.Text = sb.ToString();
        }
    }
}
