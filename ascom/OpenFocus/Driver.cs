/*
 * File: Driver.cs
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

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

using ASCOM;
using ASCOM.Interface;
using ASCOM.Utilities;

namespace ASCOM.OpenFocus
{
    [Guid("378ee2b3-3ba0-44fe-b382-cd643610814e")]
    [ClassInterface(ClassInterfaceType.None)]
    public class Focuser : IFocuser
    {
        public static string s_csDriverID = "ASCOM.OpenFocus.Focuser";
        public static string s_csDriverDescription = "OpenFocus";
        public static string s_csDeviceType = "Focuser";

        private bool _Link = false;

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
            get { return Device.Absolute; }
        }

        public void Halt()
        {
            Device.Halt();
        }

        public bool IsMoving
        {
            get { return Device.IsMoving; }
        }

        public bool Link
        {
            get { return this._Link; }
            set
            {
                switch (value)
                {
                    case true:
                        this._Link = Device.Connect();
                        break;
                    case false:
                        if (this._Link)
                        {
                            if (this.IsMoving)
                                Device.Halt();
                            Device.Disconnect();
                            this._Link = false;
                        }
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
                Device.MoveTo((Int16)val);
        }

        public int Position
        {
            get { return Device.Position; }
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
            get { return Device.TempComp; }
            set { Device.TempComp = value; }
        }

        public bool TempCompAvailable
        {
            get { return Device.TempCompAvailable; }
        }

        public double Temperature
        {
            get { return Device.Temperature; }
        }

        #endregion
    }
}
