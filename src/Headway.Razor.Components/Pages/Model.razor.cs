using Headway.Core.Attributes;
using Headway.Core.Enums;
using Headway.Razor.Components.Base;
using Headway.Razor.Components.DynamicComponents;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace Headway.Razor.Components.Pages
{
    [DynamicContainer(ContainerType.Model)]
    public partial class ModelBase : DynamicTypeComponentBase
    {
        [Parameter]
        public string TypeName { get; set; }

        [Parameter]
        public int Id { get; set; }

        protected string modelNameSpace;

        protected override async Task OnInitializedAsync()
        {
            modelNameSpace = GetModelNameSpace(TypeName);
            await base.OnInitializedAsync();
        }

        protected RenderFragment RenderListView() => __builder =>
        {
            var type = Type.GetType(modelNameSpace);
            var genericType = typeof(ModelView<>).MakeGenericType(new[] { type });
            __builder.OpenComponent(1, genericType);
            __builder.AddAttribute(2, "TypeName", TypeName);
            __builder.AddAttribute(3, "Id", Id);
            __builder.CloseComponent();
        };
    }
}
