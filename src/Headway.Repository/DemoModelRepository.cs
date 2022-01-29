using Headway.Core.Helpers;
using Headway.Core.Interface;
using Headway.Core.Model;
using Headway.Repository.Constants;
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
        private readonly GenericTreeHelperArgs genericTreeHelperArgs;

        public DemoModelRepository(ApplicationDbContext applicationDbContext, ILogger<DemoModelRepository> logger)
            : base(applicationDbContext, logger)
        {
            genericTreeHelperArgs = new GenericTreeHelperArgs
            {
                ModelIdProperty = GenericTreeArgs.DEMO_MODEL_ID,
                ItemsProperty = GenericTreeArgs.DEMO_MODEL_TREE_ITEMS,
                ItemCodeProperty = GenericTreeArgs.DEMO_MODEL_ITEM_CODE,
                ParentItemCodeProperty = GenericTreeArgs.DEMO_MODEL_PARENT_ITEM_CODE,
                OrderByProperty = GenericTreeArgs.DEMO_MODEL_TREE_ITEM_ORDER
            };
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
            var demoModel = await applicationDbContext.DemoModels
                .AsNoTracking()
                .Include(m => m.DemoModelItems)
                .Include(m => m.DemoModelTreeItems)
                .SingleAsync(m => m.DemoModelId.Equals(id))
                .ConfigureAwait(false);

            demoModel.DemoModelTreeItems = GenericTreeHelper.GetTree<DemoModel, DemoModelTreeItem>(demoModel, genericTreeHelperArgs);

            return demoModel;
        }

        public async Task<DemoModel> AddDemoModelAsync(DemoModel demoModel)
        {
            demoModel.DemoModelTreeItems = 
                GenericTreeHelper.GetFlattenedTree<DemoModel, DemoModelTreeItem>(demoModel, genericTreeHelperArgs);

            await applicationDbContext.DemoModels
                .AddAsync(demoModel)
                .ConfigureAwait(false);

            await applicationDbContext
                .SaveChangesAsync()
                .ConfigureAwait(false);

            demoModel.DemoModelTreeItems = GenericTreeHelper.GetTree<DemoModel, DemoModelTreeItem>(demoModel, genericTreeHelperArgs);

            return demoModel;
        }

        public async Task<DemoModel> UpdateDemoModelAsync(DemoModel demoModel)
        {
            var existing = await applicationDbContext.DemoModels
                .Include(m => m.DemoModelItems)
                .Include(m => m.DemoModelTreeItems)
                .FirstOrDefaultAsync(m => m.DemoModelId.Equals(demoModel.DemoModelId))
                .ConfigureAwait(false);

            if (existing == null)
            {
                throw new NullReferenceException(
                    $"{nameof(demoModel)} DemoModelId {demoModel.DemoModelId} not found.");
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

                demoModel.DemoModelTreeItems =
                    GenericTreeHelper.GetFlattenedTree<DemoModel, DemoModelTreeItem>(demoModel, genericTreeHelperArgs);

                var removeDemoModelTreeItems = (from demoModelTreeItem in existing.DemoModelTreeItems
                                                where !demoModel.DemoModelTreeItems.Any(i => i.Code.Equals(demoModelTreeItem.Code))
                                                select demoModelTreeItem)
                                                .ToList();

                applicationDbContext.RemoveRange(removeDemoModelTreeItems);

                foreach (var demoModelTreeItem in demoModel.DemoModelTreeItems)
                {
                    DemoModelTreeItem existingDemoModelTreeItem = null;

                    if (demoModelTreeItem.DemoModelTreeItemId > 0)
                    {
                        existingDemoModelTreeItem = existing.DemoModelTreeItems
                            .First(m => m.DemoModelTreeItemId.Equals(demoModelTreeItem.DemoModelTreeItemId));
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

            demoModel.DemoModelTreeItems = GenericTreeHelper.GetTree<DemoModel, DemoModelTreeItem>(demoModel, genericTreeHelperArgs);

            return demoModel;
        }

        public async Task<int> DeleteDemoModelAsync(int id)
        {
            var demoModel = await applicationDbContext.DemoModels
                .Include(m => m.DemoModelItems)
                .Include(m => m.DemoModelTreeItems)
                .FirstOrDefaultAsync(m => m.DemoModelId.Equals(id))
                .ConfigureAwait(false);

            applicationDbContext.Remove(demoModel);

            return await applicationDbContext
                .SaveChangesAsync()
                .ConfigureAwait(false);
        }
    }
}
