﻿using System;

namespace Cortex.OpenFocus
{
    public class Bootloader
    {
        private const Int16 Vendor_ID = 0x16c0;
        private const Int16 Product_ID = 0x05df;

        struct ReportID
        {
            public static byte GetInfo = 0x01;
            public static byte WriteBlock = 0x02;
            public static byte Reboot = 0x01;
        }

        static HID hid = new HID();

        public static void Connect()
        {
            hid.Open(Vendor_ID, Product_ID, null);
        }

        public static void Disconnect()
        {
            hid.Close();
        }

        public static UInt16 PageSize
        {
            get
            {
                Byte[] data = new Byte[7];
                data[0] = ReportID.GetInfo;
                data = hid.GetFeatureReport(data);

                return (UInt16)((data[2] << 8) | data[1]);
            }
        }

        public static UInt32 FlashSize
        {
            get
            {
                Byte[] data = new Byte[7];
                data[0] = ReportID.GetInfo;
                data = hid.GetFeatureReport(data);

                return (UInt16)((data[6] << 24) | (data[5] << 16) | (data[4] << 8) | data[3]);
            }
        }

        public static void WriteBlock(UInt32 address, Byte[] data)
        {
            Byte[] b = new Byte[4 + data.Length];

            b[0] = ReportID.WriteBlock;
            Buffer.BlockCopy(ToUsbInt(address, 3), 0, b, 1, 3); /* Copy the 3 least significant bytes */
            Buffer.BlockCopy(data, 0, b, 4, 128);

            hid.SendFeatureReport(b);
        }

        public static void Reboot()
        {
            Byte[] data = new Byte[7];
            data[0] = ReportID.Reboot;
            hid.SendFeatureReport(data);

            Disconnect();
        }

        private static Byte[] ToUsbInt(UInt32 value, int length)
        {
            Byte[] bytes = new Byte[length];
            for (int i = 0; i < length; i++)
            {
                bytes[i] = (byte)(value & 0xff);
                value >>= 8;
            }

            return bytes;
        }

        private static UInt32 GetUsbInt(Byte[] bytes)
        {
            UInt32 value = 0;
            int shift = 0;

            for (int i = 0; i < bytes.Length; i++)
            {
                value |= (UInt32)(bytes[i] << shift);
                shift += 8;
            }

            return value;
        }
    }
}