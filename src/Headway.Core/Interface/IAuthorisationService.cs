using Headway.Core.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Headway.Core.Interface
{
    public interface IAuthorisationService
    {
        Task<IEnumerable<User>> GetUsersAsync();
        Task<User> GetUserAsync(string userName);
        Task<User> SaveUserAsync(User user);
        Task DeleteUserAsync(string userName);
        Task<IEnumerable<Permission>> GetPermissionsAsync();
        Task<User> GetPermissionAsync(int permissionId);
        Task<User> SavePermissionAsync(Permission permission);
        Task DeletePermissionAsync(int permissionId);
        Task<IEnumerable<Role>> GetRolesAsync();
        Task<User> GetRoleAsync(int roleId);
        Task<User> SaveRoleAsync(Role role);
        Task DeleteRoleAsync(int roleId);
    }
}