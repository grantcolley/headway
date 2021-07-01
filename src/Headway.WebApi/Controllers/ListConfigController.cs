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
    public class ListConfigController : ApiControllerBase<ModelConfigController>
    {
        private readonly IConfigRepository configRepository;

        public ListConfigController(
            IConfigRepository configRepository,
            ILogger<ModelConfigController> logger)
            : base(configRepository, logger)
        {
            this.configRepository = configRepository;
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
                .GetListConfigAsync(model)
                .ConfigureAwait(false);

            return Ok(config);
        }
    }
}
