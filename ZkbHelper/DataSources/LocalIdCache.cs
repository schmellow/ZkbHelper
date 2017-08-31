using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ZkbHelper.DataSources
{
    public static class LocalIdCache
    {
        private static ConcurrentDictionary<string, string> _cache;
        private static bool _isChanged = false;

        public static void Load()
        {
            if (File.Exists("cache.json"))
            {
                var json = File.ReadAllText("cache.json");
                var pairs = JsonConvert.DeserializeObject<List<KeyValuePair<string, string>>>(json);
                _cache = new ConcurrentDictionary<string, string>(pairs);
            }
            else
            {
                _cache = new ConcurrentDictionary<string, string>();
            }
        }

        public static void Unload()
        {
            if (_isChanged && _cache.Any())
            {
                var pairs = _cache.ToList();
                File.WriteAllText("cache.json", JsonConvert.SerializeObject(pairs));
            }
        }

        public static string GetId(string characterName)
        {
            var id = "";
            _cache.TryGetValue(characterName, out id);
            return id;
        }

        public static void StoreId(string characterName, string id)
        {
            _cache[characterName] = id;
            _isChanged = true;
        }
    }
}
