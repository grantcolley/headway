using Headway.Core.Interface;
using Headway.Core.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        public async Task<IActionResult> GetUsers()
        {
            var claim = GetUserClaim();

            if (await authorisationRepository.IsAuthorisedAsync(claim, "Admin"))
            {
                var users = await authorisationRepository.GetUsersAsync(claim).ConfigureAwait(false);
                return Ok(users);
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUser(int userId)
        {
            var claim = GetUserClaim();

            if (await authorisationRepository.IsAuthorisedAsync(claim, "Admin"))
            {
                var user = await authorisationRepository.GetUserAsync(claim, userId).ConfigureAwait(false);
                return Ok(user);
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpPost("{user}")]
        public async Task<IActionResult> SaveUser(User user)
        {
            var claim = GetUserClaim();

            if (await authorisationRepository.IsAuthorisedAsync(claim, "Admin"))
            {
                var savedUser = await authorisationRepository.SaveUserAsync(claim, user).ConfigureAwait(false);
                return Ok(savedUser);
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            var claim = GetUserClaim();

            if (await authorisationRepository.IsAuthorisedAsync(claim, "Admin"))
            {
                var result = await authorisationRepository.DeleteUserAsync(claim, userId).ConfigureAwait(false);
                return Ok(result);
            }
            else
            {
                return Unauthorized();
            }
        }
    }
}
