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
    public class ModulesController : ApiControllerBase<ModulesController>
    {
        private readonly IModuleRepository moduleRepository;

        public ModulesController(
            IModuleRepository moduleRepository,
            ILogger<ModulesController> logger)
            : base((IRepository)moduleRepository, logger)
        {
            this.moduleRepository = moduleRepository;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> ClaimModules()
        {
            var authorised = await IsAuthorisedAsync(Permissions.USER)
                .ConfigureAwait(false);

            if (!authorised)
            {
                return Unauthorized();
            }

            var claim = GetUserClaim();

            var modules = await moduleRepository.GetModulesAsync(claim).ConfigureAwait(false);

            return Ok(modules);
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

            var modules = await moduleRepository
                .GetModulesAsync()
                .ConfigureAwait(false);

            return Ok(modules);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var authorised = await IsAuthorisedAsync(Permissions.ADMIN)
                .ConfigureAwait(false);

            if (!authorised)
            {
                return Unauthorized();
            }

            var module = await moduleRepository
                .GetModuleAsync(id)
                .ConfigureAwait(false);

            return Ok(module);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Module module)
        {
            var authorised = await IsAuthorisedAsync(Permissions.ADMIN)
                .ConfigureAwait(false);

            if (!authorised)
            {
                return Unauthorized();
            }

            var savedModule = await moduleRepository
                .AddModuleAsync(module)
                .ConfigureAwait(false);

            return Ok(savedModule);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Module module)
        {
            var authorised = await IsAuthorisedAsync(Permissions.ADMIN)
                .ConfigureAwait(false);

            if (!authorised)
            {
                return Unauthorized();
            }

            var savedModule = await moduleRepository
                .UpdateModuleAsync(module)
                .ConfigureAwait(false);

            return Ok(savedModule);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var authorised = await IsAuthorisedAsync(Permissions.ADMIN)
                .ConfigureAwait(false);

            if (!authorised)
            {
                return Unauthorized();
            }

            var result = await moduleRepository
                .DeleteModuleAsync(id)
                .ConfigureAwait(false);

            return Ok(result);
        }
    }
}