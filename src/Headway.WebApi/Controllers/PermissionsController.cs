using Headway.Core.Interface;
using Headway.Core.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Headway.WebApi.Controllers
{
    [ApiController]
    [EnableCors("local")]
    [Route("[controller]")]
    [Authorize(Roles = "headwayuser")]
    public class PermissionsController : ControllerBase
    {
        private readonly ILogger<UsersController> logger;
        private readonly IAuthorisationRepository authorisationRepository;

        public PermissionsController(
            IAuthorisationRepository authorisationRepository,
            ILogger<UsersController> logger)
        {
            this.authorisationRepository = authorisationRepository;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<Permission>> GetPermissions()
        {
            var claim = GetUserClaim();

            return await authorisationRepository.GetPermissionsAsync(claim);
        }

        [HttpGet("{permissionId}")]
        public async Task<Permission> GetPermission(int permissionId)
        {
            var claim = GetUserClaim();

            return await authorisationRepository.GetPermissionAsync(claim, permissionId);
        }

        [HttpPost("{permission}")]
        public async Task<Permission> SaveUser(Permission permission)
        {
            var claim = GetUserClaim();

            return await authorisationRepository.SavePermissionAsync(claim, permission);
        }

        [HttpDelete("{permissionId}")]
        public async Task DeleteUser(int permissionId)
        {
            var claim = GetUserClaim();

            await authorisationRepository.DeletePermissionAsync(claim, permissionId);
        }
    }
}
