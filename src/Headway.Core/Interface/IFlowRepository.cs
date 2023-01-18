using Headway.Core.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Headway.Core.Interface
{
    public interface IFlowRepository : IRepository
    {
        Task<IEnumerable<Flow>> GetFlowsAsync();
        Task<Flow> GetFlowAsync(int id);
        Task<Flow> AddFlowAsync(Flow flow);
        Task<Flow> UpdateFlowAsync(Flow flow);
        Task<int> DeleteFlowAsync(int id);
    }
}
