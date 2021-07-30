using Headway.Core.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Headway.WebApi.Controllers
{
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

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var authorised = await IsAuthorisedAsync("User")
                .ConfigureAwait(false);

            if (!authorised)
            {
                return Unauthorized();
            }

            var claim = GetUserClaim();

            var modules = await moduleRepository.GetModulesAsync(claim).ConfigureAwait(false);

            return Ok(modules);
        }
    }
}