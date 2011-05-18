using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace BootloadHID
{
    class Usb
    {
        [DllImport("libbootloadhid.dll")]
        private static extern int doUpload(string file, int leave);

        [DllImport("libbootloadhid.dll")]
        private static extern int uploadData(char[] dataBuffer, int startAddress, int endAddress);

        public static int DoUpload(string file, bool leave)
        {
            return doUpload(file, leave ? 1 : 0);
        }

        public static int UploadData(char[] dataBuffer, int startAddress, int endAddress)
        {
            return uploadData(dataBuffer, startAddress, endAddress);
        }
    }
}
