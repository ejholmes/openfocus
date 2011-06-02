using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

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
    }
}
