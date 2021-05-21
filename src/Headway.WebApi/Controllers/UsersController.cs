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
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> logger;
        private readonly IAuthorisationRepository authorisationRepository;

        public UsersController(
            IAuthorisationRepository authorisationRepository,
            ILogger<UsersController> logger)
        {
            this.authorisationRepository = authorisationRepository;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<User>> GetUsers()
        {
            var claim = GetUserClaim();

            return await authorisationRepository.GetUsersAsync(claim);
        }

        [HttpGet("{userId}")]
        public async Task<User> GetUser(int userId)
        {
            var claim = GetUserClaim();

            return await authorisationRepository.GetUserAsync(claim, userId);
        }

        [HttpPost("{user}")]
        public async Task<User> SaveUser(User user)
        {
            var claim = GetUserClaim();

            return await authorisationRepository.SaveUserAsync(claim, user);
        }

        [HttpDelete("{userId}")]
        public async Task DeleteUser(int userId)
        {
            var claim = GetUserClaim();

            await authorisationRepository.DeleteUserAsync(claim, userId);
        }
    }
}
