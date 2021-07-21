using Headway.Core.Interface;
using Headway.Core.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Headway.Core.Cache
{
    public class ConfigCache : IConfigCache
    {
        private readonly IConfigurationService configurationService;

        private readonly Dictionary<string, Config> cache = new();

        public ConfigCache(IConfigurationService configurationService)
        {
            this.configurationService = configurationService;
        }

        public async Task<Config> GetConfigAsync(string configName)
        {
            var config = cache.GetValueOrDefault(configName);

            if (config != null)
            {
                return config;
            }

            var result = await configurationService.GetConfigAsync(configName)
                .ConfigureAwait(false);

            if(result.IsSuccess)
            {
                if (!cache.ContainsKey(result.Result.Name))
                {
                    cache.Add(result.Result.Name, result.Result);
                }
            }

            return cache.GetValueOrDefault(configName);
        }
    }
}
