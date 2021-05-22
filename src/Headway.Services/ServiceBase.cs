using Headway.Core.Interface;
using Headway.Core.Model;
using Microsoft.AspNetCore.Components;
using System.Net.Http;

namespace Headway.Services
{
    public abstract class ServiceBase : IService
    {
        protected readonly HttpClient httpClient;
        protected readonly TokenProvider tokenProvider;
        protected readonly bool useAccessToken;
        protected readonly NavigationManager navigationManager;

        protected ServiceBase(HttpClient httpClient, NavigationManager navigationManager, 
            bool useAccessToken, TokenProvider tokenProvider)
            : this(httpClient, navigationManager, useAccessToken)
        {
            this.tokenProvider = tokenProvider;
        }

        protected ServiceBase(HttpClient httpClient, NavigationManager navigationManager, bool useAccessToken)
        {
            this.httpClient = httpClient;
            this.navigationManager = navigationManager;
            this.useAccessToken = useAccessToken;
        }

        public void AddHttpClientAuthorisationHeader()
        {
            if (useAccessToken)
            {
                var token = tokenProvider.AccessToken;
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            }
        }

        public bool IsSuccessStatusCode(HttpResponseMessage httpResponseMessage)
        {
            if(!httpResponseMessage.IsSuccessStatusCode)
            {
                navigationManager.NavigateTo($"/error");
            }

            return httpResponseMessage.IsSuccessStatusCode;
        }
    }
}
