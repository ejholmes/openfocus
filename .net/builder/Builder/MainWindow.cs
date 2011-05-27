using System;
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
        String CurrentDirectory = String.Empty;
        String BaseDirectory = String.Empty;

        public MainWindow()
        {
            InitializeComponent();

            BaseDirectory = Properties.Settings.Default.Path;
            this.tbBaseDirectory.Text = BaseDirectory;

            this.Text = this.Text + " - v" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        public void Log(string text)
        {
            this.lbLog.Items.Add(text);
            if (text == String.Empty)
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
            this.btnBuild.Enabled = false;
            BuildBurnBootloader();
            BuildBurnFirmware();

            Log("");
            this.btnBuild.Enabled = true;
        }

        private Guid GenerateGUID()
        {
            Log("Generating new GUID for seral number...");
            Guid guid = Guid.NewGuid();
            Log("GUID: " + guid.ToString());
            Log("");

            return guid;
        }

        private void BuildBurnBootloader()
        {
            CurrentDirectory = @"\bootloader\firmware";
            if (this.cbCleanFirst.Checked)
            {
                Log("Cleaning bootloader directory...");
                Make("clean");
            }
            Log("Building bootloader...");
            Make();
            
            if (this.cbBurnBootloader.Checked)
            {
                Log("Flashing to device...");
                Make("install");
                Log("Bootloader flashed to device");
            }

            Log("");
        }

        private void BuildBurnFirmware()
        {
            CurrentDirectory = @"\firmware";
            Guid guid = GenerateGUID();

            /* Generate a GUID based serial, split to individual characters and join with commas */
            string tokenized = String.Join(",", guid.ToString().ToCharArray().Select(x => "'" + x.ToString() + "'").ToArray());

            Log("Cleaning firmware directory...");
            Make("clean");
            Log("Building firmware...");

            if (this.cbGenerateSerial.Checked)
                Make("\"CFLAGS=-D\\\"USB_CFG_SERIAL_NUMBER=" + tokenized + "\\\" -DUSB_CFG_SERIAL_NUMBER_LEN=" + guid.ToString().ToCharArray().Length + "\"");
            else
                Make();

            UploadFirmware();
            
            Log("");
        }

        private void UploadFirmware()
        {
            Byte[] dataBuffer;
            uint PageSize = 0, FlashSize = 0;

            Log("Attempting to connect to bootloader");

            try
            {
                Bootloader.Connect();
                PageSize = Bootloader.PageSize;
                FlashSize = Bootloader.FlashSize;

                Log("Device Found!");
                Log("Page Size: " + PageSize.ToString() + " bytes");
                Log("Flash Size: " + FlashSize.ToString() + " bytes");
            }
            catch (DeviceNotFoundException)
            {
                try
                {
                    Device.Connect();
                    Log("Rebooting device into firmware update mode...");
                    Device.RebootToBootloader();
                    Device.Disconnect();
                    System.Threading.Thread.Sleep(2000);
                    Application.DoEvents();
                    UploadFirmware();
                    return;
                }
                catch (DeviceNotFoundException)
                {
                    Log("Device not found!");
                    return;
                }
            }

            try
            {
                string FileName = BaseDirectory + CurrentDirectory + @"\main.hex";
                dataBuffer = IntelHexParser.ParseFile(FileName, PageSize);

                if (dataBuffer.Length > (FlashSize - 2048))
                {
                    Log("File is too large!");
                    return;
                }

                FileInfo file = new FileInfo(FileName);
                Log("Opened file " + file.Name);
                Log("Ready to upload " + dataBuffer.Length.ToString() + " bytes of data");
            }
            catch (ChecksumMismatchException)
            {
                Log("Checksum mismatch! File is not valid.");
                return;
            }

            for (uint address = 0; address < dataBuffer.Length; address += PageSize)
            {
                Byte[] data = new Byte[PageSize];
                Buffer.BlockCopy(dataBuffer, (int)address, data, 0, (int)PageSize);

                Log("Writing block 0x" + String.Format("{0:x3}", address) + " ... 0x" + String.Format("{0:x3}", (address + PageSize)));

                Bootloader.WriteBlock(address, data);
            }

            Log("Firmware update complete!");
            Log("Device is rebooting");
            Bootloader.Reboot();
        }

        private void Make()
        {
            Make("");
        }

        private void Make(string Args)
        {
            String command = "make.exe";

            Process make = new Process();

            make.StartInfo.FileName = command;
            make.StartInfo.CreateNoWindow = true;
            make.StartInfo.WorkingDirectory = BaseDirectory + CurrentDirectory;
            make.StartInfo.Arguments = " " + Args;
            make.StartInfo.UseShellExecute = false;
            make.StartInfo.ErrorDialog = false;
            make.StartInfo.RedirectStandardOutput = true;
            make.Start();

            string line = String.Empty;

            Log("");

            while (!String.IsNullOrEmpty((line = make.StandardOutput.ReadLine())))
                Log(line);

            Log("");

            make.WaitForExit();
        }
    }
}
