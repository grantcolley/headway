using Headway.Core.Attributes;
using Headway.Core.Helpers;
using Headway.Core.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Headway.Services.Options
{
    public class ContainerOptionItems : IOptionItems
    {
        public Task<IEnumerable<OptionItem>> GetOptionItemsAsync()
        {
            var containers = TypeAttributeHelper.GetHeadwayTypesByAttribute(typeof(DynamicContainerAttribute));

            var optionItems = from c in containers
                              select new OptionItem
                              {
                                  Id = c.Name,
                                  Display = c.DisplayName
                              };

            return Task.FromResult(optionItems);
        }
    }
}
