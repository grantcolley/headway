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
    
    public partial class ConfigSearchItem
    {
        public int ConfigSearchItemId { get; set; }
        public int Order { get; set; }
        public string Label { get; set; }
        public string Tooltip { get; set; }
        public string Component { get; set; }
        public Nullable<int> ConfigId { get; set; }
        public string ComponentArgs { get; set; }
        public string ParameterName { get; set; }
    
        public virtual Config Config { get; set; }
    }
}
