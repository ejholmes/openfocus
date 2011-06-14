using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

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
            Cortex.OpenFocus.Device dev = new Device();
            dev.Connect(Serial);
            Version firmware = dev.FirmwareVersion;
            dev.Disconnect();

            this.tbName.Text                       = Device.Name;
            this.tbMaxPosition.Text                = Device.MaxPosition.ToString();
            this.tbTemperatureCoefficient.Text     = Device.TemperatureCoefficient.ToString();
            this.cbReverse.Checked                 = Device.Reverse;

            this.SerialNumber.Text                  = Serial;
            this.toolTip.SetToolTip(this.SerialNumber, Serial);
            this.FirmwareVersion.Text               = firmware.ToString();
        }

        private void SaveValues()
        {
            Device.Name                             = this.tbName.Text;
            Device.MaxPosition                      = UInt16.Parse(this.tbMaxPosition.Text);
            Device.TemperatureCoefficient           = double.Parse(this.tbTemperatureCoefficient.Text);
            Device.Reverse                          = this.cbReverse.Checked;
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
                    Cortex.OpenFocus.Device dev = new Device();
                    dev.Connect(Serial);

                    dev.Position = UInt16.Parse(setPosition.Position);

                    dev.Disconnect();
                }
            }
        }

        private void btnFocusMaxImport_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    FocusMax.DataPoint[] points = FocusMax.ParseFile(dialog.FileName);

                    double slope = FocusMax.Slope(points);

                    this.tbTemperatureCoefficient.Text = Math.Round(slope, 2).ToString();
                }
            }
        }
    }
}
