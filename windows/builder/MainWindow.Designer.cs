namespace Builder
{
    partial class MainWindow
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
            this.lbLog = new System.Windows.Forms.ListBox();
            this.btnBuild = new System.Windows.Forms.Button();
            this.tbBaseDirectory = new System.Windows.Forms.TextBox();
            this.gbBaseDirectory = new System.Windows.Forms.GroupBox();
            this.btnBaseDirectorySelect = new System.Windows.Forms.Button();
            this.gbOptions = new System.Windows.Forms.GroupBox();
            this.cbEEPROM = new System.Windows.Forms.CheckBox();
            this.cbBurnBootloader = new System.Windows.Forms.CheckBox();
            this.cbCleanFirst = new System.Windows.Forms.CheckBox();
            this.cbGenerateSerial = new System.Windows.Forms.CheckBox();
            this.gbProgrammer = new System.Windows.Forms.GroupBox();
            this.lblISPSerialPort = new System.Windows.Forms.Label();
            this.cboxISPSerialPort = new System.Windows.Forms.ComboBox();
            this.gbCapabilities = new System.Windows.Forms.GroupBox();
            this.cbTemperatureCompensation = new System.Windows.Forms.CheckBox();
            this.cbAbsolutePositioning = new System.Windows.Forms.CheckBox();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tpProgram = new System.Windows.Forms.TabPage();
            this.tpTesting = new System.Windows.Forms.TabPage();
            this.gbTemperatureTest = new System.Windows.Forms.GroupBox();
            this.btnTemperatureTestStop = new System.Windows.Forms.Button();
            this.btnTemperatureTestStart = new System.Windows.Forms.Button();
            this.lbTemperatureLog = new System.Windows.Forms.ListBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.tpEEPROM = new System.Windows.Forms.TabPage();
            this.btnEEPROMGenerate = new System.Windows.Forms.Button();
            this.gbEEPROMOutput = new System.Windows.Forms.GroupBox();
            this.tbEEPROMOutput = new System.Windows.Forms.RichTextBox();
            this.gbEEPROMSerialNumber = new System.Windows.Forms.GroupBox();
            this.tbEEPROMSerialNumber = new System.Windows.Forms.TextBox();
            this.gbEEPROMFlags = new System.Windows.Forms.GroupBox();
            this.cbEEPROMStayInBootloader = new System.Windows.Forms.CheckBox();
            this.gbBaseDirectory.SuspendLayout();
            this.gbOptions.SuspendLayout();
            this.gbProgrammer.SuspendLayout();
            this.gbCapabilities.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tpProgram.SuspendLayout();
            this.tpTesting.SuspendLayout();
            this.gbTemperatureTest.SuspendLayout();
            this.tpEEPROM.SuspendLayout();
            this.gbEEPROMOutput.SuspendLayout();
            this.gbEEPROMSerialNumber.SuspendLayout();
            this.gbEEPROMFlags.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbLog
            // 
            this.lbLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lbLog.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbLog.FormattingEnabled = true;
            this.lbLog.ItemHeight = 15;
            this.lbLog.Location = new System.Drawing.Point(6, 159);
            this.lbLog.Name = "lbLog";
            this.lbLog.Size = new System.Drawing.Size(755, 244);
            this.lbLog.TabIndex = 0;
            // 
            // btnBuild
            // 
            this.btnBuild.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBuild.Location = new System.Drawing.Point(658, 130);
            this.btnBuild.Name = "btnBuild";
            this.btnBuild.Size = new System.Drawing.Size(103, 23);
            this.btnBuild.TabIndex = 1;
            this.btnBuild.Text = "Build and Burn";
            this.btnBuild.UseVisualStyleBackColor = true;
            this.btnBuild.Click += new System.EventHandler(this.btnBuild_Click);
            // 
            // tbBaseDirectory
            // 
            this.tbBaseDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbBaseDirectory.Location = new System.Drawing.Point(6, 19);
            this.tbBaseDirectory.Name = "tbBaseDirectory";
            this.tbBaseDirectory.Size = new System.Drawing.Size(640, 20);
            this.tbBaseDirectory.TabIndex = 2;
            // 
            // gbBaseDirectory
            // 
            this.gbBaseDirectory.Controls.Add(this.btnBaseDirectorySelect);
            this.gbBaseDirectory.Controls.Add(this.tbBaseDirectory);
            this.gbBaseDirectory.Location = new System.Drawing.Point(6, 6);
            this.gbBaseDirectory.Name = "gbBaseDirectory";
            this.gbBaseDirectory.Size = new System.Drawing.Size(755, 46);
            this.gbBaseDirectory.TabIndex = 3;
            this.gbBaseDirectory.TabStop = false;
            this.gbBaseDirectory.Text = "Base Directory";
            // 
            // btnBaseDirectorySelect
            // 
            this.btnBaseDirectorySelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBaseDirectorySelect.Location = new System.Drawing.Point(652, 17);
            this.btnBaseDirectorySelect.Name = "btnBaseDirectorySelect";
            this.btnBaseDirectorySelect.Size = new System.Drawing.Size(95, 23);
            this.btnBaseDirectorySelect.TabIndex = 3;
            this.btnBaseDirectorySelect.Text = "Select...";
            this.btnBaseDirectorySelect.UseVisualStyleBackColor = true;
            this.btnBaseDirectorySelect.Click += new System.EventHandler(this.btnBaseDirectorySelect_Click);
            // 
            // gbOptions
            // 
            this.gbOptions.Controls.Add(this.cbEEPROM);
            this.gbOptions.Controls.Add(this.cbBurnBootloader);
            this.gbOptions.Controls.Add(this.cbCleanFirst);
            this.gbOptions.Controls.Add(this.cbGenerateSerial);
            this.gbOptions.Location = new System.Drawing.Point(6, 58);
            this.gbOptions.Name = "gbOptions";
            this.gbOptions.Size = new System.Drawing.Size(272, 66);
            this.gbOptions.TabIndex = 4;
            this.gbOptions.TabStop = false;
            this.gbOptions.Text = "Options";
            // 
            // cbEEPROM
            // 
            this.cbEEPROM.AutoSize = true;
            this.cbEEPROM.Checked = true;
            this.cbEEPROM.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbEEPROM.Location = new System.Drawing.Point(163, 42);
            this.cbEEPROM.Name = "cbEEPROM";
            this.cbEEPROM.Size = new System.Drawing.Size(100, 17);
            this.cbEEPROM.TabIndex = 3;
            this.cbEEPROM.Text = "Write EEPROM";
            this.cbEEPROM.UseVisualStyleBackColor = true;
            // 
            // cbBurnBootloader
            // 
            this.cbBurnBootloader.AutoSize = true;
            this.cbBurnBootloader.Checked = true;
            this.cbBurnBootloader.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbBurnBootloader.Location = new System.Drawing.Point(163, 19);
            this.cbBurnBootloader.Name = "cbBurnBootloader";
            this.cbBurnBootloader.Size = new System.Drawing.Size(101, 17);
            this.cbBurnBootloader.TabIndex = 2;
            this.cbBurnBootloader.Text = "Burn bootloader";
            this.cbBurnBootloader.UseVisualStyleBackColor = true;
            // 
            // cbCleanFirst
            // 
            this.cbCleanFirst.AutoSize = true;
            this.cbCleanFirst.Checked = true;
            this.cbCleanFirst.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbCleanFirst.Location = new System.Drawing.Point(6, 42);
            this.cbCleanFirst.Name = "cbCleanFirst";
            this.cbCleanFirst.Size = new System.Drawing.Size(104, 17);
            this.cbCleanFirst.TabIndex = 1;
            this.cbCleanFirst.Text = "Run make clean";
            this.cbCleanFirst.UseVisualStyleBackColor = true;
            // 
            // cbGenerateSerial
            // 
            this.cbGenerateSerial.AutoSize = true;
            this.cbGenerateSerial.Checked = true;
            this.cbGenerateSerial.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbGenerateSerial.Location = new System.Drawing.Point(6, 19);
            this.cbGenerateSerial.Name = "cbGenerateSerial";
            this.cbGenerateSerial.Size = new System.Drawing.Size(115, 17);
            this.cbGenerateSerial.TabIndex = 0;
            this.cbGenerateSerial.Text = "Generate serial no.";
            this.cbGenerateSerial.UseVisualStyleBackColor = true;
            // 
            // gbProgrammer
            // 
            this.gbProgrammer.Controls.Add(this.lblISPSerialPort);
            this.gbProgrammer.Controls.Add(this.cboxISPSerialPort);
            this.gbProgrammer.Location = new System.Drawing.Point(284, 58);
            this.gbProgrammer.Name = "gbProgrammer";
            this.gbProgrammer.Size = new System.Drawing.Size(220, 66);
            this.gbProgrammer.TabIndex = 5;
            this.gbProgrammer.TabStop = false;
            this.gbProgrammer.Text = "ISP Programmer Options";
            // 
            // lblISPSerialPort
            // 
            this.lblISPSerialPort.AutoSize = true;
            this.lblISPSerialPort.Location = new System.Drawing.Point(22, 23);
            this.lblISPSerialPort.Name = "lblISPSerialPort";
            this.lblISPSerialPort.Size = new System.Drawing.Size(55, 13);
            this.lblISPSerialPort.TabIndex = 1;
            this.lblISPSerialPort.Text = "Serial Port";
            // 
            // cboxISPSerialPort
            // 
            this.cboxISPSerialPort.FormattingEnabled = true;
            this.cboxISPSerialPort.Location = new System.Drawing.Point(83, 19);
            this.cboxISPSerialPort.Name = "cboxISPSerialPort";
            this.cboxISPSerialPort.Size = new System.Drawing.Size(121, 21);
            this.cboxISPSerialPort.TabIndex = 0;
            // 
            // gbCapabilities
            // 
            this.gbCapabilities.Controls.Add(this.cbTemperatureCompensation);
            this.gbCapabilities.Controls.Add(this.cbAbsolutePositioning);
            this.gbCapabilities.Location = new System.Drawing.Point(510, 58);
            this.gbCapabilities.Name = "gbCapabilities";
            this.gbCapabilities.Size = new System.Drawing.Size(251, 66);
            this.gbCapabilities.TabIndex = 6;
            this.gbCapabilities.TabStop = false;
            this.gbCapabilities.Text = "Capabilities";
            // 
            // cbTemperatureCompensation
            // 
            this.cbTemperatureCompensation.AutoSize = true;
            this.cbTemperatureCompensation.Checked = true;
            this.cbTemperatureCompensation.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbTemperatureCompensation.Location = new System.Drawing.Point(6, 42);
            this.cbTemperatureCompensation.Name = "cbTemperatureCompensation";
            this.cbTemperatureCompensation.Size = new System.Drawing.Size(156, 17);
            this.cbTemperatureCompensation.TabIndex = 1;
            this.cbTemperatureCompensation.Text = "Temperature Compensation";
            this.cbTemperatureCompensation.UseVisualStyleBackColor = true;
            // 
            // cbAbsolutePositioning
            // 
            this.cbAbsolutePositioning.AutoSize = true;
            this.cbAbsolutePositioning.Checked = true;
            this.cbAbsolutePositioning.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbAbsolutePositioning.Location = new System.Drawing.Point(6, 19);
            this.cbAbsolutePositioning.Name = "cbAbsolutePositioning";
            this.cbAbsolutePositioning.Size = new System.Drawing.Size(121, 17);
            this.cbAbsolutePositioning.TabIndex = 0;
            this.cbAbsolutePositioning.Text = "Absolute Positioning";
            this.cbAbsolutePositioning.UseVisualStyleBackColor = true;
            // 
            // tabControl
            // 
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.Controls.Add(this.tpProgram);
            this.tabControl.Controls.Add(this.tpTesting);
            this.tabControl.Controls.Add(this.tpEEPROM);
            this.tabControl.Location = new System.Drawing.Point(12, 12);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(775, 443);
            this.tabControl.TabIndex = 7;
            // 
            // tpProgram
            // 
            this.tpProgram.Controls.Add(this.gbBaseDirectory);
            this.tpProgram.Controls.Add(this.gbCapabilities);
            this.tpProgram.Controls.Add(this.lbLog);
            this.tpProgram.Controls.Add(this.gbProgrammer);
            this.tpProgram.Controls.Add(this.btnBuild);
            this.tpProgram.Controls.Add(this.gbOptions);
            this.tpProgram.Location = new System.Drawing.Point(4, 22);
            this.tpProgram.Name = "tpProgram";
            this.tpProgram.Padding = new System.Windows.Forms.Padding(3);
            this.tpProgram.Size = new System.Drawing.Size(767, 417);
            this.tpProgram.TabIndex = 0;
            this.tpProgram.Text = "Program";
            this.tpProgram.UseVisualStyleBackColor = true;
            // 
            // tpTesting
            // 
            this.tpTesting.Controls.Add(this.gbTemperatureTest);
            this.tpTesting.Controls.Add(this.btnConnect);
            this.tpTesting.Location = new System.Drawing.Point(4, 22);
            this.tpTesting.Name = "tpTesting";
            this.tpTesting.Padding = new System.Windows.Forms.Padding(3);
            this.tpTesting.Size = new System.Drawing.Size(767, 417);
            this.tpTesting.TabIndex = 1;
            this.tpTesting.Text = "Testing";
            this.tpTesting.UseVisualStyleBackColor = true;
            // 
            // gbTemperatureTest
            // 
            this.gbTemperatureTest.Controls.Add(this.btnTemperatureTestStop);
            this.gbTemperatureTest.Controls.Add(this.btnTemperatureTestStart);
            this.gbTemperatureTest.Controls.Add(this.lbTemperatureLog);
            this.gbTemperatureTest.Location = new System.Drawing.Point(6, 35);
            this.gbTemperatureTest.Name = "gbTemperatureTest";
            this.gbTemperatureTest.Size = new System.Drawing.Size(251, 376);
            this.gbTemperatureTest.TabIndex = 1;
            this.gbTemperatureTest.TabStop = false;
            this.gbTemperatureTest.Text = "Temperature Test";
            // 
            // btnTemperatureTestStop
            // 
            this.btnTemperatureTestStop.Enabled = false;
            this.btnTemperatureTestStop.Location = new System.Drawing.Point(170, 347);
            this.btnTemperatureTestStop.Name = "btnTemperatureTestStop";
            this.btnTemperatureTestStop.Size = new System.Drawing.Size(75, 23);
            this.btnTemperatureTestStop.TabIndex = 2;
            this.btnTemperatureTestStop.Text = "Stop";
            this.btnTemperatureTestStop.UseVisualStyleBackColor = true;
            this.btnTemperatureTestStop.Click += new System.EventHandler(this.btnTemperatureTestStop_Click);
            // 
            // btnTemperatureTestStart
            // 
            this.btnTemperatureTestStart.Enabled = false;
            this.btnTemperatureTestStart.Location = new System.Drawing.Point(89, 347);
            this.btnTemperatureTestStart.Name = "btnTemperatureTestStart";
            this.btnTemperatureTestStart.Size = new System.Drawing.Size(75, 23);
            this.btnTemperatureTestStart.TabIndex = 2;
            this.btnTemperatureTestStart.Text = "Start";
            this.btnTemperatureTestStart.UseVisualStyleBackColor = true;
            this.btnTemperatureTestStart.Click += new System.EventHandler(this.btnTemperatureTestStart_Click);
            // 
            // lbTemperatureLog
            // 
            this.lbTemperatureLog.FormattingEnabled = true;
            this.lbTemperatureLog.Location = new System.Drawing.Point(6, 19);
            this.lbTemperatureLog.Name = "lbTemperatureLog";
            this.lbTemperatureLog.Size = new System.Drawing.Size(239, 316);
            this.lbTemperatureLog.TabIndex = 2;
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(6, 6);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(755, 23);
            this.btnConnect.TabIndex = 0;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // tpEEPROM
            // 
            this.tpEEPROM.Controls.Add(this.btnEEPROMGenerate);
            this.tpEEPROM.Controls.Add(this.gbEEPROMOutput);
            this.tpEEPROM.Controls.Add(this.gbEEPROMSerialNumber);
            this.tpEEPROM.Controls.Add(this.gbEEPROMFlags);
            this.tpEEPROM.Location = new System.Drawing.Point(4, 22);
            this.tpEEPROM.Name = "tpEEPROM";
            this.tpEEPROM.Size = new System.Drawing.Size(767, 417);
            this.tpEEPROM.TabIndex = 2;
            this.tpEEPROM.Text = "EEPROM";
            this.tpEEPROM.UseVisualStyleBackColor = true;
            // 
            // btnEEPROMGenerate
            // 
            this.btnEEPROMGenerate.Location = new System.Drawing.Point(329, 391);
            this.btnEEPROMGenerate.Name = "btnEEPROMGenerate";
            this.btnEEPROMGenerate.Size = new System.Drawing.Size(75, 23);
            this.btnEEPROMGenerate.TabIndex = 5;
            this.btnEEPROMGenerate.Text = "Generate";
            this.btnEEPROMGenerate.UseVisualStyleBackColor = true;
            this.btnEEPROMGenerate.Click += new System.EventHandler(this.btnEEPROMGenerate_Click);
            // 
            // gbEEPROMOutput
            // 
            this.gbEEPROMOutput.Controls.Add(this.tbEEPROMOutput);
            this.gbEEPROMOutput.Location = new System.Drawing.Point(3, 109);
            this.gbEEPROMOutput.Name = "gbEEPROMOutput";
            this.gbEEPROMOutput.Size = new System.Drawing.Size(401, 276);
            this.gbEEPROMOutput.TabIndex = 4;
            this.gbEEPROMOutput.TabStop = false;
            this.gbEEPROMOutput.Text = "EEPROM";
            // 
            // tbEEPROMOutput
            // 
            this.tbEEPROMOutput.Location = new System.Drawing.Point(6, 19);
            this.tbEEPROMOutput.Name = "tbEEPROMOutput";
            this.tbEEPROMOutput.Size = new System.Drawing.Size(389, 251);
            this.tbEEPROMOutput.TabIndex = 0;
            this.tbEEPROMOutput.Text = "";
            // 
            // gbEEPROMSerialNumber
            // 
            this.gbEEPROMSerialNumber.Controls.Add(this.tbEEPROMSerialNumber);
            this.gbEEPROMSerialNumber.Location = new System.Drawing.Point(3, 54);
            this.gbEEPROMSerialNumber.Name = "gbEEPROMSerialNumber";
            this.gbEEPROMSerialNumber.Size = new System.Drawing.Size(401, 49);
            this.gbEEPROMSerialNumber.TabIndex = 3;
            this.gbEEPROMSerialNumber.TabStop = false;
            this.gbEEPROMSerialNumber.Text = "SerialNumber";
            // 
            // tbEEPROMSerialNumber
            // 
            this.tbEEPROMSerialNumber.Location = new System.Drawing.Point(6, 19);
            this.tbEEPROMSerialNumber.Name = "tbEEPROMSerialNumber";
            this.tbEEPROMSerialNumber.Size = new System.Drawing.Size(389, 20);
            this.tbEEPROMSerialNumber.TabIndex = 2;
            // 
            // gbEEPROMFlags
            // 
            this.gbEEPROMFlags.Controls.Add(this.cbEEPROMStayInBootloader);
            this.gbEEPROMFlags.Location = new System.Drawing.Point(3, 3);
            this.gbEEPROMFlags.Name = "gbEEPROMFlags";
            this.gbEEPROMFlags.Size = new System.Drawing.Size(401, 45);
            this.gbEEPROMFlags.TabIndex = 0;
            this.gbEEPROMFlags.TabStop = false;
            this.gbEEPROMFlags.Text = "Flags";
            // 
            // cbEEPROMStayInBootloader
            // 
            this.cbEEPROMStayInBootloader.AutoSize = true;
            this.cbEEPROMStayInBootloader.Location = new System.Drawing.Point(11, 19);
            this.cbEEPROMStayInBootloader.Name = "cbEEPROMStayInBootloader";
            this.cbEEPROMStayInBootloader.Size = new System.Drawing.Size(113, 17);
            this.cbEEPROMStayInBootloader.TabIndex = 0;
            this.cbEEPROMStayInBootloader.Text = "Stay In Bootloader";
            this.cbEEPROMStayInBootloader.UseVisualStyleBackColor = true;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(799, 467);
            this.Controls.Add(this.tabControl);
            this.MinimumSize = new System.Drawing.Size(763, 479);
            this.Name = "MainWindow";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Builder";
            this.gbBaseDirectory.ResumeLayout(false);
            this.gbBaseDirectory.PerformLayout();
            this.gbOptions.ResumeLayout(false);
            this.gbOptions.PerformLayout();
            this.gbProgrammer.ResumeLayout(false);
            this.gbProgrammer.PerformLayout();
            this.gbCapabilities.ResumeLayout(false);
            this.gbCapabilities.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.tpProgram.ResumeLayout(false);
            this.tpTesting.ResumeLayout(false);
            this.gbTemperatureTest.ResumeLayout(false);
            this.tpEEPROM.ResumeLayout(false);
            this.gbEEPROMOutput.ResumeLayout(false);
            this.gbEEPROMSerialNumber.ResumeLayout(false);
            this.gbEEPROMSerialNumber.PerformLayout();
            this.gbEEPROMFlags.ResumeLayout(false);
            this.gbEEPROMFlags.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox lbLog;
        private System.Windows.Forms.Button btnBuild;
        private System.Windows.Forms.TextBox tbBaseDirectory;
        private System.Windows.Forms.GroupBox gbBaseDirectory;
        private System.Windows.Forms.Button btnBaseDirectorySelect;
        private System.Windows.Forms.GroupBox gbOptions;
        private System.Windows.Forms.CheckBox cbGenerateSerial;
        private System.Windows.Forms.CheckBox cbCleanFirst;
        private System.Windows.Forms.CheckBox cbBurnBootloader;
        private System.Windows.Forms.GroupBox gbProgrammer;
        private System.Windows.Forms.ComboBox cboxISPSerialPort;
        private System.Windows.Forms.Label lblISPSerialPort;
        private System.Windows.Forms.GroupBox gbCapabilities;
        private System.Windows.Forms.CheckBox cbTemperatureCompensation;
        private System.Windows.Forms.CheckBox cbAbsolutePositioning;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tpProgram;
        private System.Windows.Forms.TabPage tpTesting;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.GroupBox gbTemperatureTest;
        private System.Windows.Forms.Button btnTemperatureTestStop;
        private System.Windows.Forms.Button btnTemperatureTestStart;
        private System.Windows.Forms.ListBox lbTemperatureLog;
        private System.Windows.Forms.CheckBox cbEEPROM;
        private System.Windows.Forms.TabPage tpEEPROM;
        private System.Windows.Forms.GroupBox gbEEPROMSerialNumber;
        private System.Windows.Forms.TextBox tbEEPROMSerialNumber;
        private System.Windows.Forms.GroupBox gbEEPROMFlags;
        private System.Windows.Forms.CheckBox cbEEPROMStayInBootloader;
        private System.Windows.Forms.Button btnEEPROMGenerate;
        private System.Windows.Forms.GroupBox gbEEPROMOutput;
        private System.Windows.Forms.RichTextBox tbEEPROMOutput;
    }
}

