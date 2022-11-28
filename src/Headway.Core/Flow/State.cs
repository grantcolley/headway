using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Headway.Core.Flow
{
    public class State
    {
        private readonly List<StateFunction> stateFunctions = new();

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
        public Flow Flow { get; set; }
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
        [StringLength(50)]
        public string Parent { get; set; }

        [StringLength(50)]
        public string Owner { get; set; }

        [StringLength(50)]
        public string Antecedent { get; set; }

        [StringLength(50)]
        public string Descendant { get; set; }

        public void AddStateFunction(StateFunction stateFunction)
        {
            stateFunctions.Add(stateFunction);
        }

        public virtual Task<bool> TryInitialiseAsync(object arg)
        {
            return Functions(arg, StateFunctionType.Initialize);
        }

        public virtual Task<bool> TryCompleteAsync(object arg)
        {
            return Functions(arg, StateFunctionType.Complete);
        }

        public virtual Task<bool> TryResstAsync(object arg)
        {
            return Functions(arg, StateFunctionType.Complete);
        }

        private async Task<bool> Functions(object arg, StateFunctionType stateFunctionType)
        {
            if (stateFunctions == null)
            {
                return true;
            }

            var actions = stateFunctions
                .Where(a => a.StateFunctionType.Equals(stateFunctionType))
                .OrderBy(a => a.Order)
                .ToList();

            foreach (var action in actions)
            {
                var result = await action.FunctionAsync(arg);

                if(!result)
                {
                    return false;
                }
            }

            return true;
        }
    }
}