using Headway.Core.Attributes;
using Headway.Core.Constants;
using Headway.Core.Interface;
using Headway.Core.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Headway.WebApi.Controllers
{
    [DynamicApiController]
    public class UsersController : ModelControllerBase<User, UsersController>
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
        public override async Task<IActionResult> Get()
        {
            var authorised = await IsAuthorisedAsync(HeadwayAuthorisation.ADMIN)
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
        public override async Task<IActionResult> Get(int userId)
        {
            var authorised = await IsAuthorisedAsync(HeadwayAuthorisation.ADMIN)
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
        public override async Task<IActionResult> Post([FromBody] User user)
        {
            var authorised = await IsAuthorisedAsync(HeadwayAuthorisation.ADMIN)
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
        public override async Task<IActionResult> Put([FromBody] User user)
        {
            var authorised = await IsAuthorisedAsync(HeadwayAuthorisation.ADMIN)
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
        public override async Task<IActionResult> Delete(int userId)
        {
            var authorised = await IsAuthorisedAsync(HeadwayAuthorisation.ADMIN)
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
