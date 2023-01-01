using Headway.Core.Attributes;
using Headway.Core.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;

namespace Headway.Core.Model
{
    [DynamicModel]
    public class State : ModelBase
    {
        public State()
        {
            SubStates = new List<State>();
            Transitions = new List<State>();
            Regressions = new List<State>();
            StateActions = new List<StateAction>();
        }

        /// <summary>
        /// The state identity number.
        /// </summary>
        public int StateId { get; set; }

        /// <summary>
        /// The position of the state in the flow sequence.
        /// The state with the minimum position value 
        /// is the flow's root state, which is the
        /// entry point to the flow.
        /// A state can only transition to another state that has 
        /// a greater position than itself in the flow sequence.
        /// A state can only regress to another state that has a 
        /// lower position than itself in the flow sequence.
        /// </summary>
        public int Position { get; set; }

        /// <summary>
        /// The type of the state.
        ///     Standard - after the state has been initialised control is returned to the calling code.
        ///                e.g. once the state has been initialised, control is passed back to a user 
        ///                     or calling code which will be responsible for completing the stete.
        ///                     Example usage is a state requiring a user to make a routing decision 
        ///                     after performing some user interaction like data input or data review.
        ///                     
        ///     Auto - after the state has been initialised it automatically completes or regresses itself.
        ///            e.g. during state initialisation an initialisation action can make a runtime 
        ///                 decision to either automatically complete the state andspecify the state it 
        ///                 must transition to, or to automatically regress to a specified state.
        ///                 Example usage is a state that must make a routing decision based on set 
        ///                 criteria using data available at runtime.
        ///     
        /// </summary>
        public StateType StateType { get; set; }
        public StateStatus StateStatus { get; set; }
        public Flow Flow { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        public string StateCode { get; set; }

        [Required]
        [StringLength(50)]
        public string ParentStateCode { get; set; }

        [Required]
        [StringLength(50)]
        public string Permissions { get; set; }

        [StringLength(250)]
        public string SubStateCodes { get; set; }

        [StringLength(250)]
        public string TransitionStateCodes { get; set; }

        [StringLength(250)]
        public string RegressionStateCodes { get; set; }

        [StringLength(150)]
        public string ActionConfigurationClass { get; set; }


        /// <summary>
        /// The context associated with the state.
        /// If the context is null at the time of 
        /// the flow bootstrap routine then the state 
        /// will be assigned the flow context. 
        /// </summary>
        [NotMapped]
        [JsonIgnore]
        public object Context { get; set; }

        [NotMapped]
        [JsonIgnore]
        public bool Configured { get; set; }

        [NotMapped]
        [JsonIgnore]
        public StateAutoActionResult AutoActionResult { get; set; }

        [NotMapped]
        [JsonIgnore]
        public State ParentState { get; set; }

        [NotMapped]
        [JsonIgnore]
        public string Owner { get; set; }

        [NotMapped]
        [JsonIgnore]
        public string Comment { get; set; }

        [NotMapped]
        [JsonIgnore]
        public string TransitionStateCode { get; set; }

        [NotMapped]
        [JsonIgnore]
        public string RegressionStateCode { get; set; }

        [NotMapped]
        [JsonIgnore]
        public List<State> SubStates { get; }

        [NotMapped]
        [JsonIgnore]
        public List<State> Transitions { get; }

        [NotMapped]
        [JsonIgnore]
        public List<State> Regressions { get; }

        [NotMapped]
        [JsonIgnore]
        public List<StateAction> StateActions { get; }

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
                if (string.IsNullOrWhiteSpace(TransitionStateCodes))
                {
                    return new List<string>();
                }

                return TransitionStateCodes.Split(';').ToList();
            }
        }

        [NotMapped]
        [JsonIgnore]
        public List<string> RegressionStateCodesList
        {
            get
            {
                if (string.IsNullOrWhiteSpace(RegressionStateCodes))
                {
                    return new List<string>();
                }

                return RegressionStateCodes.Split(';').ToList();
            }
        }
    }
}