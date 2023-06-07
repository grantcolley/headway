using Headway.Core.Dynamic;
using Headway.Core.Helpers;
using System.Collections.Generic;

namespace Headway.Core.Extensions
{
    public static class DynamicContainerExtensions
    {
        public static void AddDynamicArgs(this DynamicContainer dynamicContainer, List<DynamicField> dynamicFields)
        {
            var dynamicArgs = ComponentArgHelper.ExtractDynamicArgs(dynamicContainer.ComponentArgs, dynamicFields);
            dynamicArgs.AddRange(ComponentArgHelper.ExtractDynamicArgs(dynamicContainer.FlowArgs, dynamicFields));

            dynamicContainer.DynamicArgs.AddRange(dynamicArgs);
        }
    }
}
