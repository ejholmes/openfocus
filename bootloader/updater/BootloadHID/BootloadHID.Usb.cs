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

        public static int DoUpload(string file, bool leave)
        {
            return doUpload(file, leave ? 1 : 0);
        }
    }
}
