using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cortex
{
    public class DeviceNotFoundException : Exception
    {
        public DeviceNotFoundException(string errorMessage) : base(errorMessage) { }
    }

    public class CommunicationException : Exception
    {
        public CommunicationException(string errorMessage) : base(errorMessage) { }
    }

    public class ChecksumMismatchException : Exception
    {
        public ChecksumMismatchException(string errorMessage) : base(errorMessage) { }
    }
}
