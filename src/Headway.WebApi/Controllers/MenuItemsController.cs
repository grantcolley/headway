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
    public class MenuItemsController : ApiControllerBase<MenuItemsController>
    {
        private readonly IModuleRepository moduleRepository;

        public MenuItemsController(
            IModuleRepository moduleRepository,
            ILogger<MenuItemsController> logger)
            : base((IRepository)moduleRepository, logger)
        {
            this.moduleRepository = moduleRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var authorised = await IsAuthorisedAsync(HeadwayAuthorisation.ADMIN)
                .ConfigureAwait(false);

            if (!authorised)
            {
                return Unauthorized();
            }

            var menuItems = await moduleRepository
                .GetMenuItemsAsync()
                .ConfigureAwait(false);

            return Ok(menuItems);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var authorised = await IsAuthorisedAsync(HeadwayAuthorisation.ADMIN)
                .ConfigureAwait(false);

            if (!authorised)
            {
                return Unauthorized();
            }

            var menuItem = await moduleRepository
                .GetMenuItemAsync(id)
                .ConfigureAwait(false);

            return Ok(menuItem);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] MenuItem menuItem)
        {
            var authorised = await IsAuthorisedAsync(HeadwayAuthorisation.ADMIN)
                .ConfigureAwait(false);

            if (!authorised)
            {
                return Unauthorized();
            }

            var savedMenuItem = await moduleRepository
                .AddMenuItemAsync(menuItem)
                .ConfigureAwait(false);

            return Ok(savedMenuItem);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] MenuItem menuItem)
        {
            var authorised = await IsAuthorisedAsync(HeadwayAuthorisation.ADMIN)
                .ConfigureAwait(false);

            if (!authorised)
            {
                return Unauthorized();
            }

            var savedMenuItem = await moduleRepository
                .UpdateMenuItemAsync(menuItem)
                .ConfigureAwait(false);

            return Ok(savedMenuItem);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var authorised = await IsAuthorisedAsync(HeadwayAuthorisation.ADMIN)
                .ConfigureAwait(false);

            if (!authorised)
            {
                return Unauthorized();
            }

            var result = await moduleRepository
                .DeleteMenuItemAsync(id)
                .ConfigureAwait(false);

            return Ok(result);
        }
    }
}
