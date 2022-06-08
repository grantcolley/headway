using Headway.Core.Attributes;
using Headway.Core.Constants;
using Headway.Core.Interface;
using Headway.Core.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Headway.WebApi.Controllers
{
    [DynamicApiController]
    public class PermissionsController : ApiControllerBase<PermissionsController>
    {
        private readonly IAuthorisationRepository authorisationRepository;

        public PermissionsController(
            IAuthorisationRepository authorisationRepository,
            ILogger<PermissionsController> logger)
            : base(authorisationRepository, logger)
        {
            this.authorisationRepository = authorisationRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var authorised = await IsAuthorisedAsync(Permissions.ADMIN)
                .ConfigureAwait(false);

            if (!authorised)
            {
                return Unauthorized();
            }

            var permissions = await authorisationRepository
                .GetPermissionsAsync()
                .ConfigureAwait(false);

            return Ok(permissions);
        }

        [HttpGet("{permissionId}")]
        public async Task<IActionResult> Get(int permissionId)
        {
            var authorised = await IsAuthorisedAsync(Permissions.ADMIN)
                .ConfigureAwait(false);

            if (!authorised)
            {
                return Unauthorized();
            }

            var permissions = await authorisationRepository
                .GetPermissionAsync(permissionId)
                .ConfigureAwait(false);

            return Ok(permissions);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Permission permission)
        {
            var authorised = await IsAuthorisedAsync(Permissions.ADMIN)
                .ConfigureAwait(false);

            if (!authorised)
            {
                return Unauthorized();
            }

            var savedPermission = await authorisationRepository
                .AddPermissionAsync(permission)
                .ConfigureAwait(false);

            return Ok(savedPermission);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Permission permission)
        {
            var authorised = await IsAuthorisedAsync(Permissions.ADMIN)
                .ConfigureAwait(false);

            if (!authorised)
            {
                return Unauthorized();
            }

            var savedPermission = await authorisationRepository
                .UpdatePermissionAsync(permission)
                .ConfigureAwait(false);

            return Ok(savedPermission);
        }

        [HttpDelete("{permissionId}")]
        public async Task<IActionResult> Delete(int permissionId)
        {
            var authorised = await IsAuthorisedAsync(Permissions.ADMIN)
                .ConfigureAwait(false);

            if (!authorised)
            {
                return Unauthorized();
            }

            var result = await authorisationRepository
                .DeletePermissionAsync(permissionId)
                .ConfigureAwait(false);

            return Ok(result);
        }
    }
}
