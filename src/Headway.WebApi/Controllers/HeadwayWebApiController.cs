using Headway.Core.Attributes;
using Headway.Core.Helpers;
using Headway.Core.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Headway.WebApi.Controllers
{
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

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var authorised = await IsAuthorisedAsync("User")
                .ConfigureAwait(false);

            if (!authorised)
            {
                return Unauthorized();
            }

            var controllers = TypeAttributeHelper.GetEntryAssemblyTypeNamesByAttribute(typeof(DynamicApiControllerAttribute));

            return Ok(controllers);
        }
    }
}
