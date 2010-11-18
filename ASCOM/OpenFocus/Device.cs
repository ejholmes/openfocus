using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace ASCOM.OpenFocus
{
    public enum DeviceError
    {
        NO_ERROR = 0x00,
        UNDEFINED_ERROR = 0x01,
        DEVICE_NOT_FOUND_ERROR = 0x02,
        INVALID_RESOURCE_ERROR = 0x03,
        USBOPEN_ACCESS_ERROR = 0x04,
        COMMUNICATION_ERROR = 0x05
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
        private static extern byte _Connect();

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

        [DllImport(_DLL, EntryPoint = "focuser_get_capabilities")]
        private static extern byte _GetCapabilities();

        private static void HandleError(DeviceError error)
        {
            string start = "Error Code: 0x" + ((byte)error).ToString("X2");
            switch (error)
            {
                case DeviceError.DEVICE_NOT_FOUND_ERROR:
                    throw new DeviceNotFoundException(start + ": Device not found!");
                case DeviceError.COMMUNICATION_ERROR:
                    throw new CommunicationException(start + ": Could not communication with device! Has it been disconnected?");
                default:
                    throw new DeviceException();
            }
        }
        #endregion

        #region public methods
        public static bool Connect()
        {
            DeviceError err = (DeviceError)_Connect();

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
                bool rval = (_IsMoving() == 0) ? true : false;

                DeviceError error = GetLastError();

                if (error != DeviceError.NO_ERROR)
                    HandleError(error);

                return rval;
            }
        }

        public static Int16 Position
        {
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

    public class InvalidResourceException : DeviceException
    {
        public InvalidResourceException(string message)
            : base(message)
        {
        }
    }

    public class CommunicationException : DeviceException
    {
        public CommunicationException(string message)
            : base(message)
        {
        }
    }
    #endregion
}
