using Headway.Core.Attributes;
using Headway.Razor.Controls.Base;

namespace Headway.Razor.Controls.Components
{
    [DynamicComponent]
    public class LabelBase : DynamicComponentBase
    {
        public string PropertyValue
        {
            get
            {
                return Field.PropertyInfo.GetValue(Field.Model)?.ToString();
            }
        }
    }
}
