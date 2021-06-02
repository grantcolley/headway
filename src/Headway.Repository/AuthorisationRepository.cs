using Headway.Core.Interface;
using Headway.Core.Model;
using Headway.Repository.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Headway.Repository
{
    public class AuthorisationRepository : Repository, IAuthorisationRepository
    {
        public AuthorisationRepository(ApplicationDbContext applicationDbContext)
            : base (applicationDbContext)
        {
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await applicationDbContext.Users
                .Include(u => u.Permissions)
                .Include(u => u.Roles)
                .ToListAsync()
                .ConfigureAwait(false);
        }

        public async Task<User> GetUserAsync(int userId)
        {
            var user = await applicationDbContext.Users
                .Include(u => u.Permissions)
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(u => u.UserId.Equals(userId))
                .ConfigureAwait(false);
            return user;
        }

        public async Task<User> AddUserAsync(User user)
        {
            applicationDbContext.Users.Add(user);

            await applicationDbContext.SaveChangesAsync().ConfigureAwait(false);

            return user;
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            applicationDbContext.Users.Update(user);

            await applicationDbContext.SaveChangesAsync().ConfigureAwait(false);

            return user;
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            int result = 0;

            var user = await applicationDbContext.Users.FirstOrDefaultAsync(
                u => u.UserId.Equals(userId))
                .ConfigureAwait(false);

            if(user != null)
            {
                applicationDbContext.Users.Remove(user);
                result = await applicationDbContext.SaveChangesAsync()
                    .ConfigureAwait(false);
            }

            return result > 0;
        }

        public async Task<IEnumerable<Permission>> GetPermissionsAsync()
        {
            return await applicationDbContext.Permissions.ToListAsync()
                .ConfigureAwait(false);
        }

        public async Task<Permission> GetPermissionAsync(int permissionId)
        {
            return await applicationDbContext.Permissions.FirstOrDefaultAsync(
                p => p.PermissionId.Equals(permissionId))
                .ConfigureAwait(false);
        }

        public async Task<Permission> AddPermissionAsync(Permission permission)
        {
            applicationDbContext.Permissions.Add(permission);
            await applicationDbContext.SaveChangesAsync()
                            .ConfigureAwait(false);
            return permission;
        }

        public async Task<Permission> UpdatePermissionAsync(Permission permission)
        {
            applicationDbContext.Permissions.Update(permission);
            await applicationDbContext.SaveChangesAsync()
                            .ConfigureAwait(false);
            return permission;
        }

        public async Task<bool> DeletePermissionAsync(int permissionId)
        {
            int result = 0;

            var permission = await applicationDbContext.Permissions.FirstOrDefaultAsync(
                p => p.PermissionId.Equals(permissionId))
                .ConfigureAwait(false);

            if (permission != null)
            {
                applicationDbContext.Permissions.Remove(permission);
                result = await applicationDbContext.SaveChangesAsync()
                            .ConfigureAwait(false);
            }

            return result > 0;
        }

        public async Task<IEnumerable<Role>> GetRolesAsync()
        {
            return await applicationDbContext.Roles
                .Include(p => p.Permissions)
                .ToListAsync()
                .ConfigureAwait(false);
        }

        public async Task<Role> GetRoleAsync(int roleId)
        {
            return await applicationDbContext.Roles
                .Include(r => r.Permissions)
                .FirstOrDefaultAsync(r => r.RoleId.Equals(roleId))
                .ConfigureAwait(false);
        }

        public async Task<Role> AddRoleAsync(Role role)
        {
            applicationDbContext.Roles.Add(role);
            await applicationDbContext.SaveChangesAsync()
                                            .ConfigureAwait(false);
            return role;
        }

        public async Task<Role> UpdateRoleAsync(Role role)
        {
            applicationDbContext.Roles.Update(role);
            await applicationDbContext.SaveChangesAsync()
                            .ConfigureAwait(false);
            return role;
        }

        public async Task<bool> DeleteRoleAsync(int roleId)
        {
            int result = 0;

            var role = await applicationDbContext.Roles.FirstOrDefaultAsync(
                                r => r.RoleId.Equals(roleId))
                                .ConfigureAwait(false);

            if (role != null)
            {
                applicationDbContext.Roles.Remove(role);
                result = await applicationDbContext.SaveChangesAsync()
                            .ConfigureAwait(false);
            }

            return result > 0;
        }
    }
}