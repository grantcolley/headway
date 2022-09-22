using Headway.Core.Args;
using Headway.Core.Attributes;
using Headway.Core.Constants;
using Headway.Core.Dynamic;
using Headway.Blazor.Controls.Base;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Headway.Blazor.Controls.Components
{
    [DynamicComponent]
    public abstract class GenericFieldBase : ConfigComponentBase
    {
        [Parameter]
        public DynamicField Field { get; set; }

        [Parameter]
        public List<DynamicArg> ComponentArgs { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync().ConfigureAwait(false);

            await GetConfig(Field.ConfigName).ConfigureAwait(false);
        }

        protected RenderFragment RenderView() => builder =>
        {
            var type = Type.GetType(config.Model);
            var component = Type.GetType(config.Document);
            var genericType = component.MakeGenericType(new[] { type });
            builder.OpenComponent(1, genericType);
            builder.AddAttribute(2, Parameters.FIELD, Field);
            builder.AddAttribute(3, Parameters.COMPONENT_ARGS, ComponentArgs);
            builder.AddAttribute(4, Parameters.CONFIG, config);
            builder.CloseComponent();
        };
    }
}
