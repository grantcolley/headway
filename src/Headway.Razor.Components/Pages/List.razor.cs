using Headway.Core.Attributes;
using Headway.Core.Enums;
using Headway.Core.Interface;
using Headway.Core.Model;
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
        [Inject]
        public IConfigurationService ConfigurationService { get; set; }

        [Parameter]
        public string Name { get; set; }

        protected string modelNameSpace;

        protected ListConfig listConfig;

        protected override async Task OnInitializedAsync()
        {
            var response = await ConfigurationService.GetListConfigAsync(Name).ConfigureAwait(false);

            listConfig = GetResponse(response);

            modelNameSpace = GetModelNameSpace(listConfig.Model);

            await base.OnInitializedAsync();
        }

        protected RenderFragment RenderListView() => __builder =>
        {
            var type = Type.GetType(modelNameSpace);
            var genericType = typeof(ListView<>).MakeGenericType(new[] { type });
            __builder.OpenComponent(1, genericType);
            __builder.AddAttribute(2, "ConfigName", listConfig.Name);
            __builder.AddAttribute(3, "ModelName", listConfig.Model);
            __builder.CloseComponent();
        };
    }
}
