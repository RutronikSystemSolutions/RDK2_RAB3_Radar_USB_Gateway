namespace RDK2_Radar_SignalProcessing_GUI.Views
{
    partial class DBFView
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            dbfPlotView = new OxyPlot.WindowsForms.PlotView();
            SuspendLayout();
            // 
            // dbfPlotView
            // 
            dbfPlotView.Dock = DockStyle.Fill;
            dbfPlotView.Location = new Point(0, 0);
            dbfPlotView.Name = "dbfPlotView";
            dbfPlotView.PanCursor = Cursors.Hand;
            dbfPlotView.Size = new Size(1057, 496);
            dbfPlotView.TabIndex = 0;
            dbfPlotView.Text = "DBFView";
            dbfPlotView.ZoomHorizontalCursor = Cursors.SizeWE;
            dbfPlotView.ZoomRectangleCursor = Cursors.SizeNWSE;
            dbfPlotView.ZoomVerticalCursor = Cursors.SizeNS;
            // 
            // DBFView
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BorderStyle = BorderStyle.FixedSingle;
            Controls.Add(dbfPlotView);
            Name = "DBFView";
            Size = new Size(1057, 496);
            ResumeLayout(false);
        }

        #endregion

        private OxyPlot.WindowsForms.PlotView dbfPlotView;
    }
}
