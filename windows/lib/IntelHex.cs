using System;
using System.IO;

/* 
 * Intel 16 byte hex format
 * Start Code (1) | Byte Count (2) | Address (4) | Record Type (2) | Data (16) | CheckSum (2) | End Line (2)
 */

namespace Cortex
{
    public class IntelHex
    {
        private struct Line
        {
            public Byte ByteCount;
            public UInt16 Address;
            public Byte RecordType;
            public Byte[] Data;
            public Byte CheckSum;
        }

        private static Byte[] dataBuffer = new Byte[65536 + 256];

        public static Byte[] ParseFile(String file, uint pagesize)
        {
            FileStream fp = File.OpenRead(file);
            Line line;
            int address = 0;

            line = ParseLine(fp); 
            while (line.RecordType != 0x01)
            {
                address = line.Address;
                for (int i = 0; i < line.Data.Length; i++)
                {
                    dataBuffer[address] = line.Data[i];
                    address++;
                }
                line = ParseLine(fp);
            }

            fp.Close();

            uint mask = pagesize - 1;
            int endaddress = (int)((address + mask) & ~mask);

            Byte[] returnBuffer = new Byte[endaddress];
            Buffer.BlockCopy(dataBuffer, 0, returnBuffer, 0, endaddress);

            return returnBuffer;
        }

        private static Line ParseLine(FileStream fp)
        {
            Line line = new Line();

            fp.Seek(1, SeekOrigin.Current); /* Read Start */
            line.ByteCount = (Byte)ReadBytes(fp, 2);
            line.Address = (UInt16)ReadBytes(fp, 4);
            line.RecordType = (Byte)ReadBytes(fp, 2);

            line.Data = new Byte[line.ByteCount];
            for (int i = 0; i < line.ByteCount; i++)
                line.Data[i] = (Byte)ReadBytes(fp, 2);

            line.CheckSum = (Byte)ReadBytes(fp, 2);
            fp.Seek(2, SeekOrigin.Current); /* Read End */

            VerifyChecksum(line);

            return line;
        }

        private static void VerifyChecksum(Line line)
        {
            int sum = 0;
            sum += line.ByteCount;
            sum += (line.Address & 0xff) + ((line.Address >> 8) & 0xff);
            sum += line.RecordType;

            for (int i = 0; i < line.Data.Length; i++)
            {
                sum += line.Data[i];
            }

            int twos_comp = (~sum + 1) & 0xff;
            int checksum = line.CheckSum;

            if (twos_comp != checksum)
                throw new ChecksumMismatchException("Checksum mismatch!");
        }

        private static int ReadBytes(FileStream fp, int length)
        {
            Byte[] b = new Byte[length];
            fp.Read(b, 0, b.Length);
            return ParseHex(b);
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
}