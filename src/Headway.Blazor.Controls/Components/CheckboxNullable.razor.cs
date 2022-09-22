using Headway.Core.Attributes;
using Headway.Blazor.Controls.Base;

namespace Headway.Blazor.Controls.Components
{
    [DynamicComponent]
    public abstract class CheckboxNullableBase : DynamicComponentBase
    {
        public bool? PropertyValue
        {
            get
            {
                return (bool?)Field.PropertyInfo.GetValue(Field.Model);
            }
            set
            {
                Field.PropertyInfo.SetValue(Field.Model, value);
            }
        }
    }
}
