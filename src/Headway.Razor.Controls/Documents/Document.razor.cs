using Headway.Core.Attributes;
using Headway.Razor.Controls.Base;
using System.Threading.Tasks;

namespace Headway.Razor.Controls.Documents
{
    [DynamicDocument]
    public abstract class DocumentBase<T> : DynamicDocumentBase<T> where T : class, new()
    {
        protected override async Task OnInitializedAsync()
        {
            await InitializeDynamicModelAsync().ConfigureAwait(false);

            await base.OnInitializedAsync().ConfigureAwait(false);
        }
    }
}
