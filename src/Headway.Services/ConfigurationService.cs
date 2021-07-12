using Headway.Core.Interface;
using Headway.Core.Model;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Headway.Services
{
    public class ConfigurationService : ServiceBase, IConfigurationService
    {
        public ConfigurationService(HttpClient httpClient)
            : base(httpClient, false)
        {
        }

        public ConfigurationService(HttpClient httpClient, TokenProvider tokenProvider)
            : base(httpClient, true, tokenProvider)
        {
        }

        public async Task<IServiceResult<ListConfig>> GetListConfigAsync(string listConfig)
        {
            var httpResponseMessage = await httpClient.GetAsync($"ListConfig/{listConfig}").ConfigureAwait(false);
            return await GetServiceResult<ListConfig>(httpResponseMessage);
        }

        public async Task<IServiceResult<IEnumerable<ListConfig>>> GetListConfigsAsync()
        {
            var httpResponseMessage = await httpClient.GetAsync($"ListConfig").ConfigureAwait(false);
            return await GetServiceResult<IEnumerable<ListConfig>>(httpResponseMessage);
        }

        public async Task<IServiceResult<ModelConfig>> GetModelConfigAsync(string modelConfig)
        {
            var httpResponseMessage = await httpClient.GetAsync($"ModelConfig/{modelConfig}").ConfigureAwait(false);
            return await GetServiceResult<ModelConfig>(httpResponseMessage);
        }

        public async Task<IServiceResult<IEnumerable<ModelConfig>>> GetModelConfigsAsync()
        {
            var httpResponseMessage = await httpClient.GetAsync($"ModelConfig").ConfigureAwait(false);
            return await GetServiceResult<IEnumerable<ModelConfig>>(httpResponseMessage);
        }
    }
}
