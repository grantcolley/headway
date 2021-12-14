using Headway.Core.Model;
using Microsoft.AspNetCore.Components;

namespace Headway.Razor.Controls.Custom.ContainerLayout
{
    public partial class ContainerBase : ComponentBase
    {
        [CascadingParameter]
        RootContainer RootContainer { get; set; }

        [Parameter]
        public ConfigContainer ConfigContainer { get; set; }

        [Parameter]
        public ConfigContainer ParentConfigContainer { get; set; }

        protected void HandleDragStart(ConfigContainer configContainer)
        {
            var payLoad = new Payload 
            {
                ConfigContainer = configContainer
            };

            if(ParentConfigContainer == null)
            {
                payLoad.RootContainers = RootContainer.ConfigContainers;
            }
            else
            {
                payLoad.ParentConfigContainer = ParentConfigContainer;
            }

            RootContainer.Payload = payLoad;
        }
    }
}
