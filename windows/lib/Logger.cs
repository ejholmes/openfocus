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
        public enum LogType
        {
            Information,
            Warning,
            Error
        }

        public delegate void LoggerWriteEventHandler(object sender, LoggerEventArgs e);

        public static event LoggerWriteEventHandler LoggerWrite;

        public static void Write(string text, LogType type)
        {
            if (LoggerWrite != null)
                LoggerWrite(null, new LoggerEventArgs(text, type));
        }

        public static void Write(string text)
        {
            Write(text, LogType.Information);
        }

        public static void Write()
        {
            Write(String.Empty);
        }
    }

    public class LoggerEventArgs : EventArgs
    {
        private string _Text;
        private Logger.LogType _Type;

        public LoggerEventArgs(string text) : this(text, Logger.LogType.Information) { }

        public LoggerEventArgs(string text, Logger.LogType type)
            : base()
        {
            _Text = text;
            _Type = Logger.LogType.Information;
        }

        public LoggerEventArgs() : base() { }

        public string Text
        {
            get { return _Text; }
        }

        public Logger.LogType Type
        {
            get { return _Type; }
        }
    }
}
