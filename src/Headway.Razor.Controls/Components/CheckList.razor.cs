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
        protected List<ChecklistItem> checklist = new List<ChecklistItem>();

        protected override Task OnParametersSetAsync()
        {
            var list = (List<ChecklistItem>)Field.PropertyInfo.GetValue(Field.Model, null);

            if (list != null)
            {
                checklist = (List<ChecklistItem>)Field.PropertyInfo.GetValue(Field.Model, null);
            }

            return base.OnParametersSetAsync();
        }

        protected void OnCheckItem(ChecklistItem item)
        {
            item.IsChecked = !item.IsChecked;
        }
    }
}
