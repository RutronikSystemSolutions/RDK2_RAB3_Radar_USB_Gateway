namespace RDK2_Radar_SignalProcessing_GUI.Views
{
    partial class UserFeedbackView
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
            components = new System.ComponentModel.Container();
            selectedNumber = new Label();
            statusLabel = new Label();
            guiUpdateTime = new System.Windows.Forms.Timer(components);
            verticalProgressBar = new UIControls.VerticalProgressBar();
            SuspendLayout();
            // 
            // selectedNumber
            // 
            selectedNumber.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            selectedNumber.Font = new Font("Segoe UI", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            selectedNumber.Location = new Point(3, 0);
            selectedNumber.Name = "selectedNumber";
            selectedNumber.Size = new Size(331, 54);
            selectedNumber.TabIndex = 1;
            selectedNumber.Text = "0";
            selectedNumber.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // statusLabel
            // 
            statusLabel.AutoSize = true;
            statusLabel.Location = new Point(3, 54);
            statusLabel.Name = "statusLabel";
            statusLabel.Size = new Size(52, 20);
            statusLabel.TabIndex = 2;
            statusLabel.Text = "Status:";
            // 
            // guiUpdateTime
            // 
            guiUpdateTime.Tick += guiUpdateTime_Tick;
            // 
            // verticalProgressBar
            // 
            verticalProgressBar.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            verticalProgressBar.BorderStyle = BorderStyle.FixedSingle;
            verticalProgressBar.Location = new Point(234, 3);
            verticalProgressBar.Name = "verticalProgressBar";
            verticalProgressBar.Size = new Size(100, 217);
            verticalProgressBar.TabIndex = 3;
            // 
            // UserFeedbackView
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(verticalProgressBar);
            Controls.Add(statusLabel);
            Controls.Add(selectedNumber);
            Name = "UserFeedbackView";
            Size = new Size(337, 223);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label selectedNumber;
        private Label statusLabel;
        private System.Windows.Forms.Timer guiUpdateTime;
        private UIControls.VerticalProgressBar verticalProgressBar;
    }
}
