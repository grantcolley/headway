using Headway.Core.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Headway.WebApi.Controllers
{
    public class OptionsController : ApiControllerBase<OptionsController>
    {
        private readonly IOptionsRepository optionsRepository;

        public OptionsController(
            IOptionsRepository optionsRepository,
            ILogger<OptionsController> logger)
            : base(optionsRepository, logger)
        {
            this.optionsRepository = optionsRepository;
        }

        [HttpGet("{optionsCode}")]
        public async Task<IActionResult> Get(string optionsCode)
        {
            var authorised = await IsAuthorisedAsync("User")
                .ConfigureAwait(false);

            if (!authorised)
            {
                return Unauthorized();
            }

            var permissions = await optionsRepository
                .GetOptionItemsAsync(optionsCode)
                .ConfigureAwait(false);

            return Ok(permissions);
        }
    }
}
