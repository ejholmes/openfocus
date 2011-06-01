using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Cortex
{
    [ComVisible(true)]
    public class Logger
    {
        public delegate void LoggerWriteEventHandler(object sender, LoggerEventArgs e);

        public static event LoggerWriteEventHandler LoggerWrite;

        public static void Write(string text)
        {
            if (LoggerWrite != null)
                LoggerWrite(null, new LoggerEventArgs(text));
        }

        public static void Write()
        {
            Write(String.Empty);
        }
    }

    public class LoggerEventArgs : EventArgs
    {
        private string _Text;

        public LoggerEventArgs(string text)
            : base()
        {
            _Text = text;
        }

        public LoggerEventArgs() : base() { }

        public string Text
        {
            get { return _Text; }
        }
    }
}
