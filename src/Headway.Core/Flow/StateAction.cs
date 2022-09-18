using System;
using System.Threading.Tasks;

namespace Headway.Core.Flow
{
    public class StateAction<T>
    {
        public StateActionType StateActionType { get; set; }

        public Func<State, Task>? ActionAsync { get; set; }
    }
}
