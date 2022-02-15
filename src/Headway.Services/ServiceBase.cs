using Headway.Core.Interface;
using Headway.Core.Mediators;
using Headway.Core.Model;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Headway.Services
{
    public abstract class ServiceBase : IService
    {
        protected readonly HttpClient httpClient;
        protected readonly TokenProvider tokenProvider;
        protected readonly bool useAccessToken;

        protected ServiceBase(HttpClient httpClient, bool useAccessToken, TokenProvider tokenProvider)
            : this(httpClient, useAccessToken)
        {
            if (tokenProvider != null)
            {
                this.tokenProvider = tokenProvider;

                AddHttpClientAuthorisationHeader();
            }
        }

        protected ServiceBase(HttpClient httpClient, bool useAccessToken)
        {
            this.httpClient = httpClient;
            this.useAccessToken = useAccessToken;
        }

        public async Task<IResponse<T>> GetResponseAsync<T>(HttpResponseMessage httpResponseMessage)
        {
            var response = new Response<T>
            {
                IsSuccess = httpResponseMessage.IsSuccessStatusCode,
                Message = httpResponseMessage.ReasonPhrase
            };

            if (response.IsSuccess)
            {
                response.Result = await JsonSerializer.DeserializeAsync<T>
                    (await httpResponseMessage.Content.ReadAsStreamAsync().ConfigureAwait(false),
                        new JsonSerializerOptions(JsonSerializerDefaults.Web)).ConfigureAwait(false);
            }

            return response;
        }

        protected static IResponse<T> GetResponseResult<T>(T result, bool isSuccess = true, string message = null)
        {
            var response = new Response<T>
            {
                IsSuccess = isSuccess,
                Message = message,
                Result = result
            };

            return response;
        }

        private void AddHttpClientAuthorisationHeader()
        {
            if (useAccessToken
                && tokenProvider != null
                && httpClient.DefaultRequestHeaders.Authorization == null)
            {
                var token = tokenProvider.AccessToken;
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            }
        }
    }
}
