using Headway.Core.Model;
using System.Collections.Generic;
using System.Linq;

namespace Headway.Core.Extensions
{
    public static class DynamicArgsExtensions
    {
        public static DynamicArg DynamicArgOrDefault(this IEnumerable<DynamicArg> dynamicArgs, string name)
        {
            return dynamicArgs.FirstOrDefault(a => a.Name.Equals(name));
        }

        public static string DynamicArgToString(this IEnumerable<DynamicArg> dynamicArgs, string name)
        {
            return dynamicArgs.First(a => a.Name.Equals(name)).Value.ToString();
        }
    }
}
