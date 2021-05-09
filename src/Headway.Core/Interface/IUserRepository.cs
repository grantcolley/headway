using Headway.Core.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Headway.Core.Interface
{
    public interface IUserRepository
    {
        Task<User> GetUserAsync(string permittedUserName, string userName);
        Task<IEnumerable<User>> GetUsersAsync(string permittedUserName);
        Task<bool> TryUpdateUserAsync(string permittedUserName, User user);
        Task<bool> TryDeleteUserAsync(string permittedUserName, string userName);
    }
}
