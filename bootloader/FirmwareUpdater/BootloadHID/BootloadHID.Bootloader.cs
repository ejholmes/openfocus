using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace BootloadHID
{
    public class Bootloader
    {
        private static Byte[] dataBuffer = new Byte[65536 + 256];

        public static int UploadFirmware(String firmware)
        {
            //dataBuffer = IntelHexParser.ParseFile(firmware);
            Usb.DoUpload(firmware, true);
            return 0;
        }

        
    }
}
