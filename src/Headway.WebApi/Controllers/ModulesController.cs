using Headway.Core.Interface;
using Headway.Core.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Headway.WebApi.Controllers
{
    [ApiController]
    [EnableCors("local")]
    [Route("[controller]")]
    [Authorize(Roles = "headwayuser")]
    public class ModulesController : ApiControllerBase
    {
        private readonly ILogger<ModulesController> logger;
        private readonly IModuleRepository moduleRepository;

        public ModulesController(
            IModuleRepository moduleRepository,
            ILogger<ModulesController> logger)
        {
            this.moduleRepository = moduleRepository;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<Module>> Get()
        {
            var identity = (ClaimsIdentity)HttpContext.User.Identity;
            var claim = identity.FindFirst(ClaimTypes.Email);
            return await moduleRepository.GetModulesAsync(claim.Value);
        }
    }
}