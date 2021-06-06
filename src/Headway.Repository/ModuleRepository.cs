using Headway.Core.Interface;
using Headway.Core.Model;
using Headway.Repository.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Headway.Repository
{
    public class ModuleRepository : RepositoryBase, IModuleRepository
    {
        //private Module home = new Module
        //{
        //    Name = "Home",
        //    Order = 1,
        //    Categories = new List<Category>
        //    {
        //        new Category
        //        {
        //            Name = "Home Category",
        //            Order = 1,
        //            MenuItems = new List<MenuItem>
        //            {
        //                new MenuItem
        //                {
        //                    Id = 1,
        //                    Name = "Home",
        //                    ImageClass = "oi oi-home",
        //                    Path = "/",
        //                    Order = 1,
        //                    Authorised = new List<string>{ "User" }
        //                }
        //            },
        //            Authorised = new List<string>{ "User" }
        //        }
        //    },
        //    Roles = new List<string> { "User" }
        //};

        //private Module counter = new Module
        //{
        //    Name = "Counter",
        //    Order = 2,
        //    Categories = new List<Category>
        //    {
        //        new Category
        //        {
        //            Name = "Counter Category",
        //            Order = 1,
        //            MenuItems = new List<MenuItem>
        //            {
        //                new MenuItem
        //                {
        //                    Id = 2,
        //                    Name = "Counter",
        //                    ImageClass = "oi oi-plus",
        //                    Path = "counter",
        //                    Order = 1,
        //                    Authorised = new List<string>{ "User" }
        //                }
        //            },
        //            Authorised = new List<string>{ "User" }
        //        }
        //    },
        //    Roles = new List<string> { "User" }
        //};

        //private Module weather = new Module
        //{
        //    Name = "Weather",
        //    Order = 3,
        //    Categories = new List<Category>
        //    {
        //        new Category
        //        {
        //            Name = "Weather Category",
        //            Order = 1,
        //            MenuItems = new List<MenuItem>
        //            {
        //                new MenuItem
        //                {
        //                    Id = 3,
        //                    Name = "Fetch data",
        //                    ImageClass = "oi oi-list-rich",
        //                    Path = "fetchdata",
        //                    Order = 1,
        //                    Authorised = new List<string>{ "User" }
        //                }
        //            },
        //            Authorised = new List<string>{ "User" }
        //        }
        //    },
        //    Roles = new List<string> { "User" }
        //};

        //private Module configuration = new Module
        //{
        //    Name = "Administration",
        //    Order = 4,
        //    Categories = new List<Category>
        //    {
        //        new Category
        //        {
        //            Name = "Authorisation",
        //            Order = 1,
        //            MenuItems = new List<MenuItem>
        //            {
        //                new MenuItem
        //                {
        //                    Id = 5,
        //                    Name = "Users",
        //                    ImageClass = "oi oi-person",
        //                    Path = "users",
        //                    Order = 1,
        //                    Authorised = new List<string>{ "Admin" }
        //                },
        //                new MenuItem
        //                {
        //                    Id = 6,
        //                    Name = "Permissions",
        //                    ImageClass = "oi oi-key",
        //                    Path = "permissions",
        //                    Order = 1,
        //                    Authorised = new List<string>{ "Admin" }
        //                },
        //                new MenuItem
        //                {
        //                    Id = 6,
        //                    Name = "Roles",
        //                    ImageClass = "oi oi-lock-locked",
        //                    Path = "roles",
        //                    Order = 1,
        //                    Authorised = new List<string>{ "Admin" }
        //                }
        //            },
        //            Authorised = new List<string>{ "Admin" }
        //        }
        //    },
        //    Roles = new List<string> { "Admin" }
        //};

        public ModuleRepository(ApplicationDbContext applicationDbContext)
            : base(applicationDbContext)
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
                .Include(m => m.Categories.OrderBy(c => c.Order))
                .ThenInclude(c => c.MenuItems.OrderBy(mu => mu.Order))
                .Where(m => permissions.Contains(m.Permission))
                .AsNoTracking()
                .OrderBy(m => m.Order)
                .ToListAsync();

            var permittedModules = modules
                .Where(m => m.IsPermitted(permissions))
                .ToList();

            return permittedModules;
        }
    }
}