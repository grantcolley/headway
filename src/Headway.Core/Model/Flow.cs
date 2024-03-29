﻿using Headway.Core.Attributes;
using Headway.Core.Enums;
using Headway.Core.Extensions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;

namespace Headway.Core.Model
{
    /// <summary>
    /// <see cref="Flow"/> models a sequence of steps in a workflow.
    /// </summary>
    [Flow]
    [DynamicModel]
    public class Flow : ModelBase
    {
        private State rootState = default;
        private State finalState = default;
        private State activeState = default;

        public Flow()
        {
            States = new List<State>();
            History = new List<FlowHistory>();
            ReplayHistory = new List<FlowHistory>();
            StateDictionary = new Dictionary<string, State>();
        }

        /// <summary>
        /// The flow identity.
        /// </summary>
        public int FlowId { get; set; }

        /// <summary>
        /// Status of the flow:
        ///     - NotStarted
        ///     - InProgress
        ///     - Completed
        /// </summary>
        public FlowStatus FlowStatus { get; set; }

        /// <summary>
        /// The states associated with the flow.
        /// </summary>
        public List<State> States { get; set; }

        /// <summary>
        /// The name of the flow.
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// The code of the flow.
        /// </summary>
        [Required]
        [StringLength(50)]
        public string FlowCode { get; set; }

        /// <summary>
        /// The class dynamically loaded during the flow bootstrap 
        /// routine for configuring state actions to be invoked at 
        /// runtime. This class can contain actions to be applied 
        /// to all states or individual ones. 
        /// The pattern expected is '{type full name}, {assembly name}'.
        /// </summary>
        [StringLength(150)]
        public string FlowConfigurationClass { get; set; }

        /// <summary>
        /// Permission for accessing the <see cref="Flow"/>.
        /// </summary>
        [StringLength(50)]
        public string Permission { get; set; }

        /// <summary>
        /// A record of states transitioning through the flow.  
        /// </summary>
        [NotMapped]
        public List<FlowHistory> History { get; set; }

        /// <summary>
        /// A a replay of states transitioned through the flow. Excludes state resets.
        /// </summary>
        [NotMapped]
        [JsonIgnore]
        public List<FlowHistory> ReplayHistory { get; set; }

        /// <summary>
        /// A dictionary of states where the key is the StateCode.
        /// </summary>
        [NotMapped]
        [JsonIgnore]
        public Dictionary<string, State> StateDictionary { get; set; }

        /// <summary>
        /// A flag indicating whether the flow bootstrap 
        /// routine has been completed.
        /// </summary>
        [NotMapped]
        [JsonIgnore]
        public bool Bootstrapped { get; set; }

        /// <summary>
        /// A flag indicating whether actions have been 
        /// configured during the flow bootstrap routine.
        /// </summary>
        [NotMapped]
        [JsonIgnore]
        public bool ActionsConfigured { get; set; }

        /// <summary>
        /// The context associated with the flow.
        /// </summary>
        [NotMapped]
        [JsonIgnore]
        public object Context { get; set; }

        /// <summary>
        /// The active state code.
        /// </summary>
        [NotMapped]
        public string ActiveStateCode { get; set; }

        /// <summary>
        /// Flag inidcating of the active state is readonly.
        /// </summary>
        [NotMapped]
        public bool IsActiveStateReadOnly { get; set; }

        /// <summary>
        /// The active state's flow history.
        /// </summary>
        [NotMapped]
        public FlowHistory ActiveStateFlowHistory { get; set; }

        /// <summary>
        /// The active state in the flow. This is set 
        /// during the bootstrap routine and tracked 
        /// during the lifetime of the flow instance.
        /// </summary>
        [NotMapped]
        [JsonIgnore]
        public State ActiveState 
        {
            get { return activeState; }
            set
            {
                if(activeState != value)
                {
                    activeState = value;

                    if (value != null)
                    {
                        ActiveStateCode = value.StateCode;
                        ActiveStateFlowHistory = ReplayHistory?.FirstOrDefault(f => f.StateCode.Equals(ActiveStateCode));
                    }
                    else
                    {
                        ActiveStateCode = null;
                        ActiveStateFlowHistory = null;
                    }
                }
            }
        }

        /// <summary>
        /// The root state is the state with the minimum 
        /// state position value.
        /// </summary>
        [NotMapped]
        [JsonIgnore]
        public State RootState
        {
            get
            {
                if (rootState == null
                    && States.Any())
                {
                    rootState = States.FirstState();
                }

                return rootState;
            }
        }

        /// <summary>
        /// The final state is the state with the maximum 
        /// state position value.
        /// </summary>
        [NotMapped]
        [JsonIgnore]
        public State FinalState
        {
            get
            {
                if (finalState == null
                    && States.Any())
                {
                    finalState = States.LastState();
                }

                return finalState;
            }
        }
    }
}