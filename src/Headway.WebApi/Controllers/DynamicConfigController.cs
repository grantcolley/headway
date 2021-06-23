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
    public class DynamicConfigController : ApiControllerBase<DynamicConfigController>
    {
        private readonly IDynamicConfigRepository dynamicRepository;

        public DynamicConfigController(
            IDynamicConfigRepository dynamicRepository,
            ILogger<DynamicConfigController> logger)
            : base(dynamicRepository, logger)
        {
            this.dynamicRepository = dynamicRepository;
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

            var config = await dynamicRepository
                .GetDynamicModelConfigAsync(model)
                .ConfigureAwait(false);

            return Ok(config);
        }
    }
}
