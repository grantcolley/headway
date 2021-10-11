using Headway.Core.Attributes;
using Headway.Core.Constants;
using Headway.Core.Interface;
using Headway.Core.Model;
using Headway.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Headway.WebApi.Controllers
{
    [DynamicApiController]
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

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var authorised = await IsAuthorisedAsync(Roles.ADMIN)
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
            var authorised = await IsAuthorisedAsync(Roles.ADMIN)
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
            var authorised = await IsAuthorisedAsync(Roles.ADMIN)
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

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Config config)
        {
            var authorised = await IsAuthorisedAsync(Roles.ADMIN)
                .ConfigureAwait(false);

            if (!authorised)
            {
                return Unauthorized();
            }

            var savedConfig = await configRepository
                .AddConfigAsync(config)
                .ConfigureAwait(false);

            return Ok(savedConfig);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Config config)
        {
            var authorised = await IsAuthorisedAsync(Roles.ADMIN)
                .ConfigureAwait(false);

            if (!authorised)
            {
                return Unauthorized();
            }

            var savedConfig = await configRepository
                .UpdateConfigAsync(config)
                .ConfigureAwait(false);

            return Ok(savedConfig);
        }

        [HttpDelete("{configId}")]
        public async Task<IActionResult> Delete(int configId)
        {
            var authorised = await IsAuthorisedAsync(Roles.ADMIN)
                .ConfigureAwait(false);

            if (!authorised)
            {
                return Unauthorized();
            }

            var result = await configRepository
                .DeleteConfigAsync(configId)
                .ConfigureAwait(false);

            return Ok(result);
        }
    }
}
