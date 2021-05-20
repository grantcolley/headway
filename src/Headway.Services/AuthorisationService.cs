using Headway.Core.Interface;
using Headway.Core.Model;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Headway.Services
{
    public class AuthorisationService : IAuthorisationService
    {
        private readonly HttpClient httpClient;
        private readonly TokenProvider tokenProvider;
        private readonly bool useAccessToken;

        public AuthorisationService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
            useAccessToken = false;
        }

        public AuthorisationService(HttpClient httpClient, TokenProvider tokenProvider)
        {
            this.httpClient = httpClient;
            this.tokenProvider = tokenProvider;
            useAccessToken = true;
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            if (useAccessToken)
            {
                var token = tokenProvider.AccessToken;
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            }

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
    }
}