using Headway.Core.Interface;
using Headway.Core.Model;
using Headway.Repository.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Headway.Repository
{
    public class AuthorisationRepository : RepositoryBase<AuthorisationRepository>, IAuthorisationRepository
    {
        public AuthorisationRepository(ApplicationDbContext applicationDbContext, ILogger<AuthorisationRepository> logger)
            : base (applicationDbContext, logger)
        {
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await applicationDbContext.Users
                .Include(u => u.Permissions)
                .Include(u => u.Roles)
                .AsNoTracking()
                .ToListAsync()
                .ConfigureAwait(false);
        }

        public async Task<User> GetUserAsync(int userId)
        {
            return await applicationDbContext.Users
                .Include(u => u.Permissions)
                .Include(u => u.Roles)
                .AsNoTracking()
                .SingleAsync(u => u.UserId.Equals(userId))
                .ConfigureAwait(false);
        }

        public async Task<User> AddUserAsync(User addUser)
        {
            var user = new User
            {
                UserName = addUser.UserName,
                Email = addUser.Email
            };

            await applicationDbContext.Users
                .AddAsync(user)
                .ConfigureAwait(false);

            await applicationDbContext
                .SaveChangesAsync()
                .ConfigureAwait(false);

            if(addUser.Permissions.Any()
                || addUser.Roles.Any())
            {
                user.Permissions.AddRange(addUser.Permissions);
                user.Roles.AddRange(addUser.Roles);

                await applicationDbContext
                    .SaveChangesAsync()
                    .ConfigureAwait(false);
            }

            return user;
        }

        public async Task<User> UpdateUserAsync(User updateUser)
        {
            var user = await applicationDbContext.Users
                .Include(u => u.Permissions)
                .Include(u => u.Roles)
                .SingleAsync(u => u.UserId.Equals(updateUser.UserId))
                .ConfigureAwait(false);

            if (!user.UserName.Equals(updateUser.UserName))
            {
                user.UserName = updateUser.UserName;
            }

            if (!user.Email.Equals(updateUser.Email))
            {
                user.Email = updateUser.Email;
            }

            var removePermissions = user.Permissions
                .Where(up => !updateUser.Permissions.Any(p => p.PermissionId.Equals(up.PermissionId)))
                .ToList();

            foreach(var permission in removePermissions)
            {
                user.Permissions.Remove(permission);
            }

            var addPermissions = updateUser.Permissions
                .Where(up => !user.Permissions.Any(p => p.PermissionId.Equals(up.PermissionId)))
                .ToList();

            user.Permissions.AddRange(addPermissions);

            var removeRoles = user.Roles
                .Where(ur => !updateUser.Roles.Any(r => r.RoleId.Equals(ur.RoleId)))
                .ToList();

            foreach (var role in removeRoles)
            {
                user.Roles.Remove(role);
            }

            var addRoles = updateUser.Roles
                .Where(ur => !user.Roles.Any(r => r.RoleId.Equals(ur.RoleId)))
                .ToList();

            user.Roles.AddRange(addRoles);

            applicationDbContext.Users.Update(user);

            await applicationDbContext
                .SaveChangesAsync()
                .ConfigureAwait(false);

            return user;
        }

        public async Task<int> DeleteUserAsync(int userId)
        {
            var user = await applicationDbContext.Users
                .SingleAsync(u => u.UserId.Equals(userId))
                .ConfigureAwait(false);

            applicationDbContext.Users.Remove(user);

            return await applicationDbContext
                .SaveChangesAsync()
                .ConfigureAwait(false);
        }

        public async Task<IEnumerable<Permission>> GetPermissionsAsync()
        {
            return await applicationDbContext.Permissions
                .AsNoTracking()
                .ToListAsync()
                .ConfigureAwait(false);
        }

        public async Task<Permission> GetPermissionAsync(int permissionId)
        {
            return await applicationDbContext.Permissions
                .AsNoTracking()
                .SingleAsync(p => p.PermissionId.Equals(permissionId))
                .ConfigureAwait(false);
        }

        public async Task<Permission> AddPermissionAsync(Permission permission)
        {
            await applicationDbContext.Permissions
                .AddAsync(permission)
                .ConfigureAwait(false);

            await applicationDbContext
                .SaveChangesAsync()
                .ConfigureAwait(false);

            return permission;
        }

        public async Task<Permission> UpdatePermissionAsync(Permission permission)
        {
            applicationDbContext.Permissions.Update(permission);

            await applicationDbContext
                .SaveChangesAsync()
                .ConfigureAwait(false);

            return permission;
        }

        public async Task<int> DeletePermissionAsync(int permissionId)
        {
            var permission = await applicationDbContext.Permissions
                .SingleAsync(p => p.PermissionId.Equals(permissionId))
                .ConfigureAwait(false);

            applicationDbContext.Permissions.Remove(permission);

            return await applicationDbContext
                .SaveChangesAsync()
                .ConfigureAwait(false);
        }

        public async Task<IEnumerable<Role>> GetRolesAsync()
        {
            return await applicationDbContext.Roles
                .Include(p => p.Permissions)
                .AsNoTracking()
                .ToListAsync()
                .ConfigureAwait(false);
        }

        public async Task<Role> GetRoleAsync(int roleId)
        {
            return await applicationDbContext.Roles
                .Include(r => r.Permissions)
                .AsNoTracking()
                .SingleAsync(r => r.RoleId.Equals(roleId))
                .ConfigureAwait(false);
        }

        public async Task<Role> AddRoleAsync(Role addRole)
        {
            var role = new Role
            {
                Name = addRole.Name,
                Description = addRole.Description
            };

            await applicationDbContext.Roles
                .AddAsync(role)
                .ConfigureAwait(false);

            await applicationDbContext
                .SaveChangesAsync()
                .ConfigureAwait(false);

            if(addRole.Permissions.Any())
            {
                role.Permissions.AddRange(addRole.Permissions);

                await applicationDbContext
                    .SaveChangesAsync()
                    .ConfigureAwait(false);
            }

            return role;
        }

        public async Task<Role> UpdateRoleAsync(Role updateRole)
        {
            var role = await applicationDbContext.Roles
                .Include(r => r.Permissions)
                .SingleAsync(r => r.RoleId.Equals(updateRole.RoleId))
                .ConfigureAwait(false);

            if (!role.Name.Equals(updateRole.Name))
            {
                role.Name = updateRole.Name;
            }

            if (!role.Description.Equals(updateRole.Description))
            {
                role.Description = updateRole.Description;
            }

            var removePermissions = role.Permissions
                .Where(rp => !updateRole.Permissions.Any(p => p.PermissionId.Equals(rp.PermissionId)))
                .ToList();

            foreach (var permission in removePermissions)
            {
                role.Permissions.Remove(permission);
            }

            var addPermissions = updateRole.Permissions
                .Where(rp => !role.Permissions.Any(p => p.PermissionId.Equals(rp.PermissionId)))
                .ToList();

            role.Permissions.AddRange(addPermissions);

            applicationDbContext.Roles.Update(role);

            await applicationDbContext
                .SaveChangesAsync()
                .ConfigureAwait(false);

            return role;
        }

        public async Task<int> DeleteRoleAsync(int roleId)
        {
            var role = await applicationDbContext.Roles
                .SingleAsync(r => r.RoleId.Equals(roleId))
                .ConfigureAwait(false);

            applicationDbContext.Roles.Remove(role);

            return await applicationDbContext
                .SaveChangesAsync()
                .ConfigureAwait(false);
        }
    }
}