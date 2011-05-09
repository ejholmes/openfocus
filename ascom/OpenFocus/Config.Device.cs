using System;
using System.Collections.Generic;
using System.Text;

namespace ASCOM.OpenFocus
{
    public partial class Config
    {
        public class Device
        {
            private const Int16 _MaxPosition                = 10000;
            private const double _TemperatureCoefficient    = 0.0;

            private String Serial;

            public Device(string serial)
            {
                Serial = serial;
                Profile.CreateSubKey(ASCOM.OpenFocus.Focuser.s_csDriverID, Serial);
            }

            public Int16 MaxPosition
            {
                get
                {
                    String val = Read("MaxPosition", Serial);
                    if (String.IsNullOrEmpty(val))
                        return _MaxPosition;
                    else
                        return Int16.Parse(val);
                }
                set { Write("MaxPosition", value.ToString(), Serial); }
            }

            public String Name
            {
                get
                {
                    String val = Read("Name", Serial);
                    if (String.IsNullOrEmpty(val))
                        return Serial;
                    else
                        return val;
                }
                set
                {
                    if (value != Serial) Write("Name", value, Serial);
                }
            }

            public double TemperatureCoefficient
            {
                get
                {
                    String val = Read("TemperatureCoefficient", Serial);
                    if (String.IsNullOrEmpty(val))
                        return _TemperatureCoefficient;
                    else
                        return double.Parse(val);
                }
                set { Write("TemperatureCoefficient", value.ToString(), Serial); }
            }
        }
    }
}
