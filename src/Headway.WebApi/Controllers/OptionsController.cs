using Headway.Core.Constants;
using Headway.Core.Interface;
using Headway.Core.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
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

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] List<Arg> args)
        {
            var authorised = await IsAuthorisedAsync(Roles.USER)
                .ConfigureAwait(false);

            if (!authorised)
            {
                return Unauthorized();
            }

            var options = await optionsRepository
                .GetOptionItemsAsync(args)
                .ConfigureAwait(false);

            return Ok(options);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> ComplexOptions([FromBody] List<Arg> args)
        {
            var authorised = await IsAuthorisedAsync(Roles.USER)
                .ConfigureAwait(false);

            if (!authorised)
            {
                return Unauthorized();
            }

            var options = await optionsRepository
                .GetComplexOptionItemsAsync(args)
                .ConfigureAwait(false);

            return Ok(options);
        }
    }
}
