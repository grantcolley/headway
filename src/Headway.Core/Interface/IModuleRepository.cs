using Headway.Core.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Headway.Core.Interface
{
    public interface IModuleRepository : IRepository
    {
        Task<IEnumerable<Module>> GetModulesAsync(string claim);
    }
}
