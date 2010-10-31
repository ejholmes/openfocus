//tabs=4
// --------------------------------------------------------------------------------
// TODO fill in this information for your driver, then remove this line!
//
// ASCOM Focuser driver for Hypnofocus
//
// Description:	Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam 
//				nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam 
//				erat, sed diam voluptua. At vero eos et accusam et justo duo 
//				dolores et ea rebum. Stet clita kasd gubergren, no sea takimata 
//				sanctus est Lorem ipsum dolor sit amet.
//
// Implements:	ASCOM Focuser interface version: 1.0
// Author:		(XXX) Your N. Here <your@email.here>
//
// Edit Log:
//
// Date			Who	Vers	Description
// -----------	---	-----	-------------------------------------------------------
// dd-mmm-yyyy	XXX	1.0.0	Initial edit, from ASCOM Focuser Driver template
// --------------------------------------------------------------------------------
//
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

using ASCOM;
using ASCOM.Helper;
using ASCOM.Helper2;
using ASCOM.Interface;

namespace ASCOM.Hypnofocus
{
    //
    // Your driver's ID is ASCOM.Hypnofocus.Focuser
    //
    // The Guid attribute sets the CLSID for ASCOM.Hypnofocus.Focuser
    // The ClassInterface/None addribute prevents an empty interface called
    // _Focuser from being created and used as the [default] interface
    //
    [Guid("8d3f2e80-ec30-4216-9747-0f7024c5f12a")]
    [ClassInterface(ClassInterfaceType.None)]
    public class Focuser : IFocuser
    {
        //
        // Driver ID and descriptive string that shows in the Chooser
        //
        private static string s_csDriverID = "ASCOM.Hypnofocus.Focuser";
        private static string s_csDriverDescription = "Hypnofocus";

        private bool _Link = false;

        //
        // Constructor - Must be public for COM registration!
        //
        public Focuser()
        {

        }

        #region ASCOM Registration
        //
        // Register or unregister driver for ASCOM. This is harmless if already
        // registered or unregistered. 
        //
        private static void RegUnregASCOM(bool bRegister)
        {
            Helper.Profile P = new Helper.Profile();
            P.DeviceTypeV = "Focuser";					//  Requires Helper 5.0.3 or later
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
            get { return true; }
        }

        public void Halt()
        {
            Hypnofocus.Halt();
        }

        public bool IsMoving
        {
            get
            {
                return (Hypnofocus.IsMoving() == 0) ? true : false;
            }
        }

        public bool Link
        {
            get { return this._Link; }
            set
            {
                switch (value)
                {
                    case true:
                        Hypnofocus.Connect();
                        this._Link = true;
                        break;
                    case false:
                        Hypnofocus.Disconnect();
                        this._Link = false;
                        break;
                }
            }
        }

        public int MaxIncrement
        {
            get { return 10000; }
        }

        public int MaxStep
        {
            get { return 10000; }
        }

        public void Move(int val)
        {
            if (val >= 0)
            {
                Hypnofocus.MoveTo((Int16)val);
            }
        }

        public int Position
        {
            get
            {
                return Hypnofocus.GetPosition();
            }
        }

        public void SetupDialog()
        {
            SetupDialogForm F = new SetupDialogForm();
            F.ShowDialog();
        }

        public double StepSize
        {
            get { return 2; }
        }

        public bool TempComp
        {
            get { return false; }
            set { throw new PropertyNotImplementedException("TempComp", true); }
        }

        public bool TempCompAvailable
        {
            get { return false; }
        }

        public double Temperature
        {
            get { throw new PropertyNotImplementedException("Temperature", false); }
        }

        #endregion
    }
}
