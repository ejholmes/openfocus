using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace FirmwareUpdater
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();

            HID hid = new HID();
            hid.Open(0x16c0, 0x05df, null);
            Console.WriteLine(hid.GetManufacturerString());
            Console.WriteLine(hid.GetProductString());

            Byte[] data = new Byte[7];

            data = hid.GetReport(HID.RequestTypes.GetReport, 7);

            /*Byte[] reboot = new Byte[2];
            reboot[0] = HID.RequestTypes.SetReport
            hid.SetReport(*/

            Console.WriteLine("Page Size: " + ((data[2] << 8) | data[1]).ToString());
            Console.WriteLine("Flash Size: " + ((data[6] << 24) | (data[5] << 16) | (data[4] << 8) | data[3]).ToString());

            hid.Close();

            return;
        }
    }
}
