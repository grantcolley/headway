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
    public class MenuController : Controller
    {
        private readonly ILogger<MenuController> logger;
        private readonly IMenuRepository menuRepository;

        public MenuController(
            IMenuRepository menuRepository,
            ILogger<MenuController> logger)
        {
            this.menuRepository = menuRepository;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<MenuItem>> Get()
        {
            var identity = (ClaimsIdentity)HttpContext.User.Identity;
            var claim = identity.FindFirst(ClaimTypes.Email);
            return await menuRepository.GetMenuItemsAsync(claim.Value);
        }
    }
}