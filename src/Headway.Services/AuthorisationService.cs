﻿using Headway.Core.Dynamic;
using Headway.Core.Interface;
using Headway.Core.Model;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Headway.Services
{
    public class AuthorisationService : ServiceBase, IAuthorisationService
    {
        private readonly IConfigService configService;

        public AuthorisationService(HttpClient httpClient, IConfigService configService)
            : base(httpClient, false)
        {
            this.configService = configService;
        }

        public AuthorisationService(HttpClient httpClient, TokenProvider tokenProvider, IConfigService configService)
            : base(httpClient, true, tokenProvider)
        {
            this.configService = configService;
        }

        public async Task<IServiceResult<IEnumerable<User>>> GetUsersAsync()
        {
            var httpResponseMessage = await httpClient.GetAsync($"Users").ConfigureAwait(false);
            return await GetServiceResult<IEnumerable<User>>(httpResponseMessage);
        }

        public async Task<IServiceResult<User>> GetUserAsync(int userId)
        {
            var httpResponseMessage = await httpClient.GetAsync($"Users/{userId}").ConfigureAwait(false);
            return await GetServiceResult<User>(httpResponseMessage);
        }

        public async Task<IServiceResult<User>> AddUserAsync(User user)
        {
            var httpResponseMessage = await httpClient.PostAsJsonAsync($"Users", user).ConfigureAwait(false);
            return await GetServiceResult<User>(httpResponseMessage);
        }

        public async Task<IServiceResult<User>> UpdateUserAsync(User user)
        {
            var httpResponseMessage = await httpClient.PutAsJsonAsync($"Users", user).ConfigureAwait(false);
            return await GetServiceResult<User>(httpResponseMessage);
        }

        public async Task<IServiceResult<int>> DeleteUserAsync(int userId)
        {
            var httpResponseMessage = await httpClient.DeleteAsync($"Users/{userId}").ConfigureAwait(false);
            return await GetServiceResult<int>(httpResponseMessage);
        }

        public async Task<IServiceResult<IEnumerable<Permission>>> GetPermissionsAsync()
        {
            var httpResponseMessage = await httpClient.GetAsync($"Permissions").ConfigureAwait(false);
            return await GetServiceResult<IEnumerable<Permission>>(httpResponseMessage);
        }

        public async Task<IServiceResult<Permission>> GetPermissionAsync(int permissionId)
        {
            var httpResponseMessage = await httpClient.GetAsync($"Permissions/{permissionId}").ConfigureAwait(false);
            return await GetServiceResult<Permission>(httpResponseMessage);
        }

        public async Task<IServiceResult<Permission>> AddPermissionAsync(Permission permission)
        {
            var httpResponseMessage = await httpClient.PostAsJsonAsync($"Permissions", permission).ConfigureAwait(false);
            return await GetServiceResult<Permission>(httpResponseMessage);
        }

        public async Task<IServiceResult<Permission>> UpdatePermissionAsync(Permission permission)
        {
            var httpResponseMessage = await httpClient.PutAsJsonAsync($"Permissions", permission).ConfigureAwait(false);
            return await GetServiceResult<Permission>(httpResponseMessage);
        }

        public async Task<IServiceResult<int>> DeletePermissionAsync(int permissionId)
        {
            var httpResponseMessage = await httpClient.DeleteAsync($"Permissions/{permissionId}").ConfigureAwait(false);
            return await GetServiceResult<int>(httpResponseMessage);
        }

        public async Task<IServiceResult<IEnumerable<Role>>> GetRolesAsync()
        {
            var httpResponseMessage = await httpClient.GetAsync($"Roles").ConfigureAwait(false);
            return await GetServiceResult<IEnumerable<Role>>(httpResponseMessage);
        }

        public async Task<IServiceResult<Role>> GetRoleAsync(int roleId)
        {
            var httpResponseMessage = await httpClient.GetAsync($"Roles/{roleId}").ConfigureAwait(false);
            return await GetServiceResult<Role>(httpResponseMessage);
        }

        public async Task<IServiceResult<Role>> AddRoleAsync(Role role)
        {
            var httpResponseMessage = await httpClient.PostAsJsonAsync($"Roles", role).ConfigureAwait(false);
            return await GetServiceResult<Role>(httpResponseMessage);
        }

        public async Task<IServiceResult<Role>> UpdateRoleAsync(Role role)
        {
            var httpResponseMessage = await httpClient.PutAsJsonAsync($"Roles", role).ConfigureAwait(false);
            return await GetServiceResult<Role>(httpResponseMessage);
        }

        public async Task<IServiceResult<int>> DeleteRoleAsync(int roleId)
        {
            var httpResponseMessage = await httpClient.DeleteAsync($"Roles/{roleId}").ConfigureAwait(false);
            return await GetServiceResult<int>(httpResponseMessage);
        }

        public async Task<IServiceResult<DynamicList<T>>> GetDynamicListAsync<T>()
        {
            var serviceResultConfig = 
                await configService.GetListConfigAsync<T>(httpClient, tokenProvider)
                .ConfigureAwait(false);

            if (serviceResultConfig.IsSuccess)
            {
                var configPath = $"{serviceResultConfig.Result.ConfigPath}";

                using var response = await httpClient.GetAsync(configPath).ConfigureAwait(false);
                var serviceResultList = await GetServiceResult<IEnumerable<T>>(response)
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

        public async Task<IServiceResult<DynamicModel<T>>> GetDynamicModelAsync<T>(int id)
        {
            var serviceResultConfig
                = await configService.GetModelConfigAsync<T>(httpClient, tokenProvider)
                .ConfigureAwait(false);

            if (serviceResultConfig.IsSuccess)
            {
                var configPath = $"{serviceResultConfig.Result.ConfigApiPath}/{id}";

                using var response = await httpClient.GetAsync(configPath).ConfigureAwait(false);
                var serviceResultModel = await GetServiceResult<T>(response)
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

        public async Task<IServiceResult<DynamicModel<T>>> CreateDynamicModelInstanceAsync<T>()
        {
            var serviceResultConfig 
                = await configService.GetModelConfigAsync<T>(httpClient, tokenProvider)
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
                $"{dynamicModel.ModelConfig.ConfigApiPath}", dynamicModel.Model)
                .ConfigureAwait(false);

            var addResult = await GetServiceResult<T>(addResponse);

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
                $"{dynamicModel.ModelConfig.ConfigApiPath}", dynamicModel.Model)
                .ConfigureAwait(false);

            var addResult = await GetServiceResult<T>(addResponse);

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
            var configPath = $"{dynamicModel.ModelConfig.ConfigApiPath}/{dynamicModel.Id}";
            var httpResponseMessage = await httpClient.DeleteAsync($"{configPath}").ConfigureAwait(false);
            return await GetServiceResult<int>(httpResponseMessage);
        }
    }
}