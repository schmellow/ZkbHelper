using System;
using System.Windows;
using ZkbHelper.Logging;

namespace ZkbHelper
{
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            if (Configuration.LogToFile)
                Logger.Instance.SetTarget("file", new DiskLoggingTarget());
            Logger.Instance.Write("Session start: " + DateTime.Now);
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            Logger.Instance.Write("----------------------------------");
        }
    }
}
