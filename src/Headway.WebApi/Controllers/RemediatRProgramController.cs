using Headway.Core.Attributes;
using Headway.Core.Constants;
using Headway.RemediatR.Core.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Headway.WebApi.Controllers
{
    [DynamicApiController]
    public class RemediatRProgramController : ApiControllerBase<RemediatRProgramController>
    {
        private readonly IRemediatRRepository remediatRRepository;

        public RemediatRProgramController(
            IRemediatRRepository repository, 
            ILogger<RemediatRProgramController> logger) 
            : base(repository, logger)
        {
            this.remediatRRepository = repository;
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

            var programs = await remediatRRepository
                .GetProgramsAsync()
                .ConfigureAwait(false);

            return Ok(programs);
        }

        [HttpGet("{programId}")]
        public async Task<IActionResult> Get(int programId)
        {
            var authorised = await IsAuthorisedAsync(HeadwayAuthorisation.ADMIN)
                .ConfigureAwait(false);

            if (!authorised)
            {
                return Unauthorized();
            }

            var program = await remediatRRepository
                .GetProgramAsync(programId)
                .ConfigureAwait(false);

            return Ok(program);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RemediatR.Core.Model.Program program)
        {
            var authorised = await IsAuthorisedAsync(HeadwayAuthorisation.ADMIN)
                .ConfigureAwait(false);

            if (!authorised)
            {
                return Unauthorized();
            }

            var savedProgram = await remediatRRepository
                .AddProgramAsync(program)
                .ConfigureAwait(false);

            return Ok(savedProgram);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] RemediatR.Core.Model.Program program)
        {
            var authorised = await IsAuthorisedAsync(HeadwayAuthorisation.ADMIN)
                .ConfigureAwait(false);

            if (!authorised)
            {
                return Unauthorized();
            }

            var savedProgram = await remediatRRepository
                .UpdateProgramAsync(program)
                .ConfigureAwait(false);

            return Ok(savedProgram);
        }

        [HttpDelete("{programId}")]
        public async Task<IActionResult> Delete(int programId)
        {
            var authorised = await IsAuthorisedAsync(HeadwayAuthorisation.ADMIN)
                .ConfigureAwait(false);

            if (!authorised)
            {
                return Unauthorized();
            }

            var result = await remediatRRepository
                .DeleteProgramAsync(programId)
                .ConfigureAwait(false);

            return Ok(result);
        }
    }
}
