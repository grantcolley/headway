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
            var dynamicArg = dynamicArgs.FirstOrDefault(a => a.Name.Equals(name));

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
                var dynamicArgs = new List<DynamicArg>();

                if (!string.IsNullOrWhiteSpace(dynamicField.ComponentArgs))
                {
                    var componentArgs = dynamicField.ComponentArgs.Split('|');

                    foreach (var componentArg in componentArgs)
                    {
                        var nameValue = componentArg.Split(';');
                        var name = nameValue[0].Split('=');
                        var value = nameValue[1].Split('=');

                        var dynamicArg = new DynamicArg { Name = name[1] };

                        var field = dynamicFields.SingleOrDefault(f => f.PropertyName.Equals(value[1]));

                        if (field != null)
                        {
                            dynamicArg.Value = field;
                        }
                        else
                        {
                            dynamicArg.Value = value[1];
                        }

                        dynamicArgs.Add(dynamicArg);
                    }
                }

                dynamicField.Parameters.Add("ComponentArgs", dynamicArgs);
            }
        }
    }
}
