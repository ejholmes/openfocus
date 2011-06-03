using System;
using System.Collections;
using System.Collections.Generic;

using LibUsbDotNet;
using LibUsbDotNet.Info;
using LibUsbDotNet.Main;

namespace Cortex.OpenFocus
{
    public class Bootloader
    {
        private const Int16 Vendor_ID = 0x16c0;
        private const Int16 Product_ID = 0x05dc;

        private static UsbDeviceFinder finder = new UsbDeviceFinder(Vendor_ID, Product_ID);
        private static UsbDevice device;

        private struct Request
        {
            public const byte Reboot = 0x01;
            public const byte WriteBlock = 0x02;
            public const byte GetReport = 0x03;
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

        private static Byte[] GetReport()
        {
            int expected = 6;
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

        public static UInt32 FlashSize
        {
            get
            {
                Byte[] data = GetReport();
                return (UInt16)((data[5] << 24) | (data[4] << 16) | (data[3] << 8) | data[2]);
            }
        }

        public static void WriteBlock(UInt32 address, Byte[] data)
        {
            Byte[] b = new Byte[4 + data.Length];

            b[0] = 0;
            Buffer.BlockCopy(ToUsbInt(address, 3), 0, b, 1, 3); /* Copy the 3 least significant bytes */
            Buffer.BlockCopy(data, 0, b, 4, 128);

            UsbSetupPacket packet = new UsbSetupPacket((byte)UsbRequestType.TypeVendor | (byte)UsbRequestRecipient.RecipDevice | (byte)UsbEndpointDirection.EndpointOut, (byte)Request.WriteBlock, 0, 0, 0);
            int transfered;
            device.ControlTransfer(ref packet, b, b.Length, out transfered);
        }

        public static void UploadFile(string file)
        {
            Byte[] data = null;
            uint PageSize = 0, FlashSize = 0;

            Logger.Write("Attempting to connect to bootloader");

            try /* Try to connect to the bootloader */
            {
                Connect();
                PageSize = Bootloader.PageSize;
                FlashSize = Bootloader.FlashSize;

                Logger.Write("Device Found!");
                Logger.Write("Page Size: " + PageSize.ToString() + " bytes");
                Logger.Write("Flash Size: " + FlashSize.ToString() + " bytes");
            }
            catch (DeviceNotFoundException) /* If the device isn't found... */
            {
                try /* Try connecting to the device and rebooting it into the bootloader */
                {
                    Device dev = new Device();
                    dev.Connect();
                    Logger.Write("Rebooting device into firmware update mode...");
                    dev.RebootToBootloader();
                    dev.Disconnect();
                    System.Threading.Thread.Sleep(2000);
                    UploadFile(file); /* If successful, wait 2 seconds and then retry */
                    return;
                }
                catch (DeviceNotFoundException) /* If this is reach, the device probably not connected */
                {
                    Logger.Write("Device not found!", Logger.LogType.Error);
                    return;
                }
            }
            
            try
            {
                data = IntelHex.Parse(file, PageSize);

                if (data.Length > (FlashSize - 2048))
                {
                    Logger.Write("File is too large!");
                    return;
                }
                Logger.Write("Ready to upload " + data.Length.ToString() + " bytes of data");
            }
            catch (ChecksumMismatchException)
            {
                Logger.Write("Checksum mismatch! File is not valid.");
                return;
            }

            /* Now that the device is connected, write the data, page by page. */
            for (uint address = 0; address < data.Length; address += PageSize)
            {
                Byte[] page = new Byte[PageSize];
                Buffer.BlockCopy(data, (int)address, page, 0, (int)PageSize);

                Logger.Write("Writing block 0x" + String.Format("{0:x3}", address) + " ... 0x" + String.Format("{0:x3}", (address + PageSize)));

                Bootloader.WriteBlock(address, page);
            }

            Logger.Write("Firmware update complete!");
            Logger.Write("Device is rebooting");
            Bootloader.Reboot();
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
