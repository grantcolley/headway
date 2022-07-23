using Headway.Core.Attributes;
using Headway.Core.Constants;
using Headway.Core.Extensions;
using Headway.Core.Helpers;
using Headway.Core.Interface;
using Headway.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Headway.Core.Options
{
    public class ModelFieldsOptionItems : IOptionItems
    {
        public Task<IEnumerable<OptionItem>> GetOptionItemsAsync(IEnumerable<Arg> args)
        {
            string modelName = null;

            if (args.HasArg(Args.LINK_SOURCE))
            {
                modelName = args.ArgValue(Args.LINK_VALUE);
            }
            else
            {
                modelName = args.ArgValue(Args.MODEL);
            }

            if(string.IsNullOrWhiteSpace(modelName))
            {
                return Task.FromResult((new List<OptionItem> { new OptionItem() }).AsEnumerable());
            }

            var models = TypeAttributeHelper.GetHeadwayTypesByAttribute(typeof(DynamicModelAttribute));

            var model = models.First(m => m.DisplayName.Equals(modelName)
                                        || m.Namespace.Equals(modelName));

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
