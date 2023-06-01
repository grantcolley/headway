using Headway.Core.Args;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Headway.Core.Interface
{
    public interface IOptionTextItems
    {
        Task<IEnumerable<string>> GetOptionTextItemsAsync(IEnumerable<Arg> args);
    }
}
