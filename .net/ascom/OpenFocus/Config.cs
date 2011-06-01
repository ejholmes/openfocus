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
