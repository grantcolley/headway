using Headway.Core.Attributes;
using Headway.Razor.Controls.Base;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Linq.Expressions;

namespace Headway.Razor.Controls.Components
{
    [DynamicComponent]
    public abstract class DateBase : DynamicComponentBase
    {
        public DateTime? PropertyValue
        {
            get
            {
                return (DateTime?)Field.PropertyInfo.GetValue(Field.Model);
            }
            set 
            {
                Field.PropertyInfo.SetValue(Field.Model, value); 
            }
        }
    }

    public class DateControl : InputDate<DateTime?>
    {
    }
}
