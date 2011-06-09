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

            Logger.LoggerWrite += new Logger.LoggerWriteEventHandler(Logger_LoggerWrite);
        }

        void Logger_LoggerWrite(object sender, LoggerEventArgs e)
        {
            this.lbLog.Items.Add(e.Text);

            if (e.Text == String.Empty)
            {
                int current = this.lbLog.SelectedIndex;
                this.lbLog.SelectedIndex = this.lbLog.Items.Count - 1;
                this.lbLog.SelectedIndex = current;
            }
            else
            {
                this.lbLog.SelectedIndex = this.lbLog.Items.Count - 1;
            }
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
            Byte[] data = { 10, 0, 1, 2, 3, 4, 5, 6, 8, 55, 27 };
            if (!Bootloader.Connected)
                Helper.Connect();
            Bootloader.WriteEeprom(data);
            Bootloader.Disconnect();
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

        private void button5_Click(object sender, EventArgs e)
        {
            if (!Bootloader.Connected)
                Helper.Connect();
            Byte[] data = Bootloader.ReadEeprom();
            EEPROM eeprom = new EEPROM(data);
            Bootloader.Disconnect();

            List<String> bytes = new List<string>();

            foreach (Byte b in data)
                bytes.Add(b.ToString());

            Logger.Write(String.Join(", ", bytes.ToArray()));
        }
    }
}
