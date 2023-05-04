using Headway.Core.Args;
using RemediatR.Core.Enums;
using RemediatR.Core.Interface;
using RemediatR.Core.Model;
using Headway.Repository.Data;
using Headway.Repository.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Headway.Core.Model;

namespace RemediatR.Repository
{
    public class RemediatRRepository : RepositoryBase<RemediatRRepository>, IRemediatRRepository
    {
        public RemediatRRepository(ApplicationDbContext applicationDbContext, ILogger<RemediatRRepository> logger)
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

        public async Task<IEnumerable<RedressCase>> GetRedressCasesAsync(SearchArgs searchArgs)
        {
            var programArg = searchArgs.Args.First(c => c.ParameterName.Equals("Name"));
            var redressIdArg = searchArgs.Args.First(c => c.ParameterName.Equals("RedressId"));
            var customerIdArg = searchArgs.Args.First(c => c.ParameterName.Equals("CustomerId"));
            var surnameArg = searchArgs.Args.First(c => c.ParameterName.Equals("Surname"));

            int redressId = 0;
            int customerId = 0;
            string surname = string.Empty;
            string program = string.Empty;

            if (!string.IsNullOrWhiteSpace(programArg.Value))
            {
                program = programArg.Value;
            }

            if (!string.IsNullOrWhiteSpace(redressIdArg.Value))
            {
                _ = int.TryParse(redressIdArg.Value, out redressId);
            }

            if (!string.IsNullOrWhiteSpace(customerIdArg.Value))
            {
                _ = int.TryParse(customerIdArg.Value, out customerId);
            }

            if (!string.IsNullOrWhiteSpace(surnameArg.Value))
            {
                surname = surnameArg.Value.ToLowerInvariant();
            }

            List<Redress> redresses;

            if (redressId.Equals(0)
                && customerId.Equals(0)
                && string.IsNullOrWhiteSpace(surname)
                && string.IsNullOrWhiteSpace(program))
            {
                redresses = await applicationDbContext.Redresses
                    .Include(r => r.Program)
                    .Include(r => r.Product)
                        .ThenInclude(p => p.Customer)
                    .AsNoTracking()
                    .ToListAsync()
                    .ConfigureAwait(false);
            }
            else
            {
                if(redressId > 0)
                {
                    var redress = await applicationDbContext.Redresses
                        .Include(r => r.Program)
                        .Include(r => r.Product)
                        .AsNoTracking()
                        .FirstOrDefaultAsync(r => r.RedressId.Equals(redressId))
                        .ConfigureAwait(false);

                    if(redress != null)
                    {
                        return new List<RedressCase>(new[]
                        {
                            new RedressCase
                            {
                                RedressId = redress.RedressId,
                                ProgramName = redress.ProgramName,
                                CustomerName = redress.CustomerName,
                                Status = string.Empty
                            }
                        });
                    }
                    else
                    {
                        return new List<RedressCase>();
                    }
                }
                else if (string.IsNullOrWhiteSpace(program))
                {
                    redresses = await applicationDbContext.Redresses
                        .Include(r => r.Program)
                        .Include(r => r.Product)
                            .ThenInclude(p => p.Customer)
                        .Where(r => r.Product.Customer.CustomerId.Equals(customerId)
                                   || (!string.IsNullOrWhiteSpace(r.Product.Customer.Surname) 
                                   && r.Product.Customer.Surname.ToLower().Contains(surname)))
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
                             .Include(r => r.Program)
                             .Include(r => r.Product)
                             .Where(r => r.Program != null
                                        && r.Program.Name == program
                                        && (r.Product.Customer.CustomerId.Equals(customerId)
                                        || (!string.IsNullOrWhiteSpace(r.Product.Customer.Surname)
                                        && r.Product.Customer.Surname.ToLower().Contains(surname))))
                             .AsNoTracking()
                             .ToListAsync()
                             .ConfigureAwait(false);
                    }
                    else
                    {
                        redresses = await applicationDbContext.Redresses
                            .Include(r => r.Program)
                            .Include(r => r.Product)
                            .Where(r => r.Program != null && r.Program.Name == programArg.Value)
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

        public async Task<IEnumerable<NewRedressCase>> SearchNewRedressCasesAsync(SearchArgs searchArgs)
        {
            var productTypeArg = searchArgs.Args.First(c => c.ParameterName.Equals("ProductType"));
            var customerIdArg = searchArgs.Args.First(c => c.ParameterName.Equals("CustomerId"));
            var surnameArg = searchArgs.Args.First(c => c.ParameterName.Equals("Surname"));

            int customerId = 0;
            string surname = string.Empty;
            ProductType productType = ProductType.Unknown;

            if (!string.IsNullOrWhiteSpace(productTypeArg.Value))
            {
                productType = Enum.Parse<ProductType>(productTypeArg.Value);
            }

            if (!string.IsNullOrWhiteSpace(customerIdArg.Value))
            {
                _ = int.TryParse(customerIdArg.Value, out customerId);
            }

            if (!string.IsNullOrWhiteSpace(surnameArg.Value))
            {
                surname = surnameArg.Value.ToLowerInvariant();
            }

            List<Customer> customers;

            if (customerId.Equals(0)
                && string.IsNullOrWhiteSpace(surname)
                && productType.Equals(ProductType.Unknown))
            {
                customers = await applicationDbContext.Customers
                    .Include(c => c.Products)
                    .AsNoTracking()
                    .ToListAsync()
                    .ConfigureAwait(false);
            }
            else
            {
                if (customerId > 0
                    || !string.IsNullOrWhiteSpace(surname))
                {
                    if (productType.Equals(ProductType.Unknown))
                    {
                        customers = await applicationDbContext.Customers
                            .Include(c => c.Products)
                            .Where(c => c.CustomerId.Equals(customerId)
                                        || (!string.IsNullOrWhiteSpace(c.Surname)
                                        && c.Surname.ToLower().Contains(surname)))
                            .AsNoTracking()
                            .ToListAsync()
                            .ConfigureAwait(false);
                    }
                    else
                    {
                        customers = await applicationDbContext.Customers
                            .Include(c => c.Products)
                            .Where(c => c.Products.Where(p => p.ProductType == productType).ToList().Count > 0
                                        && (c.CustomerId.Equals(customerId)
                                        || (!string.IsNullOrWhiteSpace(c.Surname)
                                        && c.Surname.ToLower().Contains(surname))))
                            .AsNoTracking()
                            .ToListAsync()
                            .ConfigureAwait(false);
                    }
                }
                else
                {
                    customers = await applicationDbContext.Customers
                        .Include(c => c.Products)
                        .Where(c => c.Products.Where(p => p.ProductType == productType).ToList().Count > 0)
                        .AsNoTracking()
                        .ToListAsync()
                        .ConfigureAwait(false);
                }
            }

            var productIds = customers.SelectMany(c => c.Products).Select(p => p.ProductId).ToList();

            var redresses = await applicationDbContext.Redresses
                .Include(r => r.Program)
                .Where(r => productIds.Any(p => p.Equals(r.ProductId)))
                .AsNoTracking()
                .ToListAsync()
                .ConfigureAwait(false);
                
            var newRedressCases = new List<NewRedressCase>();

            foreach (var c in customers)
            {
                foreach (var product in c.Products)
                {
                    newRedressCases.Add(new NewRedressCase
                    {
                        CustomerId = c.CustomerId,
                        CustomerName = c.Fullname,
                        ProductId = product.ProductId,
                        ProductName = product.Name,
                        ProductType = product.ProductType.ToString(),
                        RateType = product.RateType.ToString(),
                        RepaymentType = product.RepaymentType.ToString(),
                    });
                }
            }

            static NewRedressCase f(NewRedressCase rc, Redress r)
            {
                rc.RedressId = r.RedressId;
                rc.ProgramName = r.ProgramName;
                return rc;
            }

            _ = (from rc in newRedressCases
                               join r in redresses on rc.ProductId equals r.ProductId
                               where rc.ProductId.HasValue
                               select f(rc, r)).ToList();

            return newRedressCases;
        }

        public async Task<Redress> CreateRedressAsync(DataArgs dataArgs, User user)
        {
            var redressIdArg = dataArgs.Args.First(c => c.PropertyName.Equals("RedressId"));
            var productIdArg = dataArgs.Args.First(c => c.PropertyName.Equals("ProductId"));

            int redressId = 0;
            int productId = 0;

            if (!string.IsNullOrWhiteSpace(redressIdArg.Value))
            {
                _ = int.TryParse(redressIdArg.Value, out redressId);
            }

            if (!string.IsNullOrWhiteSpace(productIdArg.Value))
            {
                _ = int.TryParse(productIdArg.Value, out productId);
            }

            if(redressId > 0)
            {
                return await (GetRedressAsync(redressId, user))
                    .ConfigureAwait(false);
            }
            else
            {
                var product = await applicationDbContext.Products
                    .Include(p => p.Customer)
                    .AsNoTracking()
                    .FirstAsync(p => p.ProductId.Equals(productId))
                    .ConfigureAwait((false));

                var flow = await applicationDbContext.Flows
                    .Include(f => f.States)
                    .FirstAsync(f => f.FlowCode.Equals("REMEDIATR"))
                    .ConfigureAwait(false);

                return new Redress
                {
                    Product = product,                    
                    RefundCalculation = new RefundCalculation(),
                    RedressFlowContext = new RedressFlowContext { FlowId = flow.FlowId, Flow = flow, CurrentUser = user }
                };
            }
        }

        public async Task<Redress> GetRedressAsync(int id, User user)
        {
            var redress = await applicationDbContext.Redresses
                .Include(r => r.Program)
                .Include(r => r.RefundCalculation)
                .Include(r => r.Product)
                    .ThenInclude(p => p.Customer)
                .Include(rc => rc.RedressFlowContext)
                    .ThenInclude(f => f.Flow)
                .Include(rc => rc.RedressFlowContext)
                    .ThenInclude(f => f.RedressFlowHistory)
                .AsNoTracking()
                .FirstAsync(r => r.RedressId.Equals(id))
                .ConfigureAwait(false);

            if(redress.RedressFlowContext != null)
            {
                redress.RedressFlowContext.CurrentUser = user;
            }

            return redress;
        }

        public async Task<Redress> AddRedressAsync(Redress redress, User user)
        {
            var product = await applicationDbContext.Products
                .Include(p => p.Customer)
                .FirstAsync(p => p.ProductId.Equals(redress.Product.ProductId))
                .ConfigureAwait(false);

            var program = await applicationDbContext.Programs
                .FirstAsync(p => p.ProgramId.Equals(redress.Program.ProgramId))
                .ConfigureAwait(false);

            var flow = await applicationDbContext.Flows
                .FirstAsync(f => f.FlowId.Equals(redress.RedressFlowContext.FlowId))
                .ConfigureAwait(false);

            redress.RedressFlowContext.FlowId = flow.FlowId;
            redress.RedressFlowContext.Flow = flow;
            redress.RedressFlowContext.CurrentUser = user;

            var newRedress = new Redress
            {
                Product = product,
                Program = program,
                RefundCalculation = redress.RefundCalculation,
                RedressCaseOwner = redress.RedressCaseOwner,
                RedressFlowContext = redress.RedressFlowContext
            };

            await applicationDbContext.RefundCalculations
                .AddAsync(newRedress.RefundCalculation)
                .ConfigureAwait(false);

            await applicationDbContext.RedressFlowContexts
                .AddAsync(newRedress.RedressFlowContext)
                .ConfigureAwait(false);

            await applicationDbContext.Redresses
                .AddAsync(newRedress)
                .ConfigureAwait(false);

            await applicationDbContext
                .SaveChangesAsync()
                .ConfigureAwait(false);

            return newRedress;
        }

        public async Task<Redress> UpdateRedressAsync(Redress redress, User user)
        {
            var existing = await applicationDbContext.Redresses
                .Include(r => r.Program)
                .Include(r => r.Product)
                .Include(r => r.RefundCalculation)
                .FirstOrDefaultAsync(p => p.RedressId.Equals(redress.RedressId));

            if (existing == null)
            {
                throw new NullReferenceException(
                    $"{nameof(redress)} redressId {redress.RedressId} not found.");
            }
            else
            {
                if (!existing.RefundCalculationId.Equals(redress.RefundCalculationId))
                {
                    throw new NotSupportedException(
                        $"{nameof(redress)} cannot change RefundCalculationId {existing.RefundCalculationId} to RefundCalculationId {redress.RefundCalculationId}.");
                }

                if (!existing.ProductId.Equals(redress.ProductId))
                {
                    throw new NotSupportedException(
                        $"{nameof(redress)} cannot change ProductId {existing.ProductId} to ProductId {redress.ProductId}.");
                }

                applicationDbContext
                    .Entry(existing)
                    .CurrentValues.SetValues(redress);

                applicationDbContext
                    .Entry(existing.RefundCalculation)
                    .CurrentValues.SetValues(redress.RefundCalculation);

                if (!existing.ProgramId.Equals(redress.ProgramId))
                {
                    var program = await applicationDbContext.Programs
                        .FirstOrDefaultAsync(p => p.ProgramId.Equals(redress.ProgramId))
                        .ConfigureAwait(false);

                    if (program == null)
                    {
                        throw new NullReferenceException(
                            $"{nameof(redress)} ProgramId {redress.ProgramId} not found.");
                    }
                    else
                    {
                        existing.Program = program;
                    }
                }
            }

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