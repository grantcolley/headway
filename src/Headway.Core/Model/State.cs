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
        /// The type of the state:
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

        /// <summary>
        /// Status of the state:
        ///     - NotStarted
        ///     - InProgress
        ///     - Completed
        /// </summary>
        public StateStatus StateStatus { get; set; }

        /// <summary>
        /// The flow associated with the state.
        /// </summary>
        public Flow Flow { get; set; }

        /// <summary>
        /// Name of the state.
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// The state code.
        /// </summary>
        [Required]
        [StringLength(50)]
        public string StateCode { get; set; }

        /// <summary>
        /// The state code of the parent associated with the state.
        /// Only applicable if the state is a sub state of another.
        /// </summary>
        [Required]
        [StringLength(50)]
        public string ParentStateCode { get; set; }

        /// <summary>
        /// The semi-colon separated list of permissions associated with the state.
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Permissions { get; set; }

        /// <summary>
        /// A semi-colon separated list of sub state codes. 
        /// Only applicable if the state is a parent of one 
        /// or more sub states.
        /// </summary>
        [StringLength(250)]
        public string SubStateCodes { get; set; }

        /// <summary>
        /// A semi-colon separated list of state codes that can be 
        /// transitioned to when the state is completed.
        /// To transition from one state to another, call 
        /// CompleteAsync passing in transitionStateCode.
        /// If transitionStateCode is null, it will transition 
        /// to the first state code in TransitionStateCodes. 
        /// If TransitionStateCodes is null i.e. it has nowhere
        /// to transition to, then:
        ///  - if the state has a parent it will attempt 
        ///    to complete the parent
        ///  - if the state has no parent it is assumed 
        ///    the end of the flow has been reached and 
        ///    the flow is therefore completed.
        ///    
        /// See <see cref="State.Position"/> for positioning 
        /// rules applicable to transitioning.
        /// </summary>
        [StringLength(250)]
        public string TransitionStateCodes { get; set; }

        /// <summary>
        /// A semi-colon separated list of state codes that can be
        /// regressed to. To rgress from one state to another,  
        /// call ResetAsync passing in regressionStateCode. 
        /// If regressionStateCode is null, it will simply 
        /// reset the state. 
        /// 
        /// See <see cref="State.Position"/>
        /// for positioning rules applicable to regressing.
        /// </summary>
        [StringLength(250)]
        public string RegressionStateCodes { get; set; }

        /// <summary>
        /// The class dynamically loaded during the state initialising 
        /// routine for configuring state actions to be invoked at runtime.
        /// The pattern expected is '{type full name}, {assembly name}'.
        /// </summary>
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

        /// <summary>
        /// A flag indicating whether actions have been 
        /// configured during the state initialising routine.
        /// </summary>
        [NotMapped]
        [JsonIgnore]
        public bool ActionsConfigured { get; set; }

        /// <summary>
        /// A flag indicating whether an auto state 
        /// must auto transition or auto regress
        /// once the initialisation routine is complete.
        /// This is only applicable when <see cref="State.StateType"/> 
        /// is <see cref="StateType.Auto"/>.
        /// The StateAutoActionResult can be set at 
        /// runtime in initialisation actions during the
        /// state's initialisation routine.
        /// </summary>
        [NotMapped]
        [JsonIgnore]
        public StateAutoActionResult AutoActionResult { get; set; }

        /// <summary>
        /// The parant of a state. Only 
        /// applicable to sub states.
        /// </summary>
        [NotMapped]
        [JsonIgnore]
        public State ParentState { get; set; }

        /// <summary>
        /// The state owner recorded in the state history.
        /// </summary>
        [NotMapped]
        [JsonIgnore]
        public string Owner { get; set; }

        /// <summary>
        /// Comments associated with the state 
        /// recorded in the state history.
        /// </summary>
        [NotMapped]
        [JsonIgnore]
        public string Comment { get; set; }

        /// <summary>
        /// The state to transition to when the current 
        /// state is completed. This can be set by user
        /// interaction or dynamically at runtime inside 
        /// an action triggered during state initialisation 
        /// or state completion.
        /// </summary>
        [NotMapped]
        [JsonIgnore]
        public string TransitionStateCode { get; set; }

        /// <summary>
        /// The state to regress to when the current 
        /// state is regressed. This can be set by user
        /// interaction or dynamically at runtime inside 
        /// an action triggered during state initialisation 
        /// or state completion.
        /// </summary>
        [NotMapped]
        [JsonIgnore]
        public string RegressionStateCode { get; set; }

        /// <summary>
        /// Sub states of a parent state. 
        /// Only applicable to parent states
        /// </summary>
        [NotMapped]
        [JsonIgnore]
        public List<State> SubStates { get; }

        /// <summary>
        /// Gets a list of transition states.
        /// based on <see cref="State.TransitionStateCodes"/>
        /// </summary>
        [NotMapped]
        [JsonIgnore]
        public List<State> Transitions { get; }

        /// <summary>
        /// Gets a list of regression states.
        /// based on <see cref="State.RegressionStateCodes"/>
        /// </summary>
        [NotMapped]
        [JsonIgnore]
        public List<State> Regressions { get; }

        /// <summary>
        /// C# actions associated wih the state to be executed
        /// during initialisation or completion routines.
        /// State actions are located in classes specified in 
        /// <see cref="State.ActionConfigurationClass"/> or 
        /// <see cref="Flow.ActionConfigurationClass"/>.
        /// </summary>
        [NotMapped]
        [JsonIgnore]
        public List<StateAction> StateActions { get; }

        /// <summary>
        /// Splits the semi-colon separated <see cref="State.Permissions"/>.
        /// </summary>
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

        /// <summary>
        /// Splits the semi-colon separated <see cref="State.SubStateCodes"/>.
        /// </summary>
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

        /// <summary>
        /// Splits the semi-colon separated <see cref="State.TransitionStateCodes"/>.
        /// </summary>
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

        /// <summary>
        /// Splits the semi-colon separated <see cref="State.RegressionStateCodes"/>.
        /// </summary>
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