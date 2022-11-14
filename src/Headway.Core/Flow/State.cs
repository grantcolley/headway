using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Headway.Core.Flow
{
    public abstract class State
    {
        public State()
        {
            Fields = new List<string>();
            SubStates = new List<string>();
            Transitions = new List<string>();
            Dependencies = new List<string>();
        }

        public int Id { get; set; }
        public StateType StateType { get; set; }
        public StateStatus StateStatus { get; set; }
        public List<string> Fields { get; set; }
        public List<string> SubStates { get; set; }
        public List<string> Transitions { get; set; }
        public List<string> Dependencies { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        public string Code { get; set; }

        [Required]
        [StringLength(150)]
        public string Model { get; set; }

        [Required]
        [StringLength(50)]
        public string Parent { get; set; }

        [Required]
        [StringLength(50)]
        public string Antecedent { get; set; }

        [Required]
        [StringLength(50)]
        public string Descendant { get; set; }
    }

    public class State<T> : State
    {
        public State(T context, State state) : this()
        {
            Context = context;

            Id = state.Id;
            Name = state.Name;
            Code = state.Code;
            Model = state.Model;
            StateType = state.StateType;
            StateStatus = state.StateStatus;

            Parent = state.Parent;
            Antecedent = state.Antecedent;
            Descendant = state.Descendant;

            Fields.AddRange(state.Fields);
            SubStates.AddRange(state.SubStates);
            Transitions.AddRange(state.Transitions);
            Dependencies.AddRange(state.Dependencies);
        }

        public State() : base()
        {
            Actions = new List<StateAction<T>>();
        }

        public T Context { get; set; }
        public List<StateAction<T>> Actions { get; set; }

        public Func<State<T>, Task<bool>> CanInitialiseAsync { get; set; }
        public Func<State<T>, Task<bool>> CanCompleteAsync { get; set; }

        public void Reset()
        {
            throw new NotImplementedException();
        }
    }
}