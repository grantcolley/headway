using Headway.Core.Args;
using Headway.Core.Attributes;
using Headway.Core.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RemediatR.Core.Constants;
using RemediatR.Core.Interface;
using RemediatR.Core.Model;
using RemediatR.Repository;
using RemediatR.Repository.Constants;
using System;
using System.Threading.Tasks;

namespace Headway.WebApi.Controllers
{
    [DynamicApiController]
    public class RemediatRRedressController : ApiModelFlowControllerBase<Redress, RemediatRRedressController, RedressFlowContext>
    {
        private readonly IRemediatRRepository<RedressFlowContext> remediatRRepository;
        private readonly IRedressFlowContextExecutionService redressFlowContextExecutionService;

        public RemediatRRedressController(
            IRemediatRRepository<RedressFlowContext> repository,
            IRedressFlowContextExecutionService redressFlowContextExecutionService,
            ILogger<RemediatRRedressController> logger) 
            : base(repository, logger)
        {
            this.remediatRRepository = repository;
            this.redressFlowContextExecutionService = redressFlowContextExecutionService;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Create([FromBody] DataArgs dataArgs)
        {
            var authorised = await IsAuthorisedAsync(RemediatRAuthorisation.REDRESS_CASE_OWNER_WRITE)
                .ConfigureAwait(false);

            if (!authorised)
            {
                return Unauthorized();
            }

            var redressCases = await remediatRRepository
                .CreateRedressAsync(dataArgs)
                .ConfigureAwait(false);

            return Ok(redressCases);
        }

        [HttpPut("[action]")]
        public override async Task<IActionResult> FlowExecution([FromBody] Redress redress)
        {
            var authorised = IsFlowUserAuthenticatedAsync(redress.RedressFlowContext);

            if (!authorised)
            {
                return Unauthorized();
            }

            var redressFlowContext = await redressFlowContextExecutionService.Execute(redress.RedressFlowContext);
            
            var savedRedress = await remediatRRepository
                .UpdateRedressAsync(redress)
                .ConfigureAwait(false);

            return Ok(savedRedress);
        }

        [HttpPost("[action]")]
        public override async Task<IActionResult> Search([FromBody] SearchArgs searchArgs)
        {
            var authorised = await IsAuthorisedAsync(RemediatRAuthorisation.REDRESS_READ)
                .ConfigureAwait(false);

            if (!authorised)
            {
                return Unauthorized();
            }

            switch (searchArgs.SourceConfig)
            {
                case RemediatRSearchSource.REDRESSCASES:
                    var redressCases = await remediatRRepository
                        .GetRedressCasesAsync(searchArgs)
                        .ConfigureAwait(false);
                    return Ok(redressCases);
                case RemediatRSearchSource.NEW_REDRESS_CASE:
                    var newRedressCases = await remediatRRepository
                        .SearchNewRedressCasesAsync(searchArgs)
                        .ConfigureAwait(false);
                    return Ok(newRedressCases);
                default:
                    throw new NotImplementedException(searchArgs.SourceConfig);
            }
        }

        [HttpGet]
        public override Task<IActionResult> Get()
        {
            throw new NotImplementedException();
        }

        [HttpGet("{redressId}")]
        public override async Task<IActionResult> Get(int redressId)
        {
            var authorised = await IsAuthorisedAsync(RemediatRAuthorisation.REDRESS_READ)
                .ConfigureAwait(false);

            if (!authorised)
            {
                return Unauthorized();
            }

            var redress = await remediatRRepository
                .GetRedressAsync(redressId)
                .ConfigureAwait(false);

            return Ok(redress);
        }

        [HttpPost]
        public override async Task<IActionResult> Post([FromBody] Redress redress)
        {
            var authorised = await IsAuthorisedAsync(RemediatRAuthorisation.REDRESS_CASE_OWNER_WRITE)
                .ConfigureAwait(false);

            if (!authorised)
            {
                return Unauthorized();
            }

            var savedRedress = await remediatRRepository
                .AddRedressAsync(redress)
                .ConfigureAwait(false);

            return Ok(savedRedress);
        }

        [HttpPut]
        public override async Task<IActionResult> Put([FromBody] Redress redress)
        {
            var authorised = await IsAuthorisedAsync(RemediatRAuthorisation.REDRESS_CASE_OWNER_WRITE)
                .ConfigureAwait(false);

            if (!authorised)
            {
                return Unauthorized();
            }

            var savedRedress = await remediatRRepository
                .UpdateRedressAsync(redress)
                .ConfigureAwait(false);

            return Ok(savedRedress);
        }

        [HttpDelete("{redressId}")]
        public override async Task<IActionResult> Delete(int redressId)
        {
            var authorised = await IsAuthorisedAsync(RemediatRAuthorisation.REDRESS_CASE_OWNER_WRITE)
                .ConfigureAwait(false);

            if (!authorised)
            {
                return Unauthorized();
            }

            var result = await remediatRRepository
                .DeleteRedressAsync(redressId)
                .ConfigureAwait(false);

            return Ok(result);
        }
    }
}