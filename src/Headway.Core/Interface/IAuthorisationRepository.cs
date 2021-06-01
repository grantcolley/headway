using Headway.Core.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Headway.Core.Interface
{
    public interface IAuthorisationRepository : IRepository
    {
        Task<IEnumerable<User>> GetUsersAsync();
        Task<User> GetUserAsync(int userId);
        Task<User> AddUserAsync(User user);
        Task<User> UpdateUserAsync(User user);
        Task<bool> DeleteUserAsync(int userId);
        Task<IEnumerable<Permission>> GetPermissionsAsync();
        Task<Permission> GetPermissionAsync(int permissionId);
        Task<Permission> AddPermissionAsync(Permission permission);
        Task<Permission> UpdatePermissionAsync(Permission permission);
        Task<bool> DeletePermissionAsync(int permissionId);
        Task<IEnumerable<Role>> GetRolesAsync();
        Task<Role> GetRoleAsync(int roleId);
        Task<Role> AddRoleAsync(Role role);
        Task<Role> UpdateRoleAsync(Role role);
        Task<bool> DeleteRoleAsync(int roleId);
    }
}
