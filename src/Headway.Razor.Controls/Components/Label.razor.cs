using Headway.Core.Dynamic;
using Microsoft.AspNetCore.Components;

namespace Headway.Razor.Controls.Components
{
    public partial class LabelBase : ComponentBase
    {
        [Parameter]
        public DynamicField Field { get; set; }

        public string PropertyValue
        {
            get
            {
                return Field.PropertyInfo.GetValue(Field.Model).ToString();
            }
        }
    }
}
