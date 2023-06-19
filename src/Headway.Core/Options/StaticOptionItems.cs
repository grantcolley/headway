using Headway.Core.Args;
using Headway.Core.Interface;
using Headway.Core.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Headway.Core.Options
{
    public class StaticOptionItems : IOptionItems
    {
        public Task<IEnumerable<OptionItem>> GetOptionItemsAsync(IEnumerable<Arg> args)
        {
            var optionItemValues = args
                .Where(a => !a.Value.Equals(Constants.Options.STATIC_OPTION_ITEMS)
                        && !a.Value.Equals(Constants.Args.READ_ONLY))
                .ToList();

            var optionItems = optionItemValues
                .Select(a => new OptionItem { Id = a.Name, Display = a.Value })
                .ToList();

            optionItems.Insert(0, new OptionItem());

            return Task.FromResult(optionItems.AsEnumerable());
        }
    }
}
