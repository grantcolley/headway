using Headway.Core.Model;
using Headway.RemediatR.Core.Enums;
using Headway.RemediatR.Core.Interface;
using Headway.RemediatR.Core.Model;
using Headway.Repository;
using Headway.Repository.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Headway.RemediatR.Repository
{
    public class RemediatRRepository : RepositoryBase<RemediatRRepository>, IRemediatRRepository
    {
        public RemediatRRepository(ApplicationDbContext applicationDbContext, ILogger<RemediatRRepository> logger)
            : base(applicationDbContext, logger)
        {
        }

        public async Task<Product> AddCustomerProductAsync(Customer customer, Product product)
        {
            await applicationDbContext.Products
                .AddAsync(product)
                .ConfigureAwait(false);

            await applicationDbContext
                .SaveChangesAsync()
                .ConfigureAwait(false);

            return product;
        }

        public async Task<int> DeleteCustomerProductAsync(Customer customer, Product product)
        {
            var deleteProduct = await applicationDbContext.Products
                .FirstAsync(p => p.ProductId.Equals(product.ProductId))
                .ConfigureAwait(false);

            applicationDbContext.Products.Remove(deleteProduct);

            return await applicationDbContext
                .SaveChangesAsync()
                .ConfigureAwait(false);
        }

        public async Task<IEnumerable<Customer>> GetCustomersAsync(SearchCriteria searchCriteria)
        {
            var customerIdClause = searchCriteria.Clauses.First(c => c.ParameterName.Equals("CustomerId"));
            var surnameClause = searchCriteria.Clauses.First(c => c.ParameterName.Equals("Surname"));

            int customerId = 0;
            string surname = string.Empty;


            if (!string.IsNullOrWhiteSpace(customerIdClause.Value))
            {
                _ = int.TryParse(customerIdClause.Value, out customerId);
            }

            if (!string.IsNullOrWhiteSpace(surnameClause.Value))
            {
                surname = surnameClause.Value.ToLowerInvariant();
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

        public async Task<Customer> UpdateCustomerAsync(Customer addCustomer)
        {
            var customer = await applicationDbContext.Customers
                .Include(c => c.Products)
                .FirstAsync(c => c.CustomerId.Equals(addCustomer.CustomerId))
                .ConfigureAwait(false);

            if (customer.Title != addCustomer.Title) customer.Title = addCustomer.Title;
            if (customer.FirstName != addCustomer.FirstName) customer.FirstName = addCustomer.FirstName;
            if (customer.Surname != addCustomer.Surname) customer.Surname = addCustomer.Surname;
            if (customer.Telephone != addCustomer.Telephone) customer.Telephone = addCustomer.Telephone;
            if (customer.Email != addCustomer.Email) customer.Email = addCustomer.Email;
            if (customer.AccountNumber != addCustomer.AccountNumber) customer.AccountNumber = addCustomer.AccountNumber;
            if (customer.SortCode != addCustomer.SortCode) customer.SortCode = addCustomer.SortCode;
            if (customer.AccountStatus != addCustomer.AccountStatus) customer.AccountStatus = addCustomer.AccountStatus;
            if (customer.Address1 != addCustomer.Address1) customer.Address1 = addCustomer.Address1;
            if (customer.Address2 != addCustomer.Address2) customer.Address2 = addCustomer.Address2;
            if (customer.Address3 != addCustomer.Address3) customer.Address3 = addCustomer.Address3;
            if (customer.Address4 != addCustomer.Address4) customer.Address4 = addCustomer.Address4;
            if (customer.Address5 != addCustomer.Address5) customer.Address5 = addCustomer.Address5;
            if (customer.Country != addCustomer.Country) customer.Country = addCustomer.Country;
            if (customer.PostCode != addCustomer.PostCode) customer.PostCode = addCustomer.PostCode;

            var removeProducts = customer.Products
                .Where(cp => !addCustomer.Products.Any(acp => acp.ProductId.Equals(cp.ProductId)))
                .ToList();

            foreach (var product in removeProducts)
            {
                customer.Products.Remove(product);
            }

            var addProducts = addCustomer.Products
                .Where(acp => !customer.Products.Any(cp => cp.ProductId.Equals(acp.ProductId)))
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

        public async Task<IEnumerable<Program>> GetProgramsAsync()
        {
            return await applicationDbContext.Programs
                .AsNoTracking()
                .ToListAsync()
                .ConfigureAwait(false);
        }

        public async Task<Program> GetProgramAsync(int id)
        {
            return await applicationDbContext.Programs
                .AsNoTracking()
                .FirstAsync(p => p.ProgramId.Equals(id))
                .ConfigureAwait(false);
        }

        public async Task<Program> AddProgramAsync(Program program)
        {
            await applicationDbContext.Programs
                .AddAsync(program)
                .ConfigureAwait(false);

            await applicationDbContext
                .SaveChangesAsync()
                .ConfigureAwait(false);

            return program;
        }

        public async Task<Program> UpdateProgramAsync(Program program)
        {
            applicationDbContext.Programs.Update(program);

            await applicationDbContext
                .SaveChangesAsync()
                .ConfigureAwait(false);

            return program;
        }

        public async Task<int> DeleteProgramAsync(int id)
        {
            var program = await applicationDbContext.Programs
                .FirstAsync(p => p.ProgramId.Equals(id))
                .ConfigureAwait(false);

            applicationDbContext.Programs.Remove(program);

            return await applicationDbContext
                .SaveChangesAsync()
                .ConfigureAwait(false);
        }

        public async Task<IEnumerable<RedressCase>> GetRedressesAsync()
        {
            var redressCases = await applicationDbContext.Redresses
                .Include(r => r.Customer)
                .Include(r => r.Program)
                .Include(r => r.Product)
                .AsNoTracking()
                .ToListAsync()
                .ConfigureAwait(false);

            return redressCases.Select(r => new RedressCase
                {
                    RedressId = r.RedressId,
                    CustomerName = r.CustomerName,
                    ProductName = r.ProductName,
                    ProgramName = r.ProgramName
                })
                .ToList();
        }

        public async Task<IEnumerable<RedressCase>> GetRedressesCasesAsync(SearchCriteria searchCriteria)
        {
            var programClause = searchCriteria.Clauses.First(c => c.ParameterName.Equals("Name"));
            var customerIdClause = searchCriteria.Clauses.First(c => c.ParameterName.Equals("CustomerId"));
            var surnameClause = searchCriteria.Clauses.First(c => c.ParameterName.Equals("Surname"));

            int customerId = 0;
            string surname = string.Empty;
            string program = string.Empty;

            if (!string.IsNullOrWhiteSpace(programClause.Value))
            {
                program = programClause.Value;
            }

            if (!string.IsNullOrWhiteSpace(customerIdClause.Value))
            {
                _ = int.TryParse(customerIdClause.Value, out customerId);
            }

            if (!string.IsNullOrWhiteSpace(surnameClause.Value))
            {
                surname = surnameClause.Value.ToLowerInvariant();
            }

            List<Redress> redresses;

            if (customerId.Equals(0)
                && string.IsNullOrWhiteSpace(surname)
                && string.IsNullOrWhiteSpace(program))
            {
                redresses = await applicationDbContext.Redresses
                    .Include(r => r.Customer)
                    .Include(r => r.Program)
                    .Include(r => r.Product)
                    .AsNoTracking()
                    .ToListAsync()
                    .ConfigureAwait(false);
            }
            else
            {
                if (string.IsNullOrWhiteSpace(program))
                {
                    redresses = await applicationDbContext.Redresses
                         .Include(r => r.Customer)
                         .Include(r => r.Program)
                         .Where(r => r.Customer != null
                                    && (r.Customer.CustomerId.Equals(customerId)
                                    || (!string.IsNullOrWhiteSpace(r.Customer.Surname)
                                    && r.Customer.Surname.ToLowerInvariant().Contains(surname))))
                         .AsNoTracking()
                         .ToListAsync()
                         .ConfigureAwait(false);
                }
                else
                {
                    if (customerId > 0
                        || !string.IsNullOrWhiteSpace(surname))
                    {
                        redresses = await applicationDbContext.Redresses
                             .Include(r => r.Customer)
                             .Include(r => r.Program)
                             .Where(r => r.Program != null
                                        && r.Program.Name == program
                                        && r.Customer != null
                                        && (r.Customer.CustomerId.Equals(customerId)
                                        || (!string.IsNullOrWhiteSpace(r.Customer.Surname)
                                        && r.Customer.Surname.ToLowerInvariant().Contains(surname))))
                             .AsNoTracking()
                             .ToListAsync()
                             .ConfigureAwait(false);
                    }
                    else
                    {
                        redresses = await applicationDbContext.Redresses
                            .Include(r => r.Customer)
                            .Include(r => r.Program)
                            .Where(r => r.Program != null && r.Program.Name == programClause.Value)
                            .AsNoTracking()
                            .ToListAsync()
                            .ConfigureAwait(false);
                    }
                }
            }

            return redresses.Select(r => new RedressCase
            {
                RedressId = r.RedressId,
                ProgramName = r.ProgramName,
                CustomerName = r.CustomerName,
                Status = string.Empty
            })
                .ToList();
        }

        public async Task<IEnumerable<RedressCase>> SearchNewRedressCasesAsync(SearchCriteria searchCriteria)
        {
            var customerIdClause = searchCriteria.Clauses.First(c => c.ParameterName.Equals("CustomerId"));
            var surnameClause = searchCriteria.Clauses.First(c => c.ParameterName.Equals("Surname"));

            int customerId = 0;
            string surname = string.Empty;
            ProductType productType;
            RateType rateType;
            RepaymentType repaymentType;

            if (!string.IsNullOrWhiteSpace(customerIdClause.Value))
            {
                _ = int.TryParse(customerIdClause.Value, out customerId);
            }

            if (!string.IsNullOrWhiteSpace(surnameClause.Value))
            {
                surname = surnameClause.Value.ToLowerInvariant();
            }

            List<RedressCase> redressCases;

            // https://stackoverflow.com/questions/17142151/linq-to-sql-multiple-tables-left-outer-join

            // from customers left outer joined to redress and product 
            if (customerId > 0
                || !string.IsNullOrWhiteSpace(surname))
            {
                redressCases = (from c in applicationDbContext.Customers
                                    join p in applicationDbContext.Products
                                    on c.CustomerId equals p.CustomerId into CustomerProduct
                                    from cp in CustomerProduct.DefaultIfEmpty()
                                    join r in applicationDbContext.Redresses
                                    on new { CustomerId = cp.CustomerId, ProductId = cp.ProductId } equals new { r.CustomerId, r.ProductId } into CustomerProductRedresses
                                    from cpr in CustomerProductRedresses.DefaultIfEmpty()
                                    where c.CustomerId == customerId
                                    || (!string.IsNullOrWhiteSpace(c.Surname) && c.Surname.ToLowerInvariant().Contains(surname))
                                    select new RedressCase
                                    {
                                        RedressId = cpr.RedressId,
                                        ProgramName = cpr.ProgramName,
                                        CustomerName = c.Surname,
                                        ProductName = cp.Name,
                                        ProductType = cp.ProductType.ToString(),
                                        RateType = cp.RateType.ToString(),
                                        RepaymentType = cp.RepaymentType.ToString(),

                                    }).ToList();
            }
            else
            {
                redressCases = (from c in applicationDbContext.Customers
                                    join p in applicationDbContext.Products
                                    on c.CustomerId equals p.CustomerId into CustomerProduct
                                    from cp in CustomerProduct.DefaultIfEmpty()
                                    join r in applicationDbContext.Redresses
                                    on new { CustomerId = cp.CustomerId, ProductId = cp.ProductId } equals new { r.CustomerId, r.ProductId } into CustomerProductRedresses
                                    from cpr in CustomerProductRedresses.DefaultIfEmpty()
                                    select new RedressCase
                                    {
                                        RedressId = cpr.RedressId,
                                        ProgramName = cpr.ProgramName,
                                        CustomerName = c.Surname,
                                        ProductName = cp.Name,
                                        ProductType = cp.ProductType.ToString(),
                                        RateType = cp.RateType.ToString(),
                                        RepaymentType = cp.RepaymentType.ToString(),

                                    }).ToList();
            }

            return redressCases;
        }

        public async Task<Redress> GetRedressAsync(int id)
        {
            return await applicationDbContext.Redresses
                .AsNoTracking()
                .FirstAsync(r => r.RedressId.Equals(id))
                .ConfigureAwait(false);
        }

        public async Task<Redress> AddRedressAsync(Redress redress)
        {
            await applicationDbContext.Redresses
                .AddAsync(redress)
                .ConfigureAwait(false);

            await applicationDbContext
                .SaveChangesAsync()
                .ConfigureAwait(false);

            return redress;
        }

        public async Task<Redress> UpdateRedressAsync(Redress redress)
        {
            applicationDbContext.Redresses.Update(redress);

            await applicationDbContext
                .SaveChangesAsync()
                .ConfigureAwait(false);

            return redress;
        }

        public async Task<int> DeleteRedressAsync(int id)
        {
            var redress = await applicationDbContext.Redresses
                .FirstAsync(r => r.RedressId.Equals(id))
                .ConfigureAwait(false);

            applicationDbContext.Redresses.Remove(redress);

            return await applicationDbContext
                .SaveChangesAsync()
                .ConfigureAwait(false);
        }
    }
}