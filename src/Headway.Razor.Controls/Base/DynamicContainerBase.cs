using Headway.Core.Dynamic;
using Microsoft.AspNetCore.Components;
using System;

namespace Headway.Razor.Controls.Base
{
    public abstract class DynamicContainerBase<T> : HeadwayComponentBase where T : class, new()
    {
        [Parameter]
        public DynamicModel<T> DynamicModel { get; set; }

        protected RenderFragment RenderView() => __builder =>
        {
            var type = Type.GetType("model");
            var component = Type.GetType("container");
            var genericType = component.MakeGenericType(new[] { type });
            __builder.OpenComponent(1, genericType);
            __builder.AddAttribute(2, "DynamicModel", DynamicModel);
            __builder.CloseComponent();
        };
    }
}
