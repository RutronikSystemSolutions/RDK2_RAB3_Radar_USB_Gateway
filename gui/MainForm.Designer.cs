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
            dbfView = new Views.DBFView();
            anglePresenceView = new Views.AnglePresenceView();
            menuStrip = new MenuStrip();
            configurationToolStripMenuItem = new ToolStripMenuItem();
            backgroundFilterToolStripMenuItem = new ToolStripMenuItem();
            thresholdToolStripMenuItem = new ToolStripMenuItem();
            rangeToolStripMenuItem = new ToolStripMenuItem();
            dataLoggerToolStripMenuItem = new ToolStripMenuItem();
            startToolStripMenuItem = new ToolStripMenuItem();
            stopToolStripMenuItem = new ToolStripMenuItem();
            rdk2ConnectionStateTextBox = new TextBox();
            rangefftView = new Views.RangeFFTView();
            dopplerfftView = new Views.DopplerFFTView();
            statusStrip1 = new StatusStrip();
            dataLoggerToolStripStatusLabel = new ToolStripStatusLabel();
            tabControl = new TabControl();
            tabPage1 = new TabPage();
            rawSignalSplitContainer = new SplitContainer();
            timeSignalView = new Views.TimeSignalView();
            tabPage2 = new TabPage();
            processedSplitContainer = new SplitContainer();
            energyOverTimeView = new Views.EnergyOverTimeView();
            tabPage3 = new TabPage();
            tabPage4 = new TabPage();
            menuStrip.SuspendLayout();
            statusStrip1.SuspendLayout();
            tabControl.SuspendLayout();
            tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)rawSignalSplitContainer).BeginInit();
            rawSignalSplitContainer.Panel1.SuspendLayout();
            rawSignalSplitContainer.Panel2.SuspendLayout();
            rawSignalSplitContainer.SuspendLayout();
            tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)processedSplitContainer).BeginInit();
            processedSplitContainer.Panel1.SuspendLayout();
            processedSplitContainer.Panel2.SuspendLayout();
            processedSplitContainer.SuspendLayout();
            tabPage3.SuspendLayout();
            tabPage4.SuspendLayout();
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
            // dbfView
            // 
            dbfView.BorderStyle = BorderStyle.FixedSingle;
            dbfView.Dock = DockStyle.Fill;
            dbfView.Location = new Point(3, 3);
            dbfView.Name = "dbfView";
            dbfView.Size = new Size(923, 400);
            dbfView.TabIndex = 6;
            // 
            // anglePresenceView
            // 
            anglePresenceView.BorderStyle = BorderStyle.FixedSingle;
            anglePresenceView.Dock = DockStyle.Fill;
            anglePresenceView.Location = new Point(3, 3);
            anglePresenceView.Name = "anglePresenceView";
            anglePresenceView.Size = new Size(923, 400);
            anglePresenceView.TabIndex = 7;
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
            configurationToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { backgroundFilterToolStripMenuItem, thresholdToolStripMenuItem, rangeToolStripMenuItem });
            configurationToolStripMenuItem.Name = "configurationToolStripMenuItem";
            configurationToolStripMenuItem.Size = new Size(114, 24);
            configurationToolStripMenuItem.Text = "Configuration";
            // 
            // backgroundFilterToolStripMenuItem
            // 
            backgroundFilterToolStripMenuItem.Name = "backgroundFilterToolStripMenuItem";
            backgroundFilterToolStripMenuItem.Size = new Size(206, 26);
            backgroundFilterToolStripMenuItem.Text = "Background filter";
            backgroundFilterToolStripMenuItem.Click += backgroundFilterToolStripMenuItem_Click;
            // 
            // thresholdToolStripMenuItem
            // 
            thresholdToolStripMenuItem.Name = "thresholdToolStripMenuItem";
            thresholdToolStripMenuItem.Size = new Size(206, 26);
            thresholdToolStripMenuItem.Text = "Threshold";
            thresholdToolStripMenuItem.Click += thresholdToolStripMenuItem_Click;
            // 
            // rangeToolStripMenuItem
            // 
            rangeToolStripMenuItem.Name = "rangeToolStripMenuItem";
            rangeToolStripMenuItem.Size = new Size(206, 26);
            rangeToolStripMenuItem.Text = "Range";
            rangeToolStripMenuItem.Click += rangeToolStripMenuItem_Click;
            // 
            // dataLoggerToolStripMenuItem
            // 
            dataLoggerToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { startToolStripMenuItem, stopToolStripMenuItem });
            dataLoggerToolStripMenuItem.Name = "dataLoggerToolStripMenuItem";
            dataLoggerToolStripMenuItem.Size = new Size(103, 24);
            dataLoggerToolStripMenuItem.Text = "Data logger";
            // 
            // startToolStripMenuItem
            // 
            startToolStripMenuItem.Name = "startToolStripMenuItem";
            startToolStripMenuItem.Size = new Size(123, 26);
            startToolStripMenuItem.Text = "Start";
            // 
            // stopToolStripMenuItem
            // 
            stopToolStripMenuItem.Enabled = false;
            stopToolStripMenuItem.Name = "stopToolStripMenuItem";
            stopToolStripMenuItem.Size = new Size(123, 26);
            stopToolStripMenuItem.Text = "Stop";
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
            rangefftView.Size = new Size(463, 400);
            rangefftView.TabIndex = 10;
            // 
            // dopplerfftView
            // 
            dopplerfftView.BorderStyle = BorderStyle.FixedSingle;
            dopplerfftView.Dock = DockStyle.Fill;
            dopplerfftView.Location = new Point(0, 0);
            dopplerfftView.Name = "dopplerfftView";
            dopplerfftView.Size = new Size(462, 400);
            dopplerfftView.TabIndex = 11;
            // 
            // statusStrip1
            // 
            statusStrip1.ImageScalingSize = new Size(20, 20);
            statusStrip1.Items.AddRange(new ToolStripItem[] { dataLoggerToolStripStatusLabel });
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
            // tabControl
            // 
            tabControl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tabControl.Controls.Add(tabPage1);
            tabControl.Controls.Add(tabPage2);
            tabControl.Controls.Add(tabPage3);
            tabControl.Controls.Add(tabPage4);
            tabControl.Location = new Point(12, 65);
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.Size = new Size(937, 439);
            tabControl.TabIndex = 13;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(rawSignalSplitContainer);
            tabPage1.Location = new Point(4, 29);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(929, 406);
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
            rawSignalSplitContainer.Size = new Size(923, 400);
            rawSignalSplitContainer.SplitterDistance = 456;
            rawSignalSplitContainer.TabIndex = 0;
            // 
            // timeSignalView
            // 
            timeSignalView.BorderStyle = BorderStyle.FixedSingle;
            timeSignalView.Dock = DockStyle.Fill;
            timeSignalView.Location = new Point(0, 0);
            timeSignalView.Name = "timeSignalView";
            timeSignalView.Size = new Size(456, 400);
            timeSignalView.TabIndex = 14;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(processedSplitContainer);
            tabPage2.Location = new Point(4, 29);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(929, 406);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Processed";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // processedSplitContainer
            // 
            processedSplitContainer.Dock = DockStyle.Fill;
            processedSplitContainer.Location = new Point(3, 3);
            processedSplitContainer.Name = "processedSplitContainer";
            // 
            // processedSplitContainer.Panel1
            // 
            processedSplitContainer.Panel1.Controls.Add(energyOverTimeView);
            // 
            // processedSplitContainer.Panel2
            // 
            processedSplitContainer.Panel2.Controls.Add(dopplerfftView);
            processedSplitContainer.Size = new Size(923, 400);
            processedSplitContainer.SplitterDistance = 457;
            processedSplitContainer.TabIndex = 0;
            // 
            // energyOverTimeView
            // 
            energyOverTimeView.BorderStyle = BorderStyle.FixedSingle;
            energyOverTimeView.Dock = DockStyle.Fill;
            energyOverTimeView.Location = new Point(0, 0);
            energyOverTimeView.Name = "energyOverTimeView";
            energyOverTimeView.Size = new Size(457, 400);
            energyOverTimeView.TabIndex = 14;
            // 
            // tabPage3
            // 
            tabPage3.Controls.Add(anglePresenceView);
            tabPage3.Location = new Point(4, 29);
            tabPage3.Name = "tabPage3";
            tabPage3.Padding = new Padding(3);
            tabPage3.Size = new Size(929, 406);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "Angle view";
            tabPage3.UseVisualStyleBackColor = true;
            // 
            // tabPage4
            // 
            tabPage4.Controls.Add(dbfView);
            tabPage4.Location = new Point(4, 29);
            tabPage4.Name = "tabPage4";
            tabPage4.Padding = new Padding(3);
            tabPage4.Size = new Size(929, 406);
            tabPage4.TabIndex = 3;
            tabPage4.Text = "DBF View";
            tabPage4.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(961, 533);
            Controls.Add(tabControl);
            Controls.Add(statusStrip1);
            Controls.Add(rdk2ConnectionStateTextBox);
            Controls.Add(connectButton);
            Controls.Add(comPortComboBox);
            Controls.Add(comPortLabel);
            Controls.Add(menuStrip);
            MainMenuStrip = menuStrip;
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
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
            processedSplitContainer.Panel1.ResumeLayout(false);
            processedSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)processedSplitContainer).EndInit();
            processedSplitContainer.ResumeLayout(false);
            tabPage3.ResumeLayout(false);
            tabPage4.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label comPortLabel;
        private ComboBox comPortComboBox;
        private Button connectButton;
        private Views.DBFView dbfView;
        private Views.AnglePresenceView anglePresenceView;
        private MenuStrip menuStrip;
        private ToolStripMenuItem configurationToolStripMenuItem;
        private ToolStripMenuItem backgroundFilterToolStripMenuItem;
        private TextBox rdk2ConnectionStateTextBox;
        private Views.RangeFFTView rangefftView;
        private ToolStripMenuItem thresholdToolStripMenuItem;
        private ToolStripMenuItem rangeToolStripMenuItem;
        private Views.DopplerFFTView dopplerfftView;
        private ToolStripMenuItem dataLoggerToolStripMenuItem;
        private ToolStripMenuItem startToolStripMenuItem;
        private ToolStripMenuItem stopToolStripMenuItem;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel dataLoggerToolStripStatusLabel;
        private TabControl tabControl;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private TabPage tabPage3;
        private TabPage tabPage4;
        private SplitContainer processedSplitContainer;
        private SplitContainer rawSignalSplitContainer;
        private Views.TimeSignalView timeSignalView;
        private Views.EnergyOverTimeView energyOverTimeView;
    }
}
