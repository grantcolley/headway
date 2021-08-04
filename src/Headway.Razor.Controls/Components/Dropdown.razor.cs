using Headway.Core.Dynamic;
using Microsoft.AspNetCore.Components;
using System.Linq.Expressions;
using System;
using Headway.Core.Attributes;
using Headway.Core.Interface;
using Headway.Core.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using Headway.Razor.Controls.Base;

namespace Headway.Razor.Controls.Components
{
    [DynamicComponent]
    public class DropdownBase : HeadwayComponentBase
    {
        [Inject]
        public IOptionsService OptionsService { get; set; }

        [Parameter]
        public DynamicField Field { get; set; }

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

        protected IEnumerable<OptionItem> OptionItems;

        public void OnValueChanged(string value)
        {
            Field.PropertyInfo.SetValue(Field.Model, value);
        }

        protected override async Task OnParametersSetAsync()
        {
            var result = await OptionsService.GetOptionItemsAsync("ModelOptionItems").ConfigureAwait(false);

            OptionItems = GetResponse(result);

            await base.OnParametersSetAsync();
        }
    }
}
