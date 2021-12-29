using Headway.Core.Attributes;
using Headway.Core.Helpers;
using Headway.Core.Model;
using Headway.Razor.Controls.Base;
using System.Collections.Generic;

namespace Headway.Razor.Controls.Components
{
    [DynamicComponent]
    public abstract class RadioArgsBase : DynamicComponentBase
    {
        protected List<Arg> args;

        public string PropertyValue
        {
            get
            {
                return Field.PropertyInfo.GetValue(Field.Model)?.ToString();
            }
            set
            {
                Field.PropertyInfo.SetValue(Field.Model, value);
            }
        }

        protected override void OnInitialized()
        {
            args = ComponentArgHelper.GetArgs(ComponentArgs);

            base.OnInitialized();
        }
    }
}
