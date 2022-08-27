using Headway.Core.Args;
using Headway.Core.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Headway.Core.Interface
{
    public interface IOptionItems
    {
        Task<IEnumerable<OptionItem>> GetOptionItemsAsync(IEnumerable<Arg> args);
    }
}
