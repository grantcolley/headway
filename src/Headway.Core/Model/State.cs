using Headway.Core.Attributes;
using Headway.Core.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Headway.Core.Model
{
    [DynamicModel]
    public class State : ModelBase
    {
        private readonly List<StateFunction> stateFunctions = new();

        public State()
        {
            SubStatesList = new List<State>();
            TransitionsList = new List<State>();
            DependenciesList = new List<State>();
        }

        public int Id { get; set; }
        public int Position { get; set; }
        public int ConfigItemId { get; set; }
        public StateType StateType { get; set; }
        public StateStatus StateStatus { get; set; }
        public Flow Flow { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        public string Code { get; set; }

        [Required]
        [StringLength(50)]
        public string Parent { get; set; }

        [Required]
        [StringLength(50)]
        public string Permission { get; set; }

        [StringLength(250)]
        public string SubStates { get; set; }

        [StringLength(250)]
        public string Transitions { get; set; }

        [StringLength(250)]
        public string Dependencies { get; set; }

        [NotMapped]
        [JsonIgnore]
        public List<State> SubStatesList { get; set; }

        [NotMapped]
        [JsonIgnore]
        public List<State> TransitionsList { get; set; }

        [NotMapped]
        [JsonIgnore]
        public List<State> DependenciesList { get; set; }

        [NotMapped]
        [JsonIgnore]
        public ConfigItem ConfigItem { get; set; }

        public void AddStateFunction(StateFunction stateFunction)
        {
            stateFunctions.Add(stateFunction);
        }

        public virtual async Task<bool> TryInitialiseAsync(object arg)
        {
            var result = false;

            var subStatePosition = SubStatesList.Min(s => s.Position);
            var subState = SubStatesList.First(s => s.Position.Equals(subStatePosition));

            result = await subState.TryInitialiseAsync(arg).ConfigureAwait(false);

            if (!result)
            {
                return result;
            }

            result = await FunctionsAsync(arg, StateFunctionType.Initialize).ConfigureAwait(false);

            if (result)
            {
                StateStatus = StateStatus.InProgress;
            }

            return result;
        }

        public virtual async Task<bool> TryCompleteAsync(object arg)
        {
            var result = await FunctionsAsync(arg, StateFunctionType.Complete).ConfigureAwait(false);

            if (result)
            {
                StateStatus = StateStatus.Completed;
            }

            return result;
        }

        public virtual async Task<bool> TryResestAsync(object arg)
        {
            var result = await FunctionsAsync(arg, StateFunctionType.Reset).ConfigureAwait(false);

            if (result)
            {
                StateStatus = StateStatus.NotStarted;
            }

            return result;
        }

        private async Task<bool> FunctionsAsync(object arg, StateFunctionType stateFunctionType)
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
                var result = await action.FunctionAsync(this, arg).ConfigureAwait(false);

                if (!result)
                {
                    return false;
                }
            }

            return true;
        }
    }
}