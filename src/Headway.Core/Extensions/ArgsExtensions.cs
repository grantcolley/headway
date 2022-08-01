using Headway.Core.Constants;
using Headway.Core.Dynamic;
using Headway.Core.Helpers;
using Headway.Core.Model;
using System.Collections.Generic;
using System.Linq;

namespace Headway.Core.Extensions
{
    public static class ArgsExtensions
    {
        public static Arg ArgOrDefault(this IEnumerable<Arg> args, string name)
        {
            return args.FirstOrDefault(a => a.Name.Equals(name));
        }

        public static string ArgValue(this IEnumerable<Arg> args, string name)
        {
            return args.First(a => a.Name.Equals(name)).Value;
        }

        public static bool HasArg(this IEnumerable<Arg> args, string name)
        {
            return args.Any(a => a.Name.Equals(name));
        }

        public static List<Arg> ToArgsList(this string componentArgs)
        {
            var args = new List<Arg>();

            var argsSplit = componentArgs.Split('|');
            for (var i = 0; i <= argsSplit.Length - 1; i++)
            {
                var argSplit = argsSplit[i].Split(';');
                var name = argSplit[0].Split('=');
                var value = argSplit[1].Split('=');
                args.Add(new Arg { Name = name[1], Value = value[1] });
            }

            return args;
        }

        public static void AddSearchArgs(this List<DynamicSearchItem> dynamicSearchItems)
        {
            foreach (var dynamicSearchItem in dynamicSearchItems)
            {
                if (!string.IsNullOrWhiteSpace(dynamicSearchItem.ComponentArgs))
                {
                    var args = dynamicSearchItem.ComponentArgs.ToArgsList();
                    dynamicSearchItem.Parameters.Add(Parameters.COMPONENT_ARGS, args);

                    var linkedSourceArg = args.ArgOrDefault(Args.LINK_SOURCE);
                    if (linkedSourceArg != null
                        && linkedSourceArg.Value != null)
                    {
                        DynamicLinkHelper.LinkSearchItems(dynamicSearchItem, dynamicSearchItems, linkedSourceArg.Value.ToString());
                    }
                }
            }
        }
    }
}
