using Headway.Core.Attributes;
using Headway.Core.Helpers;
using Headway.Core.Interface;
using Headway.Core.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Headway.Core.Options
{
    public class ModelOptionItems : IOptionItems
    {
        public Task<IEnumerable<OptionItem>> GetOptionItemsAsync()
        {
            var models = TypeAttributeHelper.GetHeadwayTypesByAttribute(typeof(DynamicModelAttribute));

            var optionItems = from m in models
                              select new OptionItem
                              {
                                  Id = m.Namespace,
                                  Display = m.DisplayName
                              };

            return Task.FromResult(optionItems);
        }
    }
}
