using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using LibUsbDotNet;
using LibUsbDotNet.Info;
using LibUsbDotNet.Main;

using Cortex;
using Cortex.OpenFocus;

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
            Cortex.OpenFocus.Device.Connect(Serial);
            UsbDeviceInfo info = Cortex.OpenFocus.Device.Descriptor;
            Cortex.OpenFocus.Device.Disconnect();

            Version firmware = new Version(info.Descriptor.BcdDevice >> 8, 0xff & info.Descriptor.BcdDevice);

            this.tbName.Text                       = Device.Name;
            this.tbMaxPosition.Text                = Device.MaxPosition.ToString();
            this.tbTemperatureCoefficient.Text     = Device.TemperatureCoefficient.ToString();

            this.SerialNumber.Text                  = Serial;
            this.toolTip.SetToolTip(this.SerialNumber, Serial);
            this.Product.Text                       = info.ProductString;
            this.FirmwareVersion.Text               = firmware.ToString();
        }

        private void SaveValues()
        {
            Device.Name                             = this.tbName.Text;
            Device.MaxPosition                      = UInt16.Parse(this.tbMaxPosition.Text);
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
                    Cortex.OpenFocus.Device.Connect(Serial);

                    Cortex.OpenFocus.Device.Position = UInt16.Parse(setPosition.Position);

                    Cortex.OpenFocus.Device.Disconnect();
                }
            }
        }
    }
}
