using System;
using System.IO;

namespace ZkbHelper.Logging
{
    public class DiskLoggingTarget : ILoggingTarget
    {
        public void Write(string message)
        {
            var now = DateTime.Now;
            var fileName = "log_" + now.Year + "_" + now.Month + "_" + now.Day + ".txt";
            File.AppendAllText(fileName, message + Environment.NewLine);
        }
    }
}
