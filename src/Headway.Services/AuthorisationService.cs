using Headway.Core.Dynamic;
using Headway.Core.Interface;
using Headway.Core.Model;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace Headway.Services
{
    public class AuthorisationService : ServiceBase, IAuthorisationService
    {
        public AuthorisationService(HttpClient httpClient)
            : base(httpClient, false)
        {
        }

        public AuthorisationService(HttpClient httpClient, TokenProvider tokenProvider)
            : base(httpClient, true, tokenProvider)
        {
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
            if(typeof(T) == typeof(Permission))
            {
                var httpResponseMessage = await httpClient.GetAsync($"Permissions/{id}").ConfigureAwait(false);

                var serviceResult = new ServiceResult<DynamicModel<T>>
                {
                    IsSuccess = httpResponseMessage.IsSuccessStatusCode,
                    Message = httpResponseMessage.ReasonPhrase
                };

                if (serviceResult.IsSuccess)
                {
                    var content = await JsonSerializer.DeserializeAsync<T>
                        (await httpResponseMessage.Content.ReadAsStreamAsync().ConfigureAwait(false),
                            new JsonSerializerOptions(JsonSerializerDefaults.Web)).ConfigureAwait(false);
                    serviceResult.Result = new DynamicModel<T>(content);
                }

                return serviceResult;
            }

            return null;
        }

        public DynamicModel<T> CreateDynamicModelInstance<T>()
        {
            var typeHelper = DynamicTypeHelper.Get<T>();
            var instance = typeHelper.CreateInstance();
            return new DynamicModel<T>(instance);
        }
    }
}