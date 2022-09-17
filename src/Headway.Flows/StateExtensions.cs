using System.Collections.Generic;
using System.Linq;

namespace Headway.Flows
{
    public static class StateExtensions
    {
        public static List<string> ToCodeList(string codes)
        {
            return new List<string>(codes.Split(';')).ToList();
        }

        public static State<T> Next<T>(State<T> state, string nextStateCode)
        {
            return state;
        }

        public static State<T> Revert<T>(State<T> state, string revertStateCode)
        {
            return state;
        }

        public static State<T> Reset<T>(State<T> state)
        {
            return state;
        }
    }
}
