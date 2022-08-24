using Headway.Core.Attributes;
using Headway.Core.Model;
using Headway.RemediatR.Core.Constants;
using Headway.RemediatR.Core.Interface;
using Headway.RemediatR.Core.Model;
using Headway.RemediatR.Repository.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Headway.WebApi.Controllers
{
    [DynamicApiController]
    public class RemediatRRedressController : ApiControllerBase<RemediatRRedressController>
    {
        private readonly IRemediatRRepository remediatRRepository;

        public RemediatRRedressController(
            IRemediatRRepository repository, 
            ILogger<RemediatRRedressController> logger) 
            : base(repository, logger)
        {
            this.remediatRRepository = repository;
        }

        //[HttpGet]
        //public async Task<IActionResult> Get()
        //{
        //    var authorised = await IsAuthorisedAsync(RemediatRAuthorisation.REDRESS_READ)
        //        .ConfigureAwait(false);

        //    if (!authorised)
        //    {
        //        return Unauthorized();
        //    }

        //    var redresses = await remediatRRepository
        //        .GetRedressesAsync()
        //        .ConfigureAwait(false);

        //    return Ok(redresses);
        //}

        [HttpGet("{redressId}")]
        public async Task<IActionResult> Get(int redressId)
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

        [HttpPost("[action]")]
        public async Task<IActionResult> Search([FromBody] SearchCriteria searchCriteria)
        {
            var authorised = await IsAuthorisedAsync(RemediatRAuthorisation.REDRESS_READ)
                .ConfigureAwait(false);

            if (!authorised)
            {
                return Unauthorized();
            }

            switch(searchCriteria.SourceConfig)
            {
                case RemediatRSearchSource.REDRESSCASES:
                    var redressCases = await remediatRRepository
                        .GetRedressCasesAsync(searchCriteria)
                        .ConfigureAwait(false);
                    return Ok(redressCases);
                case RemediatRSearchSource.NEW_REDRESS_CASE:
                    var newRedressCases = await remediatRRepository
                        .SearchNewRedressCasesAsync(searchCriteria)
                        .ConfigureAwait(false);
                    return Ok(newRedressCases);
                default:
                    throw new NotImplementedException(searchCriteria.SourceConfig);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Redress redress)
        {
            var authorised = await IsAuthorisedAsync(RemediatRAuthorisation.REDRESS_WRITE)
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
        public async Task<IActionResult> Put([FromBody] Redress redress)
        {
            var authorised = await IsAuthorisedAsync(RemediatRAuthorisation.REDRESS_WRITE)
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
        public async Task<IActionResult> Delete(int redressId)
        {
            var authorised = await IsAuthorisedAsync(RemediatRAuthorisation.REDRESS_WRITE)
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