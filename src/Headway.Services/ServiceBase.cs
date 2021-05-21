using Headway.Core.Model;
using System.Net.Http;

namespace Headway.Services
{
    public abstract class ServiceBase
    {
        protected readonly HttpClient httpClient;
        protected readonly TokenProvider tokenProvider;
        protected readonly bool useAccessToken;

        protected ServiceBase(HttpClient httpClient, bool useAccessToken, TokenProvider tokenProvider)
            : this(httpClient, useAccessToken)
        {
            this.tokenProvider = tokenProvider;
        }

        protected ServiceBase(HttpClient httpClient, bool useAccessToken)
        {
            this.httpClient = httpClient;
            this.useAccessToken = useAccessToken;
        }

        protected void AddHttpClientAuthorisationHeader()
        {
            if (useAccessToken)
            {
                var token = tokenProvider.AccessToken;
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            }
        }
    }
}
