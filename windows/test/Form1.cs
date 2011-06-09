using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Cortex;
using Cortex.OpenFocus;

namespace test
{
    public partial class Form1 : Form
    {
        String chosen = String.Empty;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ASCOM.Utilities.Chooser chooser = new ASCOM.Utilities.Chooser();
            chooser.DeviceType = "Focuser";
            chooser.Choose("ASCOM.OpenFocus.Focuser");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ASCOM.OpenFocus.Focuser f = new ASCOM.OpenFocus.Focuser();
            f.Link = true;

            f.Link = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            EEPROM eeprom = new EEPROM();
            eeprom.StayInBootloader = true;
            eeprom.SerialNumber = Guid.NewGuid().ToString();

            if (!Bootloader.Connected)
                Helper.Connect();
            Bootloader.WriteEeprom(eeprom.Data);
            Bootloader.Disconnect();
            /*Byte[] data = { 0, 0, 1, 2, 3, 4, 5, 6 };
            if (!Bootloader.Connected)
                Helper.Connect();
            Bootloader.WriteEeprom(data);
            Bootloader.Disconnect();*/
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                if (Bootloader.Connected)
                    Bootloader.Reboot();
                else
                {
                    Bootloader.Connect();
                    Bootloader.Reboot();
                    Bootloader.Disconnect();
                }
            }
            catch
            {
            }
        }
    }
}
