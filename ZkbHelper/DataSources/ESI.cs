using System;
using Newtonsoft.Json.Linq;
using ZkbHelper.Logging;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ZkbHelper
{
    public static class ESI
    {
        private const string SEARCH_COMMAND = "https://esi.tech.ccp.is/latest/search/?categories=character&datasource=tranquility&language=en-us&search={0}&strict=true&user_agent=zkbhelper";
        public static string GetCharacterIdString(string characterName)
        {
            var id = "";
            try
            {
                Dictionary<string, string> cache = null;
                // try from local cache
                if(File.Exists("cache.json"))
                {
                    cache = JsonConvert.DeserializeObject<Dictionary<string, string>>(
                        File.ReadAllText("cache.json"));
                    if (cache.TryGetValue(characterName, out id))
                    {
                        Logger.Instance.Write(string.Format("[Cache] Found '{0}' -> {1}", characterName, id));
                        return id;
                    }
                }
                // try from remote
                if (cache == null)
                    cache = new Dictionary<string, string>();
                var command = string.Format(SEARCH_COMMAND, characterName);
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
                    cache[characterName] = id;
                    Logger.Instance.Write(string.Format("[ESI] Found '{0}' -> {1}", characterName, id));
                    Task.Run(() =>
                    {
                        File.WriteAllText("cache.json", JsonConvert.SerializeObject(cache));
                    });
                }
            }
            catch(Exception ex)
            {
                Logger.Instance.Write(ex.ToString());
            }
            return id;
        }
    }
}
