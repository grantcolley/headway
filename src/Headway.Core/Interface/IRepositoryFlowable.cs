using System.Threading.Tasks;

namespace Headway.Core.Interface
{
    public interface IRepositoryFlowable<T> : IRepository
    {
        Task<T> GetFlowContextAsync(int id);
        Task<T> UpdateFlowHistoryAsync(T model);
    }
}
