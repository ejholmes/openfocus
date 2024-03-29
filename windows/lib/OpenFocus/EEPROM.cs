﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cortex.OpenFocus
{
    public class EEPROM
    {
        private struct Addresses
        {
            public const ushort StayInBootloader        = 0x0000;
            public const ushort SerialNumberLen         = 0x0040;
            public const ushort SerialNumber            = SerialNumberLen + 1;
        }

        private EEPROMVar _StayInBootloader, _SerialNumberLen, _SerialNumber;

        public EEPROM()
        {
            _StayInBootloader = new EEPROMVar(Addresses.StayInBootloader, (Byte)0);
            _SerialNumberLen = new EEPROMVar(Addresses.SerialNumberLen, String.Empty);
            _SerialNumber = new EEPROMVar(Addresses.SerialNumber, (Byte)0);
        }

        public EEPROM(Byte[] data)
        {
            Byte stayInBootloader = data[Addresses.StayInBootloader];
            _StayInBootloader = new EEPROMVar(Addresses.StayInBootloader, stayInBootloader);

            Byte serialNumberLen = data[Addresses.SerialNumberLen];
            _SerialNumberLen = new EEPROMVar(Addresses.SerialNumberLen, (Byte)serialNumberLen);

            Byte[] serialNumberBytes = new Byte[(serialNumberLen + 1) * 2];
            Buffer.BlockCopy(data, Addresses.SerialNumber, serialNumberBytes, 0, (serialNumberLen + 1) * 2);

            String serialNumber = Encoding.Unicode.GetString(serialNumberBytes);
            _SerialNumber = new EEPROMVar(Addresses.SerialNumber, serialNumber);
        }

        public bool StayInBootloader
        {
            get { return (((Byte)_StayInBootloader.Data) == 1); }
            set { _StayInBootloader.Data = (Byte)(value ? 1 : 0); }
        }

        public String SerialNumber
        {
            get { return (String)_SerialNumber.Data; }
            set { _SerialNumber.Data = value; _SerialNumberLen.Data = (Byte)value.Length; }
        }

        public Byte[] Data
        {
            get
            {
                Byte[] data = new Byte[_SerialNumber.Address + ((Byte)_SerialNumberLen.Data * 2)];

                data[_StayInBootloader.Address] = (Byte)_StayInBootloader.Data;

                
                data[_SerialNumberLen.Address] = (Byte)_SerialNumberLen.Data;
                Byte[] serialNumberBytes = Encoding.Unicode.GetBytes(((String)_SerialNumber.Data).ToCharArray());
                Buffer.BlockCopy(serialNumberBytes, 0, data, _SerialNumber.Address, serialNumberBytes.Length);

                return data;
            }
        }

    }

    internal class EEPROMVar
    {
        public EEPROMVar(ushort address, object data)
        {
            Address = address;
            Data = data;
        }

        public ushort Address;
        public object Data;
    }
}
