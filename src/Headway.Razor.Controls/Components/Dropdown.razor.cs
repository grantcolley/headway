using Headway.Core.Attributes;
using Headway.Core.Interface;
using Headway.Core.Model;
using Headway.Razor.Controls.Base;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Headway.Razor.Controls.Components
{
    [DynamicComponent]
    public class DropdownBase : DynamicComponentBase
    {
        [Inject]
        public IOptionsService OptionsService { get; set; }

        protected IEnumerable<OptionItem> OptionItems;

        public Expression<Func<string>> FieldExpression
        {
            get
            {
                return Expression.Lambda<Func<string>>(Field.MemberExpression);
            }
        }

        public string PropertyValue
        {
            get
            {
                return Field.PropertyInfo.GetValue(Field.Model)?.ToString();
            }
        }

        public virtual void OnValueChanged(string value)
        {
            Field.PropertyInfo.SetValue(Field.Model, value);
        }

        protected override async Task OnParametersSetAsync()
        {
            var result = await OptionsService.GetOptionItemsAsync(ComponentArgs).ConfigureAwait(false);

            OptionItems = GetResponse(result);

            await base.OnParametersSetAsync();
        }
    }
}
