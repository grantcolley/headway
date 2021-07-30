using Headway.Core.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Headway.WebApi.Controllers
{
    public class ConfigurationController : ApiControllerBase<ConfigurationController>
    {
        private readonly IConfigurationRepository configRepository;

        public ConfigurationController(
            IConfigurationRepository configRepository,
            ILogger<ConfigurationController> logger)
            : base(configRepository, logger)
        {
            this.configRepository = configRepository;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetConfigTypes()
        {
            var authorised = await IsAuthorisedAsync("Admin")
                .ConfigureAwait(false);

            if (!authorised)
            {
                return Unauthorized();
            }

            var config = await configRepository
                .GetConfigTypesAsync()
                .ConfigureAwait(false);

            return Ok(config);
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

            var config = await configRepository
                .GetConfigsAsync()
                .ConfigureAwait(false);

            return Ok(config);
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> Get(string name)
        {
            var authorised = await IsAuthorisedAsync("Admin")
                .ConfigureAwait(false);

            if (!authorised)
            {
                return Unauthorized();
            }

            var config = await configRepository
                .GetConfigAsync(name)
                .ConfigureAwait(false);

            return Ok(config);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var authorised = await IsAuthorisedAsync("Admin")
                .ConfigureAwait(false);

            if (!authorised)
            {
                return Unauthorized();
            }

            var config = await configRepository
                .GetConfigAsync(id)
                .ConfigureAwait(false);

            return Ok(config);
        }

        [HttpGet("[action]/{configtypeid}")]
        public async Task<IActionResult> GetConfigsByType(int configTypeId)
        {
            var authorised = await IsAuthorisedAsync("Admin")
                .ConfigureAwait(false);

            if (!authorised)
            {
                return Unauthorized();
            }

            var config = await configRepository
                .GetConfigsByTypeAsync(configTypeId)
                .ConfigureAwait(false);

            return Ok(config);
        }
    }
}
