using System;
using System.Text;
using System.IO;
using System.Collections.Generic;

/* 
 * Intel 16 byte hex format
 * Start Code (1) | Byte Count (2) | Address (4) | Record Type (2) | Data (16) | CheckSum (2) | End Line (2)
 */

namespace Cortex
{
    public class IntelHex
    {
        private enum RecordType
        {
            Data = 0x00,
            EndOfFile = 0x01,
            ExtendedSegmentAddress = 0x02,
            StartSegmentAddress = 0x03,
            ExtendedLinearAddress = 0x04,
            StartLinearAddres = 0x05
        }

        private class Line
        {
            public Byte Start               = (Byte)':';
            public Byte ByteCount           = 0x10;
            public UInt16 Address           = 0x0000;
            public RecordType RecordType    = RecordType.Data;
            public Byte[] Data;
            public Byte CheckSum            = 0x00;
            public Byte End                 = (Byte)'\n';

            public override string ToString()
            {
                StringBuilder sb = new StringBuilder();
                sb.Append((Char)Start);
                sb.Append(String.Format("{0:X2}", ByteCount));
                sb.Append(String.Format("{0:X4}", Address));
                sb.Append(String.Format("{0:X2}", (Byte)RecordType));
                for (int i = 0; i < Data.Length; i++)
                {
                    sb.Append(String.Format("{0:X2}", Data[i]));
                }
                sb.Append(String.Format("{0:X2}", CheckSum));
                sb.Append((Char)End);

                return sb.ToString();
            }
        }

        private static Byte[] dataBuffer = new Byte[65536 + 256];

        public static Byte[] Parse(String file, uint pagesize)
        {
            FileStream fp = File.OpenRead(file);
            Line line;
            int address = 0;

            line = ParseLine(fp); 
            while (line.RecordType != RecordType.EndOfFile)
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

        public static string Create(Byte[] data, int ByteCount)
        {
            StringBuilder sb = new StringBuilder();
            List<Line> lines = new List<Line>();
            Line current;

            for (int i = 0;;)
            {
                current = new Line();
                if (i == data.Length)
                {
                    break;
                }
                else if ((data.Length - i) < ByteCount) /* Last Record */
                {
                    current.ByteCount = (Byte)(data.Length - i);
                    current.Address = (UInt16)i;
                    current.RecordType = RecordType.EndOfFile;

                    current.Data = new Byte[data.Length - i];
                    Buffer.BlockCopy(data, i, current.Data, 0, data.Length - i);

                    current.CheckSum = (Byte)TwosCompliment(current);

                    lines.Add(current);

                    break;
                }
                else
                {
                    current.ByteCount = (Byte)ByteCount;
                    current.Address = (UInt16)i;

                    current.Data = new Byte[ByteCount];
                    Buffer.BlockCopy(data, i, current.Data, 0, ByteCount);

                    current.CheckSum = (Byte)TwosCompliment(current);

                    lines.Add(current);
                }

                
                i += ByteCount;
            }

            foreach (Line l in lines)
            {
                sb.Append(l.ToString());
            }

            return sb.ToString();
        }

        private static Line ParseLine(FileStream fp)
        {
            Line line = new Line();

            fp.Seek(1, SeekOrigin.Current); /* Read Start */
            line.ByteCount = (Byte)ReadBytes(fp, 2);
            line.Address = (UInt16)ReadBytes(fp, 4);
            line.RecordType = (RecordType)ReadBytes(fp, 2);

            line.Data = new Byte[line.ByteCount];
            for (int i = 0; i < line.ByteCount; i++)
                line.Data[i] = (Byte)ReadBytes(fp, 2);

            line.CheckSum = (Byte)ReadBytes(fp, 2);
            fp.Seek(2, SeekOrigin.Current); /* Read End */

            VerifyChecksum(line);

            return line;
        }

        private static int TwosCompliment(Line line)
        {
            int sum = 0;
            sum += line.ByteCount;
            sum += (line.Address & 0xff) + ((line.Address >> 8) & 0xff);
            sum += (Byte)line.RecordType;

            for (int i = 0; i < line.Data.Length; i++)
            {
                sum += line.Data[i];
            }

            int twos_comp = (~sum + 1) & 0xff;

            return twos_comp;
        }

        private static void VerifyChecksum(Line line)
        {
            if (TwosCompliment(line) != line.CheckSum)
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