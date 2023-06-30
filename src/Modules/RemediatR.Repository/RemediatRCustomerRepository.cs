using Headway.Core.Args;
using Headway.Repository.Data;
using Headway.Repository.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RemediatR.Core.Interface;
using RemediatR.Core.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RemediatR.Repository
{
    public class RemediatRCustomerRepository : RepositoryBase<RemediatRCustomerRepository>, IRemediatRCustomerRepository
    {
        public RemediatRCustomerRepository(ApplicationDbContext applicationDbContext, ILogger<RemediatRCustomerRepository> logger)
            : base(applicationDbContext, logger)
        {
        }

        public async Task<IEnumerable<Customer>> GetCustomersAsync(SearchArgs searchArgs)
        {
            var customerIdArg = searchArgs.Args.First(c => c.ParameterName.Equals("CustomerId"));
            var surnameArg = searchArgs.Args.First(c => c.ParameterName.Equals("Surname"));

            int customerId = 0;
            string surname = string.Empty;

            if (!string.IsNullOrWhiteSpace(customerIdArg.Value))
            {
                _ = int.TryParse(customerIdArg.Value, out customerId);
            }

            if (!string.IsNullOrWhiteSpace(surnameArg.Value))
            {
                surname = surnameArg.Value.ToLowerInvariant();
            }

            if (customerId > 0)
            {
                var customer = await applicationDbContext.Customers
                    .AsNoTracking()
                    .FirstAsync(c => c.CustomerId.Equals(customerId))
                    .ConfigureAwait(false);

                if (customer != null)
                {
                    return new List<Customer>(new[] { customer });
                }

                return new List<Customer>();
            }

            if (!string.IsNullOrWhiteSpace(surname))
            {
                return await applicationDbContext.Customers
                    .AsNoTracking()
                    .Where(c => !string.IsNullOrWhiteSpace(c.Surname)
                                && c.Surname.ToLower().Contains(surname))
                    .ToListAsync()
                    .ConfigureAwait(false);
            }

            return await GetCustomersAsync().ConfigureAwait(false);
        }

        public async Task<IEnumerable<Customer>> GetCustomersAsync()
        {
            return await applicationDbContext.Customers
                .AsNoTracking()
                .ToListAsync()
                .ConfigureAwait(false);
        }

        public async Task<Customer> GetCustomerAsync(int id)
        {
            return await applicationDbContext.Customers
                .AsNoTracking()
                .Include(c => c.Products)
                .FirstAsync(c => c.CustomerId.Equals(id))
                .ConfigureAwait(false);
        }

        public async Task<Customer> AddCustomerAsync(Customer customer)
        {
            await applicationDbContext.Customers
                .AddAsync(customer)
                .ConfigureAwait(false);

            await applicationDbContext
                .SaveChangesAsync()
                .ConfigureAwait(false);

            return customer;
        }

        public async Task<Customer> UpdateCustomerAsync(Customer updateCustomer)
        {
            var customer = await applicationDbContext.Customers
                .Include(c => c.Products)
                .FirstAsync(c => c.CustomerId.Equals(updateCustomer.CustomerId))
                .ConfigureAwait(false);

            if (customer.Title != updateCustomer.Title) customer.Title = updateCustomer.Title;
            if (customer.FirstName != updateCustomer.FirstName) customer.FirstName = updateCustomer.FirstName;
            if (customer.Surname != updateCustomer.Surname) customer.Surname = updateCustomer.Surname;
            if (customer.Telephone != updateCustomer.Telephone) customer.Telephone = updateCustomer.Telephone;
            if (customer.Email != updateCustomer.Email) customer.Email = updateCustomer.Email;
            if (customer.AccountNumber != updateCustomer.AccountNumber) customer.AccountNumber = updateCustomer.AccountNumber;
            if (customer.SortCode != updateCustomer.SortCode) customer.SortCode = updateCustomer.SortCode;
            if (customer.AccountStatus != updateCustomer.AccountStatus) customer.AccountStatus = updateCustomer.AccountStatus;
            if (customer.Address1 != updateCustomer.Address1) customer.Address1 = updateCustomer.Address1;
            if (customer.Address2 != updateCustomer.Address2) customer.Address2 = updateCustomer.Address2;
            if (customer.Address3 != updateCustomer.Address3) customer.Address3 = updateCustomer.Address3;
            if (customer.Address4 != updateCustomer.Address4) customer.Address4 = updateCustomer.Address4;
            if (customer.Address5 != updateCustomer.Address5) customer.Address5 = updateCustomer.Address5;
            if (customer.Country != updateCustomer.Country) customer.Country = updateCustomer.Country;
            if (customer.PostCode != updateCustomer.PostCode) customer.PostCode = updateCustomer.PostCode;

            var removeProducts = customer.Products
                .Where(cp => !updateCustomer.Products.Any(acp => acp.ProductId.Equals(cp.ProductId)))
                .ToList();

            foreach (var product in removeProducts)
            {
                customer.Products.Remove(product);
            }

            Product update(Product p, Product up)
            {
                if (p.Name != up.Name) p.Name = up.Name;
                if (p.ProductType != up.ProductType) p.ProductType = up.ProductType;
                if (p.RateType != up.RateType) p.RateType = up.RateType;
                if (p.RepaymentType != up.RepaymentType) p.RepaymentType = up.RepaymentType;
                if (p.StartDate != up.StartDate) p.StartDate = up.StartDate;
                if (p.Duration != up.Duration) p.Duration = up.Duration;
                if (p.Rate != up.Rate) p.Rate = up.Rate;
                if (p.Value != up.Value) p.Value = up.Value;

                applicationDbContext.Products.Update(p);

                return p;
            }

            _ = (from p in customer.Products
                 join up in updateCustomer.Products on p.ProductId equals up.ProductId
                 where !up.ProductId.Equals(0)
                 select update(p, up)).ToList();

            var addProducts = updateCustomer.Products
                .Where(acp => acp.ProductId.Equals(0))
                .ToList();

            customer.Products.AddRange(addProducts);

            applicationDbContext.Customers.Update(customer);

            await applicationDbContext
                .SaveChangesAsync()
                .ConfigureAwait(false);

            return customer;
        }

        public async Task<int> DeleteCustomerAsync(int id)
        {
            var customer = await applicationDbContext.Customers
                .Include(c => c.Products)
                .FirstAsync(c => c.CustomerId.Equals(id))
                .ConfigureAwait(false);

            for (int i = 0; i < customer.Products.Count; i++)
            {
                var product = customer.Products[i];
                customer.Products.Remove(product);
                applicationDbContext.Products.Remove(product);
            }

            applicationDbContext.Customers.Remove(customer);

            return await applicationDbContext
                .SaveChangesAsync()
                .ConfigureAwait(false);
        }
    }
}