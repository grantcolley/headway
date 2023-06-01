using Headway.Core.Args;
using Headway.Core.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Headway.Core.Interface
{
    public interface IOptionsApiRequest
    {
        Task<IResponse<IEnumerable<OptionCheckItem>>> GetOptionCheckItemsAsync(List<Arg> args);
        Task<IResponse<IEnumerable<OptionCheckItem>>> GetOptionCheckItemsAsync(List<DynamicArg> dynamicArgs);
        Task<IResponse<IEnumerable<OptionItem>>> GetOptionItemsAsync(List<Arg> args);
        Task<IResponse<IEnumerable<OptionItem>>> GetOptionItemsAsync(List<DynamicArg> dynamicArgs);
        Task<IResponse<IEnumerable<T>>> GetOptionItemsAsync<T>(List<DynamicArg> dynamicArgs);
    }
}
