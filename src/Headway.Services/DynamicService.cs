using Headway.Core.Dynamic;
using Headway.Core.Helpers;
using Headway.Core.Interface;
using Headway.Core.Model;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Headway.Services
{
    public class DynamicService : ConfigurationService, IDynamicService
    {
        public DynamicService(HttpClient httpClient, IConfigCache configCache)
            : base(httpClient, configCache)
        {
        }

        public DynamicService(HttpClient httpClient, TokenProvider tokenProvider, IConfigCache configCache)
            : base(httpClient, tokenProvider, configCache)
        {
        }

        public async Task<IServiceResult<DynamicList<T>>> GetDynamicListAsync<T>(IEnumerable<T> list, string config) where T : class, new()
        {
            var serviceResultConfig =
                await GetConfigAsync(config)
                .ConfigureAwait(false);

            if (serviceResultConfig.IsSuccess)
            {
                return new ServiceResult<DynamicList<T>>
                {
                    IsSuccess = serviceResultConfig.IsSuccess,
                    Message = serviceResultConfig.Message,
                    Result = new DynamicList<T>(list, serviceResultConfig.Result)
                };
            }
            else
            {
                return new ServiceResult<DynamicList<T>>
                {
                    IsSuccess = serviceResultConfig.IsSuccess,
                    Message = serviceResultConfig.Message
                };
            }
        }

        public async Task<IServiceResult<DynamicList<T>>> GetDynamicListAsync<T>(string config) where T : class, new()
        {
            var serviceResultConfig =
                await GetConfigAsync(config)
                .ConfigureAwait(false);

            if (serviceResultConfig.IsSuccess)
            {
                var configApi = serviceResultConfig.Result.ModelApi;

                using var response = await httpClient.GetAsync(configApi).ConfigureAwait(false);
                var serviceResultList = await GetServiceResultAsync<IEnumerable<T>>(response)
                    .ConfigureAwait(false);

                if (serviceResultList.IsSuccess)
                {
                    return new ServiceResult<DynamicList<T>>
                    {
                        IsSuccess = serviceResultList.IsSuccess,
                        Message = serviceResultList.Message,
                        Result = new DynamicList<T>(serviceResultList.Result, serviceResultConfig.Result)
                    };
                }
                else
                {
                    return new ServiceResult<DynamicList<T>>
                    {
                        IsSuccess = serviceResultList.IsSuccess,
                        Message = serviceResultList.Message
                    };
                }
            }
            else
            {
                return new ServiceResult<DynamicList<T>>
                {
                    IsSuccess = serviceResultConfig.IsSuccess,
                    Message = serviceResultConfig.Message
                };
            }
        }

        public async Task<IServiceResult<DynamicModel<T>>> GetDynamicModelAsync<T>(T model, string config) where T : class, new()
        {
            var serviceResultConfig =
                await GetConfigAsync(config)
                .ConfigureAwait(false);

            if (serviceResultConfig.IsSuccess)
            {
                return new ServiceResult<DynamicModel<T>>
                {
                    IsSuccess = serviceResultConfig.IsSuccess,
                    Message = serviceResultConfig.Message,
                    Result = new DynamicModel<T>(model, serviceResultConfig.Result)
                };
            }
            else
            {
                return new ServiceResult<DynamicModel<T>>
                {
                    IsSuccess = serviceResultConfig.IsSuccess,
                    Message = serviceResultConfig.Message
                };
            }
        }

        public async Task<IServiceResult<DynamicModel<T>>> GetDynamicModelAsync<T>(int id, string config) where T : class, new()
        {
            var serviceResultConfig =
                await GetConfigAsync(config)
                .ConfigureAwait(false);

            if (serviceResultConfig.IsSuccess)
            {
                var configApi = $"{serviceResultConfig.Result.ModelApi}/{id}";

                using var response = await httpClient.GetAsync(configApi).ConfigureAwait(false);
                var serviceResultModel = await GetServiceResultAsync<T>(response)
                    .ConfigureAwait(false);

                if (serviceResultModel.IsSuccess)
                {
                    return new ServiceResult<DynamicModel<T>>
                    {
                        IsSuccess = serviceResultModel.IsSuccess,
                        Message = serviceResultModel.Message,
                        Result = new DynamicModel<T>(serviceResultModel.Result, serviceResultConfig.Result)
                    };
                }
                else
                {
                    return new ServiceResult<DynamicModel<T>>
                    {
                        IsSuccess = serviceResultModel.IsSuccess,
                        Message = serviceResultModel.Message
                    };
                }
            }
            else
            {
                return new ServiceResult<DynamicModel<T>>
                {
                    IsSuccess = serviceResultConfig.IsSuccess,
                    Message = serviceResultConfig.Message
                };
            }
        }

        public async Task<IServiceResult<DynamicModel<T>>> CreateDynamicModelInstanceAsync<T>(string config) where T : class, new()
        {
            var serviceResultConfig =
                await GetConfigAsync(config)
                .ConfigureAwait(false);

            if (serviceResultConfig.IsSuccess)
            {
                var model = TypeHelper<T>.Create();

                return new ServiceResult<DynamicModel<T>>
                {
                    IsSuccess = serviceResultConfig.IsSuccess,
                    Message = serviceResultConfig.Message,
                    Result = new DynamicModel<T>(model, serviceResultConfig.Result)
                };
            }
            else
            {
                return new ServiceResult<DynamicModel<T>>
                {
                    IsSuccess = serviceResultConfig.IsSuccess,
                    Message = serviceResultConfig.Message
                };
            }
        }

        public async Task<IServiceResult<DynamicModel<T>>> AddDynamicModelAsync<T>(DynamicModel<T> dynamicModel) where T : class, new()
        {
            var addResponse = await httpClient.PostAsJsonAsync(
                dynamicModel.Config.ModelApi, dynamicModel.Model)
                .ConfigureAwait(false);

            var addResult = await GetServiceResultAsync<T>(addResponse).ConfigureAwait(false);

            var serviceResult = new ServiceResult<DynamicModel<T>>
            {
                IsSuccess = addResult.IsSuccess,
                Message = addResult.Message
            };

            if (serviceResult.IsSuccess)
            {
                serviceResult.Result = dynamicModel;
            }

            return serviceResult;
        }

        public async Task<IServiceResult<DynamicModel<T>>> UpdateDynamicModelAsync<T>(DynamicModel<T> dynamicModel) where T : class, new()
        {
            var updateResponse = await httpClient.PutAsJsonAsync(
                dynamicModel.Config.ModelApi, dynamicModel.Model)
                .ConfigureAwait(false);

            var updateResult = await GetServiceResultAsync<T>(updateResponse).ConfigureAwait(false);

            var serviceResult = new ServiceResult<DynamicModel<T>>
            {
                IsSuccess = updateResult.IsSuccess,
                Message = updateResult.Message
            };

            if (serviceResult.IsSuccess)
            {
                serviceResult.Result = dynamicModel;
            }

            return serviceResult;
        }

        public async Task<IServiceResult<int>> DeleteDynamicModelAsync<T>(DynamicModel<T> dynamicModel) where T : class, new()
        {
            var configPath = $"{dynamicModel.Config.ModelApi}/{dynamicModel.Id}";
            var httpResponseMessage = await httpClient.DeleteAsync(configPath).ConfigureAwait(false);
            return await GetServiceResultAsync<int>(httpResponseMessage).ConfigureAwait(false);
        }
    }
}
