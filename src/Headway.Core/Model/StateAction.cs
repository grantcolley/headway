using System;
using System.Threading.Tasks;
using Headway.Core.Enums;

namespace Headway.Core.Model
{
    public class StateAction
    {
        public StateActionType StateActionType { get; set; }

        public int Order { get; set; }

        public Func<State, object, Task> ActionAsync { get; set; }
    }
}