using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace FirmwareUpdater
{
    public class HID
    {
        [StructLayout(LayoutKind.Sequential, CharSet=CharSet.Auto)]
        public struct hid_device_info
        {
            public IntPtr path;

            public ushort vendor_id;

            public ushort product_id;

            public IntPtr serial_number;

            public ushort release_number;

            public IntPtr manufacturer_string;

            public IntPtr product_string;

            public ushort usage_page;

            public ushort usage;

            public int interface_number;

            public IntPtr next;
        }

        #region Function Definitions

        [DllImport("hidapi.dll")]
        private static extern IntPtr hid_enumerate(ushort vendor_id, ushort product_id);

        [DllImport("hidapi.dll")]
        private static extern IntPtr hid_open(ushort vendor_id, ushort product_id, string serial_number);

        [DllImport("hidapi.dll")]
        private static extern int hid_get_manufacturer_string(ref IntPtr handle, ref string buffer, ushort maxlen);

        [DllImport("hidapi.dll")]
        private static extern void hid_close(ref IntPtr handle);

        #endregion


        public static IntPtr Enumerate(ushort vendor_id, ushort product_id)
        {
            return hid_enumerate(vendor_id, product_id);
        }

        public static IntPtr Open(ushort vendor_id, ushort product_id, string serial_number)
        {
            return hid_open(vendor_id, product_id, serial_number);
        }

        public static int GetManufacturerString(ref IntPtr handle, ref string buffer, ushort maxlen)
        {
            return hid_get_manufacturer_string(ref handle, ref buffer, maxlen);
        }

        public static void Close(ref IntPtr handle)
        {
            hid_close(ref handle);
        }
    }
}
