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
                .ToListAsync()
                .ConfigureAwait(false);

            var permittedModules = modules
                .Where(m => m.IsPermitted(permissions))
                .ToList();

            return permittedModules;
        }
    }
}