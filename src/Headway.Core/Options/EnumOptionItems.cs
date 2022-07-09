using Headway.Core.Constants;
using Headway.Core.Interface;
using Headway.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Headway.Core.Options
{
    public class EnumOptionItems : IOptionItems
    {
        public Task<IEnumerable<OptionItem>> GetOptionItemsAsync(IEnumerable<Arg> args)
        {
            var enumName = args.First(a => a.Name.Equals(Args.ENUM)).Value.ToString();

            var type = Type.GetType(enumName);

            List<OptionItem> optionItems = new();

            var optionItemsArray = Enum.GetValues(type);

            foreach (var optionItem in optionItemsArray)
            {
                optionItems.Add(new OptionItem 
                {
                    Id = optionItem.ToString(),
                    Display = optionItem.ToString(),
                    IsNumericId = true
                });
            }

            return Task.FromResult(optionItems.AsEnumerable());
        }
    }
}
