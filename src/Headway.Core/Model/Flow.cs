using Headway.Core.Attributes;
using Headway.Core.Enums;
using Headway.Core.Extensions;
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
        public bool Bootstrapped { get; set; }

        [NotMapped]
        [JsonIgnore]
        public bool ActionsConfigured { get; set; }

        [NotMapped]
        [JsonIgnore]
        public State ActiveState { get; set; }

        [NotMapped]
        [JsonIgnore]
        public Dictionary<string, State> StateDictionary { get; set; }

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
    }
}