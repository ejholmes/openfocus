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

        public void PopulateDevices()
        {
            List<string> serials = Device.ListDevices();

            if (serials == null) return;

            List<KeyValuePair> devices = new List<KeyValuePair>();
            foreach (String serial in serials)
            {
                devices.Add(new KeyValuePair(new Config.Device(serial).Name, serial));
            }

            this.cbDevices.Items.Clear();
            this.cbDevices.Items.AddRange(devices.ToArray());
            this.cbDevices.DisplayMember = "Key";
            this.cbDevices.ValueMember = "Value";

            this.cbDevices.SelectedIndex = 0;
        }

        private void LoadValues()
        {
            #region Devices
            PopulateDevices();
            #endregion

            #region Temperature Units
            switch (Config.Units)
            {
                case Device.TemperatureUnits.Celsius:
                    this.rbUnitsCelsius.Checked = true;
                    break;
                case Device.TemperatureUnits.Fahrenheit:
                    this.rbUnitsFahrenheit.Checked = true;
                    break;
            }
            #endregion
        }

        private void SaveValues()
        {
            #region Devices
            Device.Serial = ((KeyValuePair)this.cbDevices.SelectedItem).Value.ToString();
            Config.DefaultDevice = Device.Serial;
            #endregion

            #region Temperature Units
            if (this.rbUnitsCelsius.Checked)
                Config.Units = Device.TemperatureUnits.Celsius;
            else if (this.rbUnitsFahrenheit.Checked)
                Config.Units = Device.TemperatureUnits.Fahrenheit;
            #endregion
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

        private void btnConfigureDevice_Click(object sender, EventArgs e)
        {
            using (ConfigureDeviceForm configureDevice = new ConfigureDeviceForm(((KeyValuePair)this.cbDevices.SelectedItem).Value))
            {
                if (configureDevice.ShowDialog(this) == DialogResult.OK)
                {
                    PopulateDevices();
                }
            }
        }
    }
}