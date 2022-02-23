using Headway.Core.Dynamic;
using Headway.Core.Helpers;
using Headway.Core.Interface;
using Headway.Core.Model;
using Headway.RequestApi.Requests;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Headway.RequestApi.Api
{
    public class DynamicApiRequest : ConfigurationApiRequest, IDynamicApiRequest
    {
        public DynamicApiRequest(HttpClient httpClient, IConfigCache configCache)
            : base(httpClient, configCache)
        {
        }

        public DynamicApiRequest(HttpClient httpClient, TokenProvider tokenProvider, IConfigCache configCache)
            : base(httpClient, tokenProvider, configCache)
        {
        }

        public async Task<IResponse<DynamicList<T>>> GetDynamicListAsync<T>(IEnumerable<T> list, string config) where T : class, new()
        {
            var responseConfig =
                await GetConfigAsync(config)
                .ConfigureAwait(false);

            if (responseConfig.IsSuccess)
            {
                return new Response<DynamicList<T>>
                {
                    IsSuccess = responseConfig.IsSuccess,
                    Message = responseConfig.Message,
                    Result = new DynamicList<T>(list, responseConfig.Result)
                };
            }
            else
            {
                return new Response<DynamicList<T>>
                {
                    IsSuccess = responseConfig.IsSuccess,
                    Message = responseConfig.Message
                };
            }
        }

        public async Task<IResponse<DynamicList<T>>> GetDynamicListAsync<T>(string config) where T : class, new()
        {
            var responseConfig =
                await GetConfigAsync(config)
                .ConfigureAwait(false);

            if (responseConfig.IsSuccess)
            {
                var configApi = responseConfig.Result.ModelApi;

                using var response = await httpClient.GetAsync(configApi).ConfigureAwait(false);
                var responseList = await GetResponseAsync<IEnumerable<T>>(response)
                    .ConfigureAwait(false);

                if (responseList.IsSuccess)
                {
                    return new Response<DynamicList<T>>
                    {
                        IsSuccess = responseList.IsSuccess,
                        Message = responseList.Message,
                        Result = new DynamicList<T>(responseList.Result, responseConfig.Result)
                    };
                }
                else
                {
                    return new Response<DynamicList<T>>
                    {
                        IsSuccess = responseList.IsSuccess,
                        Message = responseList.Message
                    };
                }
            }
            else
            {
                return new Response<DynamicList<T>>
                {
                    IsSuccess = responseConfig.IsSuccess,
                    Message = responseConfig.Message
                };
            }
        }

        public async Task<IResponse<DynamicModel<T>>> GetDynamicModelAsync<T>(T model, string config) where T : class, new()
        {
            var responseConfig =
                await GetConfigAsync(config)
                .ConfigureAwait(false);

            if (responseConfig.IsSuccess)
            {
                return new Response<DynamicModel<T>>
                {
                    IsSuccess = responseConfig.IsSuccess,
                    Message = responseConfig.Message,
                    Result = new DynamicModel<T>(model, responseConfig.Result)
                };
            }
            else
            {
                return new Response<DynamicModel<T>>
                {
                    IsSuccess = responseConfig.IsSuccess,
                    Message = responseConfig.Message
                };
            }
        }

        public async Task<IResponse<DynamicModel<T>>> GetDynamicModelAsync<T>(int id, string config) where T : class, new()
        {
            var responseConfig =
                await GetConfigAsync(config)
                .ConfigureAwait(false);

            if (responseConfig.IsSuccess)
            {
                var configApi = $"{responseConfig.Result.ModelApi}/{id}";

                using var response = await httpClient.GetAsync(configApi).ConfigureAwait(false);
                var responseModel = await GetResponseAsync<T>(response)
                    .ConfigureAwait(false);

                if (responseModel.IsSuccess)
                {
                    return new Response<DynamicModel<T>>
                    {
                        IsSuccess = responseModel.IsSuccess,
                        Message = responseModel.Message,
                        Result = new DynamicModel<T>(responseModel.Result, responseConfig.Result)
                    };
                }
                else
                {
                    return new Response<DynamicModel<T>>
                    {
                        IsSuccess = responseModel.IsSuccess,
                        Message = responseModel.Message
                    };
                }
            }
            else
            {
                return new Response<DynamicModel<T>>
                {
                    IsSuccess = responseConfig.IsSuccess,
                    Message = responseConfig.Message
                };
            }
        }

        public async Task<IResponse<DynamicModel<T>>> CreateDynamicModelInstanceAsync<T>(string config) where T : class, new()
        {
            var responseConfig =
                await GetConfigAsync(config)
                .ConfigureAwait(false);

            if (responseConfig.IsSuccess)
            {
                var model = TypeHelper<T>.Create();

                return new Response<DynamicModel<T>>
                {
                    IsSuccess = responseConfig.IsSuccess,
                    Message = responseConfig.Message,
                    Result = new DynamicModel<T>(model, responseConfig.Result)
                };
            }
            else
            {
                return new Response<DynamicModel<T>>
                {
                    IsSuccess = responseConfig.IsSuccess,
                    Message = responseConfig.Message
                };
            }
        }

        public async Task<IResponse<DynamicModel<T>>> AddDynamicModelAsync<T>(DynamicModel<T> dynamicModel) where T : class, new()
        {
            var addResponse = await httpClient.PostAsJsonAsync(
                dynamicModel.Config.ModelApi, dynamicModel.Model)
                .ConfigureAwait(false);

            var response = await GetResponseAsync<T>(addResponse).ConfigureAwait(false);

            if (response.IsSuccess)
            {
                return new Response<DynamicModel<T>>
                {
                    IsSuccess = response.IsSuccess,
                    Message = response.Message,
                    Result = new DynamicModel<T>(response.Result, dynamicModel.Config)
                };
            }
            else
            {
                return new Response<DynamicModel<T>>
                {
                    IsSuccess = response.IsSuccess,
                    Message = response.Message,
                    Result = dynamicModel
                };
            }
        }

        public async Task<IResponse<DynamicModel<T>>> UpdateDynamicModelAsync<T>(DynamicModel<T> dynamicModel) where T : class, new()
        {
            var updateResponse = await httpClient.PutAsJsonAsync(
                dynamicModel.Config.ModelApi, dynamicModel.Model)
                .ConfigureAwait(false);

            var response = await GetResponseAsync<T>(updateResponse).ConfigureAwait(false);

            if (response.IsSuccess)
            {
                return new Response<DynamicModel<T>>
                {
                    IsSuccess = response.IsSuccess,
                    Message = response.Message,
                    Result = new DynamicModel<T>(response.Result, dynamicModel.Config)
                };
            }
            else
            {
                return new Response<DynamicModel<T>>
                {
                    IsSuccess = response.IsSuccess,
                    Message = response.Message,
                    Result = dynamicModel
                };
            }
        }

        public async Task<IResponse<int>> DeleteDynamicModelAsync<T>(DynamicModel<T> dynamicModel) where T : class, new()
        {
            var configPath = $"{dynamicModel.Config.ModelApi}/{dynamicModel.Id}";
            var httpResponseMessage = await httpClient.DeleteAsync(configPath).ConfigureAwait(false);
            return await GetResponseAsync<int>(httpResponseMessage).ConfigureAwait(false);
        }
    }
}
