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
        private readonly ConcurrentDictionary<string, ListConfig> listConfigs = new();
        private readonly ConcurrentDictionary<string, ModelConfig> modelConfigs = new();

        public async Task<IServiceResult<ListConfig>> GetListConfigAsync<T>(string component, HttpClient httpClient, TokenProvider tokenProvider)
        {
            var model = typeof(T).Name;

            if (listConfigs.ContainsKey(model))
            {
                if (listConfigs.TryGetValue(model, out ListConfig config))
                {
                    return new ServiceResult<ListConfig>
                    {
                        IsSuccess = true,
                        Result = config
                    };
                }
            }

            using var httpResponseMessage = await httpClient.GetAsync($"ListConfig/{component}{model}").ConfigureAwait(false);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var config = await JsonSerializer.DeserializeAsync<ListConfig>
                    (await httpResponseMessage.Content.ReadAsStreamAsync().ConfigureAwait(false),
                    new JsonSerializerOptions(JsonSerializerDefaults.Web)).ConfigureAwait(false);

                if (listConfigs.TryAdd(model, config))
                {
                    return new ServiceResult<ListConfig>
                    {
                        IsSuccess = true,
                        Result = config
                    };
                }
                else
                {
                    return new ServiceResult<ListConfig>
                    {
                        IsSuccess = false,
                        Message = $"Config for {model} list is not accessible"
                    };
                }
            }
            else
            {
                return new ServiceResult<ListConfig>
                {
                    IsSuccess = httpResponseMessage.IsSuccessStatusCode,
                    Message = httpResponseMessage.ReasonPhrase
                };
            }
        }

        public async Task<IServiceResult<ModelConfig>> GetModelConfigAsync<T>(HttpClient httpClient, TokenProvider tokenProvider)
        {
            var model = typeof(T).Name;

            if (modelConfigs.ContainsKey(model))
            {
                if (modelConfigs.TryGetValue(model, out ModelConfig config))
                {
                    return new ServiceResult<ModelConfig>
                    {
                        IsSuccess = true,
                        Result = config
                    };
                }
            }

            using var httpResponseMessage = await httpClient.GetAsync($"ModelConfig/{model}").ConfigureAwait(false);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var config = await JsonSerializer.DeserializeAsync<ModelConfig>
                    (await httpResponseMessage.Content.ReadAsStreamAsync().ConfigureAwait(false),
                    new JsonSerializerOptions(JsonSerializerDefaults.Web)).ConfigureAwait(false);

                if (modelConfigs.TryAdd(model, config))
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
