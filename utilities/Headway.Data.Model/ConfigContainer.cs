//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Headway.Data.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class ConfigContainer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ConfigContainer()
        {
            this.ConfigContainers1 = new HashSet<ConfigContainer>();
            this.ConfigItems = new HashSet<ConfigItem>();
        }
    
        public int ConfigContainerId { get; set; }
        public int ConfigId { get; set; }
        public int Order { get; set; }
        public string ComponentArgs { get; set; }
        public string Name { get; set; }
        public string Container { get; set; }
        public string Code { get; set; }
        public string ParentCode { get; set; }
        public string Label { get; set; }
        public Nullable<int> ConfigContainerId1 { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ConfigContainer> ConfigContainers1 { get; set; }
        public virtual ConfigContainer ConfigContainer1 { get; set; }
        public virtual Config Config { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ConfigItem> ConfigItems { get; set; }
    }
}
