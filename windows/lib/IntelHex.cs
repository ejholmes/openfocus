﻿using System;
using System.Text;
using System.IO;
using System.Collections.Generic;

/* 
 * Intel 16 byte hex format
 * Start Code (1) | Byte Count (2) | Address (4) | Record Type (2) | Data (32) | CheckSum (2) | End Line (2)
 */

namespace Cortex
{
    public class IntelHexFile
    {
        private List<IntelHexFileLine> _Lines = new List<IntelHexFileLine>();
        private uint _PageSize = 128;

        public static IntelHexFile Open(String file)
        {
            FileStream fstream = File.OpenRead(file);
            MemoryStream fp = new MemoryStream();
            fp.SetLength(fstream.Length);
            fstream.Read(fp.GetBuffer(), 0, (int)fstream.Length);
            fstream.Close();
            return Open(fp);
        }

        private static IntelHexFile Open(MemoryStream fp)
        {
            IntelHexFile file = new IntelHexFile();
            IntelHexFileLine current;

            current = file.ParseLine(fp);
            while (current.RecordType != RecordType.EndOfFile)
            {
                file.AddLine(current);
                current = file.ParseLine(fp);
            }

            fp.Close();

            return file;
        }

        public static IntelHexFile Create(Byte[] data, int ByteCount)
        {
            IntelHexFile file = new IntelHexFile();
            IntelHexFileLine current;

            for (int i = 0; ; )
            {
                current = new IntelHexFileLine();
                if (i == data.Length)
                {
                    goto EndOfFile;
                }
                else if ((data.Length - i) < ByteCount) /* Last Record */
                {
                    current.ByteCount = (Byte)(data.Length - i);
                    current.Address = (UInt16)i;

                    current.Data = new Byte[data.Length - i];
                    Buffer.BlockCopy(data, i, current.Data, 0, data.Length - i);

                    current.CheckSum = (Byte)TwosCompliment(current);

                    file.AddLine(current);

                    goto EndOfFile;
                }
                else
                {
                    current.ByteCount = (Byte)ByteCount;
                    current.Address = (UInt16)i;

                    current.Data = new Byte[ByteCount];
                    Buffer.BlockCopy(data, i, current.Data, 0, ByteCount);

                    current.CheckSum = (Byte)TwosCompliment(current);

                    file.AddLine(current);
                }


                i += ByteCount;
            }

EndOfFile: /* End of hex file */
            current = new IntelHexFileLine();
            current.ByteCount = 0;
            current.Address = 0;
            current.RecordType = RecordType.EndOfFile;
            current.Data = new Byte[0];
            current.CheckSum = (Byte)TwosCompliment(current);
            file.AddLine(current);

            return file;
        }

        private IntelHexFileLine ParseLine(MemoryStream fp)
        {
            IntelHexFileLine line = new IntelHexFileLine();

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

        private static int TwosCompliment(IntelHexFileLine line)
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

        private void VerifyChecksum(IntelHexFileLine line)
        {
            if (TwosCompliment(line) != line.CheckSum)
                throw new ChecksumMismatchException("Checksum mismatch!");
        }

        private int ReadBytes(MemoryStream fp, int length)
        {
            Byte[] b = new Byte[length];
            fp.Read(b, 0, b.Length);
            return ParseHex(b);
        }

        private int ParseHex(Byte[] bytes)
        {
            return ParseHex(bytes, bytes.Length);
        }

        private int ParseHex(Byte[] bytes, int count)
        {
            Char[] temp = new Char[count];
            for (int i = 0; i < bytes.Length; i++)
                temp[i] = (Char)bytes[i];
            return Int32.Parse(new String(temp), System.Globalization.NumberStyles.HexNumber);
        }

        Byte[] dataBuffer = new Byte[65536 + 256];
        int address = 0;

        public void AddLine(IntelHexFileLine line)
        {
            _Lines.Add(line);

            address = line.Address;
            for (int i = 0; i < line.Data.Length; i++)
            {
                dataBuffer[address] = line.Data[i];
                address++;
            }
        }

        public List<IntelHexFileLine> Lines
        {
            get { return _Lines; }
        }

        public Byte[] Data
        {
            get
            {
                uint mask = _PageSize - 1;
                int endaddress = (int)((address + mask) & ~mask);

                Byte[] returnBuffer = new Byte[endaddress];
                Buffer.BlockCopy(dataBuffer, 0, returnBuffer, 0, endaddress);

                return returnBuffer;
            }
        }

        public uint PageSize
        {
            get { return _PageSize; }
            set { _PageSize = value; }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (IntelHexFileLine line in _Lines)
                sb.Append(line.ToString());
            return sb.ToString();
        }
    }

    public enum RecordType
    {
        Data = 0x00,
        EndOfFile = 0x01,
        ExtendedSegmentAddress = 0x02,
        StartSegmentAddress = 0x03,
        ExtendedLinearAddress = 0x04,
        StartLinearAddres = 0x05
    }

    /* Structure for an intel hex line in an intel hex file */
    public class IntelHexFileLine
    {
        public Byte Start = (Byte)':';
        public Byte ByteCount = 0x10;
        public UInt16 Address = 0x0000;
        public RecordType RecordType = RecordType.Data;
        public Byte[] Data;
        public Byte CheckSum = 0x00;
        public Byte End = (Byte)'\n';

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
}