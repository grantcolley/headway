using Headway.Core.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Headway.Core.Interface
{
    public interface IOptionsRepository : IRepository
    {
        Task<IServiceResult<IEnumerable<OptionItem>>> GetOptionItemsAsync(string source);
    }
}
