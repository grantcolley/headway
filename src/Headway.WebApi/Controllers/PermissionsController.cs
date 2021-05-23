using Headway.Core.Interface;
using Headway.Core.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Headway.WebApi.Controllers
{
    [ApiController]
    [EnableCors("local")]
    [Route("[controller]")]
    [Authorize(Roles = "headwayuser")]
    public class PermissionsController : ControllerBase
    {
        private readonly ILogger<UsersController> logger;
        private readonly IAuthorisationRepository authorisationRepository;

        public PermissionsController(
            IAuthorisationRepository authorisationRepository,
            ILogger<UsersController> logger)
        {
            this.authorisationRepository = authorisationRepository;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetPermissions()
        {
            var claim = GetUserClaim();

            if (await authorisationRepository.IsAuthorisedAsync(claim, "Admin"))
            {
                var permissions = await authorisationRepository.GetPermissionsAsync(claim)
                                                                .ConfigureAwait(false);
                return Ok(permissions);
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpGet("{permissionId}")]
        public async Task<IActionResult> GetPermission(int permissionId)
        {
            var claim = GetUserClaim();

            if (await authorisationRepository.IsAuthorisedAsync(claim, "Admin")
                                                .ConfigureAwait(false))
            {
                var permission = await authorisationRepository.GetPermissionAsync(claim, permissionId)
                                                                .ConfigureAwait(false);
                return Ok(permission);
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpPost("{permission}")]
        public async Task<IActionResult> SaveUser(Permission permission)
        {
            var claim = GetUserClaim();

            if (await authorisationRepository.IsAuthorisedAsync(claim, "Admin")
                                                .ConfigureAwait(false))
            {
                var savedPermission = await authorisationRepository.SavePermissionAsync(claim, permission)
                                                                    .ConfigureAwait(false);
                return Ok(savedPermission);
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpDelete("{permissionId}")]
        public async Task<IActionResult> DeleteUser(int permissionId)
        {
            var claim = GetUserClaim();

            if (await authorisationRepository.IsAuthorisedAsync(claim, "Admin")
                                                .ConfigureAwait(false))
            {
                var result = await authorisationRepository.DeletePermissionAsync(claim, permissionId)
                                                            .ConfigureAwait(false);
                return Ok(result);
            }
            else
            {
                return Unauthorized();
            }
        }
    }
}
