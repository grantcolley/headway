using Headway.Core.Attributes;
using Headway.Core.Helpers;
using Headway.Core.Model;
using System.Collections.Generic;

namespace Headway.Core.Cache
{
    public class DynamicTypeCache
    {
        private readonly Dictionary<string, DynamicType> modelsCache = new();

        public DynamicType GetModelType(string model)
        {
            if(!modelsCache.ContainsKey(model))
            {
                var models = TypeAttributeHelper.GetHeadwayTypesByAttribute(typeof(DynamicModelAttribute));

                foreach (var m in models)
                {
                    if (!modelsCache.ContainsKey(m.Name))
                    {
                        modelsCache.Add(m.Name, m);
                    }
                }
            }

            if(modelsCache.ContainsKey(model))
            {
                return modelsCache[model];
            }

            return null;
        }
    }
}
