using Headway.Core.Interface;
using Headway.Core.Model;
using System.Collections.Generic;

namespace Headway.Core.Cache
{
    public class ConfigCache : IConfigCache
    {
        private readonly Dictionary<string, Config> cache = new();

        private readonly object cacheLock = new();

        public void AddConfig(Config config)
        {
            lock (cacheLock)
            {
                if (!cache.ContainsKey(config.Name))
                {
                    cache.Add(config.Name, config);
                }
            }
        }

        public void AddConfigs(IEnumerable<Config> configs)
        {
            lock (cacheLock)
            {
                foreach(var config in configs)
                {
                    if (!cache.ContainsKey(config.Name))
                    {
                        cache.Add(config.Name, config);
                    }
                }
            }
        }

        public Config GetConfig(string configName)
        {
            lock (cacheLock)
            {
                return cache.GetValueOrDefault(configName);
            }
        }
    }
}
