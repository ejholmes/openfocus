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
        private IntelHex.IntelHexLine _Line = new IntelHex.IntelHexLine();
        private int _TowsComplement;

        public ChecksumMismatchException(string errorMessage) : base(errorMessage) { }

        public ChecksumMismatchException(string errorMessage, IntelHex.IntelHexLine line)
            : base(errorMessage)
        {
            _Line = line;
        }

        public ChecksumMismatchException(string errorMessage, IntelHex.IntelHexLine line, int towsComplement)
            : base(errorMessage)
        {
            _Line = line;
            _TowsComplement = towsComplement;
        }

        public IntelHex.IntelHexLine Line
        {
            get { return _Line; }
        }

        public int TwosComplement
        {
            get { return _TowsComplement; }
        }
    }
}
