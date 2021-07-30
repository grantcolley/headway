using Headway.Core.Attributes;
using Headway.Core.Constants;
using Headway.Core.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Headway.WebApi.Controllers
{
    [ApiController]
    [DynamicApiController]
    [EnableCors("local")]
    [Route("[controller]")]
    [Authorize(Roles = "headwayuser")]
    public abstract class ApiControllerBase<T> : ControllerBase
    {
        protected readonly IRepository repository;
        protected readonly ILogger<T> logger;

        protected ApiControllerBase(IRepository repository, ILogger<T> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        protected string GetUserClaim()
        {
            var identity = (ClaimsIdentity)HttpContext.User.Identity;
            var claim = identity.FindFirst(System.Security.Claims.ClaimTypes.Email);
            return claim.Value;
        }

        protected async Task<bool> IsAuthorisedAsync(string permission)
        {
            var claim = GetUserClaim();

            return await repository
                .IsAuthorisedAsync(claim, permission)
                .ConfigureAwait(false);
        }
    }
}