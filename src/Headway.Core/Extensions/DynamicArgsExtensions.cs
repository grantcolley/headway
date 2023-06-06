using Headway.Core.Args;
using Headway.Core.Constants;
using Headway.Core.Dynamic;
using Headway.Core.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace Headway.Core.Extensions
{
    public static class DynamicArgsExtensions
    {
        public static string FirstDynamicArgValueToString(this IEnumerable<DynamicArg> dynamicArgs, string name)
        {
            return dynamicArgs.First(a => a.Name.Equals(name)).Value.ToString();
        }

        public static DynamicArg FirstDynamicArgOrDefault(this IEnumerable<DynamicArg> dynamicArgs, string name)
        {
            return dynamicArgs.FirstOrDefault(a => a.Name.Equals(name));
        }

        public static string FirstDynamicArgValueToStringOrDefault(this IEnumerable<DynamicArg> dynamicArgs, string name)
        {
            var dynamicArg = dynamicArgs?.FirstDynamicArgOrDefault(name);

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

        public static List<Arg> ToArgs(this IEnumerable<DynamicArg> dynamicArgs)
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
                    arg.Value = dynamicArg.Value?.ToString();
                }

                args.Add(arg);
            }

            return args;
        }

        public static Arg FirstArgOrDefault(this IEnumerable<DynamicArg> dynamicArgs, string name)
        {
            var dynamicArg = dynamicArgs.FirstDynamicArgOrDefault(name);

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
    }
}
