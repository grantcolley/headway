using Headway.Core.Dynamic;
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

        public async Task<IServiceResult<DynamicList<T>>> GetDynamicListAsync<T>(string config)
        {
            var serviceResultConfig =
                await GetConfigAsync(config)
                .ConfigureAwait(false);

            if (serviceResultConfig.IsSuccess)
            {
                var configApi = $"{serviceResultConfig.Result.ModelApi}";

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

        public async Task<IServiceResult<DynamicModel<T>>> GetDynamicModelAsync<T>(int id, string config)
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

        public async Task<IServiceResult<DynamicModel<T>>> CreateDynamicModelInstanceAsync<T>(string config)
        {
            var serviceResultConfig =
                await GetConfigAsync(config)
                .ConfigureAwait(false);

            if (serviceResultConfig.IsSuccess)
            {
                var typeHelper = DynamicTypeHelper.Get<T>();
                var model = typeHelper.CreateInstance();
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

        public async Task<IServiceResult<DynamicModel<T>>> AddDynamicModelAsync<T>(DynamicModel<T> dynamicModel)
        {
            var addResponse = await httpClient.PostAsJsonAsync(
                $"{dynamicModel.Config.ModelApi}", dynamicModel.Model)
                .ConfigureAwait(false);

            var addResult = await GetServiceResultAsync<T>(addResponse);

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

        public async Task<IServiceResult<DynamicModel<T>>> UpdateDynamicModelAsync<T>(DynamicModel<T> dynamicModel)
        {
            var addResponse = await httpClient.PutAsJsonAsync(
                $"{dynamicModel.Config.ModelApi}", dynamicModel.Model)
                .ConfigureAwait(false);

            var addResult = await GetServiceResultAsync<T>(addResponse);

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

        public async Task<IServiceResult<int>> DeleteDynamicModelAsync<T>(DynamicModel<T> dynamicModel)
        {
            var configPath = $"{dynamicModel.Config.ModelApi}/{dynamicModel.Id}";
            var httpResponseMessage = await httpClient.DeleteAsync($"{configPath}").ConfigureAwait(false);
            return await GetServiceResultAsync<int>(httpResponseMessage);
        }
    }
}
