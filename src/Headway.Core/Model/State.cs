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
        private List<string> permissionsList;
        private List<string> subStateCodesList;
        private List<string> transitionStateCodesList;
        private List<string> regressionStateCodesList;

        public State()
        {
            SubStates = new List<State>();
            Transitions = new List<State>();
            Regressions = new List<State>();
            StateActions = new List<StateAction>();
        }

        /// <summary>
        /// The state identity.
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
        ///     Standard - standard states require interaction from an external source.
        ///                e.g. a user interactive state is a standard state. Once it has been initialised 
        ///                     and set to InProgress user interaction is required to complete it.
        ///                     Example usage is a state requiring a user to make a routing decision 
        ///                     after performing some user interaction like data input or data review.
        ///                     
        ///     Parent - after a parent state has been initialised it initialises it's first sub state. The 
        ///                 parent state remains InProgress until all sub stateshave been completed. The  
        ///                 last sub state will complete without transitioning, and instead will call on the parent
        ///                 state to complete itself and the parent state transitions to another state. 
        ///                 Example usage is a state that contains it's own sub workflow (sub states),
        ///                 that must be completed before it, the parent, can itself be completed. 
        ///                 Parent states are not user interactive. 
        ///                      
        ///     Auto - after the state has been initialised it automatically completes or regresses itself.
        ///            e.g. during state initialisation an initialisation action can make a runtime 
        ///                 decision to either automatically complete the state, including determine which  
        ///                 state it must transition to, or to which state it must regress to.
        ///                 Example usage is a state that must make a routing decision based on set 
        ///                 criteria using data available at runtime.  
        /// </summary>
        public StateType StateType { get; set; }

        /// <summary>
        /// Status of the state:
        ///     - Uninitialized
        ///     - Initialized
        ///     - InProgress
        ///     - Completed
        /// </summary>
        public StateStatus StateStatus { get; set; }

        /// <summary>
        /// A flag indicating whether the state can only be started once an
        /// owner has been allocated to it.
        /// 
        /// If set to true, at the end of the state's initialisation routine, 
        /// if the state has an owner then the state will automatically run its   
        /// start routine, which will set to InProgress.
        /// 
        /// If set to false, at the end of the state's initialisation routine,
        /// if the state doesn't have an owner, the state will remain Initialized.
        /// An external source will need to assign the state an owner then call it's
        /// start routine, which will set to InProgress.
        /// </summary>
        public bool IsOwnerRestricted { get; set; }

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
        [StringLength(50)]
        public string ParentStateCode { get; set; }

        /// <summary>
        /// The semi-colon separated list of permissions associated with the state.
        /// </summary>
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
        ///    it will attempt to complete the flow.
        ///    
        /// See <see cref="State.Position"/> for positioning 
        /// rules applicable to transitioning.
        /// </summary>
        [StringLength(250)]
        public string TransitionStateCodes { get; set; }

        /// <summary>
        /// A semi-colon separated list of state codes that can be
        /// regressed to. To regress from one state to another,  
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
        public string StateConfigurationClass { get; set; }

        /// <summary>
        /// The context associated with the state.
        /// If the context is null at the time of 
        /// the flow bootstrap routine then the state 
        /// will inherit the context of the flow. 
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
        /// must auto transition or auto regress,
        /// once the initialisation routine is complete.
        /// This is only applicable when <see cref="State.StateType"/> 
        /// is <see cref="StateType.Auto"/>.
        /// The <see cref="StateAutoActionResult"/> can be set at 
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
        /// The owner assigned to the state and 
        /// recorded in the state history.
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
        /// Sub states of a parent state forming a self contained 
        /// sub workflow. Only applicable to parent states
        /// </summary>
        [NotMapped]
        [JsonIgnore]
        public List<State> SubStates { get; }

        /// <summary>
        /// Gets a list of transition states based on.
        /// <see cref="State.TransitionStateCodes"/>
        /// </summary>
        [NotMapped]
        [JsonIgnore]
        public List<State> Transitions { get; }

        /// <summary>
        /// Gets a list of regression states based on
        /// <see cref="State.RegressionStateCodes"/>
        /// </summary>
        [NotMapped]
        [JsonIgnore]
        public List<State> Regressions { get; }

        /// <summary>
        /// C# actions associated wih the state. 
        /// Actions are executed during the following routines:
        ///  - Initialize
        ///  - Start
        ///  - Complete
        ///  - Reset
        ///  
        /// State actions are located in classes specified in 
        /// <see cref="State.StateConfigurationClass"/> or 
        /// <see cref="Flow.FlowConfigurationClass"/>.
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
                if(permissionsList != null)
                {
                    return permissionsList;
                }

                if (string.IsNullOrWhiteSpace(Permissions))
                {
                    permissionsList = new List<string>();
                }
                else
                {
                    permissionsList = Permissions.Split(';').ToList();
                }

                return permissionsList;
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
                if(subStateCodesList != null)
                {
                    return subStateCodesList; 
                }

                if(string.IsNullOrWhiteSpace(SubStateCodes))
                {
                    subStateCodesList = new List<string>();
                }
                else
                {
                    subStateCodesList = SubStateCodes.Split(';').ToList();
                }

                return subStateCodesList;
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
                if(transitionStateCodesList != null)
                {
                    return transitionStateCodesList;
                }

                if (string.IsNullOrWhiteSpace(TransitionStateCodes))
                {
                    transitionStateCodesList = new List<string>();
                }
                else
                {
                    transitionStateCodesList = TransitionStateCodes.Split(';').ToList();
                }

                return transitionStateCodesList;
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
                if(regressionStateCodesList != null)
                {
                    return regressionStateCodesList;
                }

                if (string.IsNullOrWhiteSpace(RegressionStateCodes))
                {
                    regressionStateCodesList = new List<string>();
                }
                else 
                {
                    regressionStateCodesList = RegressionStateCodes.Split(';').ToList();
                }

                return regressionStateCodesList;
            }
        }
    }
}