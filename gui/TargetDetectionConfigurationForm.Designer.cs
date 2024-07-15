namespace RDK2_Radar_SignalProcessing_GUI
{
    partial class TargetDetectionConfigurationForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            minRangeLabel = new Label();
            minRangeTrackBar = new TrackBar();
            minRangeMetersTextBox = new TextBox();
            maxRangeMetersTextBox = new TextBox();
            maxRangeTrackBar = new TrackBar();
            maxRangeLabel = new Label();
            ((System.ComponentModel.ISupportInitialize)minRangeTrackBar).BeginInit();
            ((System.ComponentModel.ISupportInitialize)maxRangeTrackBar).BeginInit();
            SuspendLayout();
            // 
            // minRangeLabel
            // 
            minRangeLabel.AutoSize = true;
            minRangeLabel.Location = new Point(12, 12);
            minRangeLabel.Name = "minRangeLabel";
            minRangeLabel.Size = new Size(83, 20);
            minRangeLabel.TabIndex = 0;
            minRangeLabel.Text = "Min Range:";
            // 
            // minRangeTrackBar
            // 
            minRangeTrackBar.Location = new Point(101, 12);
            minRangeTrackBar.Name = "minRangeTrackBar";
            minRangeTrackBar.Size = new Size(359, 56);
            minRangeTrackBar.TabIndex = 1;
            minRangeTrackBar.Scroll += minRangeTrackBar_Scroll;
            // 
            // minRangeMetersTextBox
            // 
            minRangeMetersTextBox.Location = new Point(466, 12);
            minRangeMetersTextBox.Name = "minRangeMetersTextBox";
            minRangeMetersTextBox.ReadOnly = true;
            minRangeMetersTextBox.Size = new Size(125, 27);
            minRangeMetersTextBox.TabIndex = 2;
            // 
            // maxRangeMetersTextBox
            // 
            maxRangeMetersTextBox.Location = new Point(466, 74);
            maxRangeMetersTextBox.Name = "maxRangeMetersTextBox";
            maxRangeMetersTextBox.ReadOnly = true;
            maxRangeMetersTextBox.Size = new Size(125, 27);
            maxRangeMetersTextBox.TabIndex = 5;
            // 
            // maxRangeTrackBar
            // 
            maxRangeTrackBar.Location = new Point(101, 74);
            maxRangeTrackBar.Name = "maxRangeTrackBar";
            maxRangeTrackBar.Size = new Size(359, 56);
            maxRangeTrackBar.TabIndex = 4;
            maxRangeTrackBar.Scroll += maxRangeTrackBar_Scroll;
            // 
            // maxRangeLabel
            // 
            maxRangeLabel.AutoSize = true;
            maxRangeLabel.Location = new Point(12, 74);
            maxRangeLabel.Name = "maxRangeLabel";
            maxRangeLabel.Size = new Size(86, 20);
            maxRangeLabel.TabIndex = 3;
            maxRangeLabel.Text = "Max Range:";
            // 
            // TargetDetectionConfigurationForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 141);
            Controls.Add(maxRangeMetersTextBox);
            Controls.Add(maxRangeTrackBar);
            Controls.Add(maxRangeLabel);
            Controls.Add(minRangeMetersTextBox);
            Controls.Add(minRangeTrackBar);
            Controls.Add(minRangeLabel);
            Name = "TargetDetectionConfigurationForm";
            Text = "Target Detection Configuration";
            ((System.ComponentModel.ISupportInitialize)minRangeTrackBar).EndInit();
            ((System.ComponentModel.ISupportInitialize)maxRangeTrackBar).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label minRangeLabel;
        private TrackBar minRangeTrackBar;
        private TextBox minRangeMetersTextBox;
        private TextBox maxRangeMetersTextBox;
        private TrackBar maxRangeTrackBar;
        private Label maxRangeLabel;
    }
}