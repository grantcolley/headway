using Headway.Core.Attributes;
using Headway.Core.Helpers;
using Headway.Core.Interface;
using Headway.Core.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Headway.Core.Options
{
    public class ModelFieldsOptionItems : IOptionItems
    {
        public Task<IEnumerable<OptionItem>> GetOptionItemsAsync(IEnumerable<Arg> args)
        {
            var models = TypeAttributeHelper.GetHeadwayTypesByAttribute(typeof(DynamicModelAttribute));

            List<OptionItem> optionItems= new List<OptionItem>() { new OptionItem() };

            optionItems.AddRange((from m in models
                              select new OptionItem
                              {
                                  Id = m.Namespace,
                                  Display = m.DisplayName
                              }).ToList());

            return Task.FromResult(optionItems.AsEnumerable());
        }
    }
}
