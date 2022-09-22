using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication.Internal;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace Headway.App.Blazor.WebAssembly.Account
{
    public class UserAccountFactory : AccountClaimsPrincipalFactory<RemoteUserAccount>
    {
        public UserAccountFactory(IAccessTokenProviderAccessor accessor) : base(accessor)
        {
        }

        public async override ValueTask<ClaimsPrincipal> CreateUserAsync(RemoteUserAccount account,
                                                                         RemoteAuthenticationUserOptions options)
        {
            var user = await base.CreateUserAsync(account, options);

            if (user?.Identity?.IsAuthenticated ?? false)
            {
                var identity = (ClaimsIdentity)user.Identity;
                account.AdditionalProperties.TryGetValue(ClaimTypes.Role, out var roleClaims);

                if (roleClaims != null
                    && roleClaims is JsonElement element
                    && element.ValueKind == JsonValueKind.Array)
                {
                    identity.RemoveClaim(identity.FindFirst(ClaimTypes.Role));

                    var claims = element.EnumerateArray()
                        .Select(c => new Claim(ClaimTypes.Role, c.ToString()));

                    identity.AddClaims(claims);
                }
            }

            return user ?? new ClaimsPrincipal();
        }
    }
}
