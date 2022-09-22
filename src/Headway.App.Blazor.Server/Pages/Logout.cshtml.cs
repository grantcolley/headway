using Auth0.AspNetCore.Authentication;
using Headway.Core.Constants;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace Headway.App.Blazor.Server.Pages
{
    [Authorize]
    public class LogoutModel : PageModel
    {
        private readonly string identityProvider;

        public LogoutModel(IConfiguration configuration)
        {
            identityProvider = configuration["IdentityProvider:DefaultProvider"];
        }

        public async Task<IActionResult> OnPost()
        {
            if (identityProvider.Equals(IdentityProvider.IDENTITY_SERVER_4))
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme).ConfigureAwait(false);
                var idToken = await HttpContext.GetTokenAsync("id_token").ConfigureAwait(false);
                var requestUrl = new RequestUrl("https://localhost:5001/connect/endsession");
                var url = requestUrl.CreateEndSessionUrl(idTokenHint: idToken, postLogoutRedirectUri: "https://localhost:44300/");
                return Redirect(url);
            }
            else
            {
                var authenticationProperties = new LogoutAuthenticationPropertiesBuilder()
                     .WithRedirectUri("/")
                     .Build();

                await HttpContext.SignOutAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                return Redirect("/");
            }
        }
    }
}
