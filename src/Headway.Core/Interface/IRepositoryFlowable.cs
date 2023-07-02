using System.Threading.Tasks;

namespace Headway.Core.Interface
{
    public interface IRepositoryFlowable<T> : IRepository where T : class, IFlowContext
    {
        Task<T> GetFlowContextAsync(int id);
        Task<T> UpdateFlowHistoryAsync(T model);
    }
}
