using Headway.Core.Attributes;
using Microsoft.AspNetCore.Components;
using System;

namespace Headway.RazorAdmin.Pages
{
    [DynamicComponentAttribute]
    public partial class ModelBase : ComponentBase
    {
        [Parameter]
        public string TypeName { get; set; }

        [Parameter]
        public int Id { get; set; }

        protected Type DetailsType
        {
            get { return Type.GetType(TypeName); }
        }
    }
}
