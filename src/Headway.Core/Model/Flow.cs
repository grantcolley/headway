using Headway.Core.Attributes;
using Headway.Core.Enums;
using Headway.Core.Extensions;
using Headway.Core.Interface;
using System;
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
        private State rootState;

        public Flow()
        {
            States = new List<State>();
            History = new List<FlowHistory>();
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
        [StringLength(150)]
        public string ActionSetupClass { get; set; }

        [Required]
        [StringLength(50)]
        public string Permissions { get; set; }

        [NotMapped]
        public Config Config { get; set; }

        [NotMapped]
        [JsonIgnore]
        public State ActiveState { get; set; }

        [NotMapped]
        [JsonIgnore]
        public Dictionary<string, State> StateDictionary { get; private set; }

        [NotMapped]
        [JsonIgnore]
        public object Context { get; set; }

        [NotMapped]
        [JsonIgnore]
        public State RootState
        {
            get
            {
                if (rootState == null)
                {
                    rootState = States.FirstState();
                }

                return rootState;
            }
        }

        public void Bootstrap()
        {
            StateDictionary = States.ToDictionary(s => s.StateCode, s => s);

            foreach (var state in StateDictionary)
            {
                state.Value.StateStatus = default;
                state.Value.Owner = default;
                state.Value.Flow = this;

                if (state.Value.Context != null)
                {
                    state.Value.Context = Context;
                }

                if (!string.IsNullOrWhiteSpace(state.Value.ParentStateCode))
                {
                    state.Value.ParentState = StateDictionary[state.Value.ParentStateCode];
                }

                state.Value.SubStates.Clear();
                state.Value.SubStates.AddRange(StateDictionary.GetStates(state.Value.SubStateCodesList));

                state.Value.Transitions.Clear();
                state.Value.Transitions.AddRange(StateDictionary.GetStates(state.Value.TransitionStateCodesList));
            }

            this.SetupFlowActions();

            if (History.Any())
            {
                var lastIndex = History.Count - 1;

                for (int i = 0; i < lastIndex - 1; i++)
                {
                    var history = History[i];

                    var state = StateDictionary[history.StateCode];
                    state.StateStatus = history.StateStatus;
                    state.Owner = history.Owner;

                    if (i.Equals(lastIndex))
                    {
                        ActiveState = state;
                    }
                }
            }
            else
            {
                ActiveState = RootState;
            }
        }
    }
}