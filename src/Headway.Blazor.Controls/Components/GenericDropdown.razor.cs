using Headway.Core.Args;
using Headway.Core.Attributes;
using Headway.Core.Constants;
using Headway.Core.Dynamic;
using Headway.Core.Extensions;
using Headway.Core.Helpers;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Headway.Blazor.Controls.Components
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
            await base.OnInitializedAsync().ConfigureAwait(false);

            var args = ComponentArgs.ToArgs();

            model = args.FirstArgValue(Args.MODEL);
            componentName = args.FirstArgValue(Args.COMPONENT);
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
