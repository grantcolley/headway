using Headway.Core.Interface;
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
            this.tokenProvider = tokenProvider;

            AddHttpClientAuthorisationHeader();
        }

        protected ServiceBase(HttpClient httpClient, bool useAccessToken)
        {
            this.httpClient = httpClient;
            this.useAccessToken = useAccessToken;
        }

        public async Task<IServiceResult<T>> GetServiceResultAsync<T>(HttpResponseMessage httpResponseMessage)
        {
            var serviceResult = new ServiceResult<T>
            {
                IsSuccess = httpResponseMessage.IsSuccessStatusCode,
                Message = httpResponseMessage.ReasonPhrase
            };

            if (serviceResult.IsSuccess)
            {
                serviceResult.Result = await JsonSerializer.DeserializeAsync<T>
                    (await httpResponseMessage.Content.ReadAsStreamAsync().ConfigureAwait(false),
                        new JsonSerializerOptions(JsonSerializerDefaults.Web)).ConfigureAwait(false);
            }

            return serviceResult;
        }

        protected static IServiceResult<T> GetServiceResult<T>(T result, bool isSuccess = true, string message = null)
        {
            var serviceResult = new ServiceResult<T>
            {
                IsSuccess = isSuccess,
                Message = message,
                Result = result
            };

            return serviceResult;
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
