using Headway.Core.Attributes;
using Headway.Core.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RemediatR.Core.Interface;
using System.Threading.Tasks;

namespace Headway.WebApi.Controllers
{
    [DynamicApiController]
    public class RemediatRProgramController : ApiModelControllerBase<RemediatR.Core.Model.Program, RemediatRProgramController>
    {
        private readonly IRemediatRProgramRepository remediatRProgramRepository;

        public RemediatRProgramController(
            IRemediatRProgramRepository remediatRProgramRepository, 
            ILogger<RemediatRProgramController> logger) 
            : base(remediatRProgramRepository, logger)
        {
            this.remediatRProgramRepository = remediatRProgramRepository;
        }

        [HttpGet]
        public override async Task<IActionResult> Get()
        {
            var authorised = await IsAuthorisedAsync(HeadwayAuthorisation.ADMIN)
                .ConfigureAwait(false);

            if (!authorised)
            {
                return Unauthorized();
            }

            var programs = await remediatRProgramRepository
                .GetProgramsAsync()
                .ConfigureAwait(false);

            return Ok(programs);
        }

        [HttpGet("{programId}")]
        public override async Task<IActionResult> Get(int programId)
        {
            var authorised = await IsAuthorisedAsync(HeadwayAuthorisation.ADMIN)
                .ConfigureAwait(false);

            if (!authorised)
            {
                return Unauthorized();
            }

            var program = await remediatRProgramRepository
                .GetProgramAsync(programId)
                .ConfigureAwait(false);

            return Ok(program);
        }

        [HttpPost]
        public override async Task<IActionResult> Post([FromBody] RemediatR.Core.Model.Program program)
        {
            var authorised = await IsAuthorisedAsync(HeadwayAuthorisation.ADMIN)
                .ConfigureAwait(false);

            if (!authorised)
            {
                return Unauthorized();
            }

            var savedProgram = await remediatRProgramRepository
                .AddProgramAsync(program)
                .ConfigureAwait(false);

            return Ok(savedProgram);
        }

        [HttpPut]
        public override async Task<IActionResult> Put([FromBody] RemediatR.Core.Model.Program program)
        {
            var authorised = await IsAuthorisedAsync(HeadwayAuthorisation.ADMIN)
                .ConfigureAwait(false);

            if (!authorised)
            {
                return Unauthorized();
            }

            var savedProgram = await remediatRProgramRepository
                .UpdateProgramAsync(program)
                .ConfigureAwait(false);

            return Ok(savedProgram);
        }

        [HttpDelete("{programId}")]
        public override async Task<IActionResult> Delete(int programId)
        {
            var authorised = await IsAuthorisedAsync(HeadwayAuthorisation.ADMIN)
                .ConfigureAwait(false);

            if (!authorised)
            {
                return Unauthorized();
            }

            var result = await remediatRProgramRepository
                .DeleteProgramAsync(programId)
                .ConfigureAwait(false);

            return Ok(result);
        }
    }
}
