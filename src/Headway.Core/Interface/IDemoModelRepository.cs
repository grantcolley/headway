using Headway.Core.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Headway.Core.Interface
{
    public interface IDemoModelRepository : IRepository
    {
        Task<IEnumerable<DemoModel>> GetDemoModelsAsync();
        Task<DemoModel> GetDemoModelAsync(int id);
        Task<DemoModel> AddDemoModelAsync(DemoModel demoModel);
        Task<DemoModel> UpdateDemoModelAsync(DemoModel demoModel);
        Task<int> DeleteDemoModelAsync(int id);
    }
}
