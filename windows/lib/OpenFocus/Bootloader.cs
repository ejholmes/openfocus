using System;
using System.Collections;
using System.Collections.Generic;

using LibUsbDotNet;
using LibUsbDotNet.Info;
using LibUsbDotNet.Main;

namespace Cortex.OpenFocus
{
    public static class Bootloader
    {
        private const Int16 Vendor_ID = 0x20a0;
        private const Int16 Product_ID = 0x416d;

        private static UsbDeviceFinder finder = new UsbDeviceFinder(Vendor_ID, Product_ID);
        private static UsbDevice device;

        private struct Request
        {
            public const byte Reboot = 0x01;
            public const byte WriteFlashBlock = 0x02;
            public const byte GetReport = 0x03;
            public const byte WriteEepromBlock = 0x04;
            public const byte ReadEepromBlock = 0x05;
        }

        public static List<string> ListDevices()
        {
            List<string> serials = new List<string>();
            UsbRegDeviceList regList = UsbDevice.AllDevices.FindAll(finder);

            if (regList.Count == 0) return null;

            foreach (UsbRegistry regDevice in regList)
                serials.Add(regDevice.Device.Info.SerialString);

            return serials;
        }

        public static void Connect()
        {
            device = UsbDevice.OpenUsbDevice(finder);

            if (device == null)
                throw new DeviceNotFoundException("Device not found!");

            IUsbDevice usbDev = device as IUsbDevice;
            if (!ReferenceEquals(usbDev, null))
            {
                usbDev.SetConfiguration(1);
                usbDev.ClaimInterface(0);
            }
        }

        public static void Disconnect()
        {
            device.Close();
        }

        public static bool Connected
        {
            get
            {
                return (device != null && device.IsOpen);
            }
        }

        private static Byte[] GetReport()
        {
            int expected = 8;
            Byte[] data = new Byte[expected];
            UsbSetupPacket packet = new UsbSetupPacket((byte)UsbRequestType.TypeVendor | (byte)UsbRequestRecipient.RecipDevice | (byte)UsbEndpointDirection.EndpointIn, (byte)Request.GetReport, 0, 0, 0);
            int transfered;
            device.ControlTransfer(ref packet, data, data.Length, out transfered);

            return data;
        }

        public static UInt16 PageSize
        {
            get
            {
                Byte[] data = GetReport();
                return (UInt16)((data[1] << 8) | data[0]);
            }
        }

        public static UInt16 FlashSize
        {
            get
            {
                Byte[] data = GetReport();
                return (UInt16)((data[5] << 24) | (data[4] << 16) | (data[3] << 8) | data[2]);
            }
        }

        public static UInt16 EEPROMSize
        {
            get
            {
                Byte[] data = GetReport();
                return (UInt16)((data[7] << 8) | data[6]);
            }
        }

        public static Byte[] ReadEepromBlock(UInt16 address, int length)
        {
            Byte[] data = new Byte[length];

            UsbSetupPacket packet = new UsbSetupPacket((byte)UsbRequestType.TypeVendor | (byte)UsbRequestRecipient.RecipDevice | (byte)UsbEndpointDirection.EndpointIn, (byte)Request.ReadEepromBlock, (short)address, 0, (short)data.Length);
            int transfered;
            device.ControlTransfer(ref packet, data, data.Length, out transfered);
            if (transfered != data.Length)
                throw new CommunicationException("Error sending data to device");

            return data;
        }

        public static Byte[] ReadEeprom()
        {
            UInt16 BlockSize = 128;
            UInt16 EepromSize = EEPROMSize;

            Byte[] data = new Byte[EepromSize];

            for (UInt16 address = 0; address < EepromSize; address += BlockSize)
            {
                Byte[] b = ReadEepromBlock(address, BlockSize + sizeof(UInt16));

                UInt16 receivedAddress = (UInt16)((b[0] << 8) | (b[1] & 0xff));

                Buffer.BlockCopy(b, sizeof(UInt16), data, receivedAddress, b.Length - sizeof(UInt16));
            }

            return data;
        }

        public static void WriteEepromBlock(UInt16 address, Byte[] data)
        {
            Byte[] b = new Byte[sizeof(UInt16) + data.Length];

            Buffer.BlockCopy(ToUsbInt(address, sizeof(UInt16)), 0, b, 0, sizeof(UInt16));
            Buffer.BlockCopy(data, 0, b, sizeof(UInt16), data.Length);

            UsbSetupPacket packet = new UsbSetupPacket((byte)UsbRequestType.TypeVendor | (byte)UsbRequestRecipient.RecipDevice | (byte)UsbEndpointDirection.EndpointOut, (byte)Request.WriteEepromBlock, 0, 0, (short)b.Length);
            int transfered;
            device.ControlTransfer(ref packet, b, b.Length, out transfered);
            if (transfered != b.Length)
                throw new CommunicationException("Error sending data to device");
        }

        public static void WriteEeprom(Byte[] data)
        {
            UInt16 BlockSize = 2;
            for (UInt16 address = 0; address < data.Length; address += BlockSize)
            {
                Byte[] block = new Byte[BlockSize];
                if ((address + BlockSize) > data.Length)
                    BlockSize = (UInt16)(data.Length - address);
                Buffer.BlockCopy(data, (int)address, block, 0, (int)BlockSize);

                Logger.Write("Writing eeprom block 0x" + String.Format("{0:x3}", address) + " ... 0x" + String.Format("{0:x3}", (address + BlockSize)));

                Bootloader.WriteEepromBlock(address, block);
            }
        }

        public static void WriteFlashBlock(UInt16 address, Byte[] data)
        {
            Byte[] b = new Byte[sizeof(UInt16) + data.Length];

            Buffer.BlockCopy(ToUsbInt(address, sizeof(UInt16)), 0, b, 0, sizeof(UInt16));
            Buffer.BlockCopy(data, 0, b, sizeof(UInt16), data.Length);

            UsbSetupPacket packet = new UsbSetupPacket((byte)UsbRequestType.TypeVendor | (byte)UsbRequestRecipient.RecipDevice | (byte)UsbEndpointDirection.EndpointOut, (byte)Request.WriteFlashBlock, 0, 0, (short)b.Length);
            int transfered;
            device.ControlTransfer(ref packet, b, b.Length, out transfered);
            if (transfered != b.Length)
                throw new CommunicationException("Error sending data to device");
        }

        public static void WriteFlash(Byte[] data)
        {
            /* Now that the device is connected, write the data, page by page. */
            for (UInt16 address = 0; address < data.Length; address += PageSize)
            {
                Byte[] page = new Byte[PageSize];
                Buffer.BlockCopy(data, (int)address, page, 0, (int)PageSize);

                Logger.Write("Writing block 0x" + String.Format("{0:x3}", address) + " ... 0x" + String.Format("{0:x3}", (address + PageSize)));

                Bootloader.WriteFlashBlock(address, page);
            }
        }

        public static void Reboot()
        {
            UsbSetupPacket packet = new UsbSetupPacket((byte)UsbRequestType.TypeVendor | (byte)UsbRequestRecipient.RecipDevice | (byte)UsbEndpointDirection.EndpointOut, (byte)Request.Reboot, 0, 0, 0);
            int transfered;
            object buffer = null;
            device.ControlTransfer(ref packet, buffer, 0, out transfered);

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
