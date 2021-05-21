using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Headway.WebApi.Controllers
{
    public abstract class ControllerBase : Controller
    {
        protected string GetUserClaim()
        {
            var identity = (ClaimsIdentity)HttpContext.User.Identity;
            var claim = identity.FindFirst(ClaimTypes.Email);
            return claim.Value;
        }
    }
}