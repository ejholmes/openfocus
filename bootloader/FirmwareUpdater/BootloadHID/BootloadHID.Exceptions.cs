using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BootloadHID
{
    class ChecksumMismatchException : Exception
    {
        private IntelHexParser.IntelHexLine _Line = new IntelHexParser.IntelHexLine();

        public ChecksumMismatchException(string errorMessage) : base(errorMessage) { }

        public ChecksumMismatchException(string errorMessage, IntelHexParser.IntelHexLine line)
            : base(errorMessage)
        {
            _Line = line;
        }

        public IntelHexParser.IntelHexLine Line
        {
            get { return _Line; }
        }
    }
}
