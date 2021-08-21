using Headway.Core.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Headway.Core.Interface
{
    public interface IOptionsRepository : IRepository
    {
        Task<IEnumerable<OptionItem>> GetOptionItemsAsync(string source, List<Arg> args);
    }
}
