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
            guiUpdateTime = new System.Windows.Forms.Timer(components);
            verticalProgressBar = new UIControls.VerticalProgressBar();
            leftMovePanel = new Panel();
            clickPanel = new Panel();
            rightMovePanel = new Panel();
            readyForNewMovePanel = new Panel();
            SuspendLayout();
            // 
            // guiUpdateTime
            // 
            guiUpdateTime.Tick += guiUpdateTime_Tick;
            // 
            // verticalProgressBar
            // 
            verticalProgressBar.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            verticalProgressBar.BorderStyle = BorderStyle.FixedSingle;
            verticalProgressBar.Location = new Point(256, 3);
            verticalProgressBar.Name = "verticalProgressBar";
            verticalProgressBar.Size = new Size(78, 217);
            verticalProgressBar.TabIndex = 3;
            // 
            // leftMovePanel
            // 
            leftMovePanel.BorderStyle = BorderStyle.FixedSingle;
            leftMovePanel.Location = new Point(31, 91);
            leftMovePanel.Name = "leftMovePanel";
            leftMovePanel.Size = new Size(57, 50);
            leftMovePanel.TabIndex = 5;
            // 
            // clickPanel
            // 
            clickPanel.BorderStyle = BorderStyle.FixedSingle;
            clickPanel.Location = new Point(94, 91);
            clickPanel.Name = "clickPanel";
            clickPanel.Size = new Size(57, 50);
            clickPanel.TabIndex = 6;
            // 
            // rightMovePanel
            // 
            rightMovePanel.BorderStyle = BorderStyle.FixedSingle;
            rightMovePanel.Location = new Point(157, 91);
            rightMovePanel.Name = "rightMovePanel";
            rightMovePanel.Size = new Size(57, 50);
            rightMovePanel.TabIndex = 6;
            // 
            // readyForNewMovePanel
            // 
            readyForNewMovePanel.Location = new Point(31, 59);
            readyForNewMovePanel.Name = "readyForNewMovePanel";
            readyForNewMovePanel.Size = new Size(183, 26);
            readyForNewMovePanel.TabIndex = 7;
            // 
            // UserFeedbackView
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(readyForNewMovePanel);
            Controls.Add(rightMovePanel);
            Controls.Add(clickPanel);
            Controls.Add(leftMovePanel);
            Controls.Add(verticalProgressBar);
            Name = "UserFeedbackView";
            Size = new Size(337, 223);
            ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.Timer guiUpdateTime;
        private UIControls.VerticalProgressBar verticalProgressBar;
        private Panel leftMovePanel;
        private Panel clickPanel;
        private Panel rightMovePanel;
        private Panel readyForNewMovePanel;
    }
}
