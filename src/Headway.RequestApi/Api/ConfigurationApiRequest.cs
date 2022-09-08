using Headway.Core.Constants;
using Headway.Core.Interface;
using Headway.Core.Model;
using System.Net.Http;
using System.Threading.Tasks;

namespace Headway.RequestApi.Api
{
    public class ConfigurationApiRequest : LogApiRequest, IConfigurationApiRequest
    {
        private readonly IConfigCache configCache;

        public ConfigurationApiRequest(HttpClient httpClient, IConfigCache configCache)
            : base(httpClient)
        {
            this.configCache = configCache;
        }

        public ConfigurationApiRequest(HttpClient httpClient, TokenProvider tokenProvider, IConfigCache configCache)
            : base(httpClient, tokenProvider)
        {
            this.configCache = configCache;
        }

        public async Task<IResponse<Config>> GetConfigAsync(string name)
        {
            var config = configCache.GetConfig(name);

            if(config != null)
            {
                return GetResponseResult<Config>(config);
            }

            using var httpResponseMessage = await httpClient.GetAsync($"{Controllers.CONFIGURATION}/{name}").ConfigureAwait(false);

            var response = await GetResponseAsync<Config>(httpResponseMessage).ConfigureAwait(false);

            if(response.IsSuccess)
            {
                configCache.AddConfig(response.Result);
            }

            return response;
        }
    }
}
