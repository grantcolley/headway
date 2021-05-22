using Headway.Core.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Headway.Core.Interface
{
    public interface IAuthorisationRepository : IRepository
    {
        Task<IEnumerable<User>> GetUsersAsync(string claim);
        Task<User> GetUserAsync(string claim, int userId);
        Task<User> SaveUserAsync(string claim, User user);
        Task<bool> DeleteUserAsync(string claim, int userId);
        Task<IEnumerable<Permission>> GetPermissionsAsync(string claim);
        Task<Permission> GetPermissionAsync(string claim, int permissionId);
        Task<Permission> SavePermissionAsync(string claim, Permission permission);
        Task<bool> DeletePermissionAsync(string claim, int permissionId);
        Task<IEnumerable<Role>> GetRolesAsync(string claim);
        Task<Role> GetRoleAsync(string claim, int roleId);
        Task<Role> SaveRoleAsync(string claim, Role role);
        Task<bool> DeleteRoleAsync(string claim, int roleId);
    }
}
