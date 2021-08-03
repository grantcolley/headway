using Headway.Core.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Headway.Core.Interface
{
    public interface IOptionsService
    {
        Task<IServiceResult<IEnumerable<OptionItem>>> GetOptionItemsAsync(string optionsCode);
    }
}
