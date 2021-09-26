using Headway.Core.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Headway.Core.Interface
{
    public interface IOptionsService
    {
        Task<IServiceResult<IEnumerable<OptionItem>>> GetOptionItemsAsync(List<DynamicArg> dynamicArgs);
        Task<IServiceResult<IEnumerable<T>>> GetOptionItemsAsync<T>(List<DynamicArg> dynamicArgs);
    }
}
