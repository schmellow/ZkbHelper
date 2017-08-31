using System;
using System.Windows;
using ZkbHelper.DataSources;
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
            try
            {
                LocalIdCache.Load();
            }
            catch(Exception ex)
            {
                Logger.Instance.Write(ex.ToString());
                Shutdown();
            }
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            try
            {
                LocalIdCache.Unload();
            }
            catch (Exception ex)
            {
                Logger.Instance.Write(ex.ToString());
                Shutdown();
            }
            Logger.Instance.Write("----------------------------------");
        }
    }
}
