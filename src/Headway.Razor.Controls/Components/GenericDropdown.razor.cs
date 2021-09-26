using Headway.Core.Attributes;
using Headway.Core.Dynamic;
using Headway.Core.Helpers;
using Headway.Core.Model;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Headway.Razor.Controls.Components
{
    [DynamicComponent]
    public class GenericDropdownBase : ComponentBase
    {
        private string model;
        private string componentName;

        [Parameter]
        public DynamicField Field { get; set; }

        [Parameter]
        public List<DynamicArg> ComponentArgs { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var args = ComponentArgHelper.GetArgs(ComponentArgs);

            model = args.Single(a => a.Name.Equals("Model")).Value;
            componentName = args.Single(a => a.Name.Equals("Component")).Value;

            await base.OnInitializedAsync();
        }

        protected RenderFragment RenderView() => __builder =>
        {
            var type = Type.GetType(model);
            var component = Type.GetType(componentName);
            var genericType = component.MakeGenericType(new[] { type });
            __builder.OpenComponent(1, genericType);
            __builder.AddAttribute(2, "Field", Field);
            __builder.AddAttribute(3, "ComponentArgs", ComponentArgs);
            __builder.CloseComponent();
        };
    }
}
