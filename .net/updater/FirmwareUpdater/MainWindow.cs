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
        Bootloader device;

        uint PageSize = 0;
        uint FlashSize = 0;

        Byte[] dataBuffer;

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

        private void btnFindDevice_Click(object sender, EventArgs e)
        {
            try
            {
                device = new Bootloader(0x16c0, 0x05df);
                PageSize = device.PageSize;
                FlashSize = device.FlashSize;

                Log("Device Found!");
                Log("Page Size: " + PageSize.ToString() + " bytes");
                Log("Flash Size: " + FlashSize.ToString() + " bytes");

                this.btnLocateFirmware.Enabled = true;
                this.btnFindDevice.Enabled = false;
            }
            catch
            {
                Log("Device not found!");
                Log("");
            }
        }

        private void btnLocateFirmware_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Firmware Update Files (*.hex)|*.hex|All files (*.*)|*.*";
            dialog.RestoreDirectory = true;

            if (dialog.ShowDialog(this) == DialogResult.OK)
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
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            for (uint address = 0; address < dataBuffer.Length; address += PageSize)
            {
                Byte[] data = new Byte[PageSize];
                Buffer.BlockCopy(dataBuffer, (int)address, data, 0, (int)PageSize);

                Log("Writing block 0x" + String.Format("{0:x3}", address) + " ... 0x" + String.Format("{0:x3}", (address + PageSize)));

                device.WriteBlock(address, data);
            }

            Log("Firmware update complete!");

#if !DEBUG
            Log("Device is rebooting");
            device.Reboot();
#endif
        }
    }
}
