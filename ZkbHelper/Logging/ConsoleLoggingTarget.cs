using System;

namespace ZkbHelper.Logging
{
    public class ConsoleLoggingTarget : ILoggingTarget
    {
        public void Write(string message)
        {
            Console.WriteLine(message);
        }
    }
}
