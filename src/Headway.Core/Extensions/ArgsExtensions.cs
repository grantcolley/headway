using Headway.Core.Model;
using System.Collections.Generic;
using System.Linq;

namespace Headway.Core.Extensions
{
    public static class ArgsExtensions
    {
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
    }
}
