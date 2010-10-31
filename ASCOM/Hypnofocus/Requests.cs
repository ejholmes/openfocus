using System;
using System.Collections.Generic;
using System.Text;

namespace ASCOM.Hypnofocus
{
    class Requests
    {
        public static byte FOCUSER_MOVE_TO          = 0x00;
        public static byte FOCUSER_HALT             = 0x01;
        public static byte FOCUSER_GET_POSITION     = 0x10;
        public static byte FOCUSER_GET_STATUS       = 0x11;
    }
}
