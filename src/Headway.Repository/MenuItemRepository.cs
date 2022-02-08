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
    public class MenuItemRepository : RepositoryBase<MenuItemRepository>, IMenuItemRepository
    {
        public MenuItemRepository(ApplicationDbContext applicationDbContext, ILogger<MenuItemRepository> logger)
            : base(applicationDbContext, logger)
        {
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
                .FirstOrDefaultAsync(mi => mi.MenuItemId.Equals(id))
                .ConfigureAwait(false);
        }

        public async Task<MenuItem> AddMenuItemAsync(MenuItem menuItem)
        {
            var category = await applicationDbContext.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.CategoryId.Equals(menuItem.Category.CategoryId))
                .ConfigureAwait(false);

            var newMenuItem = new MenuItem
            {
                MenuItemId = menuItem.MenuItemId,
                Name = menuItem.Name,
                Order = menuItem.Order,
                ImageClass = menuItem.ImageClass,
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
