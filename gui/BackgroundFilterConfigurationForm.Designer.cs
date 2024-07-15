namespace RDK2_Radar_SignalProcessing_GUI
{
    partial class BackgroundFilterConfigurationForm
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
            typeGroupBox = new GroupBox();
            updateButton = new Button();
            alphaTextBox = new TextBox();
            alphaTrackBar = new TrackBar();
            alphaLabel = new Label();
            fixedRadioButton = new RadioButton();
            iirRadioButton = new RadioButton();
            typeGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)alphaTrackBar).BeginInit();
            SuspendLayout();
            // 
            // typeGroupBox
            // 
            typeGroupBox.Controls.Add(updateButton);
            typeGroupBox.Controls.Add(alphaTextBox);
            typeGroupBox.Controls.Add(alphaTrackBar);
            typeGroupBox.Controls.Add(alphaLabel);
            typeGroupBox.Controls.Add(fixedRadioButton);
            typeGroupBox.Controls.Add(iirRadioButton);
            typeGroupBox.Location = new Point(12, 12);
            typeGroupBox.Name = "typeGroupBox";
            typeGroupBox.Size = new Size(776, 226);
            typeGroupBox.TabIndex = 0;
            typeGroupBox.TabStop = false;
            typeGroupBox.Text = "Type";
            // 
            // updateButton
            // 
            updateButton.Location = new Point(110, 120);
            updateButton.Name = "updateButton";
            updateButton.Size = new Size(94, 29);
            updateButton.TabIndex = 2;
            updateButton.Text = "Update";
            updateButton.UseVisualStyleBackColor = true;
            // 
            // alphaTextBox
            // 
            alphaTextBox.Location = new Point(391, 42);
            alphaTextBox.Name = "alphaTextBox";
            alphaTextBox.ReadOnly = true;
            alphaTextBox.Size = new Size(125, 27);
            alphaTextBox.TabIndex = 1;
            // 
            // alphaTrackBar
            // 
            alphaTrackBar.Location = new Point(146, 26);
            alphaTrackBar.Maximum = 100;
            alphaTrackBar.Name = "alphaTrackBar";
            alphaTrackBar.Size = new Size(239, 56);
            alphaTrackBar.TabIndex = 1;
            alphaTrackBar.Scroll += alphaTrackBar_Scroll;
            // 
            // alphaLabel
            // 
            alphaLabel.AutoSize = true;
            alphaLabel.Location = new Point(89, 45);
            alphaLabel.Name = "alphaLabel";
            alphaLabel.Size = new Size(51, 20);
            alphaLabel.TabIndex = 1;
            alphaLabel.Text = "Alpha:";
            // 
            // fixedRadioButton
            // 
            fixedRadioButton.AutoSize = true;
            fixedRadioButton.Location = new Point(23, 122);
            fixedRadioButton.Name = "fixedRadioButton";
            fixedRadioButton.Size = new Size(65, 24);
            fixedRadioButton.TabIndex = 1;
            fixedRadioButton.TabStop = true;
            fixedRadioButton.Text = "Fixed";
            fixedRadioButton.UseVisualStyleBackColor = true;
            fixedRadioButton.CheckedChanged += fixedRadioButton_CheckedChanged;
            // 
            // iirRadioButton
            // 
            iirRadioButton.AutoSize = true;
            iirRadioButton.Location = new Point(23, 43);
            iirRadioButton.Name = "iirRadioButton";
            iirRadioButton.Size = new Size(47, 24);
            iirRadioButton.TabIndex = 0;
            iirRadioButton.TabStop = true;
            iirRadioButton.Text = "IIR";
            iirRadioButton.UseVisualStyleBackColor = true;
            iirRadioButton.CheckedChanged += iirRadioButton_CheckedChanged;
            // 
            // BackgroundFilterConfigurationForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 250);
            Controls.Add(typeGroupBox);
            Name = "BackgroundFilterConfigurationForm";
            Text = "Background Filter Configuration";
            typeGroupBox.ResumeLayout(false);
            typeGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)alphaTrackBar).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox typeGroupBox;
        private RadioButton fixedRadioButton;
        private RadioButton iirRadioButton;
        private TextBox alphaTextBox;
        private TrackBar alphaTrackBar;
        private Label alphaLabel;
        private Button updateButton;
    }
}