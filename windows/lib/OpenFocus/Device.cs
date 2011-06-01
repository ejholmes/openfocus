using System;
using System.Collections.Generic;

using LibUsbDotNet;
using LibUsbDotNet.Info;
using LibUsbDotNet.Main;

namespace Cortex.OpenFocus
{
    public class Device
    {
        /* Device capabilities */
        private struct Capabilities
        {
            public const byte AbsolutePositioning = 0x01;
            public const byte TemperatureCompensation = 0x02;
        }

        /* USB Requests */
        private struct Requests
        {
            public const byte MoveTo = 0x00;
            public const byte Halt = 0x01;
            public const byte SetPosition = 0x02;
            public const byte SetTemperatureCompensation = 0x03;
            public const byte RebootToBootloader = 0x04;
            public const byte GetPosition = 0x10;
            public const byte IsMoving = 0x11;
            public const byte GetCapabilities = 0x12;
            public const byte GetTemperature = 0x13;
        }

        /* Temperature Display Units */
        public struct TemperatureUnits
        {
            public const string Celsius = "0";
            public const string Fahrenheit = "1";
        }

        private static byte _Capabilities;

        public static Int16 Vendor_ID = 0x20a0;
        public static Int16 Product_ID = 0x416b;
        public static String ManufacturerString = "Cortex Astronomy (cortexastronomy.com)";
        public static String ProductString = "OpenFocus";

        private static bool TempCompEnabled = false;

        private static UsbDeviceFinder UsbFinder = new UsbDeviceFinder(Vendor_ID, Product_ID);
        private static UsbDevice device;

        #region public methods
        public static List<string> ListDevices()
        {
            List<string> serials = new List<string>();
            UsbRegDeviceList regList = UsbDevice.AllDevices.FindAll(UsbFinder);

            if (regList.Count == 0) return null;

            foreach (UsbRegistry regDevice in regList)
                serials.Add(regDevice.Device.Info.SerialString);

            return serials;
        }

        public static bool Connect()
        {
            return Connect(String.Empty);
        }

        public static bool Connect(string serial)
        {
            if (!String.IsNullOrEmpty(serial))
                device = UsbDevice.OpenUsbDevice(new UsbDeviceFinder(serial));
            else
                device = UsbDevice.OpenUsbDevice(UsbFinder);

            /* According to V-USB licensing, we have to check manufacturer string and product string */
            if (device == null
                || device.Info.ManufacturerString != ManufacturerString
                || device.Info.ProductString != ProductString) throw new DeviceNotFoundException("Device not found");

            IUsbDevice usbDev = device as IUsbDevice;
            if (!ReferenceEquals(usbDev, null))
            {
                usbDev.SetConfiguration(1);
                usbDev.ClaimInterface(0);
            }

            GetCapabilities();

            return true;
        }

        /* Returns the descriptor from the USB device */
        public static UsbDeviceInfo Descriptor
        {
            get { return device.Info; }
        }

        /* Gets the device's capabilites as a single byte */
        public static void GetCapabilities()
        {
            UsbSetupPacket packet = new UsbSetupPacket((byte)UsbRequestType.TypeVendor | (byte)UsbRequestRecipient.RecipDevice | (byte)UsbEndpointDirection.EndpointIn, (byte)Requests.GetCapabilities, 0, 0, 0);

            int expected = 1;
            int transfered;
            byte[] buffer = new byte[expected];
            device.ControlTransfer(ref packet, buffer, expected, out transfered);

            if (transfered != expected) throw new Exception("Error Communicating With Device");

            _Capabilities = buffer[0];
        }

        /* Disconnect the device */
        public static void Disconnect()
        {
            device.Close();
        }

        /* Move focuser to position */
        public static void MoveTo(Int16 position)
        {
            UsbSetupPacket packet = new UsbSetupPacket((byte)UsbRequestType.TypeVendor | (byte)UsbRequestRecipient.RecipDevice | (byte)UsbEndpointDirection.EndpointOut, (byte)Requests.MoveTo, (short)position, 2, 1);

            int transfered;
            object buffer = null;
            device.ControlTransfer(ref packet, buffer, 0, out transfered);
        }

        /* Halt focuser */
        public static void Halt()
        {
            UsbSetupPacket packet = new UsbSetupPacket((byte)UsbRequestType.TypeVendor | (byte)UsbRequestRecipient.RecipDevice | (byte)UsbEndpointDirection.EndpointOut, (byte)Requests.Halt, 0, 0, 0);

            int transfered;
            object buffer = null;
            device.ControlTransfer(ref packet, buffer, 0, out transfered);
        }

        public static void RebootToBootloader()
        {
            UsbSetupPacket packet = new UsbSetupPacket((byte)UsbRequestType.TypeVendor | (byte)UsbRequestRecipient.RecipDevice | (byte)UsbEndpointDirection.EndpointOut, (byte)Requests.RebootToBootloader, 0, 0, 0);

            int transfered;
            object buffer = null;
            device.ControlTransfer(ref packet, buffer, 0, out transfered);
        }

        /* Returns true if moving, false if halted */
        public static bool IsMoving
        {
            get
            {
                UsbSetupPacket packet = new UsbSetupPacket((byte)UsbRequestType.TypeVendor | (byte)UsbRequestRecipient.RecipDevice | (byte)UsbEndpointDirection.EndpointIn, (byte)Requests.IsMoving, 0, 0, 0);

                int expected = 1;
                int transfered;
                byte[] buffer = new byte[expected];
                device.ControlTransfer(ref packet, buffer, expected, out transfered);

                if (transfered != expected) throw new Exception("Error Communicating With Device");

                return buffer[0] == 0 ? false : true;
            }
        }

        ///* Max position */
        //public static UInt16 MaxStep
        //{
        //    get { return new Config.Device(device.Info.SerialString).MaxPosition; }
        //}

        ///* Max steps per move */
        //public static UInt16 MaxIncrement
        //{
        //    get { return new Config.Device(device.Info.SerialString).MaxPosition; }
        //}

        /* Current focuser position */
        public static UInt16 Position
        {
            set
            {
                UsbSetupPacket packet = new UsbSetupPacket((byte)UsbRequestType.TypeVendor | (byte)UsbRequestRecipient.RecipDevice | (byte)UsbEndpointDirection.EndpointOut, (byte)Requests.SetPosition, (short)value, 2, 1);

                int transfered;
                object buffer = null;
                device.ControlTransfer(ref packet, buffer, 0, out transfered);
            }
            get
            {
                UsbSetupPacket packet = new UsbSetupPacket((byte)UsbRequestType.TypeVendor | (byte)UsbRequestRecipient.RecipDevice | (byte)UsbEndpointDirection.EndpointIn, (byte)Requests.GetPosition, 0, 0, 0);

                int expected = 2;
                int transfered;
                byte[] buffer = new byte[expected];
                device.ControlTransfer(ref packet, buffer, expected, out transfered);

                if (transfered != expected) throw new Exception("Error Communicating With Device");

                return (UInt16)((buffer[1] << 8) | buffer[0]);
            }
        }

        /* true to turn on temperature compensation, false to turn off */
        public static bool TempComp
        {
            get
            {
                return TempCompEnabled;
            }
            set
            {
                UsbSetupPacket packet = new UsbSetupPacket((byte)UsbRequestType.TypeVendor | (byte)UsbRequestRecipient.RecipDevice | (byte)UsbEndpointDirection.EndpointOut, (byte)Requests.SetTemperatureCompensation, (short)((value) ? 1 : 0), 1, 1);

                int transfered;
                object buffer = null;
                device.ControlTransfer(ref packet, buffer, 0, out transfered);

                TempCompEnabled = value;
            }
        }

        /* Current temperature */
        public static double Temperature
        {
            get
            {
                UsbSetupPacket packet = new UsbSetupPacket((byte)UsbRequestType.TypeVendor | (byte)UsbRequestRecipient.RecipDevice | (byte)UsbEndpointDirection.EndpointIn, (byte)Requests.GetTemperature, 0, 0, 0);

                int expected = 2;
                int transfered;
                byte[] buffer = new byte[expected];
                device.ControlTransfer(ref packet, buffer, expected, out transfered);

                if (transfered != expected) throw new Exception("Error Communicating With Device");

                Int16 adc = (Int16)((buffer[1] << 8) | buffer[0]);
                double kelvin = (5.00 * (double)adc * 100.00) / 1024.00;

                return kelvin;
            }
        }

        /* Returns true if focuser is capable of absolute positioning */
        public static bool Absolute
        {
            get { return ((_Capabilities & Capabilities.AbsolutePositioning) == Capabilities.AbsolutePositioning) ? true : false; }
        }

        /* Returns true if focuser is capable of temperature compensation */
        public static bool TempCompAvailable
        {
            get { return ((_Capabilities & Capabilities.TemperatureCompensation) == Capabilities.TemperatureCompensation) ? true : false; }
        }
        #endregion
    }
}
