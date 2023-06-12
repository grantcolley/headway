using Headway.Core.Args;
using Headway.Core.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RemediatR.Core.Constants;
using RemediatR.Core.Interface;
using RemediatR.Core.Model;
using System.Threading.Tasks;

namespace Headway.WebApi.Controllers
{
    [DynamicApiController]
    public class RemediatRCustomerController : ApiModelControllerBase<Customer, RemediatRCustomerController>
    {
        private readonly IRemediatRRepository remediatRRepository;

        public RemediatRCustomerController(
            IRemediatRRepository repository, 
            ILogger<RemediatRCustomerController> logger) 
            : base(repository, logger)
        {
            this.remediatRRepository = repository;
        }

        [HttpPost("[action]")]
        public override async Task<IActionResult> Search([FromBody] SearchArgs searchArgs)
        {
            var authorised = await IsAuthorisedAsync(RemediatRAuthorisation.CUSTOMER_READ)
                .ConfigureAwait(false);

            if (!authorised)
            {
                return Unauthorized();
            }

            var customers = await remediatRRepository
                .GetCustomersAsync(searchArgs)
                .ConfigureAwait(false);

            return Ok(customers);
        }

        [HttpGet]
        public override async Task<IActionResult> Get()
        {
            var authorised = await IsAuthorisedAsync(RemediatRAuthorisation.CUSTOMER_READ)
                .ConfigureAwait(false);

            if (!authorised)
            {
                return Unauthorized();
            }

            var customers = await remediatRRepository
                .GetCustomersAsync()
                .ConfigureAwait(false);

            return Ok(customers);
        }

        [HttpGet("{customerId}")]
        public override async Task<IActionResult> Get(int customerId)
        {
            var authorised = await IsAuthorisedAsync(RemediatRAuthorisation.CUSTOMER_READ)
                .ConfigureAwait(false);

            if (!authorised)
            {
                return Unauthorized();
            }

            var customer = await remediatRRepository
                .GetCustomerAsync(customerId)
                .ConfigureAwait(false);

            return Ok(customer);
        }

        [HttpPost]
        public override async Task<IActionResult> Post([FromBody] Customer customer)
        {
            var authorised = await IsAuthorisedAsync(RemediatRAuthorisation.CUSTOMER_WRITE)
                .ConfigureAwait(false);

            if (!authorised)
            {
                return Unauthorized();
            }

            var savedCustomer = await remediatRRepository
                .AddCustomerAsync(customer)
                .ConfigureAwait(false);

            return Ok(savedCustomer);
        }

        [HttpPut]
        public override async Task<IActionResult> Put([FromBody] Customer customer)
        {
            var authorised = await IsAuthorisedAsync(RemediatRAuthorisation.CUSTOMER_WRITE)
                .ConfigureAwait(false);

            if (!authorised)
            {
                return Unauthorized();
            }

            var savedCustomer = await remediatRRepository
                .UpdateCustomerAsync(customer)
                .ConfigureAwait(false);

            return Ok(savedCustomer);
        }

        [HttpDelete("{customerId}")]
        public override async Task<IActionResult> Delete(int customerId)
        {
            var authorised = await IsAuthorisedAsync(RemediatRAuthorisation.CUSTOMER_WRITE)
                .ConfigureAwait(false);

            if (!authorised)
            {
                return Unauthorized();
            }

            var result = await remediatRRepository
                .DeleteCustomerAsync(customerId)
                .ConfigureAwait(false);

            return Ok(result);
        }
    }
}
