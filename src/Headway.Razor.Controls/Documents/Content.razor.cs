using Headway.Core.Attributes;
using Headway.Core.Constants;
using Headway.Core.Dynamic;
using Headway.Core.Helpers;
using Headway.Core.Model;
using Headway.Razor.Controls.Base;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Headway.Razor.Controls.Documents
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
