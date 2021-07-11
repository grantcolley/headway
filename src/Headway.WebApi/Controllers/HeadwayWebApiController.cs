using Headway.Core.Attributes;
using Headway.Core.Helpers;
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
    public class HeadwayWebApiController : ApiControllerBase<HeadwayWebApiController>
    {
        private readonly IHeadwayWebApiRepository headwayWebApiRepository;

        public HeadwayWebApiController(
            IHeadwayWebApiRepository headwayWebApiRepository,
            ILogger<HeadwayWebApiController> logger)
            : base(headwayWebApiRepository, logger)
        {
            this.headwayWebApiRepository = headwayWebApiRepository;
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> Get(string name)
        {
            var authorised = await IsAuthorisedAsync("Admin")
                .ConfigureAwait(false);

            if (!authorised)
            {
                return Unauthorized();
            }

            var controllers = TypeAttributeHelper.GetGetEntryAssemblyAttributeImplemters(typeof(DynamicApiControllerAttribute));

            return Ok(controllers);
        }
    }
}
