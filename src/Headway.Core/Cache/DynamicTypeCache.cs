using Headway.Core.Helpers;
using Headway.Core.Model;
using System;
using System.Collections.Generic;

namespace Headway.Core.Cache
{
    public class DynamicTypeCache
    {
        private readonly Dictionary<string, DynamicType> cache = new();

        public DynamicType GetDynamicType(string dynamicType, Type type)
        {
            var cachedDynamicType = cache.GetValueOrDefault(dynamicType);

            if (cachedDynamicType != null)
            {
                return cachedDynamicType;
            }

            var dynamicTypes = TypeAttributeHelper.GetHeadwayTypesByAttribute(type);

            foreach (var dt in dynamicTypes)
            {
                if (!cache.ContainsKey(dt.Name))
                {
                    cache.Add(dt.Name, dt);
                }
            }

            return cache.GetValueOrDefault(dynamicType); ;
        }
    }
}
