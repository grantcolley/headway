using Headway.Core.Interface;
using Headway.Core.Model;
using Headway.Repository.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Headway.Repository
{
    public class AuthorisationRepository : RepositoryBase, IAuthorisationRepository
    {
        public AuthorisationRepository(ApplicationDbContext applicationDbContext)
            : base (applicationDbContext)
        {
        }

        public async Task<IEnumerable<User>> GetUsersAsync(string claim)
        {
            await ValidateClaim(claim, "Admin");

            return await applicationDbContext.Users.ToListAsync()
                .ConfigureAwait(false);
        }

        public async Task<User> GetUserAsync(string claim, int userId)
        {
            await ValidateClaim(claim, "Admin");

            return await applicationDbContext.Users.FirstOrDefaultAsync(
                u => u.UserId.Equals(userId))
                .ConfigureAwait(false);
        }

        public async Task<User> SaveUserAsync(string claim, User user)
        {
            await ValidateClaim(claim, "Admin");

            if(user.UserId.Equals(0))
            {
                var x = applicationDbContext.Users.Add(user);
                await applicationDbContext.SaveChangesAsync()
                    .ConfigureAwait(false);
                // do we have the new id?
            }
            else
            {
                applicationDbContext.Users.Update(user);
                await applicationDbContext.SaveChangesAsync()
                    .ConfigureAwait(false);
            }

            return user;
        }

        public async Task DeleteUserAsync(string claim, int userId)
        {
            await ValidateClaim(claim, "Admin");

            var user = await applicationDbContext.Users.FirstOrDefaultAsync(
                u => u.UserId.Equals(userId))
                .ConfigureAwait(false);

            if(user != null)
            {
                applicationDbContext.Users.Remove(user);
                await applicationDbContext.SaveChangesAsync()
                    .ConfigureAwait(false);
            }
        }

        public async Task<IEnumerable<Permission>> GetPermissionsAsync(string claim)
        {
            await ValidateClaim(claim, "Admin");

            return await applicationDbContext.Permissions.ToListAsync()
                .ConfigureAwait(false);
        }

        public async Task<Permission> GetPermissionAsync(string claim, int permissionId)
        {
            await ValidateClaim(claim, "Admin");

            return await applicationDbContext.Permissions.FirstOrDefaultAsync(
                p => p.PermissionId.Equals(permissionId))
                .ConfigureAwait(false);
        }

        public async Task<Permission> SavePermissionAsync(string claim, Permission permission)
        {
            await ValidateClaim(claim, "Admin");

            if (permission.PermissionId.Equals(0))
            {
                var x = applicationDbContext.Permissions.Add(permission);
                await applicationDbContext.SaveChangesAsync()
                    .ConfigureAwait(false);
                // do we have the new id?
            }
            else
            {
                applicationDbContext.Permissions.Update(permission);
                await applicationDbContext.SaveChangesAsync()
                    .ConfigureAwait(false);
            }

            return permission;
        }

        public async Task DeletePermissionAsync(string claim, int permissionId)
        {
            await ValidateClaim(claim, "Admin");

            var permission = await applicationDbContext.Permissions.FirstOrDefaultAsync(
                p => p.PermissionId.Equals(permissionId))
                .ConfigureAwait(false);

            if (permission != null)
            {
                applicationDbContext.Permissions.Remove(permission);
                await applicationDbContext.SaveChangesAsync()
                    .ConfigureAwait(false);
            }
        }

        public async Task<IEnumerable<Role>> GetRolesAsync(string claim)
        {
            await ValidateClaim(claim, "Admin");

            return await applicationDbContext.Roles.ToListAsync()
                .ConfigureAwait(false);
        }

        public async Task<Role> GetRoleAsync(string claim, int roleId)
        {
            await ValidateClaim(claim, "Admin");

            return await applicationDbContext.Roles.FirstOrDefaultAsync(
                r => r.RoleId.Equals(roleId))
                .ConfigureAwait(false);
        }

        public async Task<Role> SaveRoleAsync(string claim, Role role)
        {
            await ValidateClaim(claim, "Admin");

            if (role.RoleId.Equals(0))
            {
                var x = applicationDbContext.Roles.Add(role);
                await applicationDbContext.SaveChangesAsync()
                    .ConfigureAwait(false);
                // do we have the new id?
            }
            else
            {
                applicationDbContext.Roles.Update(role);
                await applicationDbContext.SaveChangesAsync()
                    .ConfigureAwait(false);
            }

            return role;
        }

        public async Task DeleteRoleAsync(string claim, int roleId)
        {
            await ValidateClaim(claim, "Admin");

            var role = await applicationDbContext.Roles.FirstOrDefaultAsync(
                r => r.RoleId.Equals(roleId))
                .ConfigureAwait(false);

            if (role != null)
            {
                applicationDbContext.Roles.Remove(role);
                await applicationDbContext.SaveChangesAsync()
                    .ConfigureAwait(false);
            }
        }
    }
}