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
    
    public partial class MenuItem
    {
        public int MenuItemId { get; set; }
        public int Order { get; set; }
        public Nullable<int> CategoryId { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public string NavigatePage { get; set; }
        public string Config { get; set; }
        public string Permission { get; set; }
    
        public virtual Category Category { get; set; }
    }
}
