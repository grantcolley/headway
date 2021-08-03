using Headway.Core.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Headway.Services.Options
{
    public interface IOptionItems
    {
        Task<IEnumerable<OptionItem>> GetOptionItemsAsync();
    }
}
