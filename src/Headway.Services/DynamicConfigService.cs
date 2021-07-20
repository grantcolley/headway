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
        private readonly ConcurrentDictionary<string, Config> configs = new ();

        // OBSOLETE
        private readonly ConcurrentDictionary<string, ListConfig> listConfigs = new();
        private readonly ConcurrentDictionary<string, ModelConfig> modelConfigs = new();

        public async Task<IServiceResult<Config>> GetConfigAsync(string configName, HttpClient httpClient, TokenProvider tokenProvider)
        {
            if (configs.ContainsKey(configName))
            {
                if (configs.TryGetValue(configName, out Config config))
                {
                    return new ServiceResult<Config>
                    {
                        IsSuccess = true,
                        Result = config
                    };
                }
            }

            using var httpResponseMessage = await httpClient.GetAsync($"Configuration/{configName}").ConfigureAwait(false);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var config = await JsonSerializer.DeserializeAsync<Config>
                    (await httpResponseMessage.Content.ReadAsStreamAsync().ConfigureAwait(false),
                    new JsonSerializerOptions(JsonSerializerDefaults.Web)).ConfigureAwait(false);

                if (configs.TryAdd(configName, config))
                {
                    return new ServiceResult<Config>
                    {
                        IsSuccess = true,
                        Result = config
                    };
                }
                else
                {
                    return new ServiceResult<Config>
                    {
                        IsSuccess = false,
                        Message = $"Config {configName} is not accessible"
                    };
                }
            }
            else
            {
                return new ServiceResult<Config>
                {
                    IsSuccess = httpResponseMessage.IsSuccessStatusCode,
                    Message = httpResponseMessage.ReasonPhrase
                };
            }
        }

        // OBSEOELTE

        public async Task<IServiceResult<ListConfig>> GetListConfigAsync(string configName, HttpClient httpClient, TokenProvider tokenProvider)
        {
            if (listConfigs.ContainsKey(configName))
            {
                if (listConfigs.TryGetValue(configName, out ListConfig config))
                {
                    return new ServiceResult<ListConfig>
                    {
                        IsSuccess = true,
                        Result = config
                    };
                }
            }
            
            using var httpResponseMessage = await httpClient.GetAsync($"Config/{configName}").ConfigureAwait(false);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var config = await JsonSerializer.DeserializeAsync<ListConfig>
                    (await httpResponseMessage.Content.ReadAsStreamAsync().ConfigureAwait(false),
                    new JsonSerializerOptions(JsonSerializerDefaults.Web)).ConfigureAwait(false);

                if (listConfigs.TryAdd(configName, config))
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
                        Message = $"ListConfig {configName} list is not accessible"
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
