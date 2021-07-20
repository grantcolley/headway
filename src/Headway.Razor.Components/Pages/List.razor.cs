using Headway.Core.Attributes;
using Headway.Core.Enums;
using Headway.Core.Interface;
using Headway.Core.Model;
using Headway.Razor.Components.Base;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace Headway.Razor.Components.Pages
{
    [DynamicContainer(ContainerType.List)]
    public abstract class ListBase : DynamicTypeComponentBase
    {
        [Inject]
        public IConfigurationService ConfigurationService { get; set; }

        [Parameter]
        public string Config { get; set; }

        protected string modelNameSpace;

        protected string componentNameSpace;

        protected Config config;

        protected override async Task OnInitializedAsync()
        {
            var response = await ConfigurationService.GetConfigAsync(Config).ConfigureAwait(false);

            config = GetResponse(response);

            modelNameSpace = GetTypeNamespace(config.Model, typeof(DynamicModelAttribute));

            componentNameSpace = GetTypeNamespace(config.Component, typeof(DynamicComponentAttribute));

            await base.OnInitializedAsync();
        }

        protected RenderFragment RenderListView() => __builder =>
        {
            var type = Type.GetType(modelNameSpace);
            var component = Type.GetType(componentNameSpace);
            var genericType = component.MakeGenericType(new[] { type });
            __builder.OpenComponent(1, genericType);
            __builder.AddAttribute(2, "ConfigName", config.Name);
            __builder.AddAttribute(3, "ModelName", config.Model);
            __builder.CloseComponent();
        };
    }
}
