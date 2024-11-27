namespace RDK2_Radar_SignalProcessing_GUI.Views
{
    partial class LogView
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
            logTabControl = new TabControl();
            logsTabPage = new TabPage();
            logsTextBox = new TextBox();
            logTabControl.SuspendLayout();
            logsTabPage.SuspendLayout();
            SuspendLayout();
            // 
            // logTabControl
            // 
            logTabControl.Controls.Add(logsTabPage);
            logTabControl.Dock = DockStyle.Fill;
            logTabControl.Location = new Point(0, 0);
            logTabControl.Name = "logTabControl";
            logTabControl.SelectedIndex = 0;
            logTabControl.Size = new Size(333, 377);
            logTabControl.TabIndex = 0;
            // 
            // logsTabPage
            // 
            logsTabPage.Controls.Add(logsTextBox);
            logsTabPage.Location = new Point(4, 29);
            logsTabPage.Name = "logsTabPage";
            logsTabPage.Padding = new Padding(3);
            logsTabPage.Size = new Size(325, 344);
            logsTabPage.TabIndex = 0;
            logsTabPage.Text = "Logs";
            logsTabPage.UseVisualStyleBackColor = true;
            // 
            // logsTextBox
            // 
            logsTextBox.Dock = DockStyle.Fill;
            logsTextBox.Location = new Point(3, 3);
            logsTextBox.Multiline = true;
            logsTextBox.Name = "logsTextBox";
            logsTextBox.ReadOnly = true;
            logsTextBox.Size = new Size(319, 338);
            logsTextBox.TabIndex = 0;
            // 
            // LogView
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(logTabControl);
            Name = "LogView";
            Size = new Size(333, 377);
            logTabControl.ResumeLayout(false);
            logsTabPage.ResumeLayout(false);
            logsTabPage.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TabControl logTabControl;
        private TabPage logsTabPage;
        private TextBox logsTextBox;
    }
}
