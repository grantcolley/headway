using Headway.Core.Interface;
using Headway.Core.Model;
using System.Collections.Concurrent;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Headway.Services
{
    public class ConfigService : IConfigService
    {
        private readonly ConcurrentDictionary<string, ModelConfig> cache = new();

        public async Task<IServiceResult<ModelConfig>> GetModelConfigAsync<T>(HttpClient httpClient, TokenProvider tokenProvider)
        {
            var model = typeof(T).Name;

            if (cache.ContainsKey(model))
            {
                if (cache.TryGetValue(model, out ModelConfig config))
                {
                    return new ServiceResult<ModelConfig>
                    {
                        IsSuccess = true,
                        Result = config
                    };
                }
            }

            using var httpResponseMessage = await httpClient.GetAsync($"Config/{model}").ConfigureAwait(false);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var config = await JsonSerializer.DeserializeAsync<ModelConfig>
                    (await httpResponseMessage.Content.ReadAsStreamAsync().ConfigureAwait(false),
                    new JsonSerializerOptions(JsonSerializerDefaults.Web)).ConfigureAwait(false);

                if (cache.TryAdd(model, config))
                {
                    return new ServiceResult<ModelConfig>
                    {
                        IsSuccess = true,
                        Result = config
                    };
                }
                else
                {
                    return new ServiceResult<ModelConfig>
                    {
                        IsSuccess = false,
                        Message = $"Config for {model} is not accessible"
                    };
                }
            }
            else
            {
                return new ServiceResult<ModelConfig>
                {
                    IsSuccess = httpResponseMessage.IsSuccessStatusCode,
                    Message = httpResponseMessage.ReasonPhrase
                };
            }
        }
    }
}
