using Headway.Core.Attributes;
using Headway.Blazor.Controls.Base;

namespace Headway.Blazor.Controls.Components
{
    [DynamicComponent]
    public abstract class LabelBase : DynamicComponentBase
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
