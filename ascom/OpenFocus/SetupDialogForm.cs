/*
 * File: SetupDialogForm.cs
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
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

using ASCOM.Utilities;

namespace ASCOM.OpenFocus
{
    [ComVisible(false)]					// Form not registered for COM!
    public partial class SetupDialogForm : Form
    {
        public SetupDialogForm()
        {
            ASCOM.OpenFocus.Focuser.Profile.DeviceType = ASCOM.OpenFocus.Focuser.s_csDeviceType;
            InitializeComponent();
        }

        void SetupDialogForm_Shown(object sender, System.EventArgs e)
        {
            LoadValues();
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            SaveValues();
            Dispose();
        }

        private void LoadValues()
        {
            List<string> serials = Device.ListDevices();

            if (serials == null) return;

            this.cbDevices.Items.AddRange(serials.ToArray());
            this.cbDevices.SelectedIndex = 0;

            switch (ASCOM.OpenFocus.Focuser.Profile.GetValue(ASCOM.OpenFocus.Focuser.s_csDriverID, "Units"))
            {
                case Device.TemperatureUnits.Celsius:
                    this.rbUnitsCelsius.Checked = true;
                    break;
                case Device.TemperatureUnits.Fahrenheit:
                    this.rbUnitsFahrenheit.Checked = true;
                    break;
            }
        }

        private void SaveValues()
        {
            Device.Serial = this.cbDevices.SelectedItem.ToString();
            ASCOM.OpenFocus.Focuser.Profile.WriteValue(ASCOM.OpenFocus.Focuser.s_csDriverID, "Default", Device.Serial);

            if (this.rbUnitsCelsius.Checked)
                ASCOM.OpenFocus.Focuser.Profile.WriteValue(ASCOM.OpenFocus.Focuser.s_csDriverID, "Units", Device.TemperatureUnits.Celsius);
            else if (this.rbUnitsFahrenheit.Checked)
                ASCOM.OpenFocus.Focuser.Profile.WriteValue(ASCOM.OpenFocus.Focuser.s_csDriverID, "Units", Device.TemperatureUnits.Fahrenheit);
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        private void BrowseToAscom(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("http://ascom-standards.org/");
            }
            catch (System.ComponentModel.Win32Exception noBrowser)
            {
                if (noBrowser.ErrorCode == -2147467259)
                    MessageBox.Show(noBrowser.Message);
            }
            catch (System.Exception other)
            {
                MessageBox.Show(other.Message);
            }
        }
    }
}