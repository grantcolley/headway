using Headway.Core.Dynamic;
using Headway.Core.Helpers;
using Headway.Core.Interface;
using System;
using System.Collections.Generic;

namespace Headway.Core.Cache
{
    public class DynamicTypeCache : IDynamicTypeCache
    {
        private readonly Dictionary<string, DynamicType> cache = new();

        private readonly object cacheLock = new();

        public DynamicType GetDynamicType(string dynamicType, Type type)
        {
            lock (cacheLock)
            {
                var cachedDynamicType = cache.GetValueOrDefault(dynamicType);

                if (cachedDynamicType != null)
                {
                    return cachedDynamicType;
                }
            }

            var dynamicTypes = TypeAttributeHelper.GetHeadwayTypesByAttribute(type);

            lock (cacheLock)
            {
                foreach (var dt in dynamicTypes)
                {
                    if (!cache.ContainsKey(dt.Name))
                    {
                        cache.Add(dt.Name, dt);
                    }
                }

                return cache.GetValueOrDefault(dynamicType);
            }
        }
    }
}
