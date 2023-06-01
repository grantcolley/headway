using Headway.Core.Args;
using Headway.Core.Attributes;
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
    public class ModelFieldsOptionCheckItems : IOptionCheckItems
    {
        public Task<IEnumerable<OptionCheckItem>> GetOptionCheckItemsAsync(IEnumerable<Arg> args)
        {
            string modelName = null;

            if (args.HasArg(Constants.Args.LINK_SOURCE))
            {
                modelName = args.ArgValue(Constants.Args.LINK_VALUE);
            }
            else
            {
                modelName = args.ArgValue(Constants.Args.MODEL);
            }

            if(string.IsNullOrWhiteSpace(modelName))
            {
                return Task.FromResult((new List<OptionCheckItem> { new OptionCheckItem() }).AsEnumerable());
            }

            var models = TypeAttributeHelper.GetHeadwayTypesByAttribute(typeof(DynamicModelAttribute));

            var model = models.First(m => m.DisplayName.Equals(modelName)
                                        || m.Namespace.Equals(modelName));

            var type = Type.GetType(model.Namespace);

            var propertyInfos = PropertyInfoHelper.GetPropertyInfos(type);

            List<OptionCheckItem> optionItems= new();

            optionItems.AddRange((from p in propertyInfos
                                  orderby p.Name
                                  select new OptionCheckItem
                                  {
                                      Id = p.Name,
                                      Display = p.Name
                                  }).ToList());

            return Task.FromResult(optionItems.AsEnumerable());
        }
    }
}
