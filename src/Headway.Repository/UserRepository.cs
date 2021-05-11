using Headway.Core.Interface;
using Headway.Core.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Headway.Repository
{
    public class UserRepository : IUserRepository
    {
        public Task<IEnumerable<User>> GetUsersAsync(string permittedUserName)
        {
            var users = new List<User> { new User { Email = "abc@email.com", UserName = "abc" } };
            return Task.FromResult<IEnumerable<User>>(users);
        }

        public Task<User> GetUserAsync(string permittedUserName, string userName)
        {
            throw new NotImplementedException();
        }

        public Task<User> SaveUserAsync(string permittedUserName, User user)
        {
            throw new NotImplementedException();
        }

        public Task DeleteUserAsync(string permittedUserName, string userName)
        {
            throw new NotImplementedException();
        }
    }
}