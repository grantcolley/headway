﻿using Headway.Core.Args;
using Headway.Core.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RemediatR.Core.Constants;
using RemediatR.Core.Interface;
using RemediatR.Core.Model;
using RemediatR.Repository.Constants;
using System;
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

        [HttpGet("{redressId}")]
        public async Task<IActionResult> Get(int redressId)
        {
            var authorisedUser = await GetAuthorisedUserAsync(RemediatRAuthorisation.REDRESS_READ)
                .ConfigureAwait(false);

            if (authorisedUser == null)
            {
                return Unauthorized();
            }

            var redress = await remediatRRepository
                .GetRedressAsync(redressId, authorisedUser)
                .ConfigureAwait(false);

            return Ok(redress);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Search([FromBody] SearchArgs searchArgs)
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

        [HttpPost("[action]")]
        public async Task<IActionResult> Create([FromBody] DataArgs dataArgs)
        {
            var authorisedUser = await GetAuthorisedUserAsync(RemediatRAuthorisation.REDRESS_CASE_OWNER_WRITE)
                .ConfigureAwait(false);

            if (authorisedUser == null)
            {
                return Unauthorized();
            }

            var redressCases = await remediatRRepository
                .CreateRedressAsync(dataArgs, authorisedUser)
                .ConfigureAwait(false);

            return Ok(redressCases);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Redress redress)
        {
            var authorisedUser = await GetAuthorisedUserAsync(RemediatRAuthorisation.REDRESS_CASE_OWNER_WRITE)
                .ConfigureAwait(false);

            if (authorisedUser == null)
            {
                return Unauthorized();
            }

            var savedRedress = await remediatRRepository
                .AddRedressAsync(redress, authorisedUser)
                .ConfigureAwait(false);

            return Ok(savedRedress);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Redress redress)
        {
            var authorisedUser = await GetAuthorisedUserAsync(RemediatRAuthorisation.REDRESS_CASE_OWNER_WRITE)
                .ConfigureAwait(false);

            if (authorisedUser == null)
            {
                return Unauthorized();
            }

            var savedRedress = await remediatRRepository
                .UpdateRedressAsync(redress, authorisedUser)
                .ConfigureAwait(false);

            return Ok(savedRedress);
        }

        [HttpDelete("{redressId}")]
        public async Task<IActionResult> Delete(int redressId)
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