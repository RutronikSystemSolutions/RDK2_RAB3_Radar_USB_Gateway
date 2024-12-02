namespace RDK2_Radar_SignalProcessing_GUI.UIControls
{
    partial class VerticalProgressBar
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
            valuePanel = new Panel();
            SuspendLayout();
            // 
            // valuePanel
            // 
            valuePanel.BackColor = Color.Violet;
            valuePanel.Dock = DockStyle.Top;
            valuePanel.Location = new Point(0, 0);
            valuePanel.Name = "valuePanel";
            valuePanel.Size = new Size(80, 0);
            valuePanel.TabIndex = 0;
            // 
            // VerticalProgressBar
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BorderStyle = BorderStyle.FixedSingle;
            Controls.Add(valuePanel);
            Name = "VerticalProgressBar";
            Size = new Size(80, 281);
            ResumeLayout(false);
        }

        #endregion

        private Panel valuePanel;
    }
}
