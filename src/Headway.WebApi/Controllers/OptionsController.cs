using Headway.Core.Interface;
using Headway.Core.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Text.Json;
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

        [HttpGet("{optionsCode}/{args}")]
        public async Task<IActionResult> Get(string optionsCode, string args)
        {
            var arguments = JsonSerializer.Deserialize<List<Arg>>(args);

            var authorised = await IsAuthorisedAsync("User")
                .ConfigureAwait(false);

            if (!authorised)
            {
                return Unauthorized();
            }

            var permissions = await optionsRepository
                .GetOptionItemsAsync(optionsCode, arguments)
                .ConfigureAwait(false);

            return Ok(permissions);
        }
    }
}
