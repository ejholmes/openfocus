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
        private struct Capability
        {
            public const byte AbsolutePositioning = 0x01;
            public const byte TemperatureCompensation = 0x02;
        }

        /* USB Requests */
        private struct Request
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

        public const Int16 Vendor_ID = 0x20a0;
        public const Int16 Product_ID = 0x416b;
        public const String ManufacturerString = "Cortex Astronomy (cortexastronomy.com)";
        public const String ProductString = "OpenFocus";

        private bool TempCompEnabled = false;

        private static UsbDeviceFinder UsbFinder = new UsbDeviceFinder(Vendor_ID, Product_ID);
        private UsbDevice device;

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

        public bool Connect()
        {
            return Connect(String.Empty);
        }

        public bool Connect(string serial)
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

            return true;
        }

        public bool Connected
        {
            get { return device.IsOpen; }
        }

        public String Serial
        {
            get { return device.Info.SerialString; }
        }

        public Version FirmwareVersion
        {
            get { return new Version(device.Info.Descriptor.BcdDevice >> 8, 0xff & device.Info.Descriptor.BcdDevice); }
        }

        /* Disconnect the device */
        public void Disconnect()
        {
            device.Close();
        }

        /* Move focuser to position */
        public void MoveTo(Int16 position)
        {
            UsbSetupPacket packet = new UsbSetupPacket((byte)UsbRequestType.TypeVendor | (byte)UsbRequestRecipient.RecipDevice | (byte)UsbEndpointDirection.EndpointOut, (byte)Request.MoveTo, (short)position, 2, 1);

            int transfered;
            object buffer = null;
            device.ControlTransfer(ref packet, buffer, 0, out transfered);
        }

        /* Halt focuser */
        public void Halt()
        {
            UsbSetupPacket packet = new UsbSetupPacket((byte)UsbRequestType.TypeVendor | (byte)UsbRequestRecipient.RecipDevice | (byte)UsbEndpointDirection.EndpointOut, (byte)Request.Halt, 0, 0, 0);

            int transfered;
            object buffer = null;
            device.ControlTransfer(ref packet, buffer, 0, out transfered);
        }

        public void RebootToBootloader()
        {
            UsbSetupPacket packet = new UsbSetupPacket((byte)UsbRequestType.TypeVendor | (byte)UsbRequestRecipient.RecipDevice | (byte)UsbEndpointDirection.EndpointOut, (byte)Request.RebootToBootloader, 0, 0, 0);

            int transfered;
            object buffer = null;
            device.ControlTransfer(ref packet, buffer, 0, out transfered);
        }

        /* Returns true if moving, false if halted */
        public bool IsMoving
        {
            get
            {
                UsbSetupPacket packet = new UsbSetupPacket((byte)UsbRequestType.TypeVendor | (byte)UsbRequestRecipient.RecipDevice | (byte)UsbEndpointDirection.EndpointIn, (byte)Request.IsMoving, 0, 0, 0);

                int expected = 1;
                int transfered;
                byte[] buffer = new byte[expected];
                device.ControlTransfer(ref packet, buffer, expected, out transfered);

                if (transfered != expected) throw new CommunicationException("Error Communicating With Device");

                return buffer[0] == 0 ? false : true;
            }
        }

        /* Current focuser position */
        public UInt16 Position
        {
            set
            {
                UsbSetupPacket packet = new UsbSetupPacket((byte)UsbRequestType.TypeVendor | (byte)UsbRequestRecipient.RecipDevice | (byte)UsbEndpointDirection.EndpointOut, (byte)Request.SetPosition, (short)value, 2, 1);

                int transfered;
                object buffer = null;
                device.ControlTransfer(ref packet, buffer, 0, out transfered);
            }
            get
            {
                UsbSetupPacket packet = new UsbSetupPacket((byte)UsbRequestType.TypeVendor | (byte)UsbRequestRecipient.RecipDevice | (byte)UsbEndpointDirection.EndpointIn, (byte)Request.GetPosition, 0, 0, 0);

                int expected = 2;
                int transfered;
                byte[] buffer = new byte[expected];
                device.ControlTransfer(ref packet, buffer, expected, out transfered);

                if (transfered != expected) throw new CommunicationException("Error Communicating With Device");

                return (UInt16)((buffer[1] << 8) | buffer[0]);
            }
        }

        /* true to turn on temperature compensation, false to turn off */
        public bool TempComp
        {
            get
            {
                return TempCompEnabled;
            }
            set
            {
                UsbSetupPacket packet = new UsbSetupPacket((byte)UsbRequestType.TypeVendor | (byte)UsbRequestRecipient.RecipDevice | (byte)UsbEndpointDirection.EndpointOut, (byte)Request.SetTemperatureCompensation, (short)((value) ? 1 : 0), 1, 1);

                int transfered;
                object buffer = null;
                device.ControlTransfer(ref packet, buffer, 0, out transfered);

                TempCompEnabled = value;
            }
        }

        /* Current temperature */
        public double Temperature
        {
            get
            {
                UsbSetupPacket packet = new UsbSetupPacket((byte)UsbRequestType.TypeVendor | (byte)UsbRequestRecipient.RecipDevice | (byte)UsbEndpointDirection.EndpointIn, (byte)Request.GetTemperature, 0, 0, 0);

                int expected = 2;
                int transfered;
                byte[] buffer = new byte[expected];
                device.ControlTransfer(ref packet, buffer, expected, out transfered);

                if (transfered != expected) throw new CommunicationException("Error Communicating With Device");

                Int16 adc = (Int16)((buffer[1] << 8) | buffer[0]);
                double kelvin = (5.00 * (double)adc * 100.00) / 1024.00;
                kelvin = Math.Round(kelvin, 1); /* See https://github.com/CortexAstronomy/OpenFocus/wiki/LM335-Information */

                return kelvin;
            }
        }

        /* Gets the device's capabilites as a single byte */
        private byte Capabilities
        {
            get
            {
                UsbSetupPacket packet = new UsbSetupPacket((byte)UsbRequestType.TypeVendor | (byte)UsbRequestRecipient.RecipDevice | (byte)UsbEndpointDirection.EndpointIn, (byte)Request.GetCapabilities, 0, 0, 0);

                int expected = 1;
                int transfered;
                byte[] buffer = new byte[expected];
                device.ControlTransfer(ref packet, buffer, expected, out transfered);

                if (transfered != expected) throw new CommunicationException("Error Communicating With Device");

                return buffer[0];
            }
        }

        /* Returns true if focuser is capable of absolute positioning */
        public bool Absolute
        {
            get { return ((Capabilities & Capability.AbsolutePositioning) == Capability.AbsolutePositioning) ? true : false; }
        }

        /* Returns true if focuser is capable of temperature compensation */
        public bool TempCompAvailable
        {
            get { return ((Capabilities & Capability.TemperatureCompensation) == Capability.TemperatureCompensation) ? true : false; }
        }
        #endregion
    }
}
