using Headway.Core.Args;
using Headway.Core.Constants;
using Headway.Core.Dynamic;
using Headway.Core.Helpers;
using Headway.Core.Interface;
using Headway.Core.Model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
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
                if(responseConfig.Result.UseSearchComponent)
                {
                    var list = TypeHelper<T>.CreateList();

                    return new Response<DynamicList<T>>
                    {
                        IsSuccess = responseConfig.IsSuccess,
                        Message = responseConfig.Message,
                        Result = new DynamicList<T>(list, responseConfig.Result)
                    };
                }

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

        public async Task<IResponse<DynamicList<T>>> SearchDynamicListAsync<T>(DynamicList<T> dynamicList) where T : class, new()
        {
            if (dynamicList.UseSearchCriteria)
            {
                var uri = $"{dynamicList.Config.ModelApi}/{Controllers.SEARCH_ACTION}";

                using var response = await httpClient
                    .PostAsJsonAsync(uri, dynamicList.SearchArgs)
                    .ConfigureAwait(false);

                var responseList = await GetResponseAsync<IEnumerable<T>>(response)
                    .ConfigureAwait(false);

                if(responseList.IsSuccess)
                {
                    dynamicList.RePopulateList(responseList.Result);
                }

                return new Response<DynamicList<T>>
                {
                    IsSuccess = responseList.IsSuccess,
                    Message = responseList.Message,
                    Result = dynamicList
                };
            }

            return new Response<DynamicList<T>>
            {
                IsSuccess = false,
                Message = $"{nameof(dynamicList.Config.UseSearchComponent)} is false",
                Result = dynamicList
            };
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

        public async Task<IResponse<DynamicModel<T>>> CreateDynamicModelInstanceAsync<T>(string config, string dataArgs) where T : class, new()
        {
            var responseConfig =
                await GetConfigAsync(config)
                .ConfigureAwait(false);

            if (responseConfig.IsSuccess)
            {
                var uri = $"{responseConfig.Result.ModelApi}/{Controllers.CREATE_ACTION}";

                var args = JsonSerializer.Deserialize<DataArgs>(dataArgs);

                using var response = await httpClient
                    .PostAsJsonAsync(uri, args)
                    .ConfigureAwait(false);

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
                if(responseConfig.Result.CreateLocal)
                {
                    var model = TypeHelper<T>.Create();

                    return new Response<DynamicModel<T>>
                    {
                        IsSuccess = responseConfig.IsSuccess,
                        Message = responseConfig.Message,
                        Result = new DynamicModel<T>(model, responseConfig.Result)
                    };
                }

                return await GetDynamicModelAsync<T>(0, config)
                    .ConfigureAwait(false);
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
            using var addResponse = await httpClient.PostAsJsonAsync(
                dynamicModel.Config.ModelApi, dynamicModel.Model)
                .ConfigureAwait(false);

            var response = await GetResponseAsync<T>(addResponse).ConfigureAwait(false);

            if (response.IsSuccess)
            {
                dynamicModel.Reset(response.Result);
            }

            return new Response<DynamicModel<T>>
            {
                IsSuccess = response.IsSuccess,
                Message = response.Message,
                Result = dynamicModel
            };
        }

        public async Task<IResponse<DynamicModel<T>>> UpdateDynamicModelAsync<T>(DynamicModel<T> dynamicModel) where T : class, new()
        {
            using var updateResponse = await httpClient.PutAsJsonAsync(
                dynamicModel.Config.ModelApi, dynamicModel.Model)
                .ConfigureAwait(false);

            var response = await GetResponseAsync<T>(updateResponse).ConfigureAwait(false);

            if (response.IsSuccess)
            {
                dynamicModel.Reset(response.Result);
            }

            return new Response<DynamicModel<T>>
            {
                IsSuccess = response.IsSuccess,
                Message = response.Message,
                Result = dynamicModel
            };
        }

        public async Task<IResponse<int>> DeleteDynamicModelAsync<T>(DynamicModel<T> dynamicModel) where T : class, new()
        {
            var configPath = $"{dynamicModel.Config.ModelApi}/{dynamicModel.Id}";
            using var httpResponseMessage = await httpClient.DeleteAsync(configPath).ConfigureAwait(false);
            return await GetResponseAsync<int>(httpResponseMessage).ConfigureAwait(false);
        }

        public async Task<IResponse<DynamicModel<T>>> FlowExecutionAsync<T>(DynamicModel<T> dynamicModel) where T : class, new()
        {
            var uri = $"{dynamicModel.Config.ModelApi}/{Controllers.FLOW_EXECUTION}";

            using var updateResponse = await httpClient.PostAsJsonAsync(
                uri, dynamicModel.Model)
                .ConfigureAwait(false);

            var response = await GetResponseAsync<T>(updateResponse).ConfigureAwait(false);

            if (response.IsSuccess)
            {
                dynamicModel.Reset(response.Result);
            }

            return new Response<DynamicModel<T>>
            {
                IsSuccess = response.IsSuccess,
                Message = response.Message,
                Result = dynamicModel
            };
        }
    }
}
