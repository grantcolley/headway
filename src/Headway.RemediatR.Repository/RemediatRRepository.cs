using Headway.RemediatR.Core.Interface;
using Headway.RemediatR.Core.Model;
using Headway.Repository;
using Headway.Repository.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<IEnumerable<Redress>> GetRedressesAsync()
        {
            return await applicationDbContext.Redresses
                .Include(r => r.Customer)
                .Include(r => r.Program)
                .Include(r => r.RedressProducts)
                .AsNoTracking()
                .ToListAsync()
                .ConfigureAwait(false);
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