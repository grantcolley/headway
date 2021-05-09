using Headway.Core.Interface;
using Headway.Core.Model;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Headway.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient httpClient;
        private readonly TokenProvider tokenProvider;
        private readonly bool useAccessToken;

        public UserService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
            useAccessToken = false;
        }

        public UserService(HttpClient httpClient, TokenProvider tokenProvider)
        {
            this.httpClient = httpClient;
            this.tokenProvider = tokenProvider;
            useAccessToken = true;
        }

        public Task<User> GetUserAsync(string userName)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<User>> GetUsersAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> TryDeleteUserAsync(string userName)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> TryUpdateUserAsync(User user)
        {
            throw new System.NotImplementedException();
        }
    }
}
