using Headway.Core.Attributes;
using Headway.Core.Dynamic;
using Headway.Core.Model;
using Headway.Razor.Controls.Base;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Headway.Razor.Controls.Components
{
    [DynamicComponent]
    public abstract class GenericListBase : ConfigComponentBase
    {
        [Parameter]
        public DynamicField Field { get; set; }

        [Parameter]
        public List<DynamicArg> ComponentArgs { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await GetConfig(Field.ConfigName).ConfigureAwait(false);

            await base.OnInitializedAsync().ConfigureAwait(false);
        }

        protected RenderFragment RenderView() => __builder =>
        {
            var type = Type.GetType(config.Model);
            var component = Type.GetType(config.Container);
            var genericType = component.MakeGenericType(new[] { type });
            __builder.OpenComponent(1, genericType);
            __builder.AddAttribute(2, "Field", Field);
            __builder.AddAttribute(3, "ComponentArgs", ComponentArgs);
            __builder.AddAttribute(4, "Config", config);
            __builder.CloseComponent();
        };
    }
}
