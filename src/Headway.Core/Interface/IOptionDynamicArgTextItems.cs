using Headway.Core.Args;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Headway.Core.Interface
{
    public interface IOptionDynamicArgTextItems
    {
        Task<IEnumerable<string>> GetOptionDynamicArgTextItemsAsync(IEnumerable<DynamicArg> dynamicArgs);
    }
}
