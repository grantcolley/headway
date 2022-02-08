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
    public class CategoryRepository : RepositoryBase<CategoryRepository>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext applicationDbContext, ILogger<CategoryRepository> logger)
            : base(applicationDbContext, logger)
        {
        }
        
        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            return await applicationDbContext.Categories
                .AsNoTracking()
                .Include(c => c.Module)
                .OrderBy(m => m.Order)
                .ThenBy(c => c.Order)
                .ToListAsync()
                .ConfigureAwait(false);
        }

        public async Task<Category> GetCategoryAsync(int id)
        {
            return await applicationDbContext.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.CategoryId.Equals(id))
                .ConfigureAwait(false);
        }

        public async Task<Category> AddCategoryAsync(Category category)
        {
            var module = await applicationDbContext.Modules
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ModuleId.Equals(category.Module.ModuleId))
                .ConfigureAwait(false);

            var newCategory = new Category
            {
                CategoryId = category.CategoryId,
                Name = category.Name,
                Order = category.Order,
                Permission = category.Permission,
                Module = module
            };

            await applicationDbContext.Categories
                .AddAsync(newCategory)
                .ConfigureAwait(false);

            await applicationDbContext
                .SaveChangesAsync()
                .ConfigureAwait(false);

            return newCategory;
        }

        public async Task<Category> UpdateCategoryAsync(Category category)
        {
            var existing = await applicationDbContext.Categories
                .FirstOrDefaultAsync(c => c.CategoryId.Equals(category.CategoryId))
                .ConfigureAwait(false);

            if (existing == null)
            {
                throw new NullReferenceException(
                    $"{nameof(category)} CategoryId {category.CategoryId} not found.");
            }
            else
            {
                applicationDbContext
                    .Entry(existing)
                    .CurrentValues.SetValues(category);
            }

            await applicationDbContext
                .SaveChangesAsync()
                .ConfigureAwait(false);

            return existing;
        }

        public async Task<int> DeleteCategoryAsync(int id)
        {
            var category = await applicationDbContext.Categories
                .FirstOrDefaultAsync(c => c.CategoryId.Equals(id))
                .ConfigureAwait(false);

            applicationDbContext.Remove(category);

            return await applicationDbContext
                .SaveChangesAsync()
                .ConfigureAwait(false);
        }
    }
}
