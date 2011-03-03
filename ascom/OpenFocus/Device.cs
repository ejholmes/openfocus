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

namespace ASCOM.OpenFocus
{
    public enum DeviceError
    {
        NO_ERROR = 0x00,
        DEVICE_NOT_FOUND = 0x01,
        CANT_OPEN_DEVICE = 0x02
    }

    public struct Capabilities
    {
        public const byte AbsolutePositioning = 0x01;
        public const byte TemperatureCompensation = 0x02;
    }

    public class Device
    {
        private const string _DLL = "openfocus.dll";

        private static byte _Capabilities;

        #region private methods
        [DllImport(_DLL, EntryPoint = "focuser_connect")]
        private static extern byte _Connect(string serial);

        [DllImport(_DLL, EntryPoint = "focuser_disconnect")]
        private static extern byte _Disconnect();

        [DllImport(_DLL, EntryPoint = "focuser_get_error")]
        private static extern byte _GetLastError();
        private static DeviceError GetLastError() { return (DeviceError)_GetLastError(); }

        [DllImport(_DLL, EntryPoint = "focuser_is_moving")]
        private static extern byte _IsMoving();

        [DllImport(_DLL, EntryPoint = "focuser_move_to")]
        private static extern void _MoveTo(Int16 position);

        [DllImport(_DLL, EntryPoint = "focuser_halt")]
        private static extern void _Halt();

        [DllImport(_DLL, EntryPoint = "focuser_get_position")]
        private static extern Int16 _GetPosition();

        [DllImport(_DLL, EntryPoint = "focuser_set_position")]
        private static extern Int16 _SetPosition(Int16 position);

        [DllImport(_DLL, EntryPoint = "focuser_get_capabilities")]
        private static extern byte _GetCapabilities();

        private static void HandleError(DeviceError error)
        {
            string start = "Error Code: 0x" + ((byte)error).ToString("X2");
            switch (error)
            {
                case DeviceError.DEVICE_NOT_FOUND:
                    throw new DeviceNotFoundException(start + ": Device not found!");
                default:
                    throw new DeviceException("An error occurred");
            }
        }
        #endregion

        #region public methods
        public static bool Connect()
        {
            DeviceError err = (DeviceError)_Connect(null);

            if (err == DeviceError.NO_ERROR)
            {
                _Capabilities = _GetCapabilities();
                return true;
            }
            else
            {
                HandleError(err);
                return false;
            }
        }

        public static void Disconnect()
        {
            _Disconnect();
        }

        public static void MoveTo(Int16 position)
        {
            _MoveTo(position);

            DeviceError error = GetLastError();

            if (error != DeviceError.NO_ERROR)
                HandleError(error);
        }

        public static void Halt()
        {
            _Halt();
        }

        public static bool IsMoving
        {
            get
            {
                bool rval = (_IsMoving() == 1) ? true : false;

                DeviceError error = GetLastError();

                if (error != DeviceError.NO_ERROR)
                    HandleError(error);

                return rval;
            }
        }

        public static Int16 Position
        {
            set
            {
                _SetPosition(value);
            }
            get
            {
                Int16 rval = _GetPosition();

                DeviceError error = GetLastError();

                if (error != DeviceError.NO_ERROR)
                    HandleError(error);

                return rval;
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

    #region Exception Handlers
    public class DeviceException : Exception
    {
        public DeviceException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public DeviceException(string message)
            : base(message)
        {
        }

        public DeviceException()
            : base()
        {
        }
    }

    public class DeviceNotFoundException : DeviceException
    {
        public DeviceNotFoundException(string message)
            : base(message)
        {
        }
    }
    #endregion
}
