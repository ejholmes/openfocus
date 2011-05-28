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
            this.cbBurnBootloader = new System.Windows.Forms.CheckBox();
            this.cbCleanFirst = new System.Windows.Forms.CheckBox();
            this.cbGenerateSerial = new System.Windows.Forms.CheckBox();
            this.gbProgrammer = new System.Windows.Forms.GroupBox();
            this.lblISPSerialPort = new System.Windows.Forms.Label();
            this.cboxISPSerialPort = new System.Windows.Forms.ComboBox();
            this.gbCapabilities = new System.Windows.Forms.GroupBox();
            this.cbAbsolutePositioning = new System.Windows.Forms.CheckBox();
            this.cbTemperatureCompensation = new System.Windows.Forms.CheckBox();
            this.gbBaseDirectory.SuspendLayout();
            this.gbOptions.SuspendLayout();
            this.gbProgrammer.SuspendLayout();
            this.gbCapabilities.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbLog
            // 
            this.lbLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lbLog.FormattingEnabled = true;
            this.lbLog.Location = new System.Drawing.Point(12, 165);
            this.lbLog.Name = "lbLog";
            this.lbLog.Size = new System.Drawing.Size(723, 264);
            this.lbLog.TabIndex = 0;
            // 
            // btnBuild
            // 
            this.btnBuild.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBuild.Location = new System.Drawing.Point(632, 136);
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
            this.tbBaseDirectory.Size = new System.Drawing.Size(608, 20);
            this.tbBaseDirectory.TabIndex = 2;
            // 
            // gbBaseDirectory
            // 
            this.gbBaseDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbBaseDirectory.Controls.Add(this.btnBaseDirectorySelect);
            this.gbBaseDirectory.Controls.Add(this.tbBaseDirectory);
            this.gbBaseDirectory.Location = new System.Drawing.Point(12, 12);
            this.gbBaseDirectory.Name = "gbBaseDirectory";
            this.gbBaseDirectory.Size = new System.Drawing.Size(723, 46);
            this.gbBaseDirectory.TabIndex = 3;
            this.gbBaseDirectory.TabStop = false;
            this.gbBaseDirectory.Text = "Base Directory";
            // 
            // btnBaseDirectorySelect
            // 
            this.btnBaseDirectorySelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBaseDirectorySelect.Location = new System.Drawing.Point(620, 17);
            this.btnBaseDirectorySelect.Name = "btnBaseDirectorySelect";
            this.btnBaseDirectorySelect.Size = new System.Drawing.Size(95, 23);
            this.btnBaseDirectorySelect.TabIndex = 3;
            this.btnBaseDirectorySelect.Text = "Select...";
            this.btnBaseDirectorySelect.UseVisualStyleBackColor = true;
            this.btnBaseDirectorySelect.Click += new System.EventHandler(this.btnBaseDirectorySelect_Click);
            // 
            // gbOptions
            // 
            this.gbOptions.Controls.Add(this.cbBurnBootloader);
            this.gbOptions.Controls.Add(this.cbCleanFirst);
            this.gbOptions.Controls.Add(this.cbGenerateSerial);
            this.gbOptions.Location = new System.Drawing.Point(12, 64);
            this.gbOptions.Name = "gbOptions";
            this.gbOptions.Size = new System.Drawing.Size(272, 66);
            this.gbOptions.TabIndex = 4;
            this.gbOptions.TabStop = false;
            this.gbOptions.Text = "Options";
            // 
            // cbBurnBootloader
            // 
            this.cbBurnBootloader.AutoSize = true;
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
            this.gbProgrammer.Location = new System.Drawing.Point(290, 64);
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
            this.gbCapabilities.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.gbCapabilities.Controls.Add(this.cbTemperatureCompensation);
            this.gbCapabilities.Controls.Add(this.cbAbsolutePositioning);
            this.gbCapabilities.Location = new System.Drawing.Point(516, 64);
            this.gbCapabilities.Name = "gbCapabilities";
            this.gbCapabilities.Size = new System.Drawing.Size(219, 66);
            this.gbCapabilities.TabIndex = 6;
            this.gbCapabilities.TabStop = false;
            this.gbCapabilities.Text = "Capabilities";
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
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(747, 441);
            this.Controls.Add(this.gbCapabilities);
            this.Controls.Add(this.gbProgrammer);
            this.Controls.Add(this.gbOptions);
            this.Controls.Add(this.gbBaseDirectory);
            this.Controls.Add(this.btnBuild);
            this.Controls.Add(this.lbLog);
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
    }
}

