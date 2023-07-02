using Headway.Core.Args;
using Headway.Core.Interface;
using RemediatR.Core.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RemediatR.Core.Interface
{
    public interface IRemediatRRedressRepository<T> : IRepositoryFlowable<T> where T : class, IFlowContext
    {
        Task<IEnumerable<RedressCase>> GetRedressCasesAsync(SearchArgs searchArgs);
        Task<IEnumerable<NewRedressCase>> SearchNewRedressCasesAsync(SearchArgs searchArgs);
        Task<Redress> CreateRedressAsync(DataArgs dataArgs);
        Task<Redress> GetRedressAsync(int id);
        Task<Redress> AddRedressAsync(Redress redress);
        Task<Redress> UpdateRedressAsync(Redress redress);
        Task<int> DeleteRedressAsync(int id);
    }
}
