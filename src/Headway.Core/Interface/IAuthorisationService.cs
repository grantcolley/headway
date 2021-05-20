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
    }
}