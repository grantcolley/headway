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
        private readonly List<StateAction> stateActions = new();

        public State()
        {
            SubStates = new List<State>();
            Transitions = new List<State>();
            Dependencies = new List<State>();
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
        public string Permissions { get; set; }

        [StringLength(250)]
        public string SubStateCodes { get; set; }

        [StringLength(250)]
        public string TransitionStateCodes { get; set; }

        [StringLength(250)]
        public string DependencyStateCodes { get; set; }

        [NotMapped]
        [JsonIgnore]
        public ConfigItem ConfigItem { get; set; }

        [NotMapped]
        [JsonIgnore]
        public List<string> PermissionsList
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Permissions))
                {
                    return new List<string>();
                }

                return Permissions.Split(';').ToList();
            }
        }

        [NotMapped]
        [JsonIgnore]
        public List<string> SubStateCodesList
        {
            get
            {
                if(string.IsNullOrWhiteSpace(SubStateCodes))
                {
                    return new List<string>();
                }

                return SubStateCodes.Split(';').ToList();
            }
        }

        [NotMapped]
        [JsonIgnore]
        public List<string> TransitionStateCodesList
        {
            get
            {
                if(string.IsNullOrWhiteSpace(TransitionStateCodes))
                {
                    return new List<string>();
                }

                return TransitionStateCodes.Split(';').ToList();
}
        }

        [NotMapped]
        [JsonIgnore]
        public List<string> DependencyStateCodesList
        {
            get
            {
                if (string.IsNullOrWhiteSpace(DependencyStateCodes))
                {
                    return new List<string>();
                }

                return TransitionStateCodes.Split(';').ToList();
            }
        }

        [NotMapped]
        [JsonIgnore]
        public List<State> SubStates { get;}

        [NotMapped]
        [JsonIgnore]
        public List<State> Transitions { get; }

        [NotMapped]
        [JsonIgnore]
        public List<State> Dependencies { get; }

        public void AddStateActions(StateAction stateAction)
        {
            stateActions.Add(stateAction);
        }

        public async Task ExecuteActionsAsync(object arg, StateActionType stateFunctionType)
        {
            if (stateActions == null)
            {
                return;
            }

            var actions = stateActions
                .Where(a => a.StateActionType.Equals(stateFunctionType))
                .OrderBy(a => a.Order)
                .ToList();

            foreach (var action in actions)
            {
                await action.ActionAsync(this, arg).ConfigureAwait(false);
            }
        }
    }
}