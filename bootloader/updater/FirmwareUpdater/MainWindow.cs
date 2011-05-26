using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace FirmwareUpdater
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();

            /*Bootloader b = new Bootloader();
            uint pagesize = b.PageSize;
            uint flashsize = b.FlashSize;

            Byte[] bytes = IntelHexParser.ParseFile(@"C:\Users\ejholmes\Dropbox\Cortex Astronomy\Product Development\OpenFocus\firmware\main.hex", pagesize);

            Console.WriteLine("Uploading " + bytes.Length + " bytes");

            for (uint address = 0; address < bytes.Length; address += pagesize)
            {
                Console.WriteLine("0x" + address.ToString("x") + " ... " + "0x" + (address + pagesize).ToString("x"));

                Byte[] data = new Byte[pagesize];
                Buffer.BlockCopy(bytes, (int)address, data, 0, (int)pagesize);

                b.WriteBlock(address, data);
            }

            b.Reboot();*/

            return;
        }
    }
}
