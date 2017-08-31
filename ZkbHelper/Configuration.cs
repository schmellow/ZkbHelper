using System;
using System.Configuration;
using ZkbHelper.Logging;

namespace ZkbHelper
{
    public static class Configuration
    {
        public static int RestClientRepeats => GetProperty<int>("restClientRepeats");
        public static bool EnableListenerOnStartup => GetProperty<bool>("enableListenerOnStarup");
        public static bool LogToScreen => GetProperty<bool>("logToScreen");
        public static bool LogToFile => GetProperty<bool>("logToFile");

        private static T GetProperty<T>(string name)
        {
            try
            {
                var value = ConfigurationManager.AppSettings[name];
                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch(Exception ex)
            {
                Logger.Instance.Write(ex.ToString());
                return default(T);
            }
        }

    }
}
