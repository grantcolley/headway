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
    public class FlowController : ApiControllerBase<FlowController>
    {
        private readonly IFlowRepository flowRepository;

        public FlowController(
            IFlowRepository flowRepository,
            ILogger<FlowController> logger)
            : base(flowRepository, logger)
        {
            this.flowRepository = flowRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var authorised = await IsAuthorisedAsync(HeadwayAuthorisation.ADMIN)
                .ConfigureAwait(false);

            if (!authorised)
            {
                return Unauthorized();
            }

            var flows = await flowRepository
                .GetFlowsAsync()
                .ConfigureAwait(false);

            return Ok(flows);
        }

        [HttpGet("{flowId}")]
        public async Task<IActionResult> Get(int flowId)
        {
            var authorised = await IsAuthorisedAsync(HeadwayAuthorisation.ADMIN)
                .ConfigureAwait(false);

            if (!authorised)
            {
                return Unauthorized();
            }

            var flow = await flowRepository
                .GetFlowAsync(flowId)
                .ConfigureAwait(false);

            return Ok(flow);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Flow flow)
        {
            var authorised = await IsAuthorisedAsync(HeadwayAuthorisation.ADMIN)
                .ConfigureAwait(false);

            if (!authorised)
            {
                return Unauthorized();
            }

            var savedFlow = await flowRepository
                .AddFlowAsync(flow)
                .ConfigureAwait(false);

            return Ok(savedFlow);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Flow flow)
        {
            var authorised = await IsAuthorisedAsync(HeadwayAuthorisation.ADMIN)
                .ConfigureAwait(false);

            if (!authorised)
            {
                return Unauthorized();
            }

            var savedFlow = await flowRepository
                .UpdateFlowAsync(flow)
                .ConfigureAwait(false);

            return Ok(savedFlow);
        }

        [HttpDelete("{flowId}")]
        public async Task<IActionResult> Delete(int flowId)
        {
            var authorised = await IsAuthorisedAsync(HeadwayAuthorisation.ADMIN)
                .ConfigureAwait(false);

            if (!authorised)
            {
                return Unauthorized();
            }

            var result = await flowRepository
                .DeleteFlowAsync(flowId)
                .ConfigureAwait(false);

            return Ok(result);
        }
    }
}
