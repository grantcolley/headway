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
            var modelName = args.Single(a => a.Name.Equals("Model")).Value.ToString();

            var models = TypeAttributeHelper.GetHeadwayTypesByAttribute(typeof(DynamicModelAttribute));

            var model = models.Single(m => m.DisplayName.Equals(modelName));

            var type = Type.GetType(model.Namespace);

            var propertyInfos = PropertyInfoHelper.GetPropertyInfos(type);

            List<OptionItem> optionItems= new() { new OptionItem() };

            optionItems.AddRange((from p in propertyInfos
                                  orderby p.Name
                                  select new OptionItem
                                  {
                                      Id = p.Name,
                                      Display = p.Name
                                  }).ToList());

            return Task.FromResult(optionItems.AsEnumerable());
        }
    }
}
