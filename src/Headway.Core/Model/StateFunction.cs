using System;
using System.Threading.Tasks;
using Headway.Core.Enums;

namespace Headway.Core.Model
{
    public class StateFunction
    {
        public StateFunctionType StateFunctionType { get; set; }

        public int Order { get; set; }

        public Func<State, object, Task<bool>> FunctionAsync { get; set; }
    }
}