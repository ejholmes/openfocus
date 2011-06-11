using System;
using System.Runtime.InteropServices;

using ASCOM.Interface;

using Cortex;
using Cortex.OpenFocus;

namespace ASCOM.OpenFocus
{
    [Guid("378ee2b3-3ba0-44fe-b382-cd643610814e")]
    [ClassInterface(ClassInterfaceType.None)]
    public class Focuser : IFocuser
    {
        public static string s_csDriverID = "ASCOM.OpenFocus.Focuser";
        public static string s_csDriverDescription = "OpenFocus";
        public static string s_csDeviceType = "Focuser";

        
        public String Serial                = String.Empty;

        private Config.Device DeviceConfig;

        private Device dev = new Device();

        //
        // Constructor - Must be public for COM registration!
        //
        public Focuser()
        {
            Config.Profile.DeviceType = s_csDeviceType;

            
        }

        #region ASCOM Registration
        //
        // Register or unregister driver for ASCOM. This is harmless if already
        // registered or unregistered. 
        //
        private static void RegUnregASCOM(bool bRegister)
        {
            ASCOM.Utilities.Profile P = new ASCOM.Utilities.Profile();
            P.DeviceType = "Focuser";					//  Requires Helper 5.0.3 or later
            if (bRegister)
                P.Register(s_csDriverID, s_csDriverDescription);
            else
                P.Unregister(s_csDriverID);
            try										// In case Helper becomes native .NET
            {
                Marshal.ReleaseComObject(P);
            }
            catch (Exception) { }
            P = null;
        }

        [ComRegisterFunction]
        public static void RegisterASCOM(Type t)
        {
            RegUnregASCOM(true);
        }

        [ComUnregisterFunction]
        public static void UnregisterASCOM(Type t)
        {
            RegUnregASCOM(false);
        }
        #endregion

        //
        // PUBLIC COM INTERFACE IFocuser IMPLEMENTATION
        //

        #region IFocuser Members

        public bool Absolute
        {
            get { return dev.Absolute; }
        }

        public void Halt()
        {
            dev.Halt();
        }

        public bool IsMoving
        {
            get { return dev.IsMoving; }
        }

        public bool Link
        {
            get { return dev.Connected; }
            set
            {
                switch (value)
                {
                    case true:
                        Serial = Config.DefaultDevice;
                        try
                        {
                            dev.Connect(Serial);
                        }
                        catch (DeviceNotFoundException)
                        {
                            dev.Connect();
                        }
                        DeviceConfig = new Config.Device(dev.Serial);
                        if (DeviceConfig.Position != 0)
                            dev.Position = DeviceConfig.Position;
                        break;
                    case false:
                        if (dev.Connected)
                        {
                            if (this.IsMoving)
                                dev.Halt();
                            /* Save the last position before disconnecting */
                            DeviceConfig.Position = dev.Position;
                            dev.Disconnect();
                        }
                        break;
                }
            }
        }

        public int MaxIncrement
        {
            get { return DeviceConfig.MaxPosition; }
        }

        public int MaxStep
        {
            get { return DeviceConfig.MaxPosition; }
        }

        public void Move(int val)
        {
            if (val >= 0)
                dev.MoveTo((UInt16)val);
        }

        public int Position
        {
            get { return dev.Position; }
        }

        public void SetupDialog()
        {
            SetupDialogForm F = new SetupDialogForm();
            F.ShowDialog();
        }

        public double StepSize
        {
            get { return DeviceConfig.StepSize; }
        }

        /* Set to true to enable temperature compensation */
        public bool TempComp
        {
            get { return dev.TempComp; }
            set
            {
                dev.TempComp = value;
                dev.TemperatureCoefficient = DeviceConfig.TemperatureCoefficient;
            }
        }

        /* Asks the device if it can do temperature compensation */
        public bool TempCompAvailable
        {
            get { return dev.TempCompAvailable; }
        }

        /* Gets a temperature reading from the device */
        public double Temperature
        {
            get
            {
                if (!TempCompAvailable)
                    throw new Exception("Temperature compensation is not available");
                double kelvin = dev.Temperature;
                double celsius = kelvin - 273.15;

                if (Config.Units == Config.TemperatureUnits.Celsius)
                    return celsius;
                else if (Config.Units == Config.TemperatureUnits.Fahrenheit)
                    return ((9.00 / 5.00) * celsius) + 32;
                else
                    return celsius;
            }
        }

        #endregion
    }
}
