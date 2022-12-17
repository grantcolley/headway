using Headway.Core.Attributes;
using Headway.Core.Enums;
using Headway.Core.Extensions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;

namespace Headway.Core.Model
{
    [DynamicModel]
    public class Flow : ModelBase
    {
        private bool configured;
        private State rootState;

        public Flow()
        {
            States = new List<State>();
        }

        public int FlowId { get; set; }
        public int ConfigId { get; set; }
        public FlowStatus FlowStatus { get; set; }
        public List<State> States { get; set; }
        public List<FlowHistory> History { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(150)]
        public string Model { get; set; }

        [Required]
        [StringLength(50)]
        public string Permissions { get; set; }

        [NotMapped]
        public Config Config { get; set; }

        [NotMapped]
        [JsonIgnore]
        public State ActiveState { get; private set; }

        [NotMapped]
        [JsonIgnore]
        public State RootState
        {
            get
            {
                if(rootState == null)
                {
                    var rootStatePoition = States.Min(s => s.Position);
                    rootState = States.First(s => s.Position.Equals(rootStatePoition));
                }

                return rootState;
            }
        }

        public void SetActiveState(string activeStateCode = "", StateStatus activeStateStatus = StateStatus.NotStarted)
        {
            if (!configured)
            {
                Configure();
            }

            if (string.IsNullOrWhiteSpace(activeStateCode))
            {
                ActiveState = RootState;
            }
            else
            {
                ActiveState = States.FirstOrDefault(s => s.Code.Equals(activeStateCode));
            }

            ActiveState.StateStatus = activeStateStatus;
        }

        private void Configure()
        {
            var states = States.ToDictionary(s => s.Code, s => s);

            foreach (var state in states)
            {
                if(!string.IsNullOrWhiteSpace(state.Value.ParentStateCode))
                {
                    state.Value.ParentState = states[state.Value.ParentStateCode];
                }

                state.Value.SubStates.AddRange(states.GetStates(state.Value.SubStateCodesList));
                state.Value.Transitions.AddRange(states.GetStates(state.Value.TransitionStateCodesList));
                state.Value.Dependencies.AddRange(states.GetStates(state.Value.DependencyStateCodesList));
            }

            configured = true;
        }
    }
}