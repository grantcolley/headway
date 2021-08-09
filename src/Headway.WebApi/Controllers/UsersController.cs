using Headway.Core.Attributes;
using Headway.Core.Interface;
using Headway.Core.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Headway.WebApi.Controllers
{
    [DynamicApiController]
    public class UsersController : ApiControllerBase<UsersController>
    {
        private readonly IAuthorisationRepository authorisationRepository;

        public UsersController(
            IAuthorisationRepository authorisationRepository,
            ILogger<UsersController> logger)
            : base(authorisationRepository, logger)
        {
            this.authorisationRepository = authorisationRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var authorised = await IsAuthorisedAsync("Admin")
                .ConfigureAwait(false);

            if (!authorised)
            {
                return Unauthorized();
            }

            var users = await authorisationRepository
                .GetUsersAsync()
                .ConfigureAwait(false);

            return Ok(users);
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> Get(int userId)
        {
            var authorised = await IsAuthorisedAsync("Admin")
                .ConfigureAwait(false);

            if (!authorised)
            {
                return Unauthorized();
            }

            var user = await authorisationRepository
                .GetUserAsync(userId)
                .ConfigureAwait(false);

            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] User user)
        {
            var authorised = await IsAuthorisedAsync("Admin")
                .ConfigureAwait(false);

            if (!authorised)
            {
                return Unauthorized();
            }

            var savedUser = await authorisationRepository
                .AddUserAsync(user)
                .ConfigureAwait(false);

            return Ok(savedUser);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] User user)
        {
            var authorised = await IsAuthorisedAsync("Admin")
                .ConfigureAwait(false);

            if (!authorised)
            {
                return Unauthorized();
            }

            var savedUser = await authorisationRepository
                .UpdateUserAsync(user)
                .ConfigureAwait(false);

            return Ok(savedUser);
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            var authorised = await IsAuthorisedAsync("Admin")
                .ConfigureAwait(false);

            if (!authorised)
            {
                return Unauthorized();
            }

            var result = await authorisationRepository
                .DeleteUserAsync(userId)
                .ConfigureAwait(false);

            return Ok(result);
        }
    }
}
