using Headway.Core.Constants;
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

        public static void ApplyStateReadOnlyFlag(this DynamicContainer dynamicContainer)
        {
            if (dynamicContainer.FlowContext != null)
            {
                var stateCode = dynamicContainer.DynamicArgs.FirstDynamicArgValueToStringOrDefault(FlowConstants.FLOW_STATE_CODE);

                if (!string.IsNullOrWhiteSpace(stateCode))
                {
                    bool readOnly = dynamicContainer.FlowContext.IsStateReadOnly(stateCode);

                    if (readOnly)
                    {
                        foreach (var field in dynamicContainer.DynamicFields)
                        {
                            field.ReadOnly = readOnly;
                        }
                    }
                }
            }
        }
    }
}
