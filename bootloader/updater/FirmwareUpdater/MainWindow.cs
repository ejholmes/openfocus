using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using BootloadHID;

namespace FirmwareUpdater
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnSelectFirmware_Click(object sender, EventArgs e)
        {
            String firmware =  @"C:\Users\ejholmes\Dropbox\Cortex Astronomy\Product Development\OpenFocus\firmware\main.hex";
            Bootloader.UploadFirmware(firmware);
        }
    }
}
