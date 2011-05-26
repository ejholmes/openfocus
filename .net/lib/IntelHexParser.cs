using System;
using System.IO;

/* 
 * Intel 16 byte hex format
 * Start Code (1) | Byte Count (2) | Address (4) | Record Type (2) | Data (16) | CheckSum (2) | End Line (2)
 */

namespace Cortex
{
    public class IntelHexParser
    {
        public class IntelHexLine
        {
            public Byte[] start = new Byte[1];
            public Byte[] byte_count = new Byte[2];
            public Byte[] address = new Byte[4];
            public Byte[] record_type = new Byte[2];
            public Byte[] data = new Byte[16];
            public Byte[] checksum = new Byte[2];
            public Byte[] end = new Byte[2];

            public override string ToString()
            {
                return base.ToString();
            }
        }

        private static Byte[] dataBuffer = new Byte[65536 + 256];

        public static Byte[] ParseFile(String file, uint pagesize)
        {
            FileStream fp = File.OpenRead(file);
            IntelHexLine line;
            int address = 0;

            while ((line = ParseLine(fp)) != null)
            {
                address = ParseHex(line.address);
                for (int i = 0; i < line.data.Length; i += 2)
                {
                    Byte[] b = new Byte[2];
                    b[0] = line.data[i];
                    b[1] = line.data[i + 1];
                    dataBuffer[address] = (Byte)ParseHex(b);
                    address++;
                }
            }

            fp.Close();

            uint mask = pagesize - 1;
            int endaddress = (int)((address + mask) & ~mask);

            Byte[] returnBuffer = new Byte[endaddress];
            Buffer.BlockCopy(dataBuffer, 0, returnBuffer, 0, endaddress);

            return returnBuffer;
        }

        private static IntelHexLine ParseLine(FileStream fp)
        {
            IntelHexLine line = new IntelHexLine();

            fp.Read(line.start, 0, line.start.Length); /* Read Start */
            fp.Read(line.byte_count, 0, line.byte_count.Length); /* Data Count */
            fp.Read(line.address, 0, line.address.Length); /* Read Address */
            fp.Read(line.record_type, 0, line.record_type.Length); /* Read record type */

            line.data = new Byte[ParseHex(line.byte_count) * 2];
            fp.Read(line.data, 0, ParseHex(line.byte_count) * 2); /* Read Data */

            fp.Read(line.checksum, 0, line.checksum.Length); /* Read Checksum */
            fp.Read(line.end, 0, line.end.Length); /* Read End of line */

            VerifyChecksum(line);

            if (ParseHex(line.record_type) == 0x01) /* End of file */
                return null;

            return line;
        }

        private static void VerifyChecksum(IntelHexLine line)
        {
            int sum = 0;
            sum += ParseHex(line.byte_count);

            /* Split address into bytes */
            for (int i = 0; i < line.address.Length; i += 2)
            {
                sum += (Byte)ParseHex(new Byte[2] {
                    line.address[i], 
                    line.address[i + 1]
                });
            }

            sum += ParseHex(line.record_type);

            /* Split data in bytes */
            for (int i = 0; i < line.data.Length; i += 2)
            {
                sum += (Byte)ParseHex(new Byte[2] {
                    line.data[i],
                    line.data[i + 1]
                });
            }

            int twos_comp = (~sum + 1) & 0xff;
            int checksum = ParseHex(line.checksum);

            if (twos_comp != checksum)
                throw new ChecksumMismatchException("Checksum mismatch!", line);
        }

        private static int ParseHex(Byte[] bytes)
        {
            return ParseHex(bytes, bytes.Length);
        }

        private static int ParseHex(Byte[] bytes, int count)
        {
            Char[] temp = new Char[count];
            for (int i = 0; i < bytes.Length; i++)
                temp[i] = (Char)bytes[i];
            return Int32.Parse(new String(temp), System.Globalization.NumberStyles.HexNumber);
        }
    }

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