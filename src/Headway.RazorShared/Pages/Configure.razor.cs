using Headway.Core.Attributes;
using Headway.Core.Helpers;
using Headway.Core.Model;
using Headway.RazorShared.Model;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Headway.RazorShared.Pages
{
    public partial class ConfigureBase : ComponentBase
    {
        //protected IEnumerable<DynamicType> models;
        protected IEnumerable<DynamicType> configurations;
        protected DynamicComponentConfiguration selectedConfiguration;

        private Dictionary<string, DynamicComponentConfiguration> dynamicComponents;

        protected override async Task OnInitializedAsync()
        {
            //models = TypeAttributeHelper.GetExecutingAssemblyDynamicTypesByAttribute(typeof(DynamicModelAttribute));
            configurations = TypeAttributeHelper.GetCallingAssemblyDynamicTypesByAttribute(typeof(DynamicConfigurationAttribute));

            dynamicComponents = new Dictionary<string, DynamicComponentConfiguration>();

            foreach(var configuration in configurations)
            {
                dynamicComponents.Add(configuration.Name, new DynamicComponentConfiguration { ComponentType = Type.GetType(configuration.Namespace) });
            }

            await base.OnInitializedAsync();
        }

        protected void ContainerSelectionChanged(ChangeEventArgs e)
        {
            if(e.Value != null)
            {
                selectedConfiguration = dynamicComponents[e.Value.ToString()];
            }
        }
    }
}
