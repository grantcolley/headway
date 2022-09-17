using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Headway.Flows
{
    public abstract class State
    {
        public State()
        {
            SubStates = new List<string>();
            Transitions = new List<string>();
            Dependencies = new List<string>();
        }

        // State
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public StateType StateType { get; set; }
        public StateStatus StateStatus { get; set; }

        // Navigation
        public string? Parent { get; set; }
        public string? Antecedent { get; set; }
        public string? Descendant { get; set; }
        public List<string> SubStates { get; set; }
        public List<string> Transitions { get; set; }
        public List<string> Dependencies { get; set; }
    }

    public class State<T> : State
    {
        public State()
        {
            Actions = new List<StateAction<T>>();
        }

        public T? Context { get; set; }
        public List<StateAction<T>> Actions { get; set; }

        public Func<State<T>, Task<bool>>? CanInitialiseAsync { get; set; }
        public Func<State<T>, Task<bool>>? CanCompleteAsync { get; set; }
    }
}
