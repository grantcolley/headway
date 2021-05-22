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

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            AddHttpClientAuthorisationHeader();

            return await JsonSerializer.DeserializeAsync<IEnumerable<User>>
                (await httpClient.GetStreamAsync($"Users").ConfigureAwait(false),
                    new JsonSerializerOptions(JsonSerializerDefaults.Web)).ConfigureAwait(false);
        }

        public async Task<User> GetUserAsync(int userId)
        {
            AddHttpClientAuthorisationHeader();

            return await JsonSerializer.DeserializeAsync<User>
                (await httpClient.GetStreamAsync($"Users/{userId}").ConfigureAwait(false),
                new JsonSerializerOptions(JsonSerializerDefaults.Web)).ConfigureAwait(false);
        }

        public async Task<User> SaveUserAsync(User user)
        {
            AddHttpClientAuthorisationHeader();

            var httpResponseMessage = await httpClient.PutAsJsonAsync($"Users", user)
                .ConfigureAwait(false);

            return await httpResponseMessage.Content.ReadFromJsonAsync<User>(
                new JsonSerializerOptions(JsonSerializerDefaults.Web)).ConfigureAwait(false);
        }

        public async Task DeleteUserAsync(int userId)
        {
            AddHttpClientAuthorisationHeader();

            await httpClient.DeleteAsync($"Users/{userId}").ConfigureAwait(false);
        }

        public async Task<IEnumerable<Permission>> GetPermissionsAsync()
        {
            AddHttpClientAuthorisationHeader();

            return await JsonSerializer.DeserializeAsync<IEnumerable<Permission>>
                (await httpClient.GetStreamAsync($"Permissions").ConfigureAwait(false),
                    new JsonSerializerOptions(JsonSerializerDefaults.Web)).ConfigureAwait(false);
        }

        public async Task<Permission> GetPermissionAsync(int permissionId)
        {
            AddHttpClientAuthorisationHeader();

            return await JsonSerializer.DeserializeAsync<Permission>
                (await httpClient.GetStreamAsync($"Permissions/{permissionId}").ConfigureAwait(false),
                new JsonSerializerOptions(JsonSerializerDefaults.Web)).ConfigureAwait(false);
        }

        public async Task<Permission> SavePermissionAsync(Permission permission)
        {
            AddHttpClientAuthorisationHeader();

            var httpResponseMessage = await httpClient.PutAsJsonAsync($"Permissions", permission)
                .ConfigureAwait(false);

            return await httpResponseMessage.Content.ReadFromJsonAsync<Permission>(
                new JsonSerializerOptions(JsonSerializerDefaults.Web)).ConfigureAwait(false);
        }

        public async Task DeletePermissionAsync(int permissionId)
        {
            AddHttpClientAuthorisationHeader();

            await httpClient.DeleteAsync($"Permissions/{permissionId}").ConfigureAwait(false);
        }

        public async Task<IEnumerable<Role>> GetRolesAsync()
        {
            AddHttpClientAuthorisationHeader();

            return await JsonSerializer.DeserializeAsync<IEnumerable<Role>>
                (await httpClient.GetStreamAsync($"Roles").ConfigureAwait(false),
                    new JsonSerializerOptions(JsonSerializerDefaults.Web)).ConfigureAwait(false);
        }

        public async Task<Role> GetRoleAsync(int roleId)
        {
            AddHttpClientAuthorisationHeader();

            return await JsonSerializer.DeserializeAsync<Role>
                (await httpClient.GetStreamAsync($"Roles/{roleId}").ConfigureAwait(false),
                new JsonSerializerOptions(JsonSerializerDefaults.Web)).ConfigureAwait(false);
        }

        public async Task<Role> SaveRoleAsync(Role role)
        {
            AddHttpClientAuthorisationHeader();

            var httpResponseMessage = await httpClient.PutAsJsonAsync($"Roles", role)
                .ConfigureAwait(false);

            return await httpResponseMessage.Content.ReadFromJsonAsync<Role>(
                new JsonSerializerOptions(JsonSerializerDefaults.Web)).ConfigureAwait(false);
        }

        public async Task DeleteRoleAsync(int roleId)
        {
            AddHttpClientAuthorisationHeader();

            await httpClient.DeleteAsync($"Roles/{roleId}").ConfigureAwait(false);
        }
    }
}