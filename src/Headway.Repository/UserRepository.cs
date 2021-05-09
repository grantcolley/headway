using Headway.Core.Interface;
using Headway.Core.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Headway.Repository
{
    public class UserRepository : IUserRepository
    {
        public Task DeleteUserAsync(string permittedUserName, string userName)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetUserAsync(string permittedUserName, string userName)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<User>> GetUsersAsync(string permittedUserName)
        {
            throw new NotImplementedException();
        }

        public Task<User> SaveUserAsync(string permittedUserName, User user)
        {
            throw new NotImplementedException();
        }
    }
}