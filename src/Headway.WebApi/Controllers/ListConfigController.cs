//using Headway.Core.Interface;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Cors;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;
//using System.Threading.Tasks;

//namespace Headway.WebApi.Controllers
//{
//    [ApiController]
//    [EnableCors("local")]
//    [Route("[controller]")]
//    [Authorize(Roles = "headwayuser")]
//    public class ListConfigController : ApiControllerBase<ListConfigController>
//    {
//        private readonly IConfigurationRepository configRepository;

//        public ListConfigController(
//            IConfigurationRepository configRepository,
//            ILogger<ListConfigController> logger)
//            : base(configRepository, logger)
//        {
//            this.configRepository = configRepository;
//        }

//        [HttpGet]
//        public async Task<IActionResult> Get()
//        {
//            var authorised = await IsAuthorisedAsync("Admin")
//                .ConfigureAwait(false);

//            if (!authorised)
//            {
//                return Unauthorized();
//            }

//            var config = await configRepository
//                .GetListConfigsAsync()
//                .ConfigureAwait(false);

//            return Ok(config);
//        }

//        [HttpGet("{name}")]
//        public async Task<IActionResult> Get(string name)
//        {
//            var authorised = await IsAuthorisedAsync("Admin")
//                .ConfigureAwait(false);

//            if (!authorised)
//            {
//                return Unauthorized();
//            }

//            var config = await configRepository
//                .GetListConfigAsync(name)
//                .ConfigureAwait(false);

//            return Ok(config);
//        }
//    }
//}
