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
            StateActions = new List<StateAction>();
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

        [StringLength(150)]
        public string ConfigureStateClass { get; set; }

        [NotMapped]
        [JsonIgnore]
        public ConfigItem ConfigItem { get; set; }

        [NotMapped]
        [JsonIgnore]
        public object Context { get; set; }

        [NotMapped]
        [JsonIgnore]
        public bool Configured { get; set; }

        [NotMapped]
        [JsonIgnore]
        public State ParentState { get; set; }

        [NotMapped]
        [JsonIgnore]
        public string Owner { get; set; }

        [NotMapped]
        [JsonIgnore]
        public string Comments { get; set; }

        [NotMapped]
        [JsonIgnore]
        public List<State> SubStates { get; }

        [NotMapped]
        [JsonIgnore]
        public List<State> Transitions { get; }

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
    }
}