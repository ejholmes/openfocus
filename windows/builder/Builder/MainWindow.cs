﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;

using Cortex;
using Cortex.OpenFocus;

namespace Builder
{
    public partial class MainWindow : Form
    {
        Timer tempTimer = new Timer();

        String CurrentDirectory = String.Empty;
        String BaseDirectory = String.Empty;

        public MainWindow()
        {
            InitializeComponent();

            BaseDirectory = Properties.Settings.Default.Path;
            this.tbBaseDirectory.Text = BaseDirectory;

            this.Text = this.Text + " - v" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

            Logger.LoggerWrite += new Logger.LoggerWriteEventHandler(Logger_LoggerWrite);
        }

        void Logger_LoggerWrite(object sender, LoggerEventArgs e)
        {
            if (e.Type == Logger.LogType.Error)
            {
                this.lbLog.Font = new Font(this.lbLog.Font, this.lbLog.Font.Style | FontStyle.Bold);
            }

            this.lbLog.Items.Add(e.Text);

            if (e.Text == String.Empty)
            {
                int current = this.lbLog.SelectedIndex;
                this.lbLog.SelectedIndex = this.lbLog.Items.Count - 1;
                this.lbLog.SelectedIndex = current;
            }
            else
            {
                this.lbLog.SelectedIndex = this.lbLog.Items.Count - 1;
            }
        }

        private void btnBaseDirectorySelect_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderBrowser = new FolderBrowserDialog())
            {
                folderBrowser.SelectedPath = Properties.Settings.Default.Path;
                if (folderBrowser.ShowDialog(this) == DialogResult.OK)
                {
                    BaseDirectory = folderBrowser.SelectedPath;
                    this.tbBaseDirectory.Text = BaseDirectory;
                    Properties.Settings.Default.Path = BaseDirectory;
                    Properties.Settings.Default.Save();
                }
            }
        }

        private void btnBuild_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(BaseDirectory))
            {
                MessageBox.Show("Please select the directory for the project");
                btnBaseDirectorySelect_Click(null, null);
                return;
            }
            this.btnConnect.Enabled = false;
            this.btnBuild.Enabled = false;
            BuildBurnBootloader();
            BuildBurnFirmware();

            Logger.Write();
            this.btnBuild.Enabled = true;
            this.btnConnect.Enabled = true;
        }

        private Guid GenerateGUID()
        {
            Logger.Write("Generating new GUID for seral number...");
            Guid guid = Guid.NewGuid();
            Logger.Write("GUID: " + guid.ToString());
            Logger.Write("");

            return guid;
        }

        private void BuildBurnBootloader()
        {
            CurrentDirectory = @"\firmware\bootloader";
            if (this.cbCleanFirst.Checked)
            {
                Logger.Write("Cleaning bootloader directory...");
                Make("clean");
            }
            Logger.Write("Building bootloader...");
            Make();
            
            if (this.cbBurnBootloader.Checked)
            {
                Logger.Write("Flashing to device...");
                Make("install", true);
                Logger.Write("Bootloader flashed to device");
            }

            Logger.Write("");
        }

        private void BuildBurnFirmware()
        {
            CurrentDirectory = @"\firmware";

            Logger.Write("Cleaning firmware directory...");
            Make("clean");
            Logger.Write("Building firmware...");

            List<string> Defines = new List<string>();

            if (this.cbGenerateSerial.Checked)
            {
                Guid guid = GenerateGUID();

                /* Generate a GUID based serial, split to individual characters and join with commas */
                string tokenized = String.Join(",", guid.ToString().ToCharArray().Select(x => "'" + x.ToString() + "'").ToArray());
                Defines.Add("USB_CFG_SERIAL_NUMBER=" + tokenized);
                Defines.Add("USB_CFG_SERIAL_NUMBER_LEN=" + guid.ToString().ToCharArray().Length);
            }

            if (this.cbAbsolutePositioning.Checked)
                Defines.Add("ABSOLUTE_POSITIONING_ENABLED=1");
            else
                Defines.Add("ABSOLUTE_POSITIONING_ENABLED=0");

            if (this.cbTemperatureCompensation.Checked)
                Defines.Add("TEMPERATURE_COMPENSATION_ENABLED=1");
            else
                Defines.Add("TEMPERATURE_COMPENSATION_ENABLED=0");

            StringBuilder CFLAGS = new StringBuilder();

            if (Defines.Count > 0)
            {
                CFLAGS.Append("\"CFLAGS=");

                foreach (string define in Defines)
                {
                    CFLAGS.Append("-D\\\"" + define + "\\\" ");
                }

                CFLAGS.Append("\"");
            }

            Make(CFLAGS.ToString());

            if (!this.cbBurnBootloader.Checked || (this.cbBurnBootloader.Checked && MessageBox.Show("Push the firmware button on the device", 
                "Device reboot required", 
                MessageBoxButtons.OKCancel, 
                MessageBoxIcon.Information, 
                MessageBoxDefaultButton.Button1) == DialogResult.OK))
            {
                UploadFirmware();
            }

            
            
            Logger.Write("");
        }

        private void UploadFirmware()
        {
            Bootloader.UploadFile(BaseDirectory + CurrentDirectory + @"\main.hex");
        }

        private void Make()
        {
            Make("");
        }

        private void Make(string Args)
        {
            Make(Args, true);
        }

        private void Make(string Args, bool redirect)
        {
            String command = "make.exe";

            Process make = new Process();

            make.StartInfo.FileName = command;
            make.StartInfo.CreateNoWindow = redirect;
            make.StartInfo.WorkingDirectory = BaseDirectory + CurrentDirectory;
            make.StartInfo.Arguments = " " + Args;
            make.StartInfo.ErrorDialog = false;
            make.StartInfo.UseShellExecute = false;
            make.StartInfo.RedirectStandardOutput = true;
            make.Start();

            string line = String.Empty;

            Logger.Write("");

            while (!String.IsNullOrEmpty((line = make.StandardOutput.ReadLine())))
                Logger.Write(line);

            Logger.Write("");

            make.WaitForExit();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (this.btnConnect.Text == "Connect" && Device.Connect())
            {
                this.btnTemperatureTestStart.Enabled = true;

                this.btnConnect.Text = "Disconnect";
            }
            else if (this.btnConnect.Text == "Disconnect")
            {
                Device.Disconnect();
                this.btnConnect.Text = "Connect";
            }
        }

        private void btnTemperatureTestStart_Click(object sender, EventArgs e)
        {
            this.btnTemperatureTestStart.Enabled = false;
            this.btnTemperatureTestStop.Enabled = true;

            tempTimer.Interval = 200;
            tempTimer.Tick += new EventHandler(tempTimer_Tick);

            tempTimer.Start();
        }

        void tempTimer_Tick(object sender, EventArgs e)
        {
            double temperature = Device.Temperature;

            this.lbTemperatureLog.Items.Add("Temp: " + temperature.ToString());

            this.lbTemperatureLog.SelectedIndex = this.lbTemperatureLog.Items.Count - 1;
        }

        private void btnTemperatureTestStop_Click(object sender, EventArgs e)
        {
            this.btnTemperatureTestStop.Enabled = false;
            this.btnTemperatureTestStart.Enabled = true;

            tempTimer.Stop();
        }
    }
}
