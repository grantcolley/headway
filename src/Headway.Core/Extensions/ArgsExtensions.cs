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
    }
}
