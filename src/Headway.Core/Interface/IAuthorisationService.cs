using Headway.Core.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Headway.Core.Interface
{
    public interface IAuthorisationService
    {
        Task<IEnumerable<User>> GetUsersAsync();
        Task<User> GetUserAsync(int userId);
        Task<User> SaveUserAsync(User user);
        Task DeleteUserAsync(int userId);
        Task<IEnumerable<Permission>> GetPermissionsAsync();
        Task<Permission> GetPermissionAsync(int permissionId);
        Task<Permission> SavePermissionAsync(Permission permission);
        Task DeletePermissionAsync(int permissionId);
        Task<IEnumerable<Role>> GetRolesAsync();
        Task<Role> GetRoleAsync(int roleId);
        Task<Role> SaveRoleAsync(Role role);
        Task DeleteRoleAsync(int roleId);
    }
}