using Auth0.AspNetCore.Authentication;
using Headway.Core.Constants;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace Headway.App.Blazor.Server.Pages
{
    public class LoginModel : PageModel
    {
        private readonly string identityProvider;

        public LoginModel(IConfiguration configuration)
        {
            identityProvider = configuration["IdentityProvider:DefaultProvider"];
        }

        public async Task OnGetAsync(string redirectUri)
        {
            if (string.IsNullOrWhiteSpace(redirectUri))
            {
                redirectUri = Url.Content("~/");
            }

            if (HttpContext.User.Identity.IsAuthenticated)
            {
                Response.Redirect(redirectUri);
            }

            if (identityProvider.Equals(IdentityProvider.IDENTITY_SERVER_4))
            {
                await HttpContext.ChallengeAsync(
                    OpenIdConnectDefaults.AuthenticationScheme,
                    new AuthenticationProperties { RedirectUri = redirectUri }).ConfigureAwait(false);
            }
            else
            {
                var authenticationProperties = new LoginAuthenticationPropertiesBuilder()
                    .WithRedirectUri(redirectUri)
                    .Build();

                await HttpContext.ChallengeAsync(
                    Auth0Constants.AuthenticationScheme, 
                    authenticationProperties).ConfigureAwait(false);
            }
        }
    }
}