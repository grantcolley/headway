using Headway.Core.Model;
using System;
using System.Threading.Tasks;

namespace Headway.Core.Interface
{
    public interface IRepository : IDisposable
    {
        void SetUser(string user);
        Task<bool> IsAuthorisedAsync(string claim, string permission);
        Task<User> GetAuthorisedUserAsync(string claim, string permission);
    }
}
