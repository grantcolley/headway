using System.Collections.Generic;

namespace Headway.App.Blazor.Maui.Authentication
{
    public class CustomAuthenticationStateProviderOptions
    {
        public CustomAuthenticationStateProviderOptions()
        {
            Browser = new WebBrowserAuthenticator();
            AdditionalProviderParameters = new Dictionary<string, string>();
        }

        public string Authority { get; set; }

        public string ClientId { get; set; }

        public string RedirectUri { get; set; }

        public string PostLogoutRedirectUris { get; set; }

        public string Scope { get; set; }

        public string RoleClaim { get; set; }

        public Dictionary<string, string> AdditionalProviderParameters { get; set; }

        public IdentityModel.OidcClient.Browser.IBrowser Browser { get; set; }
    }
}
