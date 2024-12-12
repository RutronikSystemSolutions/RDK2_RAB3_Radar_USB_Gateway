namespace RDK2_Radar_SignalProcessing_GUI.Views
{
    partial class DBFDopplerView
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
            splitContainer = new SplitContainer();
            scatterPlotView = new OxyPlot.WindowsForms.PlotView();
            ((System.ComponentModel.ISupportInitialize)splitContainer).BeginInit();
            splitContainer.Panel1.SuspendLayout();
            splitContainer.Panel2.SuspendLayout();
            splitContainer.SuspendLayout();
            SuspendLayout();
            // 
            // plotView
            // 
            plotView.BackColor = Color.White;
            plotView.Dock = DockStyle.Fill;
            plotView.Location = new Point(0, 0);
            plotView.Name = "plotView";
            plotView.PanCursor = Cursors.Hand;
            plotView.Size = new Size(355, 482);
            plotView.TabIndex = 0;
            plotView.Text = "plotView1";
            plotView.ZoomHorizontalCursor = Cursors.SizeWE;
            plotView.ZoomRectangleCursor = Cursors.SizeNWSE;
            plotView.ZoomVerticalCursor = Cursors.SizeNS;
            // 
            // splitContainer
            // 
            splitContainer.Dock = DockStyle.Fill;
            splitContainer.Location = new Point(0, 0);
            splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            splitContainer.Panel1.Controls.Add(plotView);
            // 
            // splitContainer.Panel2
            // 
            splitContainer.Panel2.Controls.Add(scatterPlotView);
            splitContainer.Size = new Size(752, 482);
            splitContainer.SplitterDistance = 355;
            splitContainer.TabIndex = 1;
            // 
            // scatterPlotView
            // 
            scatterPlotView.BackColor = Color.White;
            scatterPlotView.Dock = DockStyle.Fill;
            scatterPlotView.Location = new Point(0, 0);
            scatterPlotView.Name = "scatterPlotView";
            scatterPlotView.PanCursor = Cursors.Hand;
            scatterPlotView.Size = new Size(393, 482);
            scatterPlotView.TabIndex = 0;
            scatterPlotView.Text = "plotView1";
            scatterPlotView.ZoomHorizontalCursor = Cursors.SizeWE;
            scatterPlotView.ZoomRectangleCursor = Cursors.SizeNWSE;
            scatterPlotView.ZoomVerticalCursor = Cursors.SizeNS;
            // 
            // DBFDopplerView
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(splitContainer);
            Name = "DBFDopplerView";
            Size = new Size(752, 482);
            splitContainer.Panel1.ResumeLayout(false);
            splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer).EndInit();
            splitContainer.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private OxyPlot.WindowsForms.PlotView plotView;
        private SplitContainer splitContainer;
        private OxyPlot.WindowsForms.PlotView scatterPlotView;
    }
}
