using Headway.Core.Interface;
using Headway.Core.Model;
using Headway.RazorComponents.Model;
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

            AddHttpClientAuthorisationHeader();
        }

        protected ServiceBase(HttpClient httpClient, NavigationManager navigationManager, bool useAccessToken)
        {
            this.httpClient = httpClient;
            this.navigationManager = navigationManager;
            this.useAccessToken = useAccessToken;
        }

        public bool IsSuccessStatusCode(HttpResponseMessage httpResponseMessage)
        {
            if(!httpResponseMessage.IsSuccessStatusCode)
            {
                var alert = new Alert
                {
                    AlertType = "danger",
                    Title = "Error",
                    Message = httpResponseMessage.ReasonPhrase
                };

                navigationManager.NavigateTo(alert.Page);
            }

            return httpResponseMessage.IsSuccessStatusCode;
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
