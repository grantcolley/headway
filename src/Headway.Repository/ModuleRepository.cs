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
    public class ModuleRepository : RepositoryBase<ModuleRepository>, IModuleRepository
    {
        public ModuleRepository(ApplicationDbContext applicationDbContext, ILogger<ModuleRepository> logger)
            : base(applicationDbContext, logger)
        {
        }

        public async Task<IEnumerable<Module>> GetModulesAsync(string claim)
        {
            var user = await applicationDbContext.Users
                .Include(u => u.Permissions)
                .Include(u => u.Roles)
                .ThenInclude(r => r.Permissions)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email.Equals(claim))
                .ConfigureAwait(false);

            var userPermissions = user.Permissions.Select(p => p.Name).ToList();

            var rolePermissions = user.Roles
                .SelectMany(r => r.Permissions)
                .Select(p => p.Name)
                .ToList();

            userPermissions.AddRange(rolePermissions);

            var permissions = userPermissions.Distinct().ToList();

            var modules = await applicationDbContext.Modules
                .AsNoTracking()
                .Include(m => m.Categories.OrderBy(c => c.Order))
                .ThenInclude(c => c.MenuItems.OrderBy(mu => mu.Order))
                .Where(m => permissions.Contains(m.Permission))
                .OrderBy(m => m.Order)
                .ToListAsync()
                .ConfigureAwait(false);

            var permittedModules = modules
                .Where(m => m.IsPermitted(permissions))
                .ToList();

            return permittedModules;
        }

        public async Task<IEnumerable<Module>> GetModulesAsync()
        {
            return await applicationDbContext.Modules
                .AsNoTracking()
                .OrderBy(m => m.Order)
                .ToListAsync()
                .ConfigureAwait(false);
        }

        public async Task<Module> GetModuleAsync(int id)
        {
            return await applicationDbContext.Modules
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ModuleId.Equals(id))
                .ConfigureAwait(false);
        }

        public async Task<Module> AddModuleAsync(Module module)
        {
            var newModule = new Module
            {
                ModuleId = module.ModuleId,
                Name = module.Name,
                Order = module.Order,
                Permission = module.Permission
            };

            await applicationDbContext.Modules
                .AddAsync(newModule)
                .ConfigureAwait(false);

            await applicationDbContext
                .SaveChangesAsync()
                .ConfigureAwait(false);

            return newModule;
        }

        public async Task<Module> UpdateModuleAsync(Module module)
        {
            var existing = await applicationDbContext.Modules
                .FirstOrDefaultAsync(m => m.ModuleId.Equals(module.ModuleId))
                .ConfigureAwait(false);

            if (existing == null)
            {
                throw new NullReferenceException(
                    $"{nameof(module)} ModuleId {module.ModuleId} not found.");
            }
            else
            {
                applicationDbContext
                    .Entry(existing)
                    .CurrentValues.SetValues(module);
            }

            await applicationDbContext
                .SaveChangesAsync()
                .ConfigureAwait(false);

            return existing;
        }

        public async Task<int> DeleteModuleAsync(int id)
        {
            var module = await applicationDbContext.Modules
                .FirstOrDefaultAsync(m => m.ModuleId.Equals(id))
                .ConfigureAwait(false);

            applicationDbContext.Remove(module);

            return await applicationDbContext
                .SaveChangesAsync()
                .ConfigureAwait(false);
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
                .Include(c => c.Module)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.CategoryId.Equals(id))
                .ConfigureAwait(false);
        }

        public async Task<Category> AddCategoryAsync(Category category)
        {
            var module = await applicationDbContext.Modules
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

                if (category.Module == null
                    || category.Module.ModuleId.Equals(0))
                {
                    throw new ArgumentNullException(nameof(category.Module));
                }
                else
                {
                    existing.Module = category.Module;
                }
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

        public async Task<IEnumerable<MenuItem>> GetMenuItemsAsync()
        {
            var categories = await applicationDbContext.Categories
                .AsNoTracking()
                .Include(c => c.Module)
                .OrderBy(m => m.Order)
                .ThenBy(c => c.Order)
                .ToListAsync()
                .ConfigureAwait(false);

            var menuItems = await applicationDbContext.MenuItems
                .AsNoTracking()
                .Include(c => c.Category)
                .OrderBy(mi => mi.Order)
                .ToListAsync()
                .ConfigureAwait(false);

            var orderedMenuItems = (from c in categories
                                    join mi in menuItems on c.CategoryId equals mi.Category.CategoryId
                                    select mi).ToList();

            return orderedMenuItems;
        }

        public async Task<MenuItem> GetMenuItemAsync(int id)
        {
            return await applicationDbContext.MenuItems
                .AsNoTracking()
                .Include(mi => mi.Category)
                .FirstOrDefaultAsync(mi => mi.MenuItemId.Equals(id))
                .ConfigureAwait(false);
        }

        public async Task<MenuItem> AddMenuItemAsync(MenuItem menuItem)
        {
            var category = await applicationDbContext.Categories
                .FirstOrDefaultAsync(c => c.CategoryId.Equals(menuItem.Category.CategoryId))
                .ConfigureAwait(false);

            var newMenuItem = new MenuItem
            {
                MenuItemId = menuItem.MenuItemId,
                Name = menuItem.Name,
                Order = menuItem.Order,
                Icon = menuItem.Icon,
                NavigateTo = menuItem.NavigateTo,
                Config = menuItem.Config,
                Permission = menuItem.Permission,
                Category = category
            };

            await applicationDbContext.MenuItems
                .AddAsync(newMenuItem)
                .ConfigureAwait(false);

            await applicationDbContext
                .SaveChangesAsync()
                .ConfigureAwait(false);

            return newMenuItem;
        }

        public async Task<MenuItem> UpdateMenuItemAsync(MenuItem menuItem)
        {
            var existing = await applicationDbContext.MenuItems
                .Include(mi => mi.Category)
                .FirstOrDefaultAsync(mi => mi.MenuItemId.Equals(menuItem.MenuItemId))
                .ConfigureAwait(false);

            if (existing == null)
            {
                throw new NullReferenceException(
                    $"{nameof(menuItem)} MenuItemId {menuItem.MenuItemId} not found.");
            }
            else
            {
                applicationDbContext
                    .Entry(existing)
                    .CurrentValues.SetValues(menuItem);

                if (menuItem.Category == null
                    || menuItem.Category.CategoryId.Equals(0))
                {
                    throw new ArgumentNullException(nameof(menuItem.Category));
                }
                else
                {
                    existing.Category = menuItem.Category;
                }
            }

            await applicationDbContext
                .SaveChangesAsync()
                .ConfigureAwait(false);

            return existing;
        }

        public async Task<int> DeleteMenuItemAsync(int id)
        {
            var menuItem = await applicationDbContext.MenuItems
                .FirstOrDefaultAsync(mi => mi.MenuItemId.Equals(id))
                .ConfigureAwait(false);

            applicationDbContext.Remove(menuItem);

            return await applicationDbContext
                .SaveChangesAsync()
                .ConfigureAwait(false);
        }
    }
}