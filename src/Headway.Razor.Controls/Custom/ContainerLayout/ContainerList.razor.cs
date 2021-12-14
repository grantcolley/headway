using Headway.Core.Model;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace Headway.Razor.Controls.Custom.ContainerLayout
{
    public class ContainerListBase : ComponentBase 
    {
        [CascadingParameter]
        RootContainer RootContainer { get; set; }

        [Parameter]
        public ConfigContainer ConfigContainer { get; set; }

        [Parameter]
        public List<ConfigContainer> ConfigContainers { get; set; }

        protected string dropClass = "";

        protected void HandleDragEnter()
        {
            if(RootContainer.Payload == null)
            {
                dropClass = "";
                return;
            }

            if(ConfigContainers.Contains(RootContainer.Payload.ConfigContainer)
                || (ConfigContainer != null
                && ConfigContainer.Equals(RootContainer.Payload.ParentConfigContainer)))
            {
                dropClass = "";
            }
            else
            {
                dropClass = "can-drop";
            }
        }

        protected void HandleDragLeave()
        {
            dropClass = "";
        }

        protected void HandleDrop()
        {
            if (RootContainer.Payload == null)
            {
                dropClass = "";
                return;
            }

            if (!ConfigContainers.Contains(RootContainer.Payload.ConfigContainer))
            {
                if (ConfigContainer != null
                && !ConfigContainer.Equals(RootContainer.Payload.ParentConfigContainer))
                {
                    ConfigContainer.ConfigContainers.Add(RootContainer.Payload.ConfigContainer);
                }
                else
                {
                    ConfigContainers.Add(RootContainer.Payload.ConfigContainer);
                }

                if(RootContainer.Payload.ParentConfigContainer != null)
                {
                    RootContainer.Payload.ParentConfigContainer.ConfigContainers.Remove(RootContainer.Payload.ConfigContainer);
                }
                else if(RootContainer.Payload.RootContainers != null)
                {
                    RootContainer.Payload.RootContainers.Remove(RootContainer.Payload.ConfigContainer);
                }
            }

            dropClass = "";
        }
    }
}
