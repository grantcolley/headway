using Headway.Core.Constants;
using Headway.Core.Dynamic;
using Headway.Core.Model;
using System.Collections.Generic;
using System.Linq;

namespace Headway.Core.Helpers
{
    public static class ComponentArgHelper
    {
        public static List<Arg> GetArgs(IEnumerable<DynamicArg> dynamicArgs)
        {
            var args = new List<Arg>();

            foreach (var dynamicArg in dynamicArgs)
            {
                var arg = new Arg { Name = dynamicArg.Name };

                if (dynamicArg.Value is DynamicField field)
                {
                    arg.Value = field.PropertyInfo.GetValue(field.Model)?.ToString();
                }
                else
                {
                    arg.Value = dynamicArg.Value.ToString();
                }

                args.Add(arg);
            }

            return args;
        }

        public static Arg GetArg(IEnumerable<DynamicArg> dynamicArgs, string name)
        {
            var dynamicArg = dynamicArgs.FirstOrDefault(a => a.Name.Equals(name));

            if (dynamicArg == null)
            {
                return null;
            }

            var arg = new Arg { Name = dynamicArg.Name };

            if (dynamicArg.Value is DynamicField field)
            {
                arg.Value = field.PropertyInfo.GetValue(field.Model)?.ToString();
            }
            else
            {
                arg.Value = dynamicArg.Value.ToString();
            }

            return arg;
        }

        public static string GetArgValue(IEnumerable<DynamicArg> dynamicArgs, string name)
        {
            var dynamicArg = dynamicArgs?.FirstOrDefault(a => a.Name.Equals(name));

            if (dynamicArg == null)
            {
                return null;
            }

            if (dynamicArg.Value is DynamicField field)
            {
                return field.PropertyInfo.GetValue(field.Model)?.ToString();
            }
            else
            {
                return dynamicArg.Value.ToString();
            }
        }

        public static void AddDynamicArgs(List<DynamicField> dynamicFields)
        {
            foreach (var dynamicField in dynamicFields)
            {
                var dynamicArgs = ExtractDynamicArgs(dynamicField.ComponentArgs, dynamicFields);
                dynamicField.Parameters.Add(Parameters.COMPONENT_ARGS, dynamicArgs);

                var linkedSourceArg = dynamicArgs.FirstOrDefault(a => a.Name.Equals(Args.LINK_SOURCE));
                if(linkedSourceArg != null)
                {
                    var linkedField = dynamicFields.FirstOrDefault(f => f.PropertyName.Equals(linkedSourceArg.Value));
                    dynamicField.LinkSource = linkedField;
                    linkedField.LinkDependents.Add(linkedField);
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

                    bool isDynamicField = false;

                    if (nameValue.Length > 2)
                    {
                        var isField = nameValue[2].Split('=');
                        if (isField[1].Equals("true", System.StringComparison.OrdinalIgnoreCase))
                        {
                            isDynamicField = true;
                        }
                    }

                    var dynamicArg = new DynamicArg { Name = name[1] };

                    if (isDynamicField)
                    {
                        var field = dynamicFields.FirstOrDefault(f => f.PropertyName.Equals(value[1]));
                        dynamicArg.Value = field;
                    }
                    else
                    {
                        dynamicArg.Value = value[1];
                    }

                    dynamicArgs.Add(dynamicArg);
                }
            }

            return dynamicArgs;
        }
    }
}
