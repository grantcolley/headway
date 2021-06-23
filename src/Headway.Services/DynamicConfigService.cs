using Headway.Core.Dynamic;
using Headway.Core.Interface;
using Headway.Core.Model;
using System.Net.Http;
using System.Threading.Tasks;

namespace Headway.Services
{
    public class DynamicConfigService : ServiceBase, IDynamicConfigService
    {
        public DynamicConfigService(HttpClient httpClient)
            : base(httpClient, false)
        {
        }

        public DynamicConfigService(HttpClient httpClient, TokenProvider tokenProvider)
            : base(httpClient, true, tokenProvider)
        {
        }

        public async Task<IServiceResult<DynamicModelConfig>> GetDynamicModelConfigAsync<T>()
        {
            var model = typeof(T).Name;
            var httpResponseMessage = await httpClient.GetAsync($"DynamicConfig/{model}").ConfigureAwait(false);
            return await GetServiceResult<DynamicModelConfig>(httpResponseMessage);
        }
    }
}
