﻿using Headway.Core.Attributes;
using Headway.Core.Constants;
using Headway.Core.Interface;
using Headway.Core.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Headway.WebApi.Controllers
{
    [DynamicApiController]
    public class PermissionsController : ApiModelControllerBase<Permission, PermissionsController>
    {
        private readonly IAuthorisationRepository authorisationRepository;

        public PermissionsController(
            IAuthorisationRepository authorisationRepository,
            ILogger<PermissionsController> logger)
            : base(authorisationRepository, logger)
        {
            this.authorisationRepository = authorisationRepository;
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

            var permissions = await authorisationRepository
                .GetPermissionsAsync()
                .ConfigureAwait(false);

            return Ok(permissions);
        }

        [HttpGet("{permissionId}")]
        public override async Task<IActionResult> Get(int permissionId)
        {
            var authorised = await IsAuthorisedAsync(HeadwayAuthorisation.ADMIN)
                .ConfigureAwait(false);

            if (!authorised)
            {
                return Unauthorized();
            }

            var permissions = await authorisationRepository
                .GetPermissionAsync(permissionId)
                .ConfigureAwait(false);

            return Ok(permissions);
        }

        [HttpPost]
        public override async Task<IActionResult> Post([FromBody] Permission permission)
        {
            var authorised = await IsAuthorisedAsync(HeadwayAuthorisation.ADMIN)
                .ConfigureAwait(false);

            if (!authorised)
            {
                return Unauthorized();
            }

            var savedPermission = await authorisationRepository
                .AddPermissionAsync(permission)
                .ConfigureAwait(false);

            return Ok(savedPermission);
        }

        [HttpPut]
        public override async Task<IActionResult> Put([FromBody] Permission permission)
        {
            var authorised = await IsAuthorisedAsync(HeadwayAuthorisation.ADMIN)
                .ConfigureAwait(false);

            if (!authorised)
            {
                return Unauthorized();
            }

            var savedPermission = await authorisationRepository
                .UpdatePermissionAsync(permission)
                .ConfigureAwait(false);

            return Ok(savedPermission);
        }

        [HttpDelete("{permissionId}")]
        public override async Task<IActionResult> Delete(int permissionId)
        {
            var authorised = await IsAuthorisedAsync(HeadwayAuthorisation.ADMIN)
                .ConfigureAwait(false);

            if (!authorised)
            {
                return Unauthorized();
            }

            var result = await authorisationRepository
                .DeletePermissionAsync(permissionId)
                .ConfigureAwait(false);

            return Ok(result);
        }
    }
}
