using System;
using System.Threading.Tasks;

namespace Headway.Flows
{
    public class StateAction<T>
    {
        public StateActionType StateActionType { get; set; }

        public Func<State, Task>? ActionAsync { get; set; }
    }
}
