using Headway.Core.Attributes;
using Headway.Core.Interface;
using Headway.Core.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Headway.WebApi.Controllers
{
    [DynamicApiController]
    public class RolesController : ApiControllerBase<RolesController>
    {
        private readonly IAuthorisationRepository authorisationRepository;

        public RolesController(
            IAuthorisationRepository authorisationRepository,
            ILogger<RolesController> logger)
            : base(authorisationRepository, logger)
        {
            this.authorisationRepository = authorisationRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var authorised = await IsAuthorisedAsync("Admin")
                .ConfigureAwait(false);

            if (!authorised)
            {
                return Unauthorized();
            }

            var permissions = await authorisationRepository
                .GetRolesAsync()
                .ConfigureAwait(false);

            return Ok(permissions);
        }

        [HttpGet("{roleId}")]
        public async Task<IActionResult> Get(int roleId)
        {
            var authorised = await IsAuthorisedAsync("Admin")
                .ConfigureAwait(false);

            if (!authorised)
            {
                return Unauthorized();
            }

            var permissions = await authorisationRepository
                .GetRoleAsync(roleId)
                .ConfigureAwait(false);

            return Ok(permissions);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Role role)
        {
            var authorised = await IsAuthorisedAsync("Admin")
                .ConfigureAwait(false);

            if (!authorised)
            {
                return Unauthorized();
            }

            var savedPermission = await authorisationRepository
                .AddRoleAsync(role)
                .ConfigureAwait(false);

            return Ok(savedPermission);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Role role)
        {
            var authorised = await IsAuthorisedAsync("Admin")
                .ConfigureAwait(false);

            if (!authorised)
            {
                return Unauthorized();
            }

            var savedPermission = await authorisationRepository
                .UpdateRoleAsync(role)
                .ConfigureAwait(false);

            return Ok(savedPermission);
        }

        [HttpDelete("{roleId}")]
        public async Task<IActionResult> Delete(int roleId)
        {
            var authorised = await IsAuthorisedAsync("Admin")
                .ConfigureAwait(false);

            if (!authorised)
            {
                return Unauthorized();
            }

            var result = await authorisationRepository
                .DeleteRoleAsync(roleId)
                .ConfigureAwait(false);

            return Ok(result);
        }
    }
}
