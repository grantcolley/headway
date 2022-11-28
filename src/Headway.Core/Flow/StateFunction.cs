using System;
using System.Threading.Tasks;

namespace Headway.Core.Flow
{
    public class StateFunction
    {
        public StateFunctionType StateFunctionType { get; set; }

        public int Order { get; set; }

        public Func<object, Task<bool>> FunctionAsync { get; set; }
    }
}