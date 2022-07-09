using Headway.Core.Attributes;
using Headway.Core.Constants;
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
    public abstract class GenericDropdownBase : ComponentBase
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

            model = args.First(a => a.Name.Equals(Args.MODEL)).Value;
            componentName = args.First(a => a.Name.Equals(Args.COMPONENT)).Value;

            await base.OnInitializedAsync().ConfigureAwait(false);
        }

        protected RenderFragment RenderView() => __builder =>
        {
            var type = Type.GetType(model);
            var component = Type.GetType(componentName);
            var genericType = component.MakeGenericType(new[] { type });
            __builder.OpenComponent(1, genericType);
            __builder.AddAttribute(2, Parameters.FIELD, Field);
            __builder.AddAttribute(3, Parameters.COMPONENT_ARGS, ComponentArgs);
            __builder.CloseComponent();
        };
    }
}
