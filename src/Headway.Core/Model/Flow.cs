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
    [Flow]
    [DynamicModel]
    public class Flow : ModelBase
    {
        private State rootState = default;
        private State finalState = default;

        public Flow()
        {
            States = new List<State>();
            History = new List<FlowHistory>();
        }

        /// <summary>
        /// The flow identity number.
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
        /// A flag indicating wether to configure all the 
        /// flow states during the flow bootstrap routine.
        /// </summary>
        public bool ConfigureStatesDuringBootstrap { get; set; }

        /// <summary>
        /// The states associated with the flow.
        /// </summary>
        public List<State> States { get; set; }

        /// <summary>
        /// A record of states transitioning through the flow.  
        /// </summary>
        public List<FlowHistory> History { get; set; }

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
        /// Comma seperated list of permissions to give 
        /// read / write access to all states in the flow.
        /// </summary>
        [StringLength(50)]
        public string Permissions { get; set; }

        /// <summary>
        /// The name of the flow.
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

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
        /// The active state in the flow. This is set 
        /// during the bootstrap routine and tracked 
        /// during the lifetime of the flow instance.
        /// </summary>
        [NotMapped]
        [JsonIgnore]
        public State ActiveState { get; set; }

        /// <summary>
        /// A dictionary of states where the key is the StateCode.
        /// </summary>
        [NotMapped]
        [JsonIgnore]
        public Dictionary<string, State> StateDictionary { get; set; }

        /// <summary>
        /// The context associated with the flow.
        /// </summary>
        [NotMapped]
        [JsonIgnore]
        public object Context { get; set; }

        /// <summary>
        /// A full list of permissible permissions where those assigned to the 
        /// <see cref="Flow"/> have been checked. For display in the UI only.
        /// </summary>
        [NotMapped]
        public List<ChecklistItem> PermissionChecklist { get; set; }

        /// <summary>
        /// A list of permissions assigned to the <see cref="Flow"/>.
        /// For display in the UI only.
        /// </summary>
        [NotMapped]
        [JsonIgnore]
        public List<string> PermissionList
        {
            get
            {
                if (PermissionChecklist == null)
                {
                    return new List<string>();
                }

                return PermissionChecklist
                    .Where(p => p.IsChecked)
                    .Select(r => r.Name)
                    .ToList();
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
        /// The final state is the state with the minimum 
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