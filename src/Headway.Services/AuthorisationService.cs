using Headway.Core.Dynamic;
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
        private readonly IDynamicConfigService dynamicConfigService;

        public AuthorisationService(HttpClient httpClient, IDynamicConfigService dynamicConfigService)
            : base(httpClient, false)
        {
            this.dynamicConfigService = dynamicConfigService;
        }

        public AuthorisationService(HttpClient httpClient, TokenProvider tokenProvider, IDynamicConfigService dynamicConfigService)
            : base(httpClient, true, tokenProvider)
        {
            this.dynamicConfigService = dynamicConfigService;
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

        public async Task<IServiceResult<DynamicModel<T>>> GetDynamicModelAsync<T>(int id)
        {
            if (typeof(T) == typeof(Permission))
            {
                var serviceResultModel = default(IServiceResult<T>);
                using (var httpResponseMessage = await httpClient.GetAsync($"Permissions/{id}").ConfigureAwait(false))
                {
                    serviceResultModel = await GetServiceResult<T>(httpResponseMessage).ConfigureAwait(false);
                }

                if(serviceResultModel.IsSuccess)
                {
                    var serviceResultConfig
                        = await dynamicConfigService.GetDynamicModelConfigAsync<T>(httpClient, tokenProvider)
                        .ConfigureAwait(false);

                    if(serviceResultConfig.IsSuccess)
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
                            IsSuccess = serviceResultConfig.IsSuccess,
                            Message = serviceResultConfig.Message
                        };
                    }
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

            return null;
        }

        public async Task<IServiceResult<DynamicModel<T>>> CreateDynamicModelInstanceAsync<T>()
        {
            var serviceResultConfig 
                = await dynamicConfigService.GetDynamicModelConfigAsync<T>(httpClient, tokenProvider)
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
    }
}