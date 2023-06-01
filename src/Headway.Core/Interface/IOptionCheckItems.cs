using Headway.Core.Args;
using Headway.Core.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Headway.Core.Interface
{
    public interface IOptionCheckItems
    {
        Task<IEnumerable<OptionCheckItem>> GetOptionCheckItemsAsync(IEnumerable<Arg> args);
    }
}
