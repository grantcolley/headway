using Headway.Core.Model;
using Headway.Razor.Controls.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Headway.Razor.Controls.Custom.ContainerLayout
{
    public abstract class ContainerLayoutBuilderBase : DynamicComponentBase
    {
        protected List<ConfigContainer> configContainers;
        protected ConfigContainer activeContainer;

        protected override async Task OnInitializedAsync()
        {
            configContainers = (List<ConfigContainer>)Field.PropertyInfo.GetValue(Field.Model, null);

            await base.OnInitializedAsync().ConfigureAwait(false);
        }

        protected void EditAsync(ConfigContainer container)
        {
            activeContainer = container;
        }
    }
}
