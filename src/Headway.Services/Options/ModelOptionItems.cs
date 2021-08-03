using Headway.Core.Attributes;
using Headway.Core.Helpers;
using Headway.Core.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Headway.Services.Options
{
    public class ModelOptionItems : IOptionItems
    {
        public Task<IEnumerable<OptionItem>> GetOptionItemsAsync()
        {
            var models = TypeAttributeHelper.GetHeadwayTypesByAttribute(typeof(DynamicModelAttribute));

            var optionItems = from m in models
                              select new OptionItem
                              {
                                  Id = m.Name,
                                  Display = m.DisplayName
                              };

            return Task.FromResult(optionItems);
        }
    }
}
