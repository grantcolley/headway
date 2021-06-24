using Headway.Core.Dynamic;
using Headway.Core.Interface;
using Headway.Core.Model;
using System.Collections.Concurrent;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Headway.Services
{
    public class DynamicConfigService : IDynamicConfigService
    {
        private readonly ConcurrentDictionary<string, DynamicModelConfig> cache = new();

        public async Task<IServiceResult<DynamicModelConfig>> GetDynamicModelConfigAsync<T>(HttpClient httpClient, TokenProvider tokenProvider)
        {
            var model = typeof(T).Name;

            if (cache.ContainsKey(model))
            {
                if (cache.TryGetValue(model, out DynamicModelConfig config))
                {
                    return new ServiceResult<DynamicModelConfig>
                    {
                        IsSuccess = true,
                        Result = config
                    };
                }
            }

            using var httpResponseMessage = await httpClient.GetAsync($"DynamicConfig/{model}").ConfigureAwait(false);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var config = await JsonSerializer.DeserializeAsync<DynamicModelConfig>
                    (await httpResponseMessage.Content.ReadAsStreamAsync().ConfigureAwait(false),
                    new JsonSerializerOptions(JsonSerializerDefaults.Web)).ConfigureAwait(false);

                if (cache.TryAdd(model, config))
                {
                    return new ServiceResult<DynamicModelConfig>
                    {
                        IsSuccess = true,
                        Result = config
                    };
                }
                else
                {
                    return new ServiceResult<DynamicModelConfig>
                    {
                        IsSuccess = false,
                        Message = $"Config for {model} is not accessible"
                    };
                }
            }
            else
            {
                return new ServiceResult<DynamicModelConfig>
                {
                    IsSuccess = httpResponseMessage.IsSuccessStatusCode,
                    Message = httpResponseMessage.ReasonPhrase
                };
            }
        }
    }
}
