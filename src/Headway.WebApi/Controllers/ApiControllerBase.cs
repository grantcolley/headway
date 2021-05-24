using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace Headway.WebApi.Controllers
{
    [ApiController]
    public abstract class ApiControllerBase<T> : ControllerBase
    {
        protected readonly ILogger<T> logger;
        protected readonly string claim;

        protected ApiControllerBase(ILogger<T> logger)
        {
            this.logger = logger;
            claim = GetUserClaim();
        }

        protected string GetUserClaim()
        {
            var identity = (ClaimsIdentity)HttpContext.User.Identity;
            var claim = identity.FindFirst(ClaimTypes.Email);
            return claim.Value;
        }
    }
}