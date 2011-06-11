using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cortex
{
    public class DeviceNotFoundException : Exception
    {
        public DeviceNotFoundException() : base("Device not found") { }
        public DeviceNotFoundException(string errorMessage) : base(errorMessage) { }
    }

    public class CommunicationException : Exception
    {
        public CommunicationException() : base("Error communicating with the device") { }
        public CommunicationException(string errorMessage) : base(errorMessage) { }
    }

    public class ChecksumMismatchException : Exception
    {
        public ChecksumMismatchException() : base("Checksum mismatch") { }
        public ChecksumMismatchException(string errorMessage) : base(errorMessage) { }
    }
}
