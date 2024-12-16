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
            splitContainer2 = new SplitContainer();
            splitContainer1 = new SplitContainer();
            distanceView = new Views.DistanceView();
            gestureViewScatter = new Views.GestureViewScatter();
            dbfDopplerView = new Views.DBFDopplerView();
            mainSplitContainer = new SplitContainer();
            userFeedbackView = new Views.UserFeedbackView();
            logView = new Views.LogView();
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
            ((System.ComponentModel.ISupportInitialize)splitContainer2).BeginInit();
            splitContainer2.Panel1.SuspendLayout();
            splitContainer2.Panel2.SuspendLayout();
            splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)mainSplitContainer).BeginInit();
            mainSplitContainer.Panel1.SuspendLayout();
            mainSplitContainer.Panel2.SuspendLayout();
            mainSplitContainer.SuspendLayout();
            SuspendLayout();
            // 
            // comPortLabel
            // 
            comPortLabel.AutoSize = true;
            comPortLabel.Location = new Point(10, 26);
            comPortLabel.Name = "comPortLabel";
            comPortLabel.Size = new Size(63, 15);
            comPortLabel.TabIndex = 0;
            comPortLabel.Text = "COM port:";
            // 
            // comPortComboBox
            // 
            comPortComboBox.FormattingEnabled = true;
            comPortComboBox.Location = new Point(83, 23);
            comPortComboBox.Margin = new Padding(3, 2, 3, 2);
            comPortComboBox.Name = "comPortComboBox";
            comPortComboBox.Size = new Size(225, 23);
            comPortComboBox.TabIndex = 1;
            // 
            // connectButton
            // 
            connectButton.Location = new Point(313, 22);
            connectButton.Margin = new Padding(3, 2, 3, 2);
            connectButton.Name = "connectButton";
            connectButton.Size = new Size(82, 22);
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
            menuStrip.Padding = new Padding(5, 2, 0, 2);
            menuStrip.Size = new Size(841, 24);
            menuStrip.TabIndex = 8;
            menuStrip.Text = "menuStrip1";
            // 
            // configurationToolStripMenuItem
            // 
            configurationToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { thresholdToolStripMenuItem, rangeToolStripMenuItem });
            configurationToolStripMenuItem.Name = "configurationToolStripMenuItem";
            configurationToolStripMenuItem.Size = new Size(93, 20);
            configurationToolStripMenuItem.Text = "Configuration";
            // 
            // thresholdToolStripMenuItem
            // 
            thresholdToolStripMenuItem.Name = "thresholdToolStripMenuItem";
            thresholdToolStripMenuItem.Size = new Size(126, 22);
            thresholdToolStripMenuItem.Text = "Threshold";
            thresholdToolStripMenuItem.Click += thresholdToolStripMenuItem_Click;
            // 
            // rangeToolStripMenuItem
            // 
            rangeToolStripMenuItem.Name = "rangeToolStripMenuItem";
            rangeToolStripMenuItem.Size = new Size(126, 22);
            rangeToolStripMenuItem.Text = "Range";
            rangeToolStripMenuItem.Click += rangeToolStripMenuItem_Click;
            // 
            // dataLoggerToolStripMenuItem
            // 
            dataLoggerToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { startToolStripMenuItem, stopToolStripMenuItem, playbackToolStripMenuItem });
            dataLoggerToolStripMenuItem.Name = "dataLoggerToolStripMenuItem";
            dataLoggerToolStripMenuItem.Size = new Size(80, 20);
            dataLoggerToolStripMenuItem.Text = "Data logger";
            // 
            // startToolStripMenuItem
            // 
            startToolStripMenuItem.Name = "startToolStripMenuItem";
            startToolStripMenuItem.Size = new Size(121, 22);
            startToolStripMenuItem.Text = "Start";
            startToolStripMenuItem.Click += startToolStripMenuItem_Click;
            // 
            // stopToolStripMenuItem
            // 
            stopToolStripMenuItem.Enabled = false;
            stopToolStripMenuItem.Name = "stopToolStripMenuItem";
            stopToolStripMenuItem.Size = new Size(121, 22);
            stopToolStripMenuItem.Text = "Stop";
            stopToolStripMenuItem.Click += stopToolStripMenuItem_Click;
            // 
            // playbackToolStripMenuItem
            // 
            playbackToolStripMenuItem.Name = "playbackToolStripMenuItem";
            playbackToolStripMenuItem.Size = new Size(121, 22);
            playbackToolStripMenuItem.Text = "Playback";
            playbackToolStripMenuItem.Click += playbackToolStripMenuItem_Click;
            // 
            // rdk2ConnectionStateTextBox
            // 
            rdk2ConnectionStateTextBox.Location = new Point(401, 23);
            rdk2ConnectionStateTextBox.Margin = new Padding(3, 2, 3, 2);
            rdk2ConnectionStateTextBox.Name = "rdk2ConnectionStateTextBox";
            rdk2ConnectionStateTextBox.ReadOnly = true;
            rdk2ConnectionStateTextBox.Size = new Size(110, 23);
            rdk2ConnectionStateTextBox.TabIndex = 9;
            rdk2ConnectionStateTextBox.Text = "Not connected";
            // 
            // rangefftView
            // 
            rangefftView.BorderStyle = BorderStyle.FixedSingle;
            rangefftView.Dock = DockStyle.Fill;
            rangefftView.Location = new Point(0, 0);
            rangefftView.Margin = new Padding(3, 2, 3, 2);
            rangefftView.Name = "rangefftView";
            rangefftView.Size = new Size(264, 297);
            rangefftView.TabIndex = 10;
            // 
            // statusStrip1
            // 
            statusStrip1.ImageScalingSize = new Size(20, 20);
            statusStrip1.Items.AddRange(new ToolStripItem[] { dataLoggerToolStripStatusLabel, handDetectedToolStripStatusLabel });
            statusStrip1.Location = new Point(0, 378);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Padding = new Padding(1, 0, 12, 0);
            statusStrip1.Size = new Size(841, 22);
            statusStrip1.TabIndex = 12;
            statusStrip1.Text = "statusStrip1";
            // 
            // dataLoggerToolStripStatusLabel
            // 
            dataLoggerToolStripStatusLabel.Name = "dataLoggerToolStripStatusLabel";
            dataLoggerToolStripStatusLabel.Size = new Size(114, 17);
            dataLoggerToolStripStatusLabel.Text = "Data logger stopped";
            // 
            // handDetectedToolStripStatusLabel
            // 
            handDetectedToolStripStatusLabel.ForeColor = Color.Green;
            handDetectedToolStripStatusLabel.Name = "handDetectedToolStripStatusLabel";
            handDetectedToolStripStatusLabel.Size = new Size(12, 17);
            handDetectedToolStripStatusLabel.Text = "-";
            // 
            // tabControl
            // 
            tabControl.Controls.Add(tabPage1);
            tabControl.Controls.Add(tabPage2);
            tabControl.Controls.Add(gestureTimeTabPage);
            tabControl.Controls.Add(tabPage3);
            tabControl.Dock = DockStyle.Fill;
            tabControl.Location = new Point(0, 0);
            tabControl.Margin = new Padding(3, 2, 3, 2);
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.Size = new Size(541, 329);
            tabControl.TabIndex = 13;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(rawSignalSplitContainer);
            tabPage1.Location = new Point(4, 24);
            tabPage1.Margin = new Padding(3, 2, 3, 2);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3, 2, 3, 2);
            tabPage1.Size = new Size(533, 301);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Raw signals";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // rawSignalSplitContainer
            // 
            rawSignalSplitContainer.Dock = DockStyle.Fill;
            rawSignalSplitContainer.Location = new Point(3, 2);
            rawSignalSplitContainer.Margin = new Padding(3, 2, 3, 2);
            rawSignalSplitContainer.Name = "rawSignalSplitContainer";
            // 
            // rawSignalSplitContainer.Panel1
            // 
            rawSignalSplitContainer.Panel1.Controls.Add(timeSignalView);
            // 
            // rawSignalSplitContainer.Panel2
            // 
            rawSignalSplitContainer.Panel2.Controls.Add(rangefftView);
            rawSignalSplitContainer.Size = new Size(527, 297);
            rawSignalSplitContainer.SplitterDistance = 259;
            rawSignalSplitContainer.TabIndex = 0;
            // 
            // timeSignalView
            // 
            timeSignalView.BorderStyle = BorderStyle.FixedSingle;
            timeSignalView.Dock = DockStyle.Fill;
            timeSignalView.Location = new Point(0, 0);
            timeSignalView.Margin = new Padding(3, 2, 3, 2);
            timeSignalView.Name = "timeSignalView";
            timeSignalView.Size = new Size(259, 297);
            timeSignalView.TabIndex = 14;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(energyOverTimeView);
            tabPage2.Location = new Point(4, 24);
            tabPage2.Margin = new Padding(3, 2, 3, 2);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3, 2, 3, 2);
            tabPage2.Size = new Size(534, 301);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Processed";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // energyOverTimeView
            // 
            energyOverTimeView.BorderStyle = BorderStyle.FixedSingle;
            energyOverTimeView.Dock = DockStyle.Fill;
            energyOverTimeView.Location = new Point(3, 2);
            energyOverTimeView.Margin = new Padding(3, 2, 3, 2);
            energyOverTimeView.Name = "energyOverTimeView";
            energyOverTimeView.Size = new Size(528, 297);
            energyOverTimeView.TabIndex = 14;
            // 
            // gestureTimeTabPage
            // 
            gestureTimeTabPage.Controls.Add(gestureViewTime);
            gestureTimeTabPage.Location = new Point(4, 24);
            gestureTimeTabPage.Margin = new Padding(3, 2, 3, 2);
            gestureTimeTabPage.Name = "gestureTimeTabPage";
            gestureTimeTabPage.Padding = new Padding(3, 2, 3, 2);
            gestureTimeTabPage.Size = new Size(534, 301);
            gestureTimeTabPage.TabIndex = 5;
            gestureTimeTabPage.Text = "Gesture Time";
            gestureTimeTabPage.UseVisualStyleBackColor = true;
            // 
            // gestureViewTime
            // 
            gestureViewTime.BorderStyle = BorderStyle.FixedSingle;
            gestureViewTime.Dock = DockStyle.Fill;
            gestureViewTime.Location = new Point(3, 2);
            gestureViewTime.Margin = new Padding(3, 2, 3, 2);
            gestureViewTime.Name = "gestureViewTime";
            gestureViewTime.Size = new Size(528, 297);
            gestureViewTime.TabIndex = 0;
            // 
            // tabPage3
            // 
            tabPage3.Controls.Add(splitContainer2);
            tabPage3.Location = new Point(4, 24);
            tabPage3.Margin = new Padding(3, 2, 3, 2);
            tabPage3.Name = "tabPage3";
            tabPage3.Padding = new Padding(3, 2, 3, 2);
            tabPage3.Size = new Size(534, 301);
            tabPage3.TabIndex = 6;
            tabPage3.Text = "Distance Angle";
            tabPage3.UseVisualStyleBackColor = true;
            // 
            // splitContainer2
            // 
            splitContainer2.Dock = DockStyle.Fill;
            splitContainer2.Location = new Point(3, 2);
            splitContainer2.Margin = new Padding(3, 2, 3, 2);
            splitContainer2.Name = "splitContainer2";
            splitContainer2.Orientation = Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            splitContainer2.Panel1.Controls.Add(splitContainer1);
            // 
            // splitContainer2.Panel2
            // 
            splitContainer2.Panel2.Controls.Add(dbfDopplerView);
            splitContainer2.Size = new Size(528, 297);
            splitContainer2.SplitterDistance = 141;
            splitContainer2.SplitterWidth = 3;
            splitContainer2.TabIndex = 2;
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Margin = new Padding(3, 2, 3, 2);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(distanceView);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(gestureViewScatter);
            splitContainer1.Size = new Size(528, 141);
            splitContainer1.SplitterDistance = 262;
            splitContainer1.TabIndex = 1;
            // 
            // distanceView
            // 
            distanceView.BorderStyle = BorderStyle.FixedSingle;
            distanceView.Dock = DockStyle.Fill;
            distanceView.Location = new Point(0, 0);
            distanceView.Margin = new Padding(3, 2, 3, 2);
            distanceView.Name = "distanceView";
            distanceView.Size = new Size(262, 141);
            distanceView.TabIndex = 0;
            // 
            // gestureViewScatter
            // 
            gestureViewScatter.BorderStyle = BorderStyle.FixedSingle;
            gestureViewScatter.Dock = DockStyle.Fill;
            gestureViewScatter.Location = new Point(0, 0);
            gestureViewScatter.Margin = new Padding(3, 2, 3, 2);
            gestureViewScatter.Name = "gestureViewScatter";
            gestureViewScatter.Size = new Size(262, 141);
            gestureViewScatter.TabIndex = 0;
            // 
            // dbfDopplerView
            // 
            dbfDopplerView.Dock = DockStyle.Fill;
            dbfDopplerView.Location = new Point(0, 0);
            dbfDopplerView.Margin = new Padding(3, 2, 3, 2);
            dbfDopplerView.Name = "dbfDopplerView";
            dbfDopplerView.Size = new Size(528, 153);
            dbfDopplerView.TabIndex = 0;
            // 
            // mainSplitContainer
            // 
            mainSplitContainer.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            mainSplitContainer.Location = new Point(10, 49);
            mainSplitContainer.Margin = new Padding(3, 2, 3, 2);
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
            mainSplitContainer.Size = new Size(820, 329);
            mainSplitContainer.SplitterDistance = 541;
            mainSplitContainer.TabIndex = 14;
            // 
            // userFeedbackView
            // 
            userFeedbackView.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            userFeedbackView.Location = new Point(3, 2);
            userFeedbackView.Margin = new Padding(3, 2, 3, 2);
            userFeedbackView.Name = "userFeedbackView";
            userFeedbackView.Size = new Size(270, 166);
            userFeedbackView.TabIndex = 1;
            // 
            // logView
            // 
            logView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            logView.Location = new Point(0, 172);
            logView.Margin = new Padding(3, 2, 3, 2);
            logView.Name = "logView";
            logView.Size = new Size(275, 157);
            logView.TabIndex = 0;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(841, 400);
            Controls.Add(mainSplitContainer);
            Controls.Add(statusStrip1);
            Controls.Add(rdk2ConnectionStateTextBox);
            Controls.Add(connectButton);
            Controls.Add(comPortComboBox);
            Controls.Add(comPortLabel);
            Controls.Add(menuStrip);
            MainMenuStrip = menuStrip;
            Margin = new Padding(3, 2, 3, 2);
            Name = "MainForm";
            Text = "RDK2 - Radar - Signal Processing - v1.0";
            Load += MainForm_Load;
            KeyPress += MainForm_KeyPress;
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
            splitContainer2.Panel1.ResumeLayout(false);
            splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
            splitContainer2.ResumeLayout(false);
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
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
        private Views.DBFDopplerView dbfDopplerView;
        private SplitContainer splitContainer2;
    }
}
