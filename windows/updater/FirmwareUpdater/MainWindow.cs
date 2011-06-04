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
        bool wait = true;

        public MainWindow()
        {
            InitializeComponent();

            Logger.LoggerWrite += new Logger.LoggerWriteEventHandler(Logger_LoggerWrite);

            Logger.Write(this.Text + " version " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString());
        }

        void Logger_LoggerWrite(object sender, LoggerEventArgs e)
        {
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

        private void MainWindow_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            if (!done)
            {
                Logger.Write("No data uploaded...rebooting");
                try
                {
                    Bootloader.Reboot();
                }
                catch { }
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

                Logger.Write("Device Found!");
                Logger.Write("Page Size: " + PageSize.ToString() + " bytes");
                Logger.Write("Flash Size: " + FlashSize.ToString() + " bytes");

                this.btnLocateFirmware.Enabled = true;
                this.btnFindDevice.Enabled = false;
            }
            catch (DeviceNotFoundException)
            {
                try
                {
                    Device dev = new Device();
                    dev.Connect();
                    Logger.Write("Rebooting device into firmware update mode...");
                    dev.RebootToBootloader();
                    dev.Disconnect();
                    while (wait) /* Wait for device to reboot and be enumerated by the OS */
                    {
                        System.Threading.Thread.Sleep(200);
                        Application.DoEvents();
                    }
                    wait = true;
                    btnFindDevice_Click(null, null);
                }
                catch (DeviceNotFoundException)
                {
                    done = true;
                    Logger.Write("Device not found!");
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
                    IntelHexFile f = IntelHexFile.Open(dialog.FileName);
                    f.PageSize = PageSize;
                    dataBuffer = f.Data;

                    if (dataBuffer.Length > (FlashSize - 2048))
                    {
                        Logger.Write("File is too large!");
                        return;
                    }

                    FileInfo file = new FileInfo(dialog.FileName);
                    Logger.Write("Opened file " + file.Name);
                    Logger.Write("Ready to upload " + dataBuffer.Length.ToString() + " bytes of data");

                    this.btnUpload.Enabled = true;
                }
                catch (ChecksumMismatchException)
                {
                    Logger.Write("Checksum mismatch! File is not valid.");
                }
            }
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            for (uint address = 0; address < dataBuffer.Length; address += PageSize)
            {
                Byte[] data = new Byte[PageSize];
                Buffer.BlockCopy(dataBuffer, (int)address, data, 0, (int)PageSize);

                Logger.Write("Writing block 0x" + String.Format("{0:x3}", address) + " ... 0x" + String.Format("{0:x3}", (address + PageSize)));

                Bootloader.WriteBlock(address, data);
            }

            Logger.Write("Firmware update complete!");
            Logger.Write("Device is rebooting");
            Bootloader.Reboot();
            done = true;

            this.btnFindDevice.Enabled = false;
            this.btnLocateFirmware.Enabled = false;
            this.btnUpload.Enabled = false;
        }

        protected override void WndProc(ref Message m)
        {
            const int WM_DEVICECHANGE = 0x0219;
            const int DBT_DEVNODES_CHANGED = 0x0007;
            if (m.Msg == WM_DEVICECHANGE && m.WParam.ToInt32() == DBT_DEVNODES_CHANGED)
            {
                if (Bootloader.ListDevices() != null)
                    wait = false;
            }
            base.WndProc(ref m);
        }
    }
}
