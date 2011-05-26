/*
 * File: Config.cs
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

using ASCOM.Utilities;

using Cortex;
using Cortex.OpenFocus;

namespace ASCOM.OpenFocus
{
    public partial class Config
    {
        public static Profile Profile = new Profile();

        #region Default Values

        private static String _Units                    = Cortex.OpenFocus.Device.TemperatureUnits.Celsius;
        private static String _DefaultDevice            = String.Empty;

        #endregion


        #region Public Static Members

        public static String Units
        {
            get
            {
                String val = Read("Units");
                if (String.IsNullOrEmpty(val))
                    return _Units;
                else
                    return val;
            }
            set { Write("Units", value.ToString()); }
        }

        public static String DefaultDevice
        {
            get
            {
                String val = Read("DefaultDevice");
                if (String.IsNullOrEmpty(val))
                    return _DefaultDevice;
                else
                    return val;
            }
            set { Write("DefaultDevice", value); }
        }

        public static UInt16 Position
        {
            get
            {
                String val = Read("Position");
                if (String.IsNullOrEmpty(val))
                    return 0;
                else
                    return UInt16.Parse(val);
            }
            set { Write("Position", value.ToString()); }
        }

        public static Version Version
        {
            get { return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version; }
        }

        #endregion


        #region Private Profile Abstraction Functions

        private static void Write(string name, string value)
        {
            Profile.WriteValue(ASCOM.OpenFocus.Focuser.s_csDriverID, name, value);
        }

        private static void Write(string name, string value, string subkey)
        {
            Profile.WriteValue(ASCOM.OpenFocus.Focuser.s_csDriverID, name, value, subkey);
        }

        private static String Read(String name)
        {
            return Profile.GetValue(ASCOM.OpenFocus.Focuser.s_csDriverID, name);
        }

        private static String Read(String name, String subkey)
        {
            return Profile.GetValue(ASCOM.OpenFocus.Focuser.s_csDriverID, name, subkey);
        }

        #endregion
    }
}
