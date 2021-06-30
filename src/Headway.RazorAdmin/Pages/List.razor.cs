using Microsoft.AspNetCore.Components;
using System;

namespace Headway.RazorAdmin.Pages
{
    public partial class ListBase : ComponentBase
    {
        [Parameter]
        public string TypeName { get; set; }

        protected Type ListType
        {
            get { return Type.GetType(TypeName); }
        }
    }
}
