using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace ASCOM.Hypnofocus
{
    public class Hypnofocus
    {
        private const string dll = "hypnofocus.dll";

        [DllImport(dll, EntryPoint="focuser_connect")]
        public static extern int Connect();

        [DllImport(dll, EntryPoint="focuser_disconnect")]
        public static extern int Disconnect();

        [DllImport(dll, EntryPoint="focuser_is_moving")]
        public static extern int IsMoving();

        [DllImport(dll, EntryPoint="focuser_move_to")]
        public static extern void MoveTo(Int16 position);

        [DllImport(dll, EntryPoint="focuser_halt")]
        public static extern int Halt();

        [DllImport(dll, EntryPoint="focuser_get_position")]
        public static extern int GetPosition();
    }
}
