using Headway.Core.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Headway.Core.Interface
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetUsersAsync(string permittedUserName);
        Task<User> GetUserAsync(string permittedUserName, string userName);
        Task<User> SaveUserAsync(string permittedUserName, User user);
        Task DeleteUserAsync(string permittedUserName, string userName);
    }
}
