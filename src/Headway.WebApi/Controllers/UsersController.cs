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
    public class UsersController : ApiControllerBase<UsersController>
    {
        private readonly IAuthorisationRepository authorisationRepository;

        public UsersController(
            IAuthorisationRepository authorisationRepository,
            ILogger<UsersController> logger)
            : base(logger)
        {
            this.authorisationRepository = authorisationRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var claim = GetUserClaim();

            var authorised = await authorisationRepository.IsAuthorisedAsync(claim, "Admin")
                                                            .ConfigureAwait(false);
            if (!authorised)
            {
                return Unauthorized();
            }

            var users = await authorisationRepository
                                .GetUsersAsync(claim)
                                .ConfigureAwait(false);
            return Ok(users);
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> Get(int userId)
        {
            var claim = GetUserClaim();

            var authorised = await authorisationRepository.IsAuthorisedAsync(claim, "Admin")
                                                            .ConfigureAwait(false);
            if (!authorised)
            {
                return Unauthorized();
            }

            var user = await authorisationRepository
                                .GetUserAsync(claim, userId)
                                .ConfigureAwait(false);
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] User user)
        {
            var claim = GetUserClaim();

            var authorised = await authorisationRepository.IsAuthorisedAsync(claim, "Admin")
                                                            .ConfigureAwait(false);
            if (!authorised)
            {
                return Unauthorized();
            }

            var savedUser = await authorisationRepository
                                        .AddUserAsync(claim, user)
                                        .ConfigureAwait(false);
            return Ok(savedUser);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] User user)
        {
            var claim = GetUserClaim();

            var authorised = await authorisationRepository.IsAuthorisedAsync(claim, "Admin")
                                                            .ConfigureAwait(false);
            if (!authorised)
            {
                return Unauthorized();
            }

            var savedUser = await authorisationRepository
                                    .UpdateUserAsync(claim, user)
                                    .ConfigureAwait(false);
            return Ok(savedUser);
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            var claim = GetUserClaim();

            var authorised = await authorisationRepository.IsAuthorisedAsync(claim, "Admin")
                                                            .ConfigureAwait(false);
            if (!authorised)
            {
                return Unauthorized();
            }

            var result = await authorisationRepository
                                    .DeleteUserAsync(claim, userId)
                                    .ConfigureAwait(false);
            return Ok(result);
        }
    }
}
