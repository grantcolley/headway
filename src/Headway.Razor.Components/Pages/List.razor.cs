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
        public string ListConfigName { get; set; }

        protected string modelNameSpace;

        protected string componentNameSpace;

        protected ListConfig listConfig;

        protected override async Task OnInitializedAsync()
        {
            var response = await ConfigurationService.GetListConfigAsync(ListConfigName).ConfigureAwait(false);

            listConfig = GetResponse(response);

            modelNameSpace = GetTypeNamespace(listConfig.Model, typeof(DynamicModelAttribute));

            componentNameSpace = GetTypeNamespace(listConfig.Component, typeof(DynamicComponentAttribute));

            await base.OnInitializedAsync();
        }

        protected RenderFragment RenderListView() => __builder =>
        {
            var type = Type.GetType(modelNameSpace);
            var component = Type.GetType(componentNameSpace);
            var genericType = component.GetType().MakeGenericType(new[] { type });
            __builder.OpenComponent(1, genericType);
            __builder.AddAttribute(2, "ConfigName", listConfig.Name);
            __builder.AddAttribute(3, "ModelName", listConfig.Model);
            __builder.CloseComponent();
        };
    }
}
