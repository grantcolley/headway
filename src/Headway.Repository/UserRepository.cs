using Headway.Core.Interface;
using Headway.Core.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Headway.Repository
{
    public class UserRepository : IUserRepository
    {
        public Task<User> GetUserAsync(string userName)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<User>> GetUsersAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> TryDeleteUserAsync(string userName)
        {
            throw new NotImplementedException();
        }

        public Task<bool> TryUpdateUserAsync(User user)
        {
            throw new NotImplementedException();
        }
    }
}
