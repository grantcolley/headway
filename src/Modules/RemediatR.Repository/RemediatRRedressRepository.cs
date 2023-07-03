using Headway.Core.Args;
using Headway.Core.Enums;
using Headway.Repository.Data;
using Headway.Repository.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RemediatR.Core.Constants;
using RemediatR.Core.Enums;
using RemediatR.Core.Interface;
using RemediatR.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace RemediatR.Repository
{
    public class RemediatRRedressRepository : RepositoryBase<RemediatRRedressRepository>, IRemediatRRedressRepository<RedressFlowContext>
    {
        public RemediatRRedressRepository(ApplicationDbContext applicationDbContext, ILogger<RemediatRRedressRepository> logger)
            : base(applicationDbContext, logger)
        {
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

        public async Task<Redress> CreateRedressAsync(DataArgs dataArgs)
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
                return await GetRedressAsync(redressId)
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
                    .FirstAsync(f => f.FlowCode.Equals(RemediatRFlowCodes.REMEDIATR_CODE))
                    .ConfigureAwait(false);

                var authorisation = await GetAuthorisationAsync(User)
                    .ConfigureAwait(false);

                return new Redress
                {
                    Product = product,                    
                    RedressFlowContext = new RedressFlowContext
                    {
                        FlowId = flow.FlowId,
                        Flow = flow,
                        Authorisation = authorisation
                    }
                };
            }
        }

        public async Task<Redress> GetRedressAsync(int id)
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
                var authorisation = await GetAuthorisationAsync(User)
                    .ConfigureAwait(false);

                redress.RedressFlowContext.Authorisation = authorisation;
            }

            return redress;
        }

        public async Task<Redress> AddRedressAsync(Redress redress)
        {
            var product = await applicationDbContext.Products
                .Include(p => p.Customer)
                .FirstAsync(p => p.ProductId.Equals(redress.Product.ProductId))
                .ConfigureAwait(false);

            foreach (var flowHistory in redress.RedressFlowContext.Flow.History)
            {
                redress.RedressFlowContext.RedressFlowHistory.Add(new RedressFlowHistory
                {
                    Index = flowHistory.Index,

                    StateStatus = flowHistory.StateStatus,
                    FlowCode = flowHistory.FlowCode,
                    StateCode = flowHistory.StateCode,
                    Event = flowHistory.Event,
                    Owner = flowHistory.Owner,
                    Comment = flowHistory.Comment
                });
            }

            var flow = await applicationDbContext.Flows
                .FirstAsync(f => f.FlowId.Equals(redress.RedressFlowContext.FlowId))
                .ConfigureAwait(false);

            redress.RedressFlowContext.Flow = flow;

            var newRedress = new Redress
            {
                Product = product,
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

        public async Task<Redress> UpdateRedressAsync(Redress redress)
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
                .Include(r => r.RefundCalculation)
                .Include(rc => rc.RedressFlowContext)
                    .ThenInclude(f => f.Flow)
                .Include(rc => rc.RedressFlowContext)
                    .ThenInclude(f => f.RedressFlowHistory)
                .FirstAsync(r => r.RedressId.Equals(id))
                .ConfigureAwait(false);

            if (redress.RefundCalculation != null)
            {
                applicationDbContext.RefundCalculations.Remove(redress.RefundCalculation);
            }

            if (redress.RedressFlowContext != null)
            {
                for (int i = 0; i < redress.RedressFlowContext.RedressFlowHistory.Count; i++)
                {
                    var redressFlowHistory = redress.RedressFlowContext.RedressFlowHistory[i];
                    redress.RedressFlowContext.RedressFlowHistory.Remove(redressFlowHistory);
                    applicationDbContext.RedressFlowHistory.Remove(redressFlowHistory);
                }

                applicationDbContext.RedressFlowContexts.Remove(redress.RedressFlowContext);
            }

            applicationDbContext.Redresses.Remove(redress);

            return await applicationDbContext
                .SaveChangesAsync()
                .ConfigureAwait(false);
        }

        public async Task<RedressFlowContext> GetFlowContextAsync(int id)
        {
            var redressFlowContext = await applicationDbContext.RedressFlowContexts
                .Include(r => r.Flow)
                .Include(r => r.RedressFlowHistory)
                .FirstAsync(r => r.RedressFlowContextId.Equals(id))
                .ConfigureAwait(false);

            var authorisation = await GetAuthorisationAsync(User)
                .ConfigureAwait(false);

            redressFlowContext.Authorisation = authorisation;

            return redressFlowContext;
        }

        public async Task<RedressFlowContext> UpdateFlowHistoryAsync(RedressFlowContext redressFlowContext)
        {
            var existing = await applicationDbContext.RedressFlowContexts
                .Include(r => r.Flow)
                .Include(r => r.RedressFlowHistory)
                .FirstAsync(r => r.RedressFlowContextId.Equals(redressFlowContext.RedressFlowContextId))
                .ConfigureAwait(false);

            if (existing == null)
            {
                throw new NullReferenceException(
                    $"{nameof(redressFlowContext)} RedressFlowContextId {redressFlowContext.RedressFlowContextId} not found.");
            }

            foreach (var history in existing.RedressFlowHistory)
            {
                if (history.RedressFlowHistoryId.Equals(0))
                {
                    existing.RedressFlowHistory.Add(history);
                }
            }

            await applicationDbContext
                .SaveChangesAsync()
                .ConfigureAwait(false);

            return existing;
        }
    }
}