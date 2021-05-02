using Headway.Core.Interface;
using Headway.Core.Model;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Headway.Services
{
    public class MenuService : IMenuService
    {
        private readonly HttpClient httpClient;
        private readonly TokenProvider tokenProvider;
        private readonly bool useAccessToken;

        public MenuService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
            useAccessToken = false;
        }

        public MenuService(HttpClient httpClient, TokenProvider tokenProvider)
        {
            this.httpClient = httpClient;
            this.tokenProvider = tokenProvider;
            useAccessToken = true;
        }

        public async Task<IEnumerable<MenuItem>> GetMenuItemsAsync()
        {
            if (useAccessToken)
            {
                var token = tokenProvider.AccessToken;
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            }

            return await JsonSerializer.DeserializeAsync<IEnumerable<MenuItem>>
                (await httpClient.GetStreamAsync($"Menu"),
                    new JsonSerializerOptions(JsonSerializerDefaults.Web));
        }
    }
}