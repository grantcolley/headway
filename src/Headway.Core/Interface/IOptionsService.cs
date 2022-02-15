using Headway.Core.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Headway.Core.Interface
{
    public interface IOptionsService
    {
        Task<IResponse<IEnumerable<OptionItem>>> GetOptionItemsAsync(List<DynamicArg> dynamicArgs);
        Task<IResponse<IEnumerable<T>>> GetOptionItemsAsync<T>(List<DynamicArg> dynamicArgs);
    }
}
