using Microsoft.AspNetCore.Components;
using System;

namespace Headway.RazorAdmin.Pages
{
    public partial class ListBase : ComponentBase
    {
        [Parameter]
        public string TypeName { get; set; }

        protected RenderFragment RenderListView() => __builder =>
        {
            var type = Type.GetType("Headway.Core.Model.Permission, Headway.Core");
            var genericType = typeof(ListView<>).MakeGenericType(new[] { type });
            __builder.OpenComponent(1, genericType);
            __builder.AddAttribute(2, "TypeName", TypeName);
            __builder.CloseComponent();
        };
    }
}
