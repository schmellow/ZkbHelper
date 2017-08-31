using System;
using System.Net;
using ZkbHelper.Logging;

namespace ZkbHelper.DataSources
{
    public static class RestClient
    {
        public static string ExecuteGet(string command)
        {
            var repeats = Configuration.RestClientRepeats;
            if (repeats <= 0)
            {
                Logger.Instance.Write("Invalid 'repeats' value: " + repeats);
                return string.Empty;
            }
            var client = new WebClient();
            for (int i = 0; i < repeats; i++)
            {
                try
                {
                    return client.DownloadString(command);
                }
                catch(Exception ex)
                {
                    Logger.Instance.Write(ex.ToString());
                }
            }
            return string.Empty;
        }

    }
}
