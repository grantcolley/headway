using Headway.Core.Constants;
using Headway.Core.Interface;
using Headway.Core.Model;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Headway.Services
{
    public class ModuleService : IModuleService
    {
        private readonly HttpClient httpClient;
        private readonly TokenProvider tokenProvider;
        private readonly bool useAccessToken;

        public ModuleService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
            useAccessToken = false;
        }

        public ModuleService(HttpClient httpClient, TokenProvider tokenProvider)
        {
            this.httpClient = httpClient;
            this.tokenProvider = tokenProvider;
            useAccessToken = true;
        }

        public async Task<IEnumerable<Module>> GetModulesAsync()
        {
            if (useAccessToken)
            {
                var token = tokenProvider.AccessToken;
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            }

            return await JsonSerializer.DeserializeAsync<IEnumerable<Module>>
                (await httpClient.GetStreamAsync(Controllers.MODULES).ConfigureAwait(false),
                    new JsonSerializerOptions(JsonSerializerDefaults.Web)).ConfigureAwait(false);
        }
    }
}