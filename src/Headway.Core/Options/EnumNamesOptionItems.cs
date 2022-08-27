using Headway.Core.Args;
using Headway.Core.Extensions;
using Headway.Core.Interface;
using Headway.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Headway.Core.Options
{
    public class EnumNamesOptionItems : IOptionItems
    {
        public Task<IEnumerable<OptionItem>> GetOptionItemsAsync(IEnumerable<Arg> args)
        {
            var typeName = args.ArgValue(Constants.Args.TYPE);

            var type = Type.GetType(typeName);

            string[] enumIems = Enum.GetNames(type);

            var optionItems = new List<OptionItem>(enumIems.Select(i => new OptionItem { Id = i, Display = i }));

            optionItems.Insert(0, new OptionItem());

            return Task.FromResult(optionItems.AsEnumerable());
        }
    }
}
