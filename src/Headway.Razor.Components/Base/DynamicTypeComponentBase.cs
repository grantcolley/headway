using Headway.Core.Cache;
using Microsoft.AspNetCore.Components;
using System;

namespace Headway.Razor.Components.Base
{
    public abstract class DynamicTypeComponentBase : HeadwayComponentBase
    {       
        [Inject]
        DynamicTypeCache DynamicTypeCache { get; set; }

        public string GetTypeNamespace(string name, Type attribute)
        {
            var dynamicType = DynamicTypeCache.GetDynamicType(name, attribute);

            if (dynamicType == null)
            {
                RaiseAlert($"Failed to map {name} to a fully qualified type.");
                return default;
            }

            return dynamicType.Namespace;
        }
    }
}
