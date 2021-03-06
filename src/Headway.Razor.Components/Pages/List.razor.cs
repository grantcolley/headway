﻿using Headway.Core.Attributes;
using Headway.Core.Enums;
using Headway.Razor.Components.Base;
using Headway.Razor.Components.DynamicComponents;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace Headway.Razor.Components.Pages
{
    [DynamicContainer(ContainerType.List)]
    public partial class ListBase : DynamicTypeComponentBase
    {
        [Parameter]
        public string TypeName { get; set; }

        protected string typeFullName;

        protected override async Task OnInitializedAsync()
        {
            typeFullName = await GetTypeFullName(TypeName).ConfigureAwait(false);
            await base.OnInitializedAsync();
        }

        protected RenderFragment RenderListView() => __builder =>
        {
            var type = Type.GetType(typeFullName);
            var genericType = typeof(ListView<>).MakeGenericType(new[] { type });
            __builder.OpenComponent(1, genericType);
            __builder.AddAttribute(2, "TypeName", TypeName);
            __builder.CloseComponent();
        };
    }
}
