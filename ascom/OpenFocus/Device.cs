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
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

using LibUsbDotNet;
using LibUsbDotNet.Info;
using LibUsbDotNet.Main;

namespace ASCOM.OpenFocus
{
    public struct Capabilities
    {
        public const byte AbsolutePositioning = 0x01;
        public const byte TemperatureCompensation = 0x02;
    }

    public struct Requests
    {
        public const byte MoveTo = 0x00;
        public const byte Halt = 0x01;
        public const byte SetPosition = 0x02;
        public const byte GetPosition = 0x10;
        public const byte IsMoving = 0x11;
        public const byte GetCapabilities = 0x12;
    }

    public class Device
    {
        private static byte _Capabilities;
        private static Int16 Vendor_ID = 0x16c0;
        private static Int16 Product_ID = 0x05df;

        private static UsbDeviceFinder UsbFinder = new UsbDeviceFinder(Vendor_ID, Product_ID);
        private static UsbDevice device;

        #region public methods
        public static bool Connect()
        {
            device = UsbDevice.OpenUsbDevice(UsbFinder);

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

            int transfered;
            byte[] buffer = new byte[1];
            device.ControlTransfer(ref packet, buffer, 1, out transfered);

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

                int transfered;
                byte[] buffer = new byte[1];
                device.ControlTransfer(ref packet, buffer, 1, out transfered);

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

                int transfered;
                byte[] buffer = new byte[2];
                device.ControlTransfer(ref packet, buffer, 2, out transfered);

                return (Int16)((buffer[1] << 8) | buffer[0]);
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
