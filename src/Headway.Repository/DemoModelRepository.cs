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
                .Include(m => m.DemoModelItems)
                .Include(m => m.DemoModelTreeItems)
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
                .Include(m => m.DemoModelItems)
                .FirstOrDefaultAsync(m => m.DemoModelId.Equals(demoModel.DemoModelId))
                .ConfigureAwait(false);

            if (existing == null)
            {
                demoModel.DemoModelTreeItems.ForEach(m => m.DemoModel = demoModel);
                applicationDbContext.DemoModels.Add(demoModel);
            }
            else
            {
                applicationDbContext
                    .Entry(existing)
                    .CurrentValues.SetValues(demoModel);

                var removeDemoModelItems = (from demoModelItem in existing.DemoModelItems
                                   where !demoModel.DemoModelItems.Any(i => i.DemoModelItemId.Equals(demoModelItem.DemoModelItemId))
                                   select demoModelItem)
                                  .ToList();

                applicationDbContext.RemoveRange(removeDemoModelItems);

                foreach (var demoModelItem in demoModel.DemoModelItems)
                {
                    DemoModelItem existingDemoModelItem = null;

                    if(demoModelItem.DemoModelItemId > 0)
                    {
                        existingDemoModelItem = existing.DemoModelItems
                            .FirstOrDefault(m => m.DemoModelItemId.Equals(demoModelItem.DemoModelItemId));
                    }

                    if (existingDemoModelItem == null)
                    {
                        existing.DemoModelItems.Add(demoModelItem);
                    }
                    else
                    {
                        applicationDbContext.Entry(existingDemoModelItem).CurrentValues.SetValues(demoModelItem);
                    }
                }

                var removeDemoModelTreeItems = (from demoModelTreeItem in existing.DemoModelTreeItems
                                            where !demoModel.DemoModelTreeItems.Any(i => i.DemoModelTreeItemId.Equals(demoModelTreeItem.DemoModelTreeItemId))
                                            select demoModelTreeItem)
                                            .ToList();

                applicationDbContext.RemoveRange(removeDemoModelTreeItems);

                foreach (var demoModelTreeItem in demoModel.DemoModelTreeItems)
                {
                    DemoModelTreeItem existingDemoModelTreeItem = null;

                    demoModelTreeItem.DemoModel = demoModel;

                    if (demoModelTreeItem.DemoModelTreeItemId > 0)
                    {
                        existingDemoModelTreeItem = existing.DemoModelTreeItems
                            .FirstOrDefault(m => m.DemoModelTreeItemId.Equals(demoModelTreeItem.DemoModelTreeItemId));
                    }

                    if (existingDemoModelTreeItem == null)
                    {
                        existing.DemoModelTreeItems.Add(demoModelTreeItem);
                    }
                    else
                    {
                        applicationDbContext.Entry(existingDemoModelTreeItem).CurrentValues.SetValues(demoModelTreeItem);
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
