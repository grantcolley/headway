using Headway.Core.Attributes;
using Headway.Core.Constants;
using Headway.RemediatR.Core.Interface;
using Headway.RemediatR.Core.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Headway.WebApi.Controllers
{
    [DynamicApiController]
    public class RemediatRCustomerController : ApiControllerBase<RemediatRCustomerController>
    {
        private readonly IRemediatRRepository remediatRRepository;

        public RemediatRCustomerController(
            IRemediatRRepository repository, 
            ILogger<RemediatRCustomerController> logger) 
            : base(repository, logger)
        {
            this.remediatRRepository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var authorised = await IsAuthorisedAsync(Roles.REMEDIATR_CUSTOMER_READ)
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
        public async Task<IActionResult> Get(int customerId)
        {
            var authorised = await IsAuthorisedAsync(Roles.REMEDIATR_CUSTOMER_READ)
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
        public async Task<IActionResult> Post([FromBody] Customer customer)
        {
            var authorised = await IsAuthorisedAsync(Roles.REMEDIATR_CUSTOMER_WRITE)
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
        public async Task<IActionResult> Put([FromBody] Customer customer)
        {
            var authorised = await IsAuthorisedAsync(Roles.REMEDIATR_CUSTOMER_WRITE)
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
        public async Task<IActionResult> Delete(int customerId)
        {
            var authorised = await IsAuthorisedAsync(Roles.REMEDIATR_CUSTOMER_WRITE)
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
