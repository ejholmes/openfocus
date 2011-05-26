using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Cortex
{
    using size_t = UInt16;

    public class HID
    {
        IntPtr handle = IntPtr.Zero;

        #region Definitions

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private struct hid_device_info
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

        [DllImport("hidapi.dll")]
        private static extern IntPtr hid_enumerate(ushort vendor_id, ushort product_id);

        [DllImport("hidapi.dll")]
        private static extern void hid_free_enumeration(IntPtr devs);

        [DllImport("hidapi.dll")]
        private static extern IntPtr hid_open(ushort vendor_id, ushort product_id, string serial_number);

        [DllImport("hidapi.dll")]
        private static extern IntPtr hid_open_path(IntPtr path);

        [DllImport("hidapi.dll")]
        private static extern int hid_set_nonblocking(IntPtr handle, int nonblock);

        [DllImport("hidapi.dll")]
        private static extern int hid_send_feature_report(IntPtr handle, Byte[] data, size_t length);

        [DllImport("hidapi.dll")]
        private static extern int hid_get_feature_report(IntPtr handle, Byte[] data, size_t length);

        [DllImport("hidapi.dll")]
        private static extern void hid_close(IntPtr handle);

        [DllImport("hidapi.dll")]
        private static extern int hid_get_manufacturer_string(IntPtr handle, IntPtr buffer, size_t maxlen);

        [DllImport("hidapi.dll")]
        private static extern int hid_get_product_string(IntPtr handle, IntPtr buffer, size_t maxlen);

        [DllImport("hidapi.dll")]
        private static extern IntPtr hid_error(IntPtr handle);

        #endregion

        public struct DeviceInfo
        {
            public string Path;

            public ushort VendorID;

            public ushort ProductID;

            public string SerialNumber;

            public ushort ReleaseNumber;

            public string ManufacturerString;

            public string ProductString;

            public ushort UsagePage;

            public ushort Usage;

            public int InterfaceNumber;
        }

        public DeviceInfo[] Enumerate(ushort vendor_id, ushort product_id)
        {
            IntPtr ptr = hid_enumerate(vendor_id, product_id);
            hid_device_info dev_info = (hid_device_info)Marshal.PtrToStructure(ptr, typeof(hid_device_info));
            List<DeviceInfo> devices = new List<DeviceInfo>();

            for (int i = 0; ; i++)
            {
                DeviceInfo dev = new DeviceInfo();

                dev.Path = Marshal.PtrToStringAuto(dev_info.path);
                dev.VendorID = dev_info.vendor_id;
                dev.ProductID = dev_info.product_id;
                dev.SerialNumber = Marshal.PtrToStringAuto(dev_info.serial_number);
                dev.ReleaseNumber = dev_info.release_number;
                dev.ManufacturerString = Marshal.PtrToStringAuto(dev_info.manufacturer_string);
                dev.ProductString = Marshal.PtrToStringAuto(dev_info.product_string);
                dev.UsagePage = dev_info.usage_page;
                dev.Usage = dev_info.usage;
                dev.InterfaceNumber = dev_info.interface_number;

                devices.Add(dev);

                if (dev_info.next == IntPtr.Zero)
                    break;
                dev_info = (hid_device_info)Marshal.PtrToStructure(dev_info.next, typeof(hid_device_info));
            }

            hid_free_enumeration(ptr);

            return devices.ToArray();
        }

        public void Open(int vendor_id, int product_id, string serial_number)
        {
            this.handle = hid_open((ushort)vendor_id, (ushort)product_id, serial_number);

            if (this.handle == IntPtr.Zero)
                throw new Exception("Device not found!");
        }

        public void SendFeatureReport(Byte[] data)
        {
            if (hid_send_feature_report(this.handle, data, (size_t)data.Length) < 0)
                throw new Exception(LastError());
        }

        public Byte[] GetFeatureReport(Byte[] data)
        {
            if (hid_get_feature_report(this.handle, data, (size_t)data.Length) < 0)
                throw new Exception(LastError());

            return data;
        }

        public void Close()
        {
            if (this.handle != IntPtr.Zero)
                hid_close(this.handle);
        }

        public string GetManufacturerString()
        {
            IntPtr buffer = Marshal.AllocHGlobal(256);
            if (hid_get_manufacturer_string(this.handle, buffer, 256) != 0)
                throw new Exception("Error getting manufacturer string");

            string manufacturer_string = Marshal.PtrToStringAuto(buffer);
            Marshal.FreeHGlobal(buffer);

            return manufacturer_string;
        }

        public string GetProductString()
        {
            IntPtr buffer = Marshal.AllocHGlobal(256);
            if (hid_get_product_string(this.handle, buffer, 256) != 0)
                throw new Exception("Error getting product string");

            string product_string = Marshal.PtrToStringAuto(buffer);
            Marshal.FreeHGlobal(buffer);

            return product_string;
        }

        public string LastError()
        {
            return Marshal.PtrToStringAuto(hid_error(this.handle));
        }
    }
}
