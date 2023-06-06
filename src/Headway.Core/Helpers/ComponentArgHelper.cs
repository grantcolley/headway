using Headway.Core.Args;
using Headway.Core.Constants;
using Headway.Core.Dynamic;
using Headway.Core.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace Headway.Core.Helpers
{
    public static class ComponentArgHelper
    {
        public static void AddDynamicArgs(List<DynamicField> dynamicFields)
        {
            foreach (var dynamicField in dynamicFields)
            {
                var dynamicArgs = ExtractDynamicArgs(dynamicField.ComponentArgs, dynamicFields);
                dynamicField.Parameters.Add(Parameters.COMPONENT_ARGS, dynamicArgs);

                var linkedSourceArg = dynamicArgs.FirstDynamicArgOrDefault(Constants.Args.LINK_SOURCE);
                if(linkedSourceArg != null
                    && linkedSourceArg.Value != null)
                {
                    DynamicLinkHelper.LinkFields(dynamicField, dynamicFields, linkedSourceArg.Value.ToString());
                }
            }
        }

        public static void AddDynamicArgs(DynamicContainer dynamicContainer, List<DynamicField> dynamicFields)
        {
            var dynamicArgs = ExtractDynamicArgs(dynamicContainer.ComponentArgs, dynamicFields);
            dynamicContainer.DynamicArgs.AddRange(dynamicArgs);
        }

        private static List<DynamicArg> ExtractDynamicArgs(string componentArgs, List<DynamicField> dynamicFields)
        {
            var dynamicArgs = new List<DynamicArg>();

            if (!string.IsNullOrWhiteSpace(componentArgs))
            {
                var componentArgsList = componentArgs.Split('|');

                foreach (var componentArg in componentArgsList)
                {
                    var nameValue = componentArg.Split(';');
                    var name = nameValue[0].Split('=');
                    var value = nameValue[1].Split('=');

                    var dynamicArg = new DynamicArg { Name = name[1], Value = value[1] };
                    dynamicArgs.Add(dynamicArg);

                    if (dynamicArg.Name.Equals(Constants.Args.PROPAGATE_FIELDS))
                    {
                        var propagateFields = dynamicArg.Value.ToString().Split(',');
                        foreach(var propagateField in propagateFields)
                        {
                            var field = dynamicFields.FirstOrDefault(f => f.PropertyName.Equals(propagateField));
                            if(field != null)
                            {
                                dynamicArgs.Add(new DynamicArg { Name = field.PropertyName, Value = field });
                            }
                        }
                    }
                }
            }

            return dynamicArgs;
        }
    }
}
