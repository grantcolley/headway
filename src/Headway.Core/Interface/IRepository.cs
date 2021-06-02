using System;
using System.Threading.Tasks;

namespace Headway.Core.Interface
{
    public interface IRepository : IDisposable
    {
        Task<bool> IsAuthorisedAsync(string claim, string permission);
    }
}
