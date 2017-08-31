using System;
using Newtonsoft.Json.Linq;
using ZkbHelper.Logging;

namespace ZkbHelper.DataSources
{
    public static class ESI
    {
        private const string SEARCH_COMMAND = "https://esi.tech.ccp.is/latest/search/?categories=character&datasource=tranquility&language=en-us&search={0}&strict=true&user_agent=zkbhelper";
        public static string GetCharacterIdString(string characterName)
        {
            try
            {
                var normalizedName = characterName.ToLowerInvariant();
                // try cache
                var id = LocalIdCache.GetId(normalizedName);
                if (string.IsNullOrEmpty(id) == false)
                {
                    Logger.Instance.Write(string.Format("[Cache] Found '{0}' -> {1}", characterName, id));
                    return id;
                }
                // try remote
                var command = string.Format(SEARCH_COMMAND, normalizedName);
                var json = RestClient.ExecuteGet(command);
                if(string.IsNullOrEmpty(json))
                {
                    Logger.Instance.Write("[ESI] Empty response");
                }
                else if(json == "{}")
                {
                    Logger.Instance.Write("[ESI] Character not found: " + characterName);
                }
                else
                {
                    var obj = JObject.Parse(json);
                    id = obj["character"].First.Value<string>();
                    LocalIdCache.StoreId(normalizedName, id);
                    Logger.Instance.Write(string.Format("[ESI] Found '{0}' -> {1}", characterName, id));
                    return id;
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
