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
    
    public partial class DemoModelTreeItem
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DemoModelTreeItem()
        {
            this.DemoModelTreeItems1 = new HashSet<DemoModelTreeItem>();
        }
    
        public int DemoModelTreeItemId { get; set; }
        public int Order { get; set; }
        public int DemoModelId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string ParentCode { get; set; }
        public Nullable<int> DemoModelTreeItemId1 { get; set; }
    
        public virtual DemoModel DemoModel { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DemoModelTreeItem> DemoModelTreeItems1 { get; set; }
        public virtual DemoModelTreeItem DemoModelTreeItem1 { get; set; }
    }
}
