using Headway.Core.Interface;
using Headway.Core.Model;
using Headway.Repository.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Headway.Repository
{
    public class AuthorisationRepository : Repository, IAuthorisationRepository
    {
        public AuthorisationRepository(ApplicationDbContext applicationDbContext)
            : base (applicationDbContext)
        {
        }

        public async Task<IEnumerable<User>> GetUsersAsync(string claim)
        {
            return await applicationDbContext.Users.ToListAsync()
                .ConfigureAwait(false);
        }

        public async Task<User> GetUserAsync(string claim, int userId)
        {
            return await applicationDbContext.Users.FirstOrDefaultAsync(
                            u => u.UserId.Equals(userId))
                            .ConfigureAwait(false);
        }

        public async Task<User> AddUserAsync(string claim, User user)
        {
            applicationDbContext.Users.Add(user);
            await applicationDbContext.SaveChangesAsync()
                                            .ConfigureAwait(false);
            return user;
        }

        public async Task<User> UpdateUserAsync(string claim, User user)
        {
            applicationDbContext.Users.Update(user);
            await applicationDbContext.SaveChangesAsync()
                            .ConfigureAwait(false);
            return user;
        }

        public async Task<bool> DeleteUserAsync(string claim, int userId)
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

        public async Task<IEnumerable<Permission>> GetPermissionsAsync(string claim)
        {
            return await applicationDbContext.Permissions.ToListAsync()
                       .ConfigureAwait(false);
        }

        public async Task<Permission> GetPermissionAsync(string claim, int permissionId)
        {
            return await applicationDbContext.Permissions.FirstOrDefaultAsync(
                p => p.PermissionId.Equals(permissionId))
                .ConfigureAwait(false);
        }

        public async Task<Permission> AddPermissionAsync(string claim, Permission permission)
        {
            applicationDbContext.Permissions.Add(permission);
            await applicationDbContext.SaveChangesAsync()
                            .ConfigureAwait(false);
            return permission;
        }

        public async Task<Permission> UpdatePermissionAsync(string claim, Permission permission)
        {
            applicationDbContext.Permissions.Update(permission);
            await applicationDbContext.SaveChangesAsync()
                            .ConfigureAwait(false);
            return permission;
        }

        public async Task<bool> DeletePermissionAsync(string claim, int permissionId)
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

        public async Task<IEnumerable<Role>> GetRolesAsync(string claim)
        {
            return await applicationDbContext.Roles.ToListAsync()
                    .ConfigureAwait(false);
        }

        public async Task<Role> GetRoleAsync(string claim, int roleId)
        {
            return await applicationDbContext.Roles.FirstOrDefaultAsync(
                            r => r.RoleId.Equals(roleId))
                            .ConfigureAwait(false);
        }

        public async Task<Role> AddRoleAsync(string claim, Role role)
        {
            applicationDbContext.Roles.Add(role);
            await applicationDbContext.SaveChangesAsync()
                                            .ConfigureAwait(false);
            return role;
        }

        public async Task<Role> UpdateRoleAsync(string claim, Role role)
        {
            applicationDbContext.Roles.Update(role);
            await applicationDbContext.SaveChangesAsync()
                            .ConfigureAwait(false);
            return role;
        }

        public async Task<bool> DeleteRoleAsync(string claim, int roleId)
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