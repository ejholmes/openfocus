/*
 * File: Device.cs
 * Package: OpenFocus ASCOM
 *
 * Copyright (c) 2010 Eric J. Holmes
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 *
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General
 * Public License along with this library; if not, write to the
 * Free Software Foundation, Inc., 59 Temple Place, Suite 330,
 * Boston, MA  02111-1307  USA
 *
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

using LibUsbDotNet;
using LibUsbDotNet.Info;
using LibUsbDotNet.Main;

using ASCOM.Utilities;

namespace ASCOM.OpenFocus
{
    public class Device
    {
        private struct Capabilities
        {
            public const byte AbsolutePositioning       = 0x01;
            public const byte TemperatureCompensation   = 0x02;
        }

        private struct Requests
        {
            public const byte MoveTo                        = 0x00;
            public const byte Halt                          = 0x01;
            public const byte SetPosition                   = 0x02;
            public const byte SetTemperatureCompensation    = 0x03;
            public const byte GetPosition                   = 0x10;
            public const byte IsMoving                      = 0x11;
            public const byte GetCapabilities               = 0x12;
            public const byte GetTemperature                = 0x13;
        }

        public struct TemperatureUnits
        {
            public const string Celsius = "0";
            public const string Fahrenheit = "1";
        }

        private static byte _Capabilities;
        private static Int16 Vendor_ID              = 0x16c0;
        private static Int16 Product_ID             = 0x05df;

        private static bool TempCompEnabled         = false;

        private static UsbDeviceFinder UsbFinder = new UsbDeviceFinder(Vendor_ID, Product_ID);
        private static UsbDevice device;

        public static String Serial = String.Empty;

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
            if (String.IsNullOrEmpty(Serial))
                device = UsbDevice.OpenUsbDevice(UsbFinder);
            else
                device = UsbDevice.OpenUsbDevice(new UsbDeviceFinder(Serial));

            if (device == null) throw new Exception("Device Not Found");

            IUsbDevice usbDev = device as IUsbDevice;
            if (!ReferenceEquals(usbDev, null))
            {
                usbDev.SetConfiguration(1);
                usbDev.ClaimInterface(0);
            }

            GetCapabilities();

            return true;
        }

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

        public static void Disconnect()
        {
            device.Close();
        }

        public static void MoveTo(Int16 position)
        {
            UsbSetupPacket packet = new UsbSetupPacket((byte)UsbRequestType.TypeVendor | (byte)UsbRequestRecipient.RecipDevice | (byte)UsbEndpointDirection.EndpointOut, (byte)Requests.MoveTo, (short)position, 2, 1);

            int transfered;
            object buffer = null;
            device.ControlTransfer(ref packet, buffer, 0, out transfered); 
        }

        public static void Halt()
        {
            UsbSetupPacket packet = new UsbSetupPacket((byte)UsbRequestType.TypeVendor | (byte)UsbRequestRecipient.RecipDevice | (byte)UsbEndpointDirection.EndpointOut, (byte)Requests.Halt, 0, 0, 0);

            int transfered;
            object buffer = null;
            device.ControlTransfer(ref packet, buffer, 0, out transfered);
        }

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

        public static Int16 Position
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

                return (Int16)((buffer[1] << 8) | buffer[0]);
            }
        }

        public static bool TempComp
        {
            get
            {
                return TempCompEnabled;
            }
            set
            {
                UsbSetupPacket packet = new UsbSetupPacket((byte)UsbRequestType.TypeVendor | (byte)UsbRequestRecipient.RecipDevice | (byte)UsbEndpointDirection.EndpointOut, (byte)Requests.SetTemperatureCompensation, (short)((value)?1:0), 1, 1);

                int transfered;
                object buffer = null;
                device.ControlTransfer(ref packet, buffer, 0, out transfered); 

                TempCompEnabled = value;
            }
        }

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

                string units = ASCOM.OpenFocus.Focuser.Profile.GetValue(ASCOM.OpenFocus.Focuser.s_csDriverID, "Units");

                Int16 adc = (Int16)((buffer[1] << 8) | buffer[0]);
                double kelvin = (5.00 * (double)adc * 100.00) / 1024.00;
                double celsius = kelvin - 273.15;

                if (units == Device.TemperatureUnits.Celsius)
                    return celsius;
                else if (units == Device.TemperatureUnits.Fahrenheit)
                    return ((9.00/5.00) * celsius) + 32;
                else
                    return celsius;
            }
        }

        public static bool Absolute
        {
            get { return ((_Capabilities & Capabilities.AbsolutePositioning) == Capabilities.AbsolutePositioning) ? true : false; }
        }

        public static bool TempCompAvailable
        {
            get { return ((_Capabilities & Capabilities.TemperatureCompensation) == Capabilities.TemperatureCompensation) ? true : false; }
        }
        #endregion
    }
}
