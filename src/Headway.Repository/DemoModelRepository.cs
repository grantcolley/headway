using Headway.Core.Interface;
using Headway.Core.Model;
using Headway.Repository.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Headway.Repository
{
    public class DemoModelRepository : RepositoryBase<DemoModelRepository>, IDemoModelRepository
    {
        public DemoModelRepository(ApplicationDbContext applicationDbContext, ILogger<DemoModelRepository> logger)
            : base(applicationDbContext, logger)
        {
        }

        public async Task<IEnumerable<DemoModel>> GetDemoModelsAsync()
        {
            return await applicationDbContext.DemoModels
                .AsNoTracking()
                .ToListAsync()
                .ConfigureAwait(false);
        }

        public async Task<DemoModel> GetDemoModelAsync(int id)
        {
            return await applicationDbContext.DemoModels
                .AsNoTracking()
                .Include(m => m.DemoModels)
                .SingleAsync(m => m.DemoModelId.Equals(id))
                .ConfigureAwait(false);
        }

        public async Task<DemoModel> AddDemoModelAsync(DemoModel demoModel)
        {
            await applicationDbContext.DemoModels
                .AddAsync(demoModel)
                .ConfigureAwait(false);

            await applicationDbContext
                .SaveChangesAsync()
                .ConfigureAwait(false);

            return demoModel;
        }

        public async Task<DemoModel> UpdateDemoModelAsync(DemoModel demoModel)
        {
            var existing = await applicationDbContext.DemoModels
                .Include(m => m.DemoModels)
                .FirstOrDefaultAsync(m => m.DemoModelId.Equals(demoModel.DemoModelId))
                .ConfigureAwait(false);

            if (existing == null)
            {
                applicationDbContext.DemoModels.Add(demoModel);
            }
            else
            {
                applicationDbContext
                    .Entry(existing)
                    .CurrentValues.SetValues(demoModel);

                foreach (var dm in demoModel.DemoModels)
                {
                    var existingDm = existing.DemoModels
                        .FirstOrDefault(m => m.DemoModelId.Equals(dm.DemoModelId));

                    if (existingDm == null)
                    {
                        existing.DemoModels.Add(dm);
                    }
                    else
                    {
                        applicationDbContext.Entry(existingDm).CurrentValues.SetValues(dm);
                    }
                }

                foreach (var dm in demoModel.DemoModels)
                {
                    if (!demoModel.DemoModels.Any(m => m.DemoModelId.Equals(dm.DemoModelId)))
                    {
                        applicationDbContext.Remove(dm);
                    }
                }
            }

            await applicationDbContext
                .SaveChangesAsync()
                .ConfigureAwait(false);

            return demoModel;
        }

        public async Task<int> DeleteDemoModelAsync(int id)
        {
            var demoModel = await applicationDbContext.DemoModels
                .SingleAsync(m => m.DemoModelId.Equals(id))
                .ConfigureAwait(false);

            applicationDbContext.DemoModels.Remove(demoModel);

            return await applicationDbContext
                .SaveChangesAsync()
                .ConfigureAwait(false);
        }
    }
}
