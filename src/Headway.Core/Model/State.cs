using Headway.Core.Attributes;
using Headway.Core.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Headway.Core.Model
{
    [DynamicModel]
    public class State : ModelBase
    {
        private readonly List<StateFunction> stateFunctions = new();

        public State()
        {
            SubStates = new List<State>();
            Transitions = new List<State>();
            Dependencies = new List<State>();
        }

        public int Id { get; set; }
        public int ConfigItemId { get; set; }
        public StateType StateType { get; set; }
        public StateStatus StateStatus { get; set; }
        public Flow Flow { get; set; }
        public List<State> SubStates { get; set; }
        public List<State> Transitions { get; set; }
        public List<State> Dependencies { get; set; }

        [NotMapped]
        public ConfigItem ConfigItem { get; set; }

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

        public virtual Task<bool> TryResestAsync(object arg)
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
                var result = await action.FunctionAsync(this, arg);

                if (!result)
                {
                    return false;
                }
            }

            return true;
        }
    }
}