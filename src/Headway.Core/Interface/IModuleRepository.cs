using Headway.Core.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Headway.Core.Interface
{
    public interface IModuleRepository
    {
        Task<IEnumerable<Module>> GetModulesAsync(string userName);
    }
}
