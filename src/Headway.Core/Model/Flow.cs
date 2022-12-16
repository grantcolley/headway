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
    public class Flow : ModelBase
    {
        public Flow()
        {
            States = new List<State>();
        }

        public int FlowId { get; set; }
        public int ConfigId { get; set; }
        public List<State> States { get; set; }

        [NotMapped]
        public Config Config { get; set; }

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
        [JsonIgnore]
        public State ActiveState { get; private set; }

        public void SetActiveState(string activeStateCode = "", StateStatus activeStateStatus = StateStatus.NotStarted)
        {
            if(States.Any())
            {
                ActiveState = States.FirstOrDefault(s => s.Code.Equals(activeStateCode));
                ActiveState.StateStatus = activeStateStatus;
            }
        }
    }
}