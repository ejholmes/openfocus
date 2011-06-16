using System;
using System.Collections.Generic;
using System.Timers;

using LibUsbDotNet;
using LibUsbDotNet.Info;
using LibUsbDotNet.Main;

namespace Cortex.OpenFocus
{
    public class Device
    {
        /// <summary>
        /// Device capabilities
        /// </summary>
        private struct Capability
        {
            public const byte AbsolutePositioning = 0x01;
            public const byte TemperatureCompensation = 0x02;
        }

        /// <summary>
        /// USB Requests
        /// </summary>
        private struct Request
        {
            public const byte MoveTo = 0x00;
            public const byte Halt = 0x01;
            public const byte SetPosition = 0x02;
            public const byte Reverse = 0x03;
            public const byte RebootToBootloader = 0x04;
            public const byte GetPosition = 0x10;
            public const byte IsMoving = 0x11;
            public const byte GetCapabilities = 0x12;
            public const byte GetTemperature = 0x13;
        }
        /// <summary>
        /// Vendor ID
        /// </summary>
        public const Int16 Vendor_ID = 0x20a0;
        /// <summary>
        /// Product ID
        /// </summary>
        public const Int16 Product_ID = 0x416b;

        private bool TempCompEnabled = false;
        private Timer TempCompTimer = new Timer(10000);
        private double LastTemperature = 0.0;

        private static UsbDeviceFinder UsbFinder = new UsbDeviceFinder(Vendor_ID, Product_ID);
        private UsbDevice device;

        #region public methods

        public Device()
        {
            TempCompTimer.Elapsed += new ElapsedEventHandler(TempCompTimer_Elapsed);
        }

        /// <summary>
        /// Static function for finding available openfocus devices on the system
        /// </summary>
        /// <returns>List of serials for the devices currently connected</returns>
        public static List<string> ListDevices()
        {
            List<string> serials = new List<string>();
            UsbRegDeviceList regList = UsbDevice.AllDevices.FindAll(UsbFinder);

            if (regList.Count == 0) return null;

            foreach (UsbRegistry regDevice in regList)
                serials.Add(regDevice.Device.Info.SerialString);

            return serials;
        }

        /// <summary>
        /// Connect to the device
        /// </summary>
        /// <returns></returns>
        public bool Connect()
        {
            return Connect(String.Empty);
        }

        /// <summary>
        /// Function to connect to the device.
        /// </summary>
        /// <param name="serial">Connect to specific device with serial (optional)</param>
        /// <returns></returns>
        public bool Connect(string serial)
        {
            if (!String.IsNullOrEmpty(serial))
                device = UsbDevice.OpenUsbDevice(new UsbDeviceFinder(serial));
            else
                device = UsbDevice.OpenUsbDevice(UsbFinder);

            if (device == null)
                throw new DeviceNotFoundException();

            IUsbDevice usbDev = device as IUsbDevice;
            if (!ReferenceEquals(usbDev, null))
            {
                usbDev.SetConfiguration(1);
                usbDev.ClaimInterface(0);
            }

            return true;
        }

        /// <summary>
        /// True if the device is currently connected, false if not
        /// </summary>
        public bool Connected
        {
            get
            {
                return (device != null && device.IsOpen);
            }
        }

        /// <summary>
        /// The serial number for the currently connected device
        /// </summary>
        public String Serial
        {
            get { return device.Info.SerialString; }
        }

        /// <summary>
        /// The firmware version of the currently connected device
        /// </summary>
        public Version FirmwareVersion
        {
            get { return new Version(device.Info.Descriptor.BcdDevice >> 8, 0xff & device.Info.Descriptor.BcdDevice); }
        }

        /// <summary>
        /// Disconnect from the currently connected device
        /// </summary>
        public void Disconnect()
        {
            device.Close();
        }

        /// <summary>
        /// Issues a move command to the focuser
        /// </summary>
        /// <param name="position">Absolute position to move to</param>
        public void MoveTo(UInt16 position)
        {
            UsbSetupPacket packet = new UsbSetupPacket((byte)UsbRequestType.TypeVendor | (byte)UsbRequestRecipient.RecipDevice | (byte)UsbEndpointDirection.EndpointOut, (byte)Request.MoveTo, (short)position, 0, 0);

            int transfered;
            device.ControlTransfer(ref packet, null, 0, out transfered);
        }

        /// <summary>
        /// Issues a halt command to the focuser
        /// </summary>
        public void Halt()
        {
            UsbSetupPacket packet = new UsbSetupPacket((byte)UsbRequestType.TypeVendor | (byte)UsbRequestRecipient.RecipDevice | (byte)UsbEndpointDirection.EndpointOut, (byte)Request.Halt, 0, 0, 0);

            int transfered;
            device.ControlTransfer(ref packet, null, 0, out transfered);
        }

        /// <summary>
        /// Reboots the focuser into the bootloader for uploading new firmware. Don't use this if you don't know what you're doing.
        /// </summary>
        public void RebootToBootloader()
        {
            UsbSetupPacket packet = new UsbSetupPacket((byte)UsbRequestType.TypeVendor | (byte)UsbRequestRecipient.RecipDevice | (byte)UsbEndpointDirection.EndpointOut, (byte)Request.RebootToBootloader, 0, 0, 0);

            int transfered;
            device.ControlTransfer(ref packet, null, 0, out transfered);
        }

        /// <summary>
        /// Returns true if moving, false if halted
        /// </summary>
        public bool IsMoving
        {
            get
            {
                UsbSetupPacket packet = new UsbSetupPacket((byte)UsbRequestType.TypeVendor | (byte)UsbRequestRecipient.RecipDevice | (byte)UsbEndpointDirection.EndpointIn, (byte)Request.IsMoving, 0, 0, 0);

                int expected = 1;
                int transfered;
                byte[] buffer = new byte[expected];
                device.ControlTransfer(ref packet, buffer, expected, out transfered);

                if (transfered != expected) throw new CommunicationException();

                return buffer[0] == 0 ? false : true;
            }
        }

        /// <summary>
        /// Either sets the current position reported by the focuser, or gets the current position
        /// </summary>
        public UInt16 Position
        {
            set
            {
                UsbSetupPacket packet = new UsbSetupPacket((byte)UsbRequestType.TypeVendor | (byte)UsbRequestRecipient.RecipDevice | (byte)UsbEndpointDirection.EndpointOut, (byte)Request.SetPosition, (short)value, 0, 0);

                int transfered;
                device.ControlTransfer(ref packet, null, 0, out transfered);
            }
            get
            {
                UsbSetupPacket packet = new UsbSetupPacket((byte)UsbRequestType.TypeVendor | (byte)UsbRequestRecipient.RecipDevice | (byte)UsbEndpointDirection.EndpointIn, (byte)Request.GetPosition, 0, 0, 0);

                int expected = 2;
                int transfered;
                byte[] buffer = new byte[expected];
                device.ControlTransfer(ref packet, buffer, expected, out transfered);

                if (transfered != expected) throw new CommunicationException();

                return (UInt16)((buffer[1] << 8) | buffer[0]);
            }
        }

        public bool Reverse
        {
            set
            {
                UsbSetupPacket packet = new UsbSetupPacket((byte)UsbRequestType.TypeVendor | (byte)UsbRequestRecipient.RecipDevice | (byte)UsbEndpointDirection.EndpointOut, (byte)Request.Reverse, (short)((value)?1:0), 0, 0);

                int transfered;
                device.ControlTransfer(ref packet, null, 0, out transfered);
            }
        }

        /// <summary>
        /// Set to true to enable temperature compensation, false to disable. Returns current temperature compensation status
        /// </summary>
        public bool TempComp
        {
            get
            {
                return TempCompEnabled;
            }
            set
            {
                TempCompEnabled = value;
                TempCompTimer.Enabled = value;
            }
        }

        /// <summary>
        /// Returns current temperature read by the device in degrees kelvin
        /// </summary>
        public double Temperature
        {
            get
            {
                UsbSetupPacket packet = new UsbSetupPacket((byte)UsbRequestType.TypeVendor | (byte)UsbRequestRecipient.RecipDevice | (byte)UsbEndpointDirection.EndpointIn, (byte)Request.GetTemperature, 0, 0, 0);

                int expected = 2;
                int transfered;
                byte[] buffer = new byte[expected];
                device.ControlTransfer(ref packet, buffer, expected, out transfered);

                if (transfered != expected) throw new CommunicationException();

                Int16 adc = (Int16)((buffer[1] << 8) | buffer[0]);
                double kelvin = (5.00 * (double)adc * 100.00) / 1024.00;
                kelvin = Math.Round(kelvin, 2); /* See https://github.com/CortexAstronomy/OpenFocus/wiki/LM335-Information */

                return kelvin;
            }
        }

        /// <summary>
        /// Gets the capabilities byte from the device
        /// </summary>
        private byte Capabilities
        {
            get
            {
                UsbSetupPacket packet = new UsbSetupPacket((byte)UsbRequestType.TypeVendor | (byte)UsbRequestRecipient.RecipDevice | (byte)UsbEndpointDirection.EndpointIn, (byte)Request.GetCapabilities, 0, 0, 0);

                int expected = 1;
                int transfered;
                byte[] buffer = new byte[expected];
                device.ControlTransfer(ref packet, buffer, expected, out transfered);

                if (transfered != expected) throw new CommunicationException();

                return buffer[0];
            }
        }

        /// <summary>
        /// Returns true if the device supports absolute positioning
        /// </summary>
        public bool Absolute
        {
            get { return ((Capabilities & Capability.AbsolutePositioning) == Capability.AbsolutePositioning) ? true : false; }
        }

        /// <summary>
        /// Returns true if the device supports temperature compensation
        /// </summary>
        public bool TempCompAvailable
        {
            get { return ((Capabilities & Capability.TemperatureCompensation) == Capability.TemperatureCompensation) ? true : false; }
        }
        /// <summary>
        /// Temperature coefficient to be used for temperature compensation
        /// </summary>
        public double TemperatureCoefficient = 0.0;

        /// <summary>
        /// <para>This event fires when the timer interval is up</para>
        /// <para>We check to see if the temperature has changed then calculate how far we should move the focuser according to the temperature coefficient</para>
        /// </summary>
        private void TempCompTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            double CurrentTemperature = Temperature;

            if (LastTemperature != 0 && !IsMoving)
            {
                double delta = CurrentTemperature - LastTemperature;
                MoveTo((UInt16)(Position + (TemperatureCoefficient * delta)));

                LastTemperature = CurrentTemperature;
            }
        }

        #endregion
    }
}
