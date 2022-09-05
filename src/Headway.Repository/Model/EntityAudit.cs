using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Headway.Repository.Model
{
    public class EntityAudit
    {
        public int Id { get; set; }
        public string ClrType { get; set; }
        public string TableName { get; set; }
        public string Action { get; set; }
        public string EntityId { get; set; }
        public string OldValues { get; set; }
        public string NewValues { get; set; }
        public string User { get; set; }
        public DateTime DateTime { get; set; }

        [NotMapped]
        public List<PropertyEntry> TemporaryProperties { get; } = new List<PropertyEntry>();

        [NotMapped]
        public Dictionary<string, object> OldValuesDictionary { get; } = new Dictionary<string, object>();

        [NotMapped]
        public Dictionary<string, object> NewValuesDictionary { get; } = new Dictionary<string, object>();
    }
}