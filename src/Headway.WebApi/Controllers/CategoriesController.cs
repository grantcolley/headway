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
    public class CategoriesController : ApiControllerBase<CategoriesController>
    {
        private readonly IModuleRepository moduleRepository;

        public CategoriesController(
            IModuleRepository moduleRepository,
            ILogger<CategoriesController> logger)
            : base((IRepository)moduleRepository, logger)
        {
            this.moduleRepository = moduleRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var authorised = await IsAuthorisedAsync(Roles.ADMIN)
                .ConfigureAwait(false);

            if (!authorised)
            {
                return Unauthorized();
            }

            var categories = await moduleRepository
                .GetCategoriesAsync()
                .ConfigureAwait(false);

            return Ok(categories);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var authorised = await IsAuthorisedAsync(Roles.ADMIN)
                .ConfigureAwait(false);

            if (!authorised)
            {
                return Unauthorized();
            }

            var category = await moduleRepository
                .GetCategoryAsync(id)
                .ConfigureAwait(false);

            return Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Category category)
        {
            var authorised = await IsAuthorisedAsync(Roles.ADMIN)
                .ConfigureAwait(false);

            if (!authorised)
            {
                return Unauthorized();
            }

            var savedCategory = await moduleRepository
                .AddCategoryAsync(category)
                .ConfigureAwait(false);

            return Ok(savedCategory);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Category category)
        {
            var authorised = await IsAuthorisedAsync(Roles.ADMIN)
                .ConfigureAwait(false);

            if (!authorised)
            {
                return Unauthorized();
            }

            var savedCategory = await moduleRepository
                .UpdateCategoryAsync(category)
                .ConfigureAwait(false);

            return Ok(savedCategory);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var authorised = await IsAuthorisedAsync(Roles.ADMIN)
                .ConfigureAwait(false);

            if (!authorised)
            {
                return Unauthorized();
            }

            var result = await moduleRepository
                .DeleteCategoryAsync(id)
                .ConfigureAwait(false);

            return Ok(result);
        }
    }
}
