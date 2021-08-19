using Headway.Core.Attributes;
using Headway.Core.Helpers;
using Headway.Core.Interface;
using Headway.Core.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Headway.Core.Options
{
    public class ControllerOptionItems : IOptionItems
    {
        public Task<IEnumerable<OptionItem>> GetOptionItemsAsync()
        {
            var controllers = TypeAttributeHelper.GetEntryAssemblyTypeNamesByAttribute(typeof(DynamicApiControllerAttribute));

            var optionItems = from c in controllers
                              select new OptionItem
                              {
                                  Id = c.Name.Replace("Controller", ""),
                                  Display = c.DisplayName.Replace("Controller", "")
                              };

            return Task.FromResult(optionItems);
        }
    }
}
