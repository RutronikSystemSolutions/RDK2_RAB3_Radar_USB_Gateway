namespace RDK2_Radar_SignalProcessing_GUI
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            comPortLabel = new Label();
            comPortComboBox = new ComboBox();
            connectButton = new Button();
            menuStrip = new MenuStrip();
            configurationToolStripMenuItem = new ToolStripMenuItem();
            thresholdToolStripMenuItem = new ToolStripMenuItem();
            rangeToolStripMenuItem = new ToolStripMenuItem();
            dataLoggerToolStripMenuItem = new ToolStripMenuItem();
            startToolStripMenuItem = new ToolStripMenuItem();
            stopToolStripMenuItem = new ToolStripMenuItem();
            playbackToolStripMenuItem = new ToolStripMenuItem();
            rdk2ConnectionStateTextBox = new TextBox();
            rangefftView = new Views.RangeFFTView();
            statusStrip1 = new StatusStrip();
            dataLoggerToolStripStatusLabel = new ToolStripStatusLabel();
            handDetectedToolStripStatusLabel = new ToolStripStatusLabel();
            tabControl = new TabControl();
            tabPage1 = new TabPage();
            rawSignalSplitContainer = new SplitContainer();
            timeSignalView = new Views.TimeSignalView();
            tabPage2 = new TabPage();
            energyOverTimeView = new Views.EnergyOverTimeView();
            gestureTimeTabPage = new TabPage();
            gestureViewTime = new Views.GestureViewTime();
            tabPage3 = new TabPage();
            splitContainer1 = new SplitContainer();
            distanceView = new Views.DistanceView();
            gestureViewScatter = new Views.GestureViewScatter();
            dbfTabPage = new TabPage();
            mainSplitContainer = new SplitContainer();
            userFeedbackView = new Views.UserFeedbackView();
            logView = new Views.LogView();
            dbfDopplerView = new Views.DBFDopplerView();
            menuStrip.SuspendLayout();
            statusStrip1.SuspendLayout();
            tabControl.SuspendLayout();
            tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)rawSignalSplitContainer).BeginInit();
            rawSignalSplitContainer.Panel1.SuspendLayout();
            rawSignalSplitContainer.Panel2.SuspendLayout();
            rawSignalSplitContainer.SuspendLayout();
            tabPage2.SuspendLayout();
            gestureTimeTabPage.SuspendLayout();
            tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            dbfTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)mainSplitContainer).BeginInit();
            mainSplitContainer.Panel1.SuspendLayout();
            mainSplitContainer.Panel2.SuspendLayout();
            mainSplitContainer.SuspendLayout();
            SuspendLayout();
            // 
            // comPortLabel
            // 
            comPortLabel.AutoSize = true;
            comPortLabel.Location = new Point(12, 34);
            comPortLabel.Name = "comPortLabel";
            comPortLabel.Size = new Size(77, 20);
            comPortLabel.TabIndex = 0;
            comPortLabel.Text = "COM port:";
            // 
            // comPortComboBox
            // 
            comPortComboBox.FormattingEnabled = true;
            comPortComboBox.Location = new Point(95, 31);
            comPortComboBox.Name = "comPortComboBox";
            comPortComboBox.Size = new Size(257, 28);
            comPortComboBox.TabIndex = 1;
            // 
            // connectButton
            // 
            connectButton.Location = new Point(358, 30);
            connectButton.Name = "connectButton";
            connectButton.Size = new Size(94, 29);
            connectButton.TabIndex = 2;
            connectButton.Text = "Connect";
            connectButton.UseVisualStyleBackColor = true;
            connectButton.Click += connectButton_Click;
            // 
            // menuStrip
            // 
            menuStrip.ImageScalingSize = new Size(20, 20);
            menuStrip.Items.AddRange(new ToolStripItem[] { configurationToolStripMenuItem, dataLoggerToolStripMenuItem });
            menuStrip.Location = new Point(0, 0);
            menuStrip.Name = "menuStrip";
            menuStrip.Size = new Size(961, 28);
            menuStrip.TabIndex = 8;
            menuStrip.Text = "menuStrip1";
            // 
            // configurationToolStripMenuItem
            // 
            configurationToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { thresholdToolStripMenuItem, rangeToolStripMenuItem });
            configurationToolStripMenuItem.Name = "configurationToolStripMenuItem";
            configurationToolStripMenuItem.Size = new Size(114, 24);
            configurationToolStripMenuItem.Text = "Configuration";
            // 
            // thresholdToolStripMenuItem
            // 
            thresholdToolStripMenuItem.Name = "thresholdToolStripMenuItem";
            thresholdToolStripMenuItem.Size = new Size(157, 26);
            thresholdToolStripMenuItem.Text = "Threshold";
            thresholdToolStripMenuItem.Click += thresholdToolStripMenuItem_Click;
            // 
            // rangeToolStripMenuItem
            // 
            rangeToolStripMenuItem.Name = "rangeToolStripMenuItem";
            rangeToolStripMenuItem.Size = new Size(157, 26);
            rangeToolStripMenuItem.Text = "Range";
            rangeToolStripMenuItem.Click += rangeToolStripMenuItem_Click;
            // 
            // dataLoggerToolStripMenuItem
            // 
            dataLoggerToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { startToolStripMenuItem, stopToolStripMenuItem, playbackToolStripMenuItem });
            dataLoggerToolStripMenuItem.Name = "dataLoggerToolStripMenuItem";
            dataLoggerToolStripMenuItem.Size = new Size(103, 24);
            dataLoggerToolStripMenuItem.Text = "Data logger";
            // 
            // startToolStripMenuItem
            // 
            startToolStripMenuItem.Name = "startToolStripMenuItem";
            startToolStripMenuItem.Size = new Size(150, 26);
            startToolStripMenuItem.Text = "Start";
            startToolStripMenuItem.Click += startToolStripMenuItem_Click;
            // 
            // stopToolStripMenuItem
            // 
            stopToolStripMenuItem.Enabled = false;
            stopToolStripMenuItem.Name = "stopToolStripMenuItem";
            stopToolStripMenuItem.Size = new Size(150, 26);
            stopToolStripMenuItem.Text = "Stop";
            stopToolStripMenuItem.Click += stopToolStripMenuItem_Click;
            // 
            // playbackToolStripMenuItem
            // 
            playbackToolStripMenuItem.Name = "playbackToolStripMenuItem";
            playbackToolStripMenuItem.Size = new Size(150, 26);
            playbackToolStripMenuItem.Text = "Playback";
            playbackToolStripMenuItem.Click += playbackToolStripMenuItem_Click;
            // 
            // rdk2ConnectionStateTextBox
            // 
            rdk2ConnectionStateTextBox.Location = new Point(458, 31);
            rdk2ConnectionStateTextBox.Name = "rdk2ConnectionStateTextBox";
            rdk2ConnectionStateTextBox.ReadOnly = true;
            rdk2ConnectionStateTextBox.Size = new Size(125, 27);
            rdk2ConnectionStateTextBox.TabIndex = 9;
            rdk2ConnectionStateTextBox.Text = "Not connected";
            // 
            // rangefftView
            // 
            rangefftView.BorderStyle = BorderStyle.FixedSingle;
            rangefftView.Dock = DockStyle.Fill;
            rangefftView.Location = new Point(0, 0);
            rangefftView.Name = "rangefftView";
            rangefftView.Size = new Size(303, 400);
            rangefftView.TabIndex = 10;
            // 
            // statusStrip1
            // 
            statusStrip1.ImageScalingSize = new Size(20, 20);
            statusStrip1.Items.AddRange(new ToolStripItem[] { dataLoggerToolStripStatusLabel, handDetectedToolStripStatusLabel });
            statusStrip1.Location = new Point(0, 507);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(961, 26);
            statusStrip1.TabIndex = 12;
            statusStrip1.Text = "statusStrip1";
            // 
            // dataLoggerToolStripStatusLabel
            // 
            dataLoggerToolStripStatusLabel.Name = "dataLoggerToolStripStatusLabel";
            dataLoggerToolStripStatusLabel.Size = new Size(148, 20);
            dataLoggerToolStripStatusLabel.Text = "Data logger stopped";
            // 
            // handDetectedToolStripStatusLabel
            // 
            handDetectedToolStripStatusLabel.ForeColor = Color.Green;
            handDetectedToolStripStatusLabel.Name = "handDetectedToolStripStatusLabel";
            handDetectedToolStripStatusLabel.Size = new Size(15, 20);
            handDetectedToolStripStatusLabel.Text = "-";
            // 
            // tabControl
            // 
            tabControl.Controls.Add(tabPage1);
            tabControl.Controls.Add(tabPage2);
            tabControl.Controls.Add(gestureTimeTabPage);
            tabControl.Controls.Add(tabPage3);
            tabControl.Controls.Add(dbfTabPage);
            tabControl.Dock = DockStyle.Fill;
            tabControl.Location = new Point(0, 0);
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.Size = new Size(619, 439);
            tabControl.TabIndex = 13;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(rawSignalSplitContainer);
            tabPage1.Location = new Point(4, 29);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(611, 406);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Raw signals";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // rawSignalSplitContainer
            // 
            rawSignalSplitContainer.Dock = DockStyle.Fill;
            rawSignalSplitContainer.Location = new Point(3, 3);
            rawSignalSplitContainer.Name = "rawSignalSplitContainer";
            // 
            // rawSignalSplitContainer.Panel1
            // 
            rawSignalSplitContainer.Panel1.Controls.Add(timeSignalView);
            // 
            // rawSignalSplitContainer.Panel2
            // 
            rawSignalSplitContainer.Panel2.Controls.Add(rangefftView);
            rawSignalSplitContainer.Size = new Size(605, 400);
            rawSignalSplitContainer.SplitterDistance = 298;
            rawSignalSplitContainer.TabIndex = 0;
            // 
            // timeSignalView
            // 
            timeSignalView.BorderStyle = BorderStyle.FixedSingle;
            timeSignalView.Dock = DockStyle.Fill;
            timeSignalView.Location = new Point(0, 0);
            timeSignalView.Name = "timeSignalView";
            timeSignalView.Size = new Size(298, 400);
            timeSignalView.TabIndex = 14;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(energyOverTimeView);
            tabPage2.Location = new Point(4, 29);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(611, 406);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Processed";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // energyOverTimeView
            // 
            energyOverTimeView.BorderStyle = BorderStyle.FixedSingle;
            energyOverTimeView.Dock = DockStyle.Fill;
            energyOverTimeView.Location = new Point(3, 3);
            energyOverTimeView.Name = "energyOverTimeView";
            energyOverTimeView.Size = new Size(605, 400);
            energyOverTimeView.TabIndex = 14;
            // 
            // gestureTimeTabPage
            // 
            gestureTimeTabPage.Controls.Add(gestureViewTime);
            gestureTimeTabPage.Location = new Point(4, 29);
            gestureTimeTabPage.Name = "gestureTimeTabPage";
            gestureTimeTabPage.Padding = new Padding(3);
            gestureTimeTabPage.Size = new Size(611, 406);
            gestureTimeTabPage.TabIndex = 5;
            gestureTimeTabPage.Text = "Gesture Time";
            gestureTimeTabPage.UseVisualStyleBackColor = true;
            // 
            // gestureViewTime
            // 
            gestureViewTime.BorderStyle = BorderStyle.FixedSingle;
            gestureViewTime.Dock = DockStyle.Fill;
            gestureViewTime.Location = new Point(3, 3);
            gestureViewTime.Name = "gestureViewTime";
            gestureViewTime.Size = new Size(605, 400);
            gestureViewTime.TabIndex = 0;
            // 
            // tabPage3
            // 
            tabPage3.Controls.Add(splitContainer1);
            tabPage3.Location = new Point(4, 29);
            tabPage3.Name = "tabPage3";
            tabPage3.Padding = new Padding(3);
            tabPage3.Size = new Size(611, 406);
            tabPage3.TabIndex = 6;
            tabPage3.Text = "Distance Angle";
            tabPage3.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(3, 3);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(distanceView);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(gestureViewScatter);
            splitContainer1.Size = new Size(605, 400);
            splitContainer1.SplitterDistance = 301;
            splitContainer1.TabIndex = 1;
            // 
            // distanceView
            // 
            distanceView.BorderStyle = BorderStyle.FixedSingle;
            distanceView.Dock = DockStyle.Fill;
            distanceView.Location = new Point(0, 0);
            distanceView.Name = "distanceView";
            distanceView.Size = new Size(301, 400);
            distanceView.TabIndex = 0;
            // 
            // gestureViewScatter
            // 
            gestureViewScatter.BorderStyle = BorderStyle.FixedSingle;
            gestureViewScatter.Dock = DockStyle.Fill;
            gestureViewScatter.Location = new Point(0, 0);
            gestureViewScatter.Name = "gestureViewScatter";
            gestureViewScatter.Size = new Size(300, 400);
            gestureViewScatter.TabIndex = 0;
            // 
            // dbfTabPage
            // 
            dbfTabPage.Controls.Add(dbfDopplerView);
            dbfTabPage.Location = new Point(4, 29);
            dbfTabPage.Name = "dbfTabPage";
            dbfTabPage.Padding = new Padding(3);
            dbfTabPage.Size = new Size(611, 406);
            dbfTabPage.TabIndex = 7;
            dbfTabPage.Text = "DBF";
            dbfTabPage.UseVisualStyleBackColor = true;
            // 
            // mainSplitContainer
            // 
            mainSplitContainer.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            mainSplitContainer.Location = new Point(12, 65);
            mainSplitContainer.Name = "mainSplitContainer";
            // 
            // mainSplitContainer.Panel1
            // 
            mainSplitContainer.Panel1.Controls.Add(tabControl);
            // 
            // mainSplitContainer.Panel2
            // 
            mainSplitContainer.Panel2.Controls.Add(userFeedbackView);
            mainSplitContainer.Panel2.Controls.Add(logView);
            mainSplitContainer.Size = new Size(937, 439);
            mainSplitContainer.SplitterDistance = 619;
            mainSplitContainer.TabIndex = 14;
            // 
            // userFeedbackView
            // 
            userFeedbackView.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            userFeedbackView.Location = new Point(3, 3);
            userFeedbackView.Name = "userFeedbackView";
            userFeedbackView.Size = new Size(308, 221);
            userFeedbackView.TabIndex = 1;
            // 
            // logView
            // 
            logView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            logView.Location = new Point(0, 230);
            logView.Name = "logView";
            logView.Size = new Size(314, 209);
            logView.TabIndex = 0;
            // 
            // dbfDopplerView
            // 
            dbfDopplerView.Dock = DockStyle.Fill;
            dbfDopplerView.Location = new Point(3, 3);
            dbfDopplerView.Name = "dbfDopplerView";
            dbfDopplerView.Size = new Size(605, 400);
            dbfDopplerView.TabIndex = 0;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(961, 533);
            Controls.Add(mainSplitContainer);
            Controls.Add(statusStrip1);
            Controls.Add(rdk2ConnectionStateTextBox);
            Controls.Add(connectButton);
            Controls.Add(comPortComboBox);
            Controls.Add(comPortLabel);
            Controls.Add(menuStrip);
            MainMenuStrip = menuStrip;
            Name = "MainForm";
            Text = "RDK2 - Radar - Signal Processing - v1.0";
            Load += MainForm_Load;
            menuStrip.ResumeLayout(false);
            menuStrip.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            tabControl.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            rawSignalSplitContainer.Panel1.ResumeLayout(false);
            rawSignalSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)rawSignalSplitContainer).EndInit();
            rawSignalSplitContainer.ResumeLayout(false);
            tabPage2.ResumeLayout(false);
            gestureTimeTabPage.ResumeLayout(false);
            tabPage3.ResumeLayout(false);
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            dbfTabPage.ResumeLayout(false);
            mainSplitContainer.Panel1.ResumeLayout(false);
            mainSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)mainSplitContainer).EndInit();
            mainSplitContainer.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label comPortLabel;
        private ComboBox comPortComboBox;
        private Button connectButton;
        private MenuStrip menuStrip;
        private ToolStripMenuItem configurationToolStripMenuItem;
        private TextBox rdk2ConnectionStateTextBox;
        private Views.RangeFFTView rangefftView;
        private ToolStripMenuItem thresholdToolStripMenuItem;
        private ToolStripMenuItem rangeToolStripMenuItem;
        private ToolStripMenuItem dataLoggerToolStripMenuItem;
        private ToolStripMenuItem startToolStripMenuItem;
        private ToolStripMenuItem stopToolStripMenuItem;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel dataLoggerToolStripStatusLabel;
        private TabControl tabControl;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private SplitContainer rawSignalSplitContainer;
        private Views.TimeSignalView timeSignalView;
        private Views.EnergyOverTimeView energyOverTimeView;
        private TabPage gestureTimeTabPage;
        private Views.GestureViewTime gestureViewTime;
        private Views.GestureViewScatter gestureViewScatter;
        private TabPage tabPage3;
        private Views.DistanceView distanceView;
        private SplitContainer splitContainer1;
        private SplitContainer mainSplitContainer;
        private Views.LogView logView;
        private ToolStripStatusLabel handDetectedToolStripStatusLabel;
        private ToolStripMenuItem playbackToolStripMenuItem;
        private Views.UserFeedbackView userFeedbackView;
        private TabPage dbfTabPage;
        private Views.DBFDopplerView dbfDopplerView;
    }
}
