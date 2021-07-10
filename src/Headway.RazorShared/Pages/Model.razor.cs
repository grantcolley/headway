using Headway.Core.Attributes;
using Headway.RazorShared.Base;
using Headway.RazorShared.Components;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace Headway.RazorShared.Pages
{
    [DynamicComponentAttribute]
    public partial class ModelBase : DynamicTypeComponentBase
    {
        [Parameter]
        public string TypeName { get; set; }

        [Parameter]
        public int Id { get; set; }

        protected string typeFullName;

        protected override async Task OnInitializedAsync()
        {
            typeFullName = await GetTypeFullName(TypeName).ConfigureAwait(false);
            await base.OnInitializedAsync();
        }

        protected RenderFragment RenderListView() => __builder =>
        {
            var type = Type.GetType(typeFullName);
            var genericType = typeof(ModelView<>).MakeGenericType(new[] { type });
            __builder.OpenComponent(1, genericType);
            __builder.AddAttribute(2, "TypeName", TypeName);
            __builder.AddAttribute(3, "Id", Id);
            __builder.CloseComponent();
        };
    }
}
