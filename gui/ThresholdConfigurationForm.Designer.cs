namespace RDK2_Radar_SignalProcessing_GUI
{
    partial class ThresholdConfigurationForm
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
            setThresholdValueButton = new Button();
            thresholdValueTextBox = new TextBox();
            SuspendLayout();
            // 
            // setThresholdValueButton
            // 
            setThresholdValueButton.Location = new Point(157, 12);
            setThresholdValueButton.Name = "setThresholdValueButton";
            setThresholdValueButton.Size = new Size(94, 29);
            setThresholdValueButton.TabIndex = 3;
            setThresholdValueButton.Text = "Set";
            setThresholdValueButton.UseVisualStyleBackColor = true;
            setThresholdValueButton.Click += setThresholdValueButton_Click;
            // 
            // thresholdValueTextBox
            // 
            thresholdValueTextBox.Location = new Point(12, 13);
            thresholdValueTextBox.Name = "thresholdValueTextBox";
            thresholdValueTextBox.Size = new Size(139, 27);
            thresholdValueTextBox.TabIndex = 2;
            // 
            // ThresholdConfigurationForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(357, 67);
            Controls.Add(setThresholdValueButton);
            Controls.Add(thresholdValueTextBox);
            Name = "ThresholdConfigurationForm";
            Text = "Threshold configuration";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button setThresholdValueButton;
        private TextBox thresholdValueTextBox;
    }
}