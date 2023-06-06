using Headway.Core.Args;
using Headway.Core.Constants;
using Headway.Core.Dynamic;
using Headway.Core.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace Headway.Core.Extensions
{
    public static class DynamicFieldsExtensions
    {
        public static void PropagateDynamicArgs(this List<DynamicField> dynamicFields, List<DynamicArg> sourceArgs)
        {
            foreach (var dynamicField in dynamicFields)
            {
                var componentArg = dynamicField.Parameters.FirstOrDefault(a => a.Key.Equals(Parameters.COMPONENT_ARGS)).Value;
                if (componentArg is List<DynamicArg> dynamicArg)
                {
                    var linkedSourceArg = dynamicArg.FirstDynamicArgOrDefault(Constants.Args.LINK_SOURCE);
                    if (linkedSourceArg != null
                        && linkedSourceArg.Value != null)
                    {
                        DynamicLinkHelper.LinkFields(dynamicField, sourceArgs, linkedSourceArg.Value.ToString());
                    }
                }
            }
        }
    }
}
