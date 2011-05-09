/*
 * File: Config.Device.cs
 * Package: OpenFocus ASCOM
 *
 * Copyright (c) 2010 Eric J. Holmes
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 *
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General
 * Public License along with this library; if not, write to the
 * Free Software Foundation, Inc., 59 Temple Place, Suite 330,
 * Boston, MA  02111-1307  USA
 *
 */

/*
 * This file basically abstracts the ASCOM profile class
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace ASCOM.OpenFocus
{
    public partial class Config
    {
        public class Device
        {
            private const UInt16 _MaxPosition                = 10000;
            private const double _TemperatureCoefficient    = 0.0;

            private String Serial;

            public Device(string serial)
            {
                Serial = serial;
                Profile.CreateSubKey(ASCOM.OpenFocus.Focuser.s_csDriverID, Serial);
            }

            public UInt16 MaxPosition
            {
                get
                {
                    String val = Read("MaxPosition", Serial);
                    if (String.IsNullOrEmpty(val))
                        return _MaxPosition;
                    else
                        return UInt16.Parse(val);
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
