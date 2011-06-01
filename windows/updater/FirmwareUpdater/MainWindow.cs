using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Runtime.InteropServices;

using Cortex;
using Cortex.OpenFocus;

namespace FirmwareUpdater
{
    public partial class MainWindow : Form
    {
        uint PageSize = 0;
        uint FlashSize = 0;

        Byte[] dataBuffer;

        bool done = true;

        public MainWindow()
        {
            InitializeComponent();
        }

        public void Log(string text)
        {
            lbLog.Items.Add(text);
            if (text != String.Empty)
                lbLog.SelectedIndex = lbLog.Items.Count - 1;
        }

        private void MainWindow_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            if (!done)
            {
                Log("No data uploaded...rebooting");
                Bootloader.Reboot();
            }
        }

        private void btnFindDevice_Click(object sender, EventArgs e)
        {
            try
            {
                done = false;
                Bootloader.Connect();
                PageSize = Bootloader.PageSize;
                FlashSize = Bootloader.FlashSize;

                Log("Device Found!");
                Log("Page Size: " + PageSize.ToString() + " bytes");
                Log("Flash Size: " + FlashSize.ToString() + " bytes");

                this.btnLocateFirmware.Enabled = true;
                this.btnFindDevice.Enabled = false;
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
                    btnFindDevice_Click(null, null);
                }
                catch (DeviceNotFoundException)
                {
                    Log("Device not found!");
                }
            }
        }

        private void btnLocateFirmware_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Firmware Update Files (*.hex)|*.hex|All files (*.*)|*.*";
            dialog.RestoreDirectory = true;

            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    dataBuffer = IntelHexParser.ParseFile(dialog.FileName, PageSize);

                    if (dataBuffer.Length > (FlashSize - 2048))
                    {
                        Log("File is too large!");
                        return;
                    }

                    FileInfo file = new FileInfo(dialog.FileName);
                    Log("Opened file " + file.Name);
                    Log("Ready to upload " + dataBuffer.Length.ToString() + " bytes of data");

                    this.btnUpload.Enabled = true;
                }
                catch (ChecksumMismatchException)
                {
                    Log("Checksum mismatch! File is not valid.");
                }
            }
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
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
            done = true;

            this.btnFindDevice.Enabled = false;
            this.btnLocateFirmware.Enabled = false;
            this.btnUpload.Enabled = false;
        }
    }
}
