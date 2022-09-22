using Headway.Core.Attributes;
using Headway.Core.Dynamic;
using Headway.Blazor.Controls.Base;
using System.Threading.Tasks;

namespace Headway.Blazor.Controls.Documents
{
    [DynamicDocument]
    public abstract class ContentBase<T> : GenericComponentBase<T> where T : class, new()
    {
        protected DynamicModel<T> dynamicModel;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync().ConfigureAwait(false);

            var model = (T)Field.PropertyInfo.GetValue(Field.Model, null);

            dynamicModel = await GetDynamicModelAsync(model, Field.ConfigName);
        }
    }
}
