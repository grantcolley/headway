using Headway.Core.Cache;
using Microsoft.AspNetCore.Components;

namespace Headway.Razor.Components.Base
{
    public abstract class DynamicTypeComponentBase : HeadwayComponentBase
    {       
        [Inject]
        DynamicTypeCache DynamicTypeCache { get; set; }

        public string GetModelNameSpace(string name)
        {
            var model = DynamicTypeCache.GetModelType(name);

            if (model == null)
            {
                RaiseAlert($"Failed to map {name} to a fully qualified type name.");
                return default;
            }

            return model.Namespace;
        }
    }
}
