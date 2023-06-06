﻿using Headway.Core.Args;
using Headway.Core.Dynamic;
using System.Collections.Generic;
using System.Linq;

namespace Headway.Core.Extensions
{
    public static class DynamicArgsExtensions
    {
        public static DynamicArg GetDynamicArgOrDefault(this IEnumerable<DynamicArg> dynamicArgs, string name)
        {
            return dynamicArgs.FirstOrDefault(a => a.Name.Equals(name));
        }

        public static string DynamicArgValueToString(this IEnumerable<DynamicArg> dynamicArgs, string name)
        {
            return dynamicArgs.First(a => a.Name.Equals(name)).Value.ToString();
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
    }
}
