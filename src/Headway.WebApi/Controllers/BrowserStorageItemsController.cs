using Headway.Core.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Headway.WebApi.Controllers
{
    [ApiController]
    [EnableCors("local")]
    [Route("[controller]")]
    [Authorize(Roles = "headwayuser")]
    public class BrowserStorageItemsController : ApiControllerBase<BrowserStorageItemsController>
    {
        private readonly IBrowserStorageRepository browserStorageRepository;

        public BrowserStorageItemsController(
            IBrowserStorageRepository browserStorageRepository,
            ILogger<BrowserStorageItemsController> logger)
            : base(browserStorageRepository, logger)
        {
            this.browserStorageRepository = browserStorageRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var authorised = await IsAuthorisedAsync("User")
                .ConfigureAwait(false);

            if (!authorised)
            {
                return Unauthorized();
            }

            var browserStorageItems = await browserStorageRepository
                .GetBrowserStorageItemsAsync()
                .ConfigureAwait(false);

            return Ok(browserStorageItems);
        }
    }
}
