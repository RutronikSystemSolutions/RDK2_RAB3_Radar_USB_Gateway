namespace RDK2_Radar_SignalProcessing_GUI.Views
{
    partial class GestureViewTime
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
            plotView = new OxyPlot.WindowsForms.PlotView();
            splitContainer1 = new SplitContainer();
            fftPlotView = new OxyPlot.WindowsForms.PlotView();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            SuspendLayout();
            // 
            // plotView
            // 
            plotView.BackColor = Color.White;
            plotView.Dock = DockStyle.Fill;
            plotView.Location = new Point(0, 0);
            plotView.Name = "plotView";
            plotView.PanCursor = Cursors.Hand;
            plotView.Size = new Size(512, 546);
            plotView.TabIndex = 0;
            plotView.Text = "plotView1";
            plotView.ZoomHorizontalCursor = Cursors.SizeWE;
            plotView.ZoomRectangleCursor = Cursors.SizeNWSE;
            plotView.ZoomVerticalCursor = Cursors.SizeNS;
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(fftPlotView);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(plotView);
            splitContainer1.Size = new Size(1008, 546);
            splitContainer1.SplitterDistance = 492;
            splitContainer1.TabIndex = 1;
            // 
            // fftPlotView
            // 
            fftPlotView.BackColor = Color.White;
            fftPlotView.Dock = DockStyle.Fill;
            fftPlotView.Location = new Point(0, 0);
            fftPlotView.Name = "fftPlotView";
            fftPlotView.PanCursor = Cursors.Hand;
            fftPlotView.Size = new Size(492, 546);
            fftPlotView.TabIndex = 0;
            fftPlotView.Text = "plotView1";
            fftPlotView.ZoomHorizontalCursor = Cursors.SizeWE;
            fftPlotView.ZoomRectangleCursor = Cursors.SizeNWSE;
            fftPlotView.ZoomVerticalCursor = Cursors.SizeNS;
            // 
            // GestureViewTime
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BorderStyle = BorderStyle.FixedSingle;
            Controls.Add(splitContainer1);
            Name = "GestureViewTime";
            Size = new Size(1008, 546);
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private OxyPlot.WindowsForms.PlotView plotView;
        private SplitContainer splitContainer1;
        private OxyPlot.WindowsForms.PlotView fftPlotView;
    }
}
