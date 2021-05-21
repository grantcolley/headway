using Headway.Core.Interface;
using Headway.Core.Model;
using System.Collections.Generic;
using System.Net.Http;
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
                (await httpClient.GetStreamAsync($"Users"),
                    new JsonSerializerOptions(JsonSerializerDefaults.Web));
        }

        public Task<User> GetUserAsync(string userName)
        {
            throw new System.NotImplementedException();
        }

        public Task<User> SaveUserAsync(User user)
        {
            throw new System.NotImplementedException();
        }

        public Task DeleteUserAsync(string userName)
        {
            throw new System.NotImplementedException();
        }

        public async Task<IEnumerable<Permission>> GetPermissionsAsync()
        {
            AddHttpClientAuthorisationHeader();

            return await JsonSerializer.DeserializeAsync<IEnumerable<Permission>>
                (await httpClient.GetStreamAsync($"Permissions"),
                    new JsonSerializerOptions(JsonSerializerDefaults.Web));
        }

        public Task<User> GetPermissionAsync(int permissionId)
        {
            throw new System.NotImplementedException();
        }

        public Task<User> SavePermissionAsync(Permission permission)
        {
            throw new System.NotImplementedException();
        }

        public Task DeletePermissionAsync(int permissionId)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<Role>> GetRolesAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<User> GetRoleAsync(int roleId)
        {
            throw new System.NotImplementedException();
        }

        public Task<User> SaveRoleAsync(Role role)
        {
            throw new System.NotImplementedException();
        }

        public Task DeleteRoleAsync(int roleId)
        {
            throw new System.NotImplementedException();
        }
    }
}