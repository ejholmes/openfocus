using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

using LibUsbDotNet;

/*
 * Intel 16 byte hex format
 * 
 * Start Code (1) | Byte Count (2) | Address (4) | Record Type (2) | Data (16) | CheckSum (2) | End Line (2)
 * :10010000214601360121470136007EFE09D21901400d0a
 * :100110002146017EB7C20001FF5F160021480119880d0a
 * :10012000194E79234623965778239EDA3F01B2CAA70d0a
 * :100130003F0156702B5E712B722B732146013421C70d0a
 * :00000001FF0d0a
 */

namespace BootloadHID
{
    public class Bootloader
    {
        private class IntelHexLine
        {
            private Byte[] _start = new Byte[1];
            private Byte[] _byte_count = new Byte[2];
            private Byte[] _address = new Byte[4];
            private Byte[] _record_type = new Byte[2];
            private Byte[] _data = new Byte[16];
            private Byte[] _checksum = new Byte[2];
            private Byte[] _end = new Byte[2];

            public Byte[] start
            {
                set { _start = value; }
                get { return _start; }
            }

            public Byte[] byte_count
            {
                set { _byte_count = value; }
                get { return _byte_count; }
            }

            public Byte[] address
            {
                set { _address = value; }
                get { return _address; }
            }

            public Byte[] record_type
            {
                set { _record_type = value; }
                get { return _record_type; }
            }

            public Byte[] data
            {
                set { _data = value; }
                get { return _data; }
            }

            public Byte[] checksum
            {
                set { _checksum = value; }
                get { return _checksum; }
            }

            public Byte[] end
            {
                set { _end = value; }
                get { return _end; }
            }
        };

        private static Byte[] dataBuffer = new Byte[65536 + 256];

        public static int UploadFirmware(String firmware)
        {
            ParseIntelHexFile(firmware);
            return 0;
        }

        private static void ParseIntelHexFile(String file)
        {
            FileStream fp = File.OpenRead(file);
            IntelHexLine line;

            while ((line = ParseLine(fp)) != null)
            {
                int address = ParseHex(line.address);
                for (int i = 0; i < line.data.Length; i+=2)
                {
                    Byte[] b = new Byte[2];
                    b[0] = line.data[i];
                    b[1] = line.data[i+1];
                    dataBuffer[address] = (Byte)ParseHex(b);
                    address++;
                }
            }

            fp.Close();
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

            if (!VerifyChecksum(line))
                throw new Exception("Checksum mismatch!");

            if (ParseHex(line.record_type) == 0x01) /* End of file */
                return null;

            return line;
        }

        private static bool VerifyChecksum(IntelHexLine line)
        {
            int sum = 0;
            sum += ParseHex(line.byte_count);
            sum += ParseHex(line.address);
            sum += ParseHex(line.record_type);

            int address = ParseHex(line.address);
            for (int i = 0; i < line.data.Length; i += 2)
            {
                Byte[] b = new Byte[2];
                b[0] = line.data[i];
                b[1] = line.data[i + 1];
                sum += (Byte)ParseHex(b);
                address++;
            }

            int twos_comp = (~sum + 1) & 0xff;
            int checksum = ParseHex(line.checksum);

            if (twos_comp != checksum)
                return false;
            else
                return true;
        }

        private static int ParseHex(Byte[] bytes)
        {
            return ParseHex(bytes, bytes.Length);
        }

        private static int ParseHex(Byte[] bytes, int count)
        {
            Char[] temp = new Char[bytes.Length];
            for (int i = 0; i < bytes.Length; i++)
                temp[i] = (Char)bytes[i];
            return Int32.Parse(new String(temp), System.Globalization.NumberStyles.HexNumber);
        }
    }
}
