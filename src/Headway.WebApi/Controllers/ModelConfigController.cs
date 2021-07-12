using Headway.Core.Interface;
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
    public class ModelConfigController : ApiControllerBase<ModelConfigController>
    {
        private readonly IConfigRepository configRepository;

        public ModelConfigController(
            IConfigRepository configRepository,
            ILogger<ModelConfigController> logger)
            : base(configRepository, logger)
        {
            this.configRepository = configRepository;
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

            var config = await configRepository
                .GetModelConfigsAsync()
                .ConfigureAwait(false);

            return Ok(config);
        }

        [HttpGet("{model}")]
        public async Task<IActionResult> Get(string model)
        {
            var authorised = await IsAuthorisedAsync("Admin")
                .ConfigureAwait(false);

            if (!authorised)
            {
                return Unauthorized();
            }

            var config = await configRepository
                .GetModelConfigAsync(model)
                .ConfigureAwait(false);

            return Ok(config);
        }
    }
}
