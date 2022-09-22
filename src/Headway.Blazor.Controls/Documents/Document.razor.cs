using Headway.Core.Attributes;
using Headway.Blazor.Controls.Base;
using System.Threading.Tasks;

namespace Headway.Blazor.Controls.Documents
{
    [DynamicDocument]
    public abstract class DocumentBase<T> : DynamicDocumentBase<T> where T : class, new()
    {
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync().ConfigureAwait(false);

            await InitializeDynamicModelAsync().ConfigureAwait(false);
        }
    }
}
