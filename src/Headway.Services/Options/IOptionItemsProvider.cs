using Headway.Core.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Headway.Services.Options
{
    public interface IOptionItemsProvider
    {
        OptionItemsSource OptionItemsSource { get; set; }
        Task<IEnumerable<OptionItem>> GetOptionItemsAsync(string source);
    }
}
