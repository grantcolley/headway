using Headway.Core.Attributes;
using Headway.Core.Model;
using Headway.Razor.Controls.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Headway.Razor.Controls.Components
{
    [DynamicComponent]
    public abstract class CheckListBase : DynamicComponentBase
    {
        protected List<ChecklistItem> checklist;

        protected override Task OnParametersSetAsync()
        {
            checklist = (List<ChecklistItem>)Field.PropertyInfo.GetValue(Field.Model, null);

            return base.OnParametersSetAsync();
        }
    }
}
