using Headway.RemediatR.Core.Interface;
using Headway.RemediatR.Core.Model;
using Headway.Repository;
using Headway.Repository.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
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
                .SingleAsync(p => p.ProductId.Equals(product.ProductId))
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
                .SingleAsync(c => c.CustomerId.Equals(id))
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

        public async Task<Customer> UpdateCustomerAsync(Customer customer)
        {
            applicationDbContext.Customers.Update(customer);

            await applicationDbContext
                .SaveChangesAsync()
                .ConfigureAwait(false);

            return customer;
        }

        public async Task<int> DeleteCustomerAsync(int id)
        {
            var customer = await applicationDbContext.Customers
                .SingleAsync(c => c.CustomerId.Equals(id))
                .ConfigureAwait(false);

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
                .SingleAsync(p => p.ProgramId.Equals(id))
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
                .SingleAsync(p => p.ProgramId.Equals(id))
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
                .Include(r => r.Products)
                .AsNoTracking()
                .ToListAsync()
                .ConfigureAwait(false);
        }

        public async Task<Redress> GetRedressAsync(int id)
        {
            return await applicationDbContext.Redresses
                .AsNoTracking()
                .SingleAsync(r => r.RedressId.Equals(id))
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
                .SingleAsync(r => r.RedressId.Equals(id))
                .ConfigureAwait(false);

            applicationDbContext.Redresses.Remove(redress);

            return await applicationDbContext
                .SaveChangesAsync()
                .ConfigureAwait(false);
        }
    }
}