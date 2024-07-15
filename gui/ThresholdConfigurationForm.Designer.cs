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
            thresholdTypeGroupBox = new GroupBox();
            setThresholdValueButton = new Button();
            thresholdValueTextBox = new TextBox();
            gocaCFARradioButton = new RadioButton();
            fixedRadioButton = new RadioButton();
            thresholdTypeGroupBox.SuspendLayout();
            SuspendLayout();
            // 
            // thresholdTypeGroupBox
            // 
            thresholdTypeGroupBox.Controls.Add(setThresholdValueButton);
            thresholdTypeGroupBox.Controls.Add(thresholdValueTextBox);
            thresholdTypeGroupBox.Controls.Add(gocaCFARradioButton);
            thresholdTypeGroupBox.Controls.Add(fixedRadioButton);
            thresholdTypeGroupBox.Location = new Point(12, 12);
            thresholdTypeGroupBox.Name = "thresholdTypeGroupBox";
            thresholdTypeGroupBox.Size = new Size(869, 173);
            thresholdTypeGroupBox.TabIndex = 0;
            thresholdTypeGroupBox.TabStop = false;
            thresholdTypeGroupBox.Text = "Type";
            // 
            // setThresholdValueButton
            // 
            setThresholdValueButton.Location = new Point(335, 40);
            setThresholdValueButton.Name = "setThresholdValueButton";
            setThresholdValueButton.Size = new Size(94, 29);
            setThresholdValueButton.TabIndex = 3;
            setThresholdValueButton.Text = "Set";
            setThresholdValueButton.UseVisualStyleBackColor = true;
            setThresholdValueButton.Click += setThresholdValueButton_Click;
            // 
            // thresholdValueTextBox
            // 
            thresholdValueTextBox.Location = new Point(190, 41);
            thresholdValueTextBox.Name = "thresholdValueTextBox";
            thresholdValueTextBox.Size = new Size(139, 27);
            thresholdValueTextBox.TabIndex = 2;
            // 
            // gocaCFARradioButton
            // 
            gocaCFARradioButton.AutoSize = true;
            gocaCFARradioButton.Location = new Point(43, 91);
            gocaCFARradioButton.Name = "gocaCFARradioButton";
            gocaCFARradioButton.Size = new Size(108, 24);
            gocaCFARradioButton.TabIndex = 1;
            gocaCFARradioButton.TabStop = true;
            gocaCFARradioButton.Text = "GOCA CFAR";
            gocaCFARradioButton.UseVisualStyleBackColor = true;
            // 
            // fixedRadioButton
            // 
            fixedRadioButton.AutoSize = true;
            fixedRadioButton.Location = new Point(43, 42);
            fixedRadioButton.Name = "fixedRadioButton";
            fixedRadioButton.Size = new Size(65, 24);
            fixedRadioButton.TabIndex = 0;
            fixedRadioButton.TabStop = true;
            fixedRadioButton.Text = "Fixed";
            fixedRadioButton.UseVisualStyleBackColor = true;
            // 
            // ThresholdConfigurationForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(893, 482);
            Controls.Add(thresholdTypeGroupBox);
            Name = "ThresholdConfigurationForm";
            Text = "Threshold configuration";
            thresholdTypeGroupBox.ResumeLayout(false);
            thresholdTypeGroupBox.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox thresholdTypeGroupBox;
        private RadioButton gocaCFARradioButton;
        private RadioButton fixedRadioButton;
        private Button setThresholdValueButton;
        private TextBox thresholdValueTextBox;
    }
}