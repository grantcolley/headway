using Headway.Core.Args;
using Headway.Core.Dynamic;
using System.Collections.Generic;
using System.Linq;

namespace Headway.Core.Helpers
{
    public static class ComponentArgHelper
    {
        public static List<DynamicArg> ExtractDynamicArgs(string componentArgs, List<DynamicField> dynamicFields)
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
