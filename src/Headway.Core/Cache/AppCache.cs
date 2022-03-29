using Headway.Core.Interface;
using System.Collections.Generic;

namespace Headway.Core.Cache
{
    public class AppCache : IAppCache
    {
        private readonly Dictionary<string, object> cache = new();

        private readonly object cacheLock = new();

        public T Get<T>(string key)
        {
            lock (cacheLock)
            {
                if(cache.ContainsKey(key))
                {
                    return (T)cache.GetValueOrDefault<string, object>(key);
                }
                else
                {
                    return default(T);
                }
            }
        }

        public bool Remove(string key)
        {
            lock (cacheLock)
            {
                if (cache.ContainsKey(key))
                {
                    return cache.Remove(key);
                }

                return true;
            }
        }

        public void Set(string key, object value)
        {
            lock (cacheLock)
            {
                if (cache.ContainsKey(key))
                {
                    cache[key] = value;
                }
                else
                {
                    cache.Add(key, value);
                }
            }
        }
    }
}
