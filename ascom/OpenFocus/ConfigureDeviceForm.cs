﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using LibUsbDotNet;
using LibUsbDotNet.Info;
using LibUsbDotNet.Main;

namespace ASCOM.OpenFocus
{
    public partial class ConfigureDeviceForm : Form
    {
        public String Serial;
        public Config.Device Device;

        public ConfigureDeviceForm(String s)
        {
            InitializeComponent();

            Serial = s;
            Device = new Config.Device(Serial);
            LoadValues();
        }

        private void LoadValues()
        {
            ASCOM.OpenFocus.Device.Connect(Serial);
            UsbDeviceInfo info = ASCOM.OpenFocus.Device.Descriptor;
            ASCOM.OpenFocus.Device.Disconnect();

            this.tbName.Text                       = Device.Name;
            this.tbMaxPosition.Text                = Device.MaxPosition.ToString();
            this.tbTemperatureCoefficient.Text     = Device.TemperatureCoefficient.ToString();

            this.SerialNumber.Text                  = Serial;
            this.Product.Text                       = info.ProductString;
        }

        private void SaveValues()
        {
            Device.Name                             = this.tbName.Text;
            Device.MaxPosition                      = Int16.Parse(this.tbMaxPosition.Text);
            Device.TemperatureCoefficient           = double.Parse(this.tbTemperatureCoefficient.Text);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveValues();
        }

        private void btnSetPosition_Click(object sender, EventArgs e)
        {
            using (SetPositionForm setPosition = new SetPositionForm())
            {
                if (setPosition.ShowDialog(this) == DialogResult.OK)
                {
                    ASCOM.OpenFocus.Device.Connect(Serial);

                    ASCOM.OpenFocus.Device.Position = Int16.Parse(setPosition.Position);

                    ASCOM.OpenFocus.Device.Disconnect();
                }
            }
        }
    }
}
