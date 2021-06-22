using Microsoft.AspNetCore.Components;
using System;

namespace Headway.RazorAdmin.Pages
{
    public partial class DetailsBase : ComponentBase
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
