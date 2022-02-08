using Headway.Core.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Headway.Core.Interface
{
    public interface IModuleRepository : IRepository
    {
        Task<IEnumerable<Module>> GetModulesAsync(string claim);
        Task<IEnumerable<Module>> GetModulesAsync();
        Task<Module> GetModuleAsync(int id);
        Task<Module> AddModuleAsync(Module module);
        Task<Module> UpdateModuleAsync(Module module);
        Task<int> DeleteModuleAsync(int id);
    }
}
