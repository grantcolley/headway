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

        public async Task<IServiceResult<Config>> GetConfigAsync(string name)
        {
            var config = configCache.GetConfig(name);

            if(config != null)
            {
                return GetServiceResult<Config>(config);
            }

            var httpResponseMessage = await httpClient.GetAsync($"Configuration/{name}").ConfigureAwait(false);

            var serviceResult = await GetServiceResult<Config>(httpResponseMessage).ConfigureAwait(false);

            if(serviceResult.IsSuccess)
            {
                configCache.AddConfig(serviceResult.Result);
            }

            return serviceResult;
        }

        public async Task<IServiceResult<IEnumerable<Config>>> GetConfigsAsync()
        {
            var httpResponseMessage = await httpClient.GetAsync($"Configuration").ConfigureAwait(false);

            var serviceResult = await GetServiceResult<IEnumerable<Config>>(httpResponseMessage);

            if (serviceResult.IsSuccess)
            {
                configCache.AddConfigs(serviceResult.Result);
            }

            return serviceResult;
        }

        ///// <summary>
        ///// OBSOLETE
        ///// </summary>
        //public async Task<IServiceResult<ListConfig>> GetListConfigAsync(string listConfig)
        //{
        //    var httpResponseMessage = await httpClient.GetAsync($"ListConfig/{listConfig}").ConfigureAwait(false);
        //    return await GetServiceResult<ListConfig>(httpResponseMessage);
        //}

        //public async Task<IServiceResult<IEnumerable<ListConfig>>> GetListConfigsAsync()
        //{
        //    var httpResponseMessage = await httpClient.GetAsync($"ListConfig").ConfigureAwait(false);
        //    return await GetServiceResult<IEnumerable<ListConfig>>(httpResponseMessage);
        //}

        //public async Task<IServiceResult<ModelConfig>> GetModelConfigAsync(string modelConfig)
        //{
        //    var httpResponseMessage = await httpClient.GetAsync($"ModelConfig/{modelConfig}").ConfigureAwait(false);
        //    return await GetServiceResult<ModelConfig>(httpResponseMessage);
        //}

        //public async Task<IServiceResult<IEnumerable<ModelConfig>>> GetModelConfigsAsync()
        //{
        //    var httpResponseMessage = await httpClient.GetAsync($"ModelConfig").ConfigureAwait(false);
        //    return await GetServiceResult<IEnumerable<ModelConfig>>(httpResponseMessage);
        //}
    }
}
