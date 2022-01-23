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
            var demoModel = await applicationDbContext.DemoModels
                .AsNoTracking()
                .Include(m => m.DemoModelItems)
                .Include(m => m.DemoModelTreeItems)
                .SingleAsync(m => m.DemoModelId.Equals(id))
                .ConfigureAwait(false);

            demoModel.DemoModelTreeItems = GetTree(demoModel.DemoModelTreeItems, id);

            return demoModel;
        }

        public async Task<DemoModel> AddDemoModelAsync(DemoModel demoModel)
        {
            demoModel.DemoModelTreeItems = GetFlattenedTree(demoModel.DemoModelTreeItems, demoModel.DemoModelId);

            await applicationDbContext.DemoModels
                .AddAsync(demoModel)
                .ConfigureAwait(false);

            await applicationDbContext
                .SaveChangesAsync()
                .ConfigureAwait(false);

            demoModel.DemoModelTreeItems = GetTree(demoModel.DemoModelTreeItems, demoModel.DemoModelId);

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
                    GetFlattenedTree(demoModel.DemoModelTreeItems, demoModel.DemoModelId);

                var removeDemoModelTreeItems = (from demoModelTreeItem in existing.DemoModelTreeItems
                                                where !demoModel.DemoModelTreeItems.Any(i => i.ItemCode.Equals(demoModelTreeItem.ItemCode))
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

            demoModel.DemoModelTreeItems = GetTree(demoModel.DemoModelTreeItems, demoModel.DemoModelId);

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

        private static List<DemoModelTreeItem> GetTree(List<DemoModelTreeItem> demoModelTreeItems, int demoModelId)
        {
            List<DemoModelTreeItem> demoModelTree = new();

            ValidateFlattenedTree(demoModelTreeItems, demoModelId);

            foreach (var demoModelTreeItem in demoModelTreeItems)
            {
                var children = demoModelTreeItems
                    .Where(dm => !string.IsNullOrWhiteSpace(dm.ParentItemCode)
                                && dm.ParentItemCode.Equals(demoModelTreeItem.ItemCode))
                    .OrderBy(dm => dm.Order);

                demoModelTreeItem.DemoModelTreeItems
                    .AddRange(children);

                demoModelTree.Add(demoModelTreeItem);
            }

            return demoModelTree
                .Where(m => string.IsNullOrWhiteSpace(m.ParentItemCode))
                .OrderBy(m => m.Order)
                .ToList();
        }

        private static List<DemoModelTreeItem> GetFlattenedTree(List<DemoModelTreeItem> demoModelTreeItems, int demoModelId)
        {
            List<DemoModelTreeItem> flattenedTree = new();
            FlattenTree(demoModelTreeItems, demoModelId, flattenedTree);
            ValidateFlattenedTree(flattenedTree, demoModelId);
            return flattenedTree;
        }

        private static void ValidateFlattenedTree(List<DemoModelTreeItem> demoModelTreeItems, int demoModelId)
        {
            if (demoModelTreeItems.Any(m => string.IsNullOrWhiteSpace(m.ItemCode)))
            {
                throw new Exception($"DemoModelId {demoModelId} tree items missing item code");
            }

            var itemCodes = demoModelTreeItems.Select(m => m.ItemCode).ToList();
            var children = demoModelTreeItems
                .Where(m => !string.IsNullOrWhiteSpace(m.ParentItemCode)).ToList();

            foreach (var child in children)
            {
                if (!itemCodes.Any(c => c.Equals(child.ParentItemCode)))
                {
                    child.ParentItemCode = string.Empty;
                }
            }
        }

        private static void FlattenTree(List<DemoModelTreeItem> demoModelTreeItems, int demoModelId, List<DemoModelTreeItem> demoModelTree)
        {
            foreach (var demoModelTreeItem in demoModelTreeItems)
            {
                demoModelTreeItem.DemoModelId = demoModelId;
                demoModelTree.Add(demoModelTreeItem);
                FlattenTree(demoModelTreeItem.DemoModelTreeItems, demoModelId, demoModelTree);
                demoModelTreeItem.DemoModelTreeItems.Clear();
            }
        }
    }
}
