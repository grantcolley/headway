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
    public class DemoModelController : ApiControllerBase<DemoModelController>
    {
        private readonly IDemoModelRepository demoModelRepository;

        public DemoModelController(
            IDemoModelRepository demoModelRepository,
            ILogger<DemoModelController> logger)
            : base(demoModelRepository, logger)
        {
            this.demoModelRepository = demoModelRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var authorised = await IsAuthorisedAsync(HeadwayAuthorisation.DEVELOPER)
                .ConfigureAwait(false);

            if (!authorised)
            {
                return Unauthorized();
            }

            var demoModels = await demoModelRepository
                .GetDemoModelsAsync()
                .ConfigureAwait(false);

            return Ok(demoModels);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var authorised = await IsAuthorisedAsync(HeadwayAuthorisation.DEVELOPER)
                .ConfigureAwait(false);

            if (!authorised)
            {
                return Unauthorized();
            }

            var demoModel = await demoModelRepository
                .GetDemoModelAsync(id)
                .ConfigureAwait(false);

            return Ok(demoModel);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] DemoModel demoModel)
        {
            var authorised = await IsAuthorisedAsync(HeadwayAuthorisation.DEVELOPER)
                .ConfigureAwait(false);

            if (!authorised)
            {
                return Unauthorized();
            }

            var savedDemoModel = await demoModelRepository
                .AddDemoModelAsync(demoModel)
                .ConfigureAwait(false);

            return Ok(savedDemoModel);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] DemoModel demoModel)
        {
            var authorised = await IsAuthorisedAsync(HeadwayAuthorisation.DEVELOPER)
                .ConfigureAwait(false);

            if (!authorised)
            {
                return Unauthorized();
            }

            var savedDemoModel = await demoModelRepository
                .UpdateDemoModelAsync(demoModel)
                .ConfigureAwait(false);

            return Ok(savedDemoModel);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var authorised = await IsAuthorisedAsync(HeadwayAuthorisation.DEVELOPER)
                .ConfigureAwait(false);

            if (!authorised)
            {
                return Unauthorized();
            }

            var result = await demoModelRepository
                .DeleteDemoModelAsync(id)
                .ConfigureAwait(false);

            return Ok(result);
        }
    }
}