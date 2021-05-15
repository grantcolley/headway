using Headway.Core.Interface;
using Headway.Core.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Headway.WebApi.Controllers
{
    [ApiController]
    [EnableCors("local")]
    [Route("[controller]")]
    [Authorize(Roles = "headwayuser")]
    public class UsersController : Controller
    {
        private readonly ILogger<UsersController> logger;
        private readonly IUserRepository userRepository;

        public UsersController(
            IUserRepository userRepository,
            ILogger<UsersController> logger)
        {
            this.userRepository = userRepository;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<User>> GetUsers()
        {
            var identity = (ClaimsIdentity)HttpContext.User.Identity;
            var claim = identity.FindFirst(ClaimTypes.Email);
            return await userRepository.GetUsersAsync(claim.Value);
        }

        [HttpGet("{userName}")]
        public async Task<User> GetUser(string userName)
        {
            var identity = (ClaimsIdentity)HttpContext.User.Identity;
            var claim = identity.FindFirst(ClaimTypes.Name);
            return await userRepository.GetUserAsync(claim.Value, userName);
        }

        [HttpPost("{userName}")]
        public async Task<User> SaveUser(User userName)
        {
            var identity = (ClaimsIdentity)HttpContext.User.Identity;
            var claim = identity.FindFirst(ClaimTypes.Name);
            return await userRepository.SaveUserAsync(claim.Value, userName);
        }

        [HttpDelete("{userName}")]
        public async Task DeleteUser(string userName)
        {
            var identity = (ClaimsIdentity)HttpContext.User.Identity;
            var claim = identity.FindFirst(ClaimTypes.Name);
            await userRepository.DeleteUserAsync(claim.Value, userName);
        }
    }
}
