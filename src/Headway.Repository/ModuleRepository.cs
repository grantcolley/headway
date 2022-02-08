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

        public async Task<IEnumerable<Module>> GetModulesAsync()
        {
            return await applicationDbContext.Modules
                .AsNoTracking()
                .OrderBy(m => m.Order)
                .ToListAsync()
                .ConfigureAwait(false);
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
    }
}