using Headway.Core.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Headway.Core.Interface
{
    public interface IUserService
    {
        Task<User> GetUserAsync(string userName);
        Task<IEnumerable<User>> GetUsersAsync();
        Task<bool> TryUpdateUserAsync(User user);
        Task<bool> TryDeleteUserAsync(string userName);
    }
}