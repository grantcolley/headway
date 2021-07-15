using Headway.Core.Attributes;
using Headway.Core.Helpers;
using Headway.Core.Model;
using Headway.Razor.Configuration.Model;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;

namespace Headway.Razor.Configuration.Pages
{
    public partial class ConfigureBase : ComponentBase
    {
        //protected IEnumerable<DynamicType> models;
        protected IEnumerable<DynamicType> configurations;
        protected DynamicComponentConfiguration selectedConfiguration;

        private Dictionary<string, DynamicComponentConfiguration> dynamicComponents;

        protected override void OnInitialized()
        {
            //models = TypeAttributeHelper.GetExecutingAssemblyDynamicTypesByAttribute(typeof(DynamicModelAttribute));
            configurations = TypeAttributeHelper.GetCallingAssemblyDynamicRazorComponentsByAttribute(typeof(DynamicConfigurationAttribute));

            dynamicComponents = new Dictionary<string, DynamicComponentConfiguration>();

            foreach(var configuration in configurations)
            {
                dynamicComponents.Add(configuration.Name, new DynamicComponentConfiguration 
                {
                    ComponentType = Type.GetType(configuration.Namespace) 
                });
            }

            selectedConfiguration = dynamicComponents["DynamicDefault"];

            base.OnInitialized();
        }

        protected void ContainerSelectionChanged(ChangeEventArgs e)
        {
            selectedConfiguration = dynamicComponents[e.Value.ToString()];
        }
    }
}
