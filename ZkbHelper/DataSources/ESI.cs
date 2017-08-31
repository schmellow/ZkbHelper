using System;
using Newtonsoft.Json.Linq;
using ZkbHelper.Logging;

namespace ZkbHelper
{
    public static class ESI
    {
        private const string SEARCH_COMMAND = "https://esi.tech.ccp.is/latest/search/?categories=character&datasource=tranquility&language=en-us&search={0}&strict=true&user_agent=zkbhelper";
        public static string GetCharacterIdString(string characterName)
        {
            try
            {
                var command = string.Format(SEARCH_COMMAND, characterName);
                var json = RestClient.ExecuteGet(command);
                if(string.IsNullOrEmpty(json))
                {
                    Logger.Instance.Write("Empty response from ESI");
                }
                else if(json == "{}")
                {
                    Logger.Instance.Write("Character not found: " + characterName);
                }
                else
                {
                    var obj = JObject.Parse(json);
                    return obj["character"].First.Value<string>();
                }
            }
            catch(Exception ex)
            {
                Logger.Instance.Write(ex.ToString());
            }
            return string.Empty;
        }

    }
}
