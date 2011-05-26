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

            /*IntPtr ptr = HID.Enumerate(0x0, 0x0);
            HID.hid_device_info info = (HID.hid_device_info)Marshal.PtrToStructure(ptr, typeof(HID.hid_device_info));
            info = (HID.hid_device_info)Marshal.PtrToStructure(info.next, typeof(HID.hid_device_info));
            info = (HID.hid_device_info)Marshal.PtrToStructure(info.next, typeof(HID.hid_device_info));

            string p = Marshal.PtrToStringAnsi(info.product_string);*/

            //IntPtr handle = HID.Open(0x16c0, 0x05df, null);

            /*string buffer = String.Empty;
            int ret = HID.GetManufacturerString(ref handle, ref buffer, 256);*/

            //HID.Close(ref handle);

            return;
        }
    }
}
