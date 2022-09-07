using Headway.Core.Attributes;
using Headway.Core.Constants;
using Headway.Core.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Headway.WebApi.Controllers
{
    [DynamicApiController]
    public class LogController : ApiControllerBase<LogController>
    {
        private readonly ILogRepository logRepository;

        public LogController(
            ILogRepository logRepository,
            ILogger<LogController> logger)
            : base(logRepository, logger)
        {
            this.logRepository = logRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Core.Model.Log log)
        {
            var authorised = await IsAuthorisedAsync(HeadwayAuthorisation.USER)
                .ConfigureAwait(false);

            if (!authorised)
            {
                return Unauthorized();
            }

            logRepository.LogAsync(log);

            return Ok();
        }
    }
}