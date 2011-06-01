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
        private IntelHexParser.IntelHexLine _Line = new IntelHexParser.IntelHexLine();
        private int _TowsComplement;

        public ChecksumMismatchException(string errorMessage) : base(errorMessage) { }

        public ChecksumMismatchException(string errorMessage, IntelHexParser.IntelHexLine line)
            : base(errorMessage)
        {
            _Line = line;
        }

        public ChecksumMismatchException(string errorMessage, IntelHexParser.IntelHexLine line, int towsComplement)
            : base(errorMessage)
        {
            _Line = line;
            _TowsComplement = towsComplement;
        }

        public IntelHexParser.IntelHexLine Line
        {
            get { return _Line; }
        }

        public int TwosComplement
        {
            get { return _TowsComplement; }
        }
    }
}
