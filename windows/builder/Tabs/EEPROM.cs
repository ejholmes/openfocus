using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Cortex;
using Cortex.OpenFocus;

namespace Builder
{
    public partial class MainWindow
    {
        private void btnEEPROMGenerate_Click(object sender, EventArgs e)
        {
            EEPROM eeprom = new EEPROM();

            eeprom.StayInBootloader = this.cbEEPROMStayInBootloader.Checked;

            eeprom.SerialNumber = this.tbEEPROMSerialNumber.Text;

            IntelHexFile file = IntelHexFile.Create(eeprom.Data, 8);

            this.tbEEPROMOutput.Text = file.ToString();
        }
    }
}
