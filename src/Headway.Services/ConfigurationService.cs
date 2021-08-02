using Headway.Core.Interface;
using Headway.Core.Model;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Headway.Services
{
    public class ConfigurationService : ServiceBase, IConfigurationService
    {
        private readonly IConfigCache configCache;

        public ConfigurationService(HttpClient httpClient, IConfigCache configCache)
            : base(httpClient, false)
        {
            this.configCache = configCache;
        }

        public ConfigurationService(HttpClient httpClient, TokenProvider tokenProvider, IConfigCache configCache)
            : base(httpClient, true, tokenProvider)
        {
            this.configCache = configCache;
        }

        public async Task<IServiceResult<IEnumerable<Config>>> GetConfigsAsync()
        {
            var httpResponseMessage = await httpClient.GetAsync($"Configuration").ConfigureAwait(false);

            var serviceResult = await GetServiceResultAsync<IEnumerable<Config>>(httpResponseMessage);

            if (serviceResult.IsSuccess)
            {
                configCache.AddConfigs(serviceResult.Result);
            }

            return serviceResult;
        }

        public async Task<IServiceResult<Config>> GetConfigAsync(string name)
        {
            var config = configCache.GetConfig(name);

            if(config != null)
            {
                return GetServiceResult<Config>(config);
            }

            var httpResponseMessage = await httpClient.GetAsync($"Configuration/{name}").ConfigureAwait(false);

            var serviceResult = await GetServiceResultAsync<Config>(httpResponseMessage).ConfigureAwait(false);

            if(serviceResult.IsSuccess)
            {
                configCache.AddConfig(serviceResult.Result);
            }

            return serviceResult;
        }
    }
}
